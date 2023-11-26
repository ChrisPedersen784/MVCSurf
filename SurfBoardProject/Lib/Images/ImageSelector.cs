using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Images
{
    public class ImageSelector
    {

        public async Task<List<string>> imagesAsync()
        {
            List<string> ImageList = new List<string>();

            ImageList.Add("/Funboard.jpg");
            ImageList.Add("/Longboard.jpg");
            ImageList.Add("/Shortboard.jpg");

            return ImageList;
        }
    }
}
