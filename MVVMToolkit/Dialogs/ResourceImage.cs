using System.Windows.Media.Imaging;

namespace PB.MVVMToolkit.Dialogs
{
    /// <summary>
    /// Gets an embedded image from the .res assembly based on a file name and extension
    /// </summary>
    internal static class ResourceImage
    {
        /// <summary>
        /// Gets an icon image from a resource assembly
        /// </summary>
        /// <param name="name">File name</param>
        /// <returns>Bitmap Image</returns>
        internal static BitmapImage GetIcon(string name)
        {
            //Create the resource reader stream
            var stream = ResourceAssembly.GetAssembly().GetManifestResourceStream(ResourceAssembly.GetNameSpace() + "Images." + name);
            var image = new BitmapImage();

            var assembly = ResourceAssembly.GetAssembly();
            var space = ResourceAssembly.GetNameSpace();
            var newstream = assembly.GetManifestResourceStream(space+"Images."+"dialog_error_icon.png");

            //If image cannot be found, return null
            try
            {
                //Construct and return image
                image.BeginInit();
                image.StreamSource = stream;
                image.EndInit();

                //return constructed BitmapImage
                return image;
            }
            catch
            {
                return null;
            }
        }

    }
}
