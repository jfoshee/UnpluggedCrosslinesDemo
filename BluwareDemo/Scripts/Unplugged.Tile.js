if (typeof Unplugged === "undefined") Unplugged = {};
Unplugged.Tile = {};

Unplugged.Tile.loadTexture = function (index) {
    var path = index;
    if (index.hasOwnProperty("i") && index.hasOwnProperty("j") && index.hasOwnProperty("k")) {
        path = "/Image/Tile?i=" + index.i + "&j=" + index.j + "&k=" + index.k;
    }
    else if (!isNaN(index)) {
        path = "/Image/Slice?index=" + index;
    }
    return THREE.ImageUtils.loadTexture(path);
};

Unplugged.Tile.loadMaterial = function (i) {
    var textureMap = Unplugged.Tile.loadTexture(i);
    var material = new THREE.MeshBasicMaterial({ color: 0xFFFFFF, map: textureMap });
    return material;
};

Unplugged.Tile.loadMesh = function (i) {
    var material = Unplugged.Tile.loadMaterial(i);
    var geometry = new THREE.PlaneGeometry(1, 1);
    var mesh = new THREE.Mesh(geometry, material);
    mesh.doubleSided = true;
    return mesh;
};

Unplugged.Tile.loadScene = function (i) {
    var mesh = Unplugged.Tile.loadMesh(i);
    var scene = Unplugged.createScene();
    scene.add(mesh);
    return scene;
};

Unplugged.Tile.createDisplay = function (index) {
    var scene = this.loadScene(index);
    return Unplugged.createDisplay(scene);
};

Unplugged.Tile.render = function (index) {
    var display = this.createDisplay(index);
    display.renderLoop();
};
