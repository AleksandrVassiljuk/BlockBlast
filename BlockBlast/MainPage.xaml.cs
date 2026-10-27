
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using BlockBlast.Models;
using BlockBlast.Services;
using BlockBlast.ViewModels;
using BlockBlast.Views;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Storage;

namespace BlockBlast
{
    public partial class MainPage : ContentPage
    {
        private readonly GameViewModel _viewModel;
        private readonly DatabaseService _databaseService;

        private const int BoardSize = 8;
        private bool _savingResult;

        public MainPage()
        {
            InitializeComponent();

            _viewModel = new GameViewModel();
            _databaseService = new DatabaseService();

            BindingContext = _viewModel;

            _viewModel.PropertyChanged += ViewModel_PropertyChanged;

            _viewModel.AvailableBlocks.CollectionChanged +=
                (sender, args) => DrawAvailableBlocks();

            DrawBoard();
            DrawAvailableBlocks();
            UpdateLanguage();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            UpdateLanguage();
            DrawBoard();
            DrawAvailableBlocks();
        }

        // Kontrollib, kas valitud on hele teema
        private bool IsLightTheme()
        {
            return Application.Current?.RequestedTheme == AppTheme.Light;
        }

        // Uuendab tõlked
        private void UpdateLanguage()
        {
            GameTitleLabel.Text = LocalizationService.Get("GameTitle");
            PuzzleGameLabel.Text = LocalizationService.Get("PuzzleGame");

            ScoreTitleLabel.Text = LocalizationService.Get("Score");
            BestTitleLabel.Text = LocalizationService.Get("Best");
            ComboTitleLabel.Text = LocalizationService.Get("Combo");
            BonusTitleLabel.Text = LocalizationService.Get("Bonus");

            SelectBlockLabel.Text = LocalizationService.Get("SelectBlock");

            NewGameButton.Text = LocalizationService.Get("NewGame");
            PauseButton.Text = LocalizationService.Get("PauseResume");
            RecordsButton.Text = LocalizationService.Get("Records");
            SettingsButton.Text = LocalizationService.Get("Settings");

            HowToPlayLabel.Text = LocalizationService.Get("HowToPlay");

            Step1Label.Text = "1. " + LocalizationService.Get("Step1");
            Step2Label.Text = "2. " + LocalizationService.Get("Step2");
            Step3Label.Text = "3. " + LocalizationService.Get("Step3");
            Step4Label.Text = "4. " + LocalizationService.Get("Step4");
            Step5Label.Text = "5. " + LocalizationService.Get("Step5");
        }

        // Mängu oleku muutused
        private void ViewModel_PropertyChanged(
            object? sender,
            PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GameViewModel.Board) ||
                e.PropertyName == nameof(GameViewModel.CellColors))
            {
                DrawBoard();
            }

            if (e.PropertyName == nameof(GameViewModel.SelectedBlock))
            {
                DrawAvailableBlocks();
            }

            if (e.PropertyName == nameof(GameViewModel.IsPaused) ||
                e.PropertyName == nameof(GameViewModel.IsGameOver))
            {
                UpdateBoardState();
            }

            if (e.PropertyName == nameof(GameViewModel.IsGameOver) &&
                _viewModel.IsGameOver)
            {
                _ = SaveGameResultAsync();
            }
        }

        // 8x8 mängulaud
        private void DrawBoard()
        {
            GameGrid.Children.Clear();
            GameGrid.RowDefinitions.Clear();
            GameGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < BoardSize; i++)
            {
                GameGrid.RowDefinitions.Add(new RowDefinition
                {
                    Height = GridLength.Star
                });

                GameGrid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = GridLength.Star
                });
            }

            double screenWidth =
                DeviceDisplay.MainDisplayInfo.Width /
                DeviceDisplay.MainDisplayInfo.Density;

            double cellSize = Math.Min(
                Math.Max((screenWidth - 70) / BoardSize, 20),
                42);

            bool light = IsLightTheme();

            Color emptyColor = light
                ? Color.FromArgb("#DCE5F2")
                : Color.FromArgb("#30405F");

            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    int selectedRow = row;
                    int selectedCol = col;

                    bool filled = _viewModel.Board[row, col] == 1;

                    Color cellColor = filled
                        ? _viewModel.CellColors[row, col]
                            ?? Colors.DodgerBlue
                        : emptyColor;

                    var cell = new Border
                    {
                        BackgroundColor = cellColor,
                        StrokeThickness = 0,
                        StrokeShape = new RoundRectangle
                        {
                            CornerRadius = new CornerRadius(5)
                        },
                        WidthRequest = cellSize,
                        HeightRequest = cellSize
                    };

                    var tap = new TapGestureRecognizer();

                    tap.Tapped += (sender, args) =>
                    {
                        if (_viewModel.IsPaused ||
                            _viewModel.IsGameOver ||
                            _viewModel.SelectedBlock == null)
                        {
                            return;
                        }

                        _viewModel.PlaceBlockCommand.Execute(
                            $"{selectedRow},{selectedCol}");
                    };

                    cell.GestureRecognizers.Add(tap);
                    GameGrid.Add(cell, col, row);
                }
            }

            UpdateBoardState();
        }

        // Paus ja mängu lõpp
        private void UpdateBoardState()
        {
            GameGrid.Opacity = _viewModel.IsPaused ? 0.45 : 1.0;

            GameGrid.InputTransparent =
                _viewModel.IsPaused || _viewModel.IsGameOver;
        }

        // Kolm valitavat klotsi
        private void DrawAvailableBlocks()
        {
            BlocksGrid.Children.Clear();

            var blocks = _viewModel.AvailableBlocks.ToList();
            bool light = IsLightTheme();

            for (int i = 0; i < blocks.Count; i++)
            {
                Block block = blocks[i];

                bool isSelected = _viewModel.SelectedBlock == block;

                Color background = isSelected
                    ? (light
                        ? Color.FromArgb("#D0E2FF")
                        : Color.FromArgb("#405A91"))
                    : (light
                        ? Colors.White
                        : Color.FromArgb("#1E2A47"));

                var container = new Border
                {
                    BackgroundColor = background,
                    Stroke = isSelected
                        ? Color.FromArgb("#377DFF")
                        : Colors.Transparent,
                    StrokeThickness = isSelected ? 3 : 1,
                    Padding = new Thickness(6),
                    StrokeShape = new RoundRectangle
                    {
                        CornerRadius = new CornerRadius(12)
                    },
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Fill
                };

                var shapeGrid = new Grid
                {
                    RowSpacing = 3,
                    ColumnSpacing = 3,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };

                int squareSize = block.Width >= 4 ? 15 : 19;

                for (int r = 0; r < block.Height; r++)
                {
                    shapeGrid.RowDefinitions.Add(new RowDefinition
                    {
                        Height = new GridLength(squareSize)
                    });
                }

                for (int c = 0; c < block.Width; c++)
                {
                    shapeGrid.ColumnDefinitions.Add(new ColumnDefinition
                    {
                        Width = new GridLength(squareSize)
                    });
                }

                for (int r = 0; r < block.Height; r++)
                {
                    for (int c = 0; c < block.Width; c++)
                    {
                        if (block.Shape[r, c] == 0)
                            continue;

                        var square = new Border
                        {
                            BackgroundColor = block.Color,
                            StrokeThickness = 0,
                            StrokeShape = new RoundRectangle
                            {
                                CornerRadius = new CornerRadius(4)
                            }
                        };

                        shapeGrid.Add(square, c, r);
                    }
                }

                container.Content = shapeGrid;

                var tap = new TapGestureRecognizer();

                tap.Tapped += (sender, args) =>
                {
                    if (_viewModel.IsPaused || _viewModel.IsGameOver)
                        return;

                    _viewModel.SelectBlockCommand.Execute(block);
                };

                container.GestureRecognizers.Add(tap);
                BlocksGrid.Add(container, i, 0);
            }
        }

        // Salvestab tulemuse SQLite andmebaasi
        private async Task SaveGameResultAsync()
        {
            if (_savingResult)
                return;

            _savingResult = true;

            int finalScore = _viewModel.Score;
            int bestScore = _viewModel.BestScore;

            try
            {
                var record = new GameRecord
                {
                    PlayerName = Preferences.Default.Get(
                        "BlockBlastPlayerName", "Player"),
                    Score = finalScore,
                    PlayedAt = DateTime.Now
                };

                await _databaseService.SaveRecordAsync(record);

                bool english = LocalizationService.CurrentLanguage == "en";

                string message = english
                    ? $"Game Over!\n\nYour score: {finalScore}\n" +
                      $"Best score: {bestScore}\n\nResult saved!"
                    : $"Mäng on läbi!\n\nSinu tulemus: {finalScore}\n" +
                      $"Parim tulemus: {bestScore}\n\nTulemus salvestati!";

                await DisplayAlertAsync("GAME OVER!", message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync(
                    "Salvestamise viga", ex.Message, "OK");
            }
            finally
            {
                _savingResult = false;
            }
        }

        private async void RecordsButton_Clicked(
            object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RecordsPage());
        }

        private async void SettingsButton_Clicked(
            object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }
    }
}
