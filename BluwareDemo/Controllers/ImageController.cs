using System.Web.Mvc;
using Unplugged.Volume;
using Unplugged.Web.Controllers;

namespace BluwareDemo.Controllers
{
    public class ImageController : ImageControllerBase
    {
        private ITilePathProvider[] _pathProviders = new ITilePathProvider[] 
        {
            new DefaultTilePathProvider
            {
                ParentDirectory = @"C:\Users\jfoshee\Documents\visual studio 2010\Projects\BluwareDemo\BluwareDemo\ImageCache",
                Name = "Inline"
            },            
            new DefaultTilePathProvider
            {
                ParentDirectory = @"C:\Users\jfoshee\Documents\visual studio 2010\Projects\BluwareDemo\BluwareDemo\ImageCache",
                Name = "Crossline"
            },
        };

        public override FileResult Tile(int i, int j, int k)
        {
            var path = _pathProviders[k].GetTilePath(i, j);
            return new FilePathResult(path, "image");
        }
    }
}
