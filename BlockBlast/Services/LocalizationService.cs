
using System;
using System.Globalization;
using System.Resources;
using Microsoft.Maui.Storage;

namespace BlockBlast.Services
{
    public static class LocalizationService
    {
        private static readonly ResourceManager Resources =
            new ResourceManager(
                "BlockBlast.Resources.Localization.AppResources",
                typeof(LocalizationService).Assembly);

        public static string CurrentLanguage =>
            Preferences.Default.Get("BlockBlastLanguage", "et");

        public static string Get(string key)
        {
            var culture = new CultureInfo(CurrentLanguage);

            return Resources.GetString(key, culture) ?? key;
        }

        public static void SetLanguage(string language)
        {
            if (language != "et" && language != "en")
                return;

            Preferences.Default.Set("BlockBlastLanguage", language);

            var culture = new CultureInfo(language);

            CultureInfo.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }
    }
}
