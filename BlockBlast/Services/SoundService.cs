
using System;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using Plugin.Maui.Audio;

namespace BlockBlast.Services
{
    public class SoundService
    {
        private readonly IAudioManager _audioManager;

        public SoundService()
        {
            _audioManager = AudioManager.Current;
        }

        private async Task PlayAsync(string fileName)
        {
            if (!Preferences.Default.Get("BlockBlastSound", true))
                return;

            try
            {
                var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
                var player = _audioManager.CreatePlayer(stream);

                player.PlaybackEnded += (sender, args) =>
                {
                    player.Dispose();
                    stream.Dispose();
                };

                player.Play();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"Heli mängimise viga: {ex.Message}");
            }
        }

        // Heli klotsi asetamisel
        public Task PlayPlaceAsync()
        {
            return PlayAsync("place.wav");
        }

        // Heli rea või veeru eemaldamisel
        public Task PlayClearAsync()
        {
            return PlayAsync("clear.wav");
        }
    }
}
