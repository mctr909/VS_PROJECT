/// <reference path="math.js"/>
class RenderMsgId {
	static DrawModel = 0;
	static BindModel = 1;
}

class ShaderAttribute {
	/**
	 * @param {WebGLRenderingContext} gl 
	 * @param {WebGLProgram} program 
	 * @param {number} type 
	 * @param {number} size 
	 * @param {string} name 
	 */
	constructor(gl, program, type, size, name) {
		this._gl = gl;
		this._location = gl.getAttribLocation(program, name);
		this._type = type;
		this._size = size;
		this._name = name;
	}
	get Name() { return this._name; }
	/**
	 * バッファをバインドする
	 * @param {WebGLBuffer} vbo
	 */
	bindBuffer(vbo) {
		// バッファをバインドする
		this._gl.bindBuffer(this._gl.ARRAY_BUFFER, vbo);
		// attributeLocationを有効にする
		this._gl.enableVertexAttribArray(this._location);
		// attributeLocationを通知し登録する
		this._gl.vertexAttribPointer(this._location, this._size, this._type, false, 0, 0);
	}
}

class Render {
	/**
	 * @param {HTMLCanvasElement} objCanvas 
	 * @param {number} width 
	 * @param {number} height 
	 */
	constructor(objCanvas, width, height) {
		/**
		 * canvas
		 * @type {HTMLCanvasElement}
		 */
		this.mCanvas = objCanvas;
		this.mCanvas.width = width;
		this.mCanvas.height = height;
		/**
		 * WebGLコンテキスト 
		 * @type {WebGLRenderingContext}
		 */
		this.gl = this.mCanvas.getContext('webgl') || this.mCanvas.getContext('experimental-webgl');
		/** メッセージキュー */
		this.mMessage = new Array();
		/** 登録モデル */
		this.mModels = null;
		/** バインド中モデル */
		this.mBindingModel = null;
		/** ビュー */
		this.mMatView = new Mat();
		/** プロジェクション */
		this.mMatProj = new Mat();
		/** ビュー×プロジェクション */
		this.mMatViewProj = new Mat();
		/** カメラ */
		this.mCam = null;
		/** 光源 */
		this.mLight = null;
		/** attribute */
		this.mAttr = null;
		/** uniformLocation */
		this.mUniLoc = null;
	
		this._initialize();
		this._loadModel();
	}

	/**
	 * 初期化
	 */
	_initialize() {
		this.gl.enable(this.gl.DEPTH_TEST);	// 深度テスト有効化
		this.gl.depthFunc(this.gl.LEQUAL);	// 深度テスト(手前側を表示)
		this.gl.enable(this.gl.CULL_FACE);	// カリング有効化

		// 頂点シェーダとフラグメントシェーダの生成
		// プログラムオブジェクトの生成とリンク
		let v_shader = this._create_shader('vs');
		let f_shader = this._create_shader('fs');
		let prg = this._create_program(v_shader, f_shader);

		// attributeを取得
		this.mAttr = {
			pos: new ShaderAttribute(this.gl, prg, this.gl.FLOAT, 3, "position"),
			nor: new ShaderAttribute(this.gl, prg, this.gl.FLOAT, 3, "normal"),
			col: new ShaderAttribute(this.gl, prg, this.gl.FLOAT, 4, "color")
		};

		// uniformLocationの取得
		this.mUniLoc = {
			matMVP: this.gl.getUniformLocation(prg, 'mvpMatrix'),
			matModel: this.gl.getUniformLocation(prg, 'mMatrix'),
			matInvModel: this.gl.getUniformLocation(prg, 'invMatrix'),
			lightDirection: this.gl.getUniformLocation(prg, 'lightDirection'),
			eyeDirection: this.gl.getUniformLocation(prg, 'eyeDirection'),
			ambientColor: this.gl.getUniformLocation(prg, 'ambientColor')
		};
	}

	/**
	 * シェーダを生成
	 * @param {string} shaderElementId
	 * @returns {WebGLShader}
	 */
	_create_shader(shaderElementId) {
		// HTMLからscriptタグへの参照を取得
		let scriptElement = document.getElementById(shaderElementId);
		if (!scriptElement) {
			// scriptタグが存在しない場合は抜ける
			return null;
		}

		// scriptタグのtype属性をチェックして格納
		let shader;
		switch (scriptElement.type) {
			case 'x-shader/x-vertex':
				// 頂点シェーダの場合
				shader = this.gl.createShader(this.gl.VERTEX_SHADER);
				break;
			case 'x-shader/x-fragment':
				// フラグメントシェーダの場合
				shader = this.gl.createShader(this.gl.FRAGMENT_SHADER);
				break;
			default:
				return;
		}

		// 生成されたシェーダにソースを割り当てる
		this.gl.shaderSource(shader, scriptElement.text);

		// シェーダをコンパイルする
		this.gl.compileShader(shader);

		// シェーダが正しくコンパイルされたかチェック
		if (this.gl.getShaderParameter(shader, this.gl.COMPILE_STATUS)) {
			// 成功していたらシェーダを返して終了
			return shader;
		} else {
			// 失敗していたらエラーログをアラートする
			alert(this.gl.getShaderInfoLog(shader));
			return null;
		}
	}

	/**
	 * プログラムオブジェクトを生成しシェーダをリンク
	 * @param  {...WebGLShader} shaders
	 * @returns {WebGLProgram}
	 */
	_create_program(...shaders) {
		// プログラムオブジェクトの生成
		let program = this.gl.createProgram();

		// プログラムオブジェクトにシェーダを割り当てる
		for (let i in shaders) {
			this.gl.attachShader(program, shaders[i]);
		}

		// シェーダをリンク
		this.gl.linkProgram(program);

		// シェーダのリンクが正しく行なわれたかチェック
		if (this.gl.getProgramParameter(program, this.gl.LINK_STATUS)) {
			// 成功していたらプログラムオブジェクトを有効にする
			this.gl.useProgram(program);
			// プログラムオブジェクトを返して終了
			return program;
		} else {
			// 失敗していたらエラーログをアラートする
			alert(this.gl.getProgramInfoLog(program));
			return null;
		}
	}

	/**
	 * VBOを生成
	 * @param {number[]} data 
	 */
	_create_vbo(data) {
		// バッファオブジェクトの生成
		let vbo = this.gl.createBuffer();
		// バッファをバインドする
		this.gl.bindBuffer(this.gl.ARRAY_BUFFER, vbo);
		// バッファにデータをセット
		this.gl.bufferData(this.gl.ARRAY_BUFFER, new Float32Array(data), this.gl.STATIC_DRAW);
		// バッファのバインドを無効化
		this.gl.bindBuffer(this.gl.ARRAY_BUFFER, null);
		// 生成した VBO を返して終了
		return vbo;
	}

	/**
	 * IBOを生成
	 * @param {number[]} data 
	 */
	_create_ibo(data) {
		// バッファオブジェクトの生成
		let ibo = this.gl.createBuffer();
		// バッファをバインドする
		this.gl.bindBuffer(this.gl.ELEMENT_ARRAY_BUFFER, ibo);
		// バッファにデータをセット
		this.gl.bufferData(this.gl.ELEMENT_ARRAY_BUFFER, new Int16Array(data), this.gl.STATIC_DRAW);
		// バッファのバインドを無効化
		this.gl.bindBuffer(this.gl.ELEMENT_ARRAY_BUFFER, null);
		// 生成したIBOを返して終了
		return ibo;
	}

	/**
	 * モデルデータ読み込み 
	 */
	_loadModel() {
		let torusData = torus(256, 256, 1.0, 2.0);
		let sphereData = sphere(256, 256, 1.0);
		this.mModels = [
			{
				name: "torus",
				position: this._create_vbo(torusData[0]),
				normal: this._create_vbo(torusData[1]),
				color: this._create_vbo(torusData[2]),
				index: this._create_ibo(torusData[3]),
				indexCount: torusData[3].length
			},
			{
				name: "sphere",
				position: this._create_vbo(sphereData[0]),
				normal: this._create_vbo(sphereData[1]),
				color: this._create_vbo(sphereData[2]),
				index: this._create_ibo(sphereData[4]),
				indexCount: sphereData[4].length
			}
		];
	}

	/**
	 * モデルデータのバインド
	 * @param {number} modelIndex 
	 */
	_bindModel(modelIndex) {
		this.mBindingModel = this.mModels[modelIndex];
		// VBOをバインドする
		this.mAttr.pos.bindBuffer(this.mBindingModel.position);
		this.mAttr.nor.bindBuffer(this.mBindingModel.normal);
		this.mAttr.col.bindBuffer(this.mBindingModel.color);
		// IBOをバインドする
		this.gl.bindBuffer(this.gl.ELEMENT_ARRAY_BUFFER, this.mBindingModel.index);
	}

	/**
	 * モデルを描画
	 * @param {Mat} matModel 
	 */
	_drawModel(matModel) {
		// モデル×ビュー×プロジェクション
		let matMVP = new Mat();
		Mat.multiply(this.mMatViewProj, matModel, matMVP);
		// モデル逆行列
		let matInv = new Mat();
		Mat.inverse(matModel, matInv);
		// uniformへ座標変換行列を登録し描画する
		this.gl.uniformMatrix4fv(this.mUniLoc.matMVP, false, matMVP.Array);
		this.gl.uniformMatrix4fv(this.mUniLoc.matModel, false, matModel.Array);
		this.gl.uniformMatrix4fv(this.mUniLoc.matInvModel, false, matInv.Array);
		this.gl.uniform3fv(this.mUniLoc.eyeDirection, this.mCam.position);
		this.gl.uniform3fv(this.mUniLoc.lightDirection, this.mLight.direction);
		this.gl.uniform4fv(this.mUniLoc.ambientColor, this.mLight.ambientColor);
		this.gl.drawElements(this.gl.TRIANGLES, this.mBindingModel.indexCount, this.gl.UNSIGNED_SHORT, 0);
	}

	/** 
	 * @param {RenderMsgId}id
	 * @param {any}value
	 */
	pushMessage(id, value) {
		this.mMessage.push({id:id, value:value});
	}

	/** */
	main() {
		// canvasを初期化
		this.gl.clearColor(0.0, 0.0, 0.0, 1.0);
		this.gl.clearDepth(1.0);
		this.gl.clear(this.gl.COLOR_BUFFER_BIT | this.gl.DEPTH_BUFFER_BIT);

		// カメラの位置カメラの向き
		this.mCam = {
			position: [0.0, 0.0, 80.0],
			upDirection: [0.0, 1.0, 0.0]
		};

		// 光源の向き, 環境光の色
		this.mLight = {
			direction: [1.0, 1.0, 1.0],
			ambientColor: [0.1, 0.1, 0.1, 1.0]
		};

		// ビュー×プロジェクション座標変換行列
		this.mMatView.lookAt(this.mCam.position, [0, 0, 0], this.mCam.upDirection);
		this.mMatProj.perspective(45, 0.1, 200, this.mCanvas.width / this.mCanvas.height);
		Mat.multiply(this.mMatProj, this.mMatView, this.mMatViewProj);

		// メッセージキューの実行
		while (0 < this.mMessage.length) {
			let msg = this.mMessage.shift();
			switch (msg.id) {
			case RenderMsgId.BindModel:
				this._bindModel(msg.value);
				break;
			case RenderMsgId.DrawModel:
				this._drawModel(msg.value);
				break;
			}
		}

		// コンテキストの再描画
		this.gl.flush();
	}
}

function hsva(h, s, v, a) {
	if(s > 1 || v > 1 || a > 1){return;}
	var th = h % 360;
	var i = Math.floor(th / 60);
	var f = th / 60 - i;
	var m = v * (1 - s);
	var n = v * (1 - s * f);
	var k = v * (1 - s * (1 - f));
	var color = new Array();
	if(!s > 0 && !s < 0){
		color.push(v, v, v, a); 
	} else {
		var r = new Array(v, n, m, m, k, v);
		var g = new Array(k, v, v, n, m, m);
		var b = new Array(m, m, k, v, v, n);
		color.push(r[i], g[i], b[i], a);
	}
	return color;
}

function torus(row, column, irad, orad){
	let pos = new Array();
	let nor = new Array();
	let col = new Array();
	let idx = new Array();

	for(var i = 0; i <= row; i++){
		var r = Math.PI * 2 / row * i;
		var rr = Math.cos(r);
		var ry = Math.sin(r);
		for(var ii = 0; ii <= column; ii++){
			var tr = Math.PI * 2 / column * ii;
			var tx = (rr * irad + orad) * Math.cos(tr);
			var ty = ry * irad;
			var tz = (rr * irad + orad) * Math.sin(tr);
			var rx = rr * Math.cos(tr);
			var rz = rr * Math.sin(tr);
			pos.push(tx, ty, tz);
			nor.push(rx, ry, rz);
			var tc = hsva(360 / column * ii, 1, 1, 1);
			col.push(tc[0], tc[1], tc[2], tc[3]);
		}
	}
	for(i = 0; i < row; i++){
		for(ii = 0; ii < column; ii++){
			r = (column + 1) * i + ii;
			idx.push(r, r + column + 1, r + 1);
			idx.push(r + column + 1, r + column + 2, r + 1);
		}
	}
	return [pos, nor, col, idx];
}

function sphere(row, column, rad, color) {
	var i, j, tc;
	var pos = new Array(), nor = new Array(),
		col = new Array(), st  = new Array(), idx = new Array();
	for(i = 0; i <= row; i++){
		var r = Math.PI / row * i;
		var ry = Math.cos(r);
		var rr = Math.sin(r);
		for(j = 0; j <= column; j++){
			var tr = Math.PI * 2 / column * j;
			var tx = rr * rad * Math.cos(tr);
			var ty = ry * rad;
			var tz = rr * rad * Math.sin(tr);
			var rx = rr * Math.cos(tr);
			var rz = rr * Math.sin(tr);
			if(color){
				tc = color;
			}else{
				tc = hsva(360 / row * i, 1, 1, 1);
			}
			pos.push(tx, ty, tz);
			nor.push(rx, ry, rz);
			col.push(tc[0], tc[1], tc[2], tc[3]);
			st.push(1 - 1 / column * j, 1 / row * i);
		}
	}
	r = 0;
	for(i = 0; i < row; i++){
		for(j = 0; j < column; j++){
			r = (column + 1) * i + j;
			idx.push(r, r + 1, r + column + 2);
			idx.push(r, r + column + 2, r + column + 1);
		}
	}
	return [pos, nor, col, st, idx];
}
