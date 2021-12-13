using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace sos_solulutio.ViewModels.logic
{
    public class AppFunctions
    {
        public async Task<MediaFile> TakePicture()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsPickPhotoSupported)
            {
                return null;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions { SaveToAlbum = true });

            return file;
        }

        public async Task<MediaFile> BrowsePicture()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                return null;
            }

            var file = await CrossMedia.Current.PickPhotoAsync();

            return file;
        }

        public string Summerize(string text)
        {
            if (text.Length < 50)
                return text;

            return text.Substring(0, 50) + "...";
        }
    }
}
