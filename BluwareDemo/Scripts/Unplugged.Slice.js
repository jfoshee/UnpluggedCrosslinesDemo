if (typeof Unplugged === "undefined") Unplugged = {};
Unplugged.Slice = {}

Unplugged.Slice._getTileImage = function (tile) {
    if (tile.hasOwnProperty("materials"))
        return tile.materials[0].map.image;
    else
        return { width: 1, height: 1 };
}

Unplugged.Slice._getTileSize = function (tile) {
    var image = this._getTileImage(tile);
    return { w: image.width, h: image.height };
}

Unplugged.Slice._setTileScale = function (tile, firstTile) {
    if (!tile.hasOwnProperty("scale")) return;
    if (firstTile == null) return;
    var first = this._getTileSize(firstTile);
    var size = this._getTileSize(tile);
    tile.scale.x = size.w / first.w;
    tile.scale.y = -size.h / first.h;   // Flip vertically
    tile.position.x -= (1 - tile.scale.x) / 2;
    tile.position.y -= (1 + tile.scale.y) / 2;
}

Unplugged.Slice._isImageLoaded = function (tile) {
    return this._getTileSize(tile).w !== 0;
}

Unplugged.Slice._setTileScaleWhenImageLoaded = function(tile, firstTile) {
    var tileImage = this._getTileImage(tile);
    var tempOnLoad = tileImage.onload;
    tileImage.onload =  function () {
        Unplugged.Slice._setTileScale(tile, firstTile);
        if (tempOnLoad) tempOnLoad();
    }
}

Unplugged.Slice._setTilePosition = function (tile, i, j) {
    if (!tile.hasOwnProperty("position")) return;
    tile.position.x = i;
    tile.position.y = j;
    tile.scale.y = -1;
}

Unplugged.Slice.loadScene = function (iTiles, jTiles, k) {
    var scene = Unplugged.createScene();
    var tile, firstTile;
    for (var i = 0; i !== iTiles; i += 1) {
        for (var j = 0; j !== jTiles; j += 1) {
            tile = Unplugged.Tile.loadMesh({ i: i, j: j, k: k });
            if (i === 0 && j === 0)
                firstTile = tile;
            this._setTilePosition(tile, i, j);
            if (this._isImageLoaded(tile)) {
                this._setTileScale(tile, firstTile);
            }
            else {
                this._setTileScaleWhenImageLoaded(tile, firstTile);
            }
            scene.add(tile);
        }
    }
    return scene;
}

Unplugged.Slice.createDisplay = function (iTiles, jTiles, k) {
    var scene = this.loadScene(iTiles, jTiles, k);
    return Unplugged.createDisplay(scene);
}

Unplugged.Slice.render = function (iTiles, jTiles, kSlice) {
    var display = this.createDisplay(iTiles, jTiles, kSlice);
    display.renderLoop();
}
