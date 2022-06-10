using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.System.Threading;
using Windows.System;
using Windows.UI.Xaml.Core;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Media.Playback;
using Windows.Media.Core;
using System.Threading.Tasks;

namespace Dodgegame1
{
    public class MusicManager
    {
        MediaElement backgroundmusic = new MediaElement();
        MediaElement ExplosionSound = new MediaElement();
        MediaElement VictorySound = new MediaElement();
        MediaElement LosingSound = new MediaElement();
        MediaElement LostLifeSound = new MediaElement();
        public async Task<MediaElement> Playbackgroundmusic()
        {
            //creating background as a new media element
            var background = new MediaElement();
            //going to the folder and choosing the correct file
            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            var file = await folder.GetFileAsync("spacerace.mp3");
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            //playing the sound
            background.SetSource(stream, "");
            background.Play();
            backgroundmusic = background;
            return background;
        }
        public async Task<MediaElement> PlayExplosionSound()
        {
            var Explosion = new MediaElement();
            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            var file = await folder.GetFileAsync("Explosion.mp3");
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            Explosion.SetSource(stream, "");
            Explosion.Play();
            Explosion.Volume = 0.5;
            Explosion = ExplosionSound;
            return Explosion;
        }
        public async Task<MediaElement> PlayVictorySound()
        {
            var Victory = new MediaElement();
            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            var file = await folder.GetFileAsync("WinningSound.mp3");
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            Victory.SetSource(stream, "");
            Victory.Play();
            Victory = VictorySound;
            return Victory;
        }
        public async Task<MediaElement> PlayLosingSound()
        {
            var Lost = new MediaElement();
            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            var file = await folder.GetFileAsync("LoseGameSound.mp3");
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            Lost.SetSource(stream, "");
            Lost.Play();
            Lost = LosingSound;
            return Lost;
        }
        public async Task<MediaElement> PlayLostLife()
        {
            var LoseLife = new MediaElement();
            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            var file = await folder.GetFileAsync("PlayerHurt.wav");
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            LoseLife.SetSource(stream, "");
            LoseLife.Play();
            LoseLife = LostLifeSound;
            return LoseLife;
        }
        public void StopBackGroundMusic()
        {
            backgroundmusic.Stop();
        }
        public void StopExplosionSound()
        {
            ExplosionSound.Stop();
        }
        public void StopLosingSound()
        {
            LosingSound.Stop();
        }
        public void StopVictorySound()
        {
            VictorySound.Stop();
        }
    }
}
