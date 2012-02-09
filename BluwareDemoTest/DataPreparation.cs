using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unplugged.Segy;

namespace BluwareDemoTest
{
    [TestClass]
    public class DataPreparation
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void PrepareImageData()
        {
            var reader = new SegyReader { InlineNumberLocation = 17, CrosslineNumberLocation = 13 };
            var segyPath = @"C:\Users\jfoshee\Desktop\RMOTC Data\RMOTC Seismic data set\3D_Seismic\filt_mig.sgy";
            var segy = reader.Read(segyPath);

            var inlineBitmap = GetBitmapForMiddle(segy, t => t.Header.InlineNumber);
            inlineBitmap.Dispose();
            var crosslineBitmap = GetBitmapForMiddle(segy, t => t.Header.CrosslineNumber);
            crosslineBitmap.Dispose();
        }

        private Bitmap GetBitmapForMiddle(ISegyFile segy, Func<ITrace, int> numberSelector)
        {
            var imageWriter = new ImageWriter();
            var numbers = segy.Traces.Select(numberSelector).Distinct();
            int middleNumber = numbers.ElementAt(numbers.Count() / 2);
            Console.WriteLine("line num: " + middleNumber);
            var inlineTraces = segy.Traces.Where(t => numberSelector(t) == middleNumber).ToList();
            var bitmapPath = Path.GetTempFileName() + ".png";
            var bitmap = imageWriter.GetBitmap(inlineTraces);
            bitmap.Save(bitmapPath, ImageFormat.Png);
            TestContext.AddResultFile(bitmapPath);
            return bitmap;
        }
    }
}
