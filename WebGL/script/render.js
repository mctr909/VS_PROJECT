/// <reference path="render_base.js"/>

class WebGLType {
    static INT = WebGLRenderingContext.INT;
    static FLOAT = WebGLRenderingContext.FLOAT;
    static VEC2 = WebGLRenderingContext.FLOAT_VEC2;
    static VEC3 = WebGLRenderingContext.FLOAT_VEC3;
    static MAT4 = WebGLRenderingContext.FLOAT_MAT4;
}

class ShaderAttribute {
    /**
     * @param {WebGLRenderingContext} gl
     * @param {WebGLProgram} program
     * @param {WebGLBuffer} vbo
     * @param {string} name
     * @param {number} type
     * @param {number} size
     */
    constructor(gl, program, vbo, name, type, size) {
        this._gl = gl;
        this._vbo = vbo;
        this._name = name;
        this._type = type;
        this._size = size;
        this._location = gl.getAttribLocation(program, name);
    }
    get VBO() { return this._vbo; }
    get Name() { return this._name; }
    get Type() { return this._type; }
    get Size() { return this._size; }
    get Location() { return this._location; }
    /**
     * VBOをバインドして登録
     */
    setVBO() {
		// バッファをバインドする
		this._gl.bindBuffer(this._gl.ARRAY_BUFFER, this._vbo);
		// attributeLocationを有効にする
		this._gl.enableVertexAttribArray(this._location);
		// attributeLocationを通知し登録する
		this._gl.vertexAttribPointer(
            this._location, this._size, this._type, false, 0, 0);
    }
}

class Render extends RenderBase {
    /**
     * @param {HTMLCanvasElement} canvasObj 
     */
    constructor(canvasObj) {
        super(canvasObj);
    }

    /**
     * @override
     * @returns {string}
     */
    get Version() { return "WebGL 2.0"; }

    /**
     * @protected @override
     * @param {any} sender
     * @param {string} id
     * @param {string} instanceId
     */
    _modelLoad(sender, id, instanceId) {

    }

    /**
     * @protected @override
     * @param {any} sender
     * @param {string} id
     * @param {string} instanceId
     */
    _modelFetch(sender, id, instanceId) {

    }

    /**
     * @protected @override
     * @param {any} sender
     * @param {string} id
     * @param {string} instanceId
     */
    _modelPurge(sender, id, instanceId) {

    }

    /**
     * @protected @override
     * @param {any} sender
     */
    _modelPurgeAll(sender) {

    }

    /**
     * @protected @override
     * @param {any} sender
     * @param {number} alpha
     */
    _modelVisible(sender, alpha) {

    }

    /**
     * @protected @override
     * @param {any} sender
     * @param {Posture} posture
     */
    _modelPosture(sender, posture) {

    }

    /**
     * @protected @override
     * @param {any} sender
     * @param {Point} position
     */
    _modelPosition(sender, position) {

    }

    /**
     * @protected @override
     * @param {any} sender
     * @param {BoneInfo[]} boneArray
     */
    _modelBone(sender, boneArray) {

    }

    /** @protected @override */
    _initialize() {

    }

    /** @protected @override */
    _rendering() {
    }
}

