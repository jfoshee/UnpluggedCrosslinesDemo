if (typeof Unplugged === "undefined") Unplugged = {};

Unplugged.createScene = function () {
    return new THREE.Scene();
};

Unplugged.createCamera = function () {
    var camera = new THREE.PerspectiveCamera(45, window.innerWidth / window.innerHeight, 0.01, 100);
    camera.position.z = 1;
    return camera;
};

Unplugged.createControls = function (camera) {
    return new THREE.TrackballControls(camera, document);
};

Unplugged.createRenderer = function () {
    var renderer = new THREE.WebGLRenderer();
    renderer.setSize(window.innerWidth, window.innerHeight);
    document.body.appendChild(renderer.domElement);
    return renderer;
};

Unplugged.createRenderLoop = function (renderer, scene, controls) {
    var renderLoop = function () {
        controls.update();
        renderer.render(scene, controls.object);
        window.setTimeout(renderLoop, 1000 / 60);
        // TODO: Use RequestAnimationFrame
    };
    return renderLoop;
};

Unplugged.createDisplay = function (scene) {
    var camera = this.createCamera();
    var renderer = this.createRenderer();
    var controls = this.createControls(camera);
    return {
        scene: scene,
        camera: camera,
        controls: controls,
        renderer: renderer,
        renderLoop: this.createRenderLoop(renderer, scene, controls)
    };
}
