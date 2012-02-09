﻿using System.Web;
using System.Web.Mvc;
using Unplugged.Volume;
using Unplugged.Web.Controllers;

namespace BluwareDemo.Controllers
{
    public class ImageController : ImageControllerBase
    {
        public override FileResult Tile(int i, int j, int k)
        {
            var pathProviders = GetPathProviders();
            var path = pathProviders[k].GetTilePath(i, j);
            return new FilePathResult(path, "image");
        }

        private ITilePathProvider[] GetPathProviders()
        {
            var imageCachePath = HttpContext.Server.MapPath("~/ImageCache");
            var pathProviders = new ITilePathProvider[] 
            {
                new DefaultTilePathProvider
                {
                    ParentDirectory = imageCachePath,
                    Name = "Inline"
                },            
                new DefaultTilePathProvider
                {
                    ParentDirectory = imageCachePath,
                    Name = "Crossline"
                },
            };
            return pathProviders;
        }
    }
}
