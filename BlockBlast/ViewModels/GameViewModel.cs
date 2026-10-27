
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using BlockBlast.Models;
using BlockBlast.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Storage;

namespace BlockBlast.ViewModels
{
    public class GameViewModel : INotifyPropertyChanged
    {
        private readonly GameBoard _gameBoard;
        private readonly BlockFactory _blockFactory;
        private readonly SoundService _soundService;

        private int _score;
        private int _bestScore;
        private bool _isGameOver;
        private bool _isPaused;
        private Block? _selectedBlock;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<Block> AvailableBlocks { get; }
            = new ObservableCollection<Block>();

        // Mängulaua andmed
        public int[,] Board => _gameBoard.Grid;

        // Mängulaua ruutude värvid
        public Color?[,] CellColors => _gameBoard.CellColors;

        public int Score
        {
            get => _score;
            private set
            {
                if (_score == value)
                    return;

                _score = value;
                OnPropertyChanged();
            }
        }

        public int BestScore
        {
            get => _bestScore;
            private set
            {
                if (_bestScore == value)
                    return;

                _bestScore = value;
                OnPropertyChanged();
            }
        }

        public int Combo => _gameBoard.Combo;

        public int LastBonus => _gameBoard.LastBonus;

        public bool IsGameOver
        {
            get => _isGameOver;
            private set
            {
                if (_isGameOver == value)
                    return;

                _isGameOver = value;
                OnPropertyChanged();
            }
        }

        public bool IsPaused
        {
            get => _isPaused;
            private set
            {
                if (_isPaused == value)
                    return;

                _isPaused = value;
                OnPropertyChanged();
            }
        }

        public Block? SelectedBlock
        {
            get => _selectedBlock;
            private set
            {
                if (_selectedBlock == value)
                    return;

                _selectedBlock = value;
                OnPropertyChanged();
            }
        }

        public ICommand NewGameCommand { get; }
        public ICommand PauseCommand { get; }
        public ICommand SelectBlockCommand { get; }
        public ICommand PlaceBlockCommand { get; }

        public GameViewModel()
        {
            _gameBoard = new GameBoard();
            _blockFactory = new BlockFactory();
            _soundService = new SoundService();

            _bestScore = Preferences.Default.Get(
                "BlockBlastBestScore", 0);

            NewGameCommand = new Command(StartNewGame);
            PauseCommand = new Command(TogglePause);

            SelectBlockCommand = new Command<Block>(
                SelectBlock);

            PlaceBlockCommand = new Command<string>(
                PlaceBlockAtPosition);

            StartNewGame();
        }

        // Alustab uut mängu
        private void StartNewGame()
        {
            _gameBoard.Reset();

            Score = 0;
            IsGameOver = false;
            IsPaused = false;
            SelectedBlock = null;

            OnPropertyChanged(nameof(Combo));
            OnPropertyChanged(nameof(LastBonus));

            GenerateBlocks();

            OnPropertyChanged(nameof(Board));
            OnPropertyChanged(nameof(CellColors));

            CheckGameOver();
        }

        // Genereerib kolm uut klotsi
        private void GenerateBlocks()
        {
            AvailableBlocks.Clear();

            foreach (Block block in
                _blockFactory.GenerateThreeBlocks())
            {
                AvailableBlocks.Add(block);
            }
        }

        // Valib klotsi
        private void SelectBlock(Block? block)
        {
            if (IsGameOver || IsPaused || block == null)
                return;

            if (!AvailableBlocks.Contains(block))
                return;

            SelectedBlock = block;
        }

        // Paigutab klotsi mängulauale
        private void PlaceBlockAtPosition(string? position)
        {
            if (IsGameOver || IsPaused ||
                SelectedBlock == null)
                return;

            if (string.IsNullOrWhiteSpace(position))
                return;

            string[] parts = position.Split(',');

            if (parts.Length != 2 ||
                !int.TryParse(parts[0], out int row) ||
                !int.TryParse(parts[1], out int col))
                return;

            Block block = SelectedBlock;

            if (block.Height > GameBoard.BoardSize ||
                block.Width > GameBoard.BoardSize)
                return;

            // Arvutab klotsi asukoha vajutatud ruudu ümber
            int startRow = row - block.Height / 2;
            int startCol = col - block.Width / 2;

            startRow = Math.Clamp(
                startRow, 0,
                GameBoard.BoardSize - block.Height);

            startCol = Math.Clamp(
                startCol, 0,
                GameBoard.BoardSize - block.Width);

            // Otsib lähima sobiva asukoha
            if (!_gameBoard.CanPlaceBlock(
                block, startRow, startCol))
            {
                bool found = false;
                int bestDistance = int.MaxValue;
                int bestRow = 0;
                int bestCol = 0;

                for (int r = 0;
                     r <= GameBoard.BoardSize - block.Height;
                     r++)
                {
                    for (int c = 0;
                         c <= GameBoard.BoardSize - block.Width;
                         c++)
                    {
                        if (!_gameBoard.CanPlaceBlock(block, r, c))
                            continue;

                        int distance =
                            Math.Abs(r - startRow) +
                            Math.Abs(c - startCol);

                        if (distance < bestDistance)
                        {
                            bestDistance = distance;
                            bestRow = r;
                            bestCol = c;
                            found = true;
                        }
                    }
                }

                if (!found)
                {
                    CheckGameOver();
                    return;
                }

                startRow = bestRow;
                startCol = bestCol;
            }

            // Paigutab klotsi mängulauale
            bool placed = _gameBoard.PlaceBlock(
                block, startRow, startCol);

            if (!placed)
                return;

            // Mängib õige heliefekti
            if (_gameBoard.LastClearedLines > 0)
            {
                // Rida või veerg eemaldati
                _ = _soundService.PlayClearAsync();
            }
            else
            {
                // Tavaline klotsi asetamine
                _ = _soundService.PlayPlaceAsync();
            }

            // Uuendab punktid
            Score = _gameBoard.Score;

            OnPropertyChanged(nameof(Combo));
            OnPropertyChanged(nameof(LastBonus));

            // Uuendab mängulauda ja värve
            OnPropertyChanged(nameof(Board));
            OnPropertyChanged(nameof(CellColors));

            // Salvestab parima tulemuse
            if (Score > BestScore)
            {
                BestScore = Score;

                Preferences.Default.Set(
                    "BlockBlastBestScore", BestScore);
            }

            // Eemaldab kasutatud klotsi
            AvailableBlocks.Remove(block);
            SelectedBlock = null;

            // Kui kõik kolm klotsi on kasutatud,
            // genereerib uued
            if (AvailableBlocks.Count == 0)
            {
                GenerateBlocks();
            }

            CheckGameOver();
        }

        // Kontrollib, kas mõni klots veel mahub
        private void CheckGameOver()
        {
            if (IsGameOver)
                return;

            foreach (Block block in AvailableBlocks)
            {
                if (_gameBoard.HasAvailableMove(block))
                    return;
            }

            IsGameOver = true;
        }

        // Paus / jätka
        private void TogglePause()
        {
            if (IsGameOver)
                return;

            IsPaused = !IsPaused;
        }

        private void OnPropertyChanged(
            [CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
        }
    }
}
