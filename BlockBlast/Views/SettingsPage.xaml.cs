
using System;
using BlockBlast.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace BlockBlast.Views
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            LoadSettings();
            UpdateLanguage();
        }

        // Laeb varem salvestatud seaded
        private void LoadSettings()
        {
            PlayerNameEntry.Text = Preferences.Default.Get(
                "BlockBlastPlayerName", "Player");

            SoundSwitch.IsToggled = Preferences.Default.Get(
                "BlockBlastSound", true);

            string theme = Preferences.Default.Get(
                "BlockBlastTheme", "Dark");

            ThemePicker.SelectedIndex = theme == "Light" ? 1 : 0;

            string language = LocalizationService.CurrentLanguage;

            LanguagePicker.SelectedIndex = language == "en" ? 1 : 0;
        }

        // Uuendab seadete lehe tekste
        private void UpdateLanguage()
        {
            bool english = LocalizationService.CurrentLanguage == "en";

            Title = LocalizationService.Get("Settings");

            SettingsTitleLabel.Text =
                LocalizationService.Get("Settings");

            SettingsSubtitleLabel.Text = english
                ? "Customize your game settings"
                : "Muuda mängu seadeid";

            PlayerNameLabel.Text =
                LocalizationService.Get("PlayerName");

            PlayerNameEntry.Placeholder = english
                ? "Enter player name"
                : "Sisesta mängija nimi";

            SoundLabel.Text =
                LocalizationService.Get("Sound");

            SoundDescriptionLabel.Text = english
                ? "Game sound effects"
                : "Mängu heliefektid";

            ThemeLabel.Text =
                LocalizationService.Get("Theme");

            ThemePicker.Title = english
                ? "Choose theme"
                : "Vali teema";

            LanguageLabel.Text =
                LocalizationService.Get("Language");

            LanguagePicker.Title = english
                ? "Choose language"
                : "Vali keel";

            SaveSettingsButton.Text =
                LocalizationService.Get("SaveSettings");

            BackButton.Text =
                LocalizationService.Get("BackToGame");
        }

        // Salvestab kõik seaded
        private async void SaveButton_Clicked(
            object sender,
            EventArgs e)
        {
            string playerName =
                PlayerNameEntry.Text?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(playerName))
            {
                bool english =
                    LocalizationService.CurrentLanguage == "en";

                await DisplayAlertAsync(
                    english ? "Error" : "Viga",
                    english
                        ? "Enter a player name!"
                        : "Sisesta mängija nimi!",
                    "OK");

                return;
            }

            // Mängija nimi
            Preferences.Default.Set(
                "BlockBlastPlayerName", playerName);

            // Heli
            Preferences.Default.Set(
                "BlockBlastSound", SoundSwitch.IsToggled);

            // Teema
            string theme = ThemePicker.SelectedIndex == 1
                ? "Light"
                : "Dark";

            Preferences.Default.Set(
                "BlockBlastTheme", theme);

            if (Application.Current != null)
            {
                Application.Current.UserAppTheme =
                    theme == "Light"
                        ? AppTheme.Light
                        : AppTheme.Dark;
            }

            // Keel
            string language = LanguagePicker.SelectedIndex == 1
                ? "en"
                : "et";

            LocalizationService.SetLanguage(language);

            // Uuendab seadete lehe tekste
            UpdateLanguage();

            bool isEnglish = language == "en";

            await DisplayAlertAsync(
                isEnglish ? "Settings" : "Seaded",
                isEnglish
                    ? "Settings saved successfully!"
                    : "Seaded edukalt salvestatud!",
                "OK");

            // Tagasi mängu
            await Navigation.PopAsync();
        }

        // Tagasi mängu ilma salvestamata
        private async void BackButton_Clicked(
            object sender,
            EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
