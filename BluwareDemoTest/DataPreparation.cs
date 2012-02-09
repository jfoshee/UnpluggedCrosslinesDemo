using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unplugged.Segy;
using Unplugged.Volume;

namespace BluwareDemoTest
{
    [TestClass]
    public class DataPreparation
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Canary()
        {
        }

        [TestMethod, Ignore]
        public void PrepareImageData()
        {
            var reader = new SegyReader { InlineNumberLocation = 17, CrosslineNumberLocation = 13 };
            var segyPath = @"C:\Users\jfoshee\Desktop\RMOTC Data\RMOTC Seismic data set\3D_Seismic\filt_mig.sgy";
            var segy = reader.Read(segyPath);

            using (var inlineBitmap = GetBitmapForMiddle(segy, t => t.Header.InlineNumber))
                MakeTiles("Inline", inlineBitmap);
            using (var crosslineBitmap = GetBitmapForMiddle(segy, t => t.Header.CrosslineNumber))
                MakeTiles("Crossline", crosslineBitmap);
        }

        private void MakeTiles(string name, Bitmap image)
        {
            var tilePathProvider = new DefaultTilePathProvider
            {
                ParentDirectory = @"C:\Users\jfoshee\Documents\visual studio 2010\Projects\BluwareDemo\BluwareDemo\ImageCache",
                Name = name
            };
            var tileWriter = new TileWriter { TilePathProvider = tilePathProvider };
            var tileCount = tileWriter.Write(image, 64, 64);
            Console.WriteLine("Tile count: " + tileCount);
        }

        private Bitmap GetBitmapForMiddle(ISegyFile segy, Func<ITrace, int> numberSelector)
        {
            var imageWriter = new ImageWriter { SetNullValuesToTransparent = false };
            var numbers = segy.Traces.Select(numberSelector).Distinct();
            var index = numbers.Count() / 2;
            int middleNumber = numbers.ElementAt(index);
            Console.WriteLine("line num: " + middleNumber);
            Console.WriteLine("line index: " + index);
            var inlineTraces = segy.Traces.Where(t => numberSelector(t) == middleNumber).ToList();
            var bitmapPath = Path.GetTempFileName() + ".png";
            var bitmap = imageWriter.GetBitmap(inlineTraces);
            bitmap.Save(bitmapPath, ImageFormat.Png);
            TestContext.AddResultFile(bitmapPath);
            return bitmap;
        }
    }
}
