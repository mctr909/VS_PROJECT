/// <reference path="math.js"/>
class RenderMsgId {
	static get DrawModel() { return 0 };
	static get BindModel() { return 1 };
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
	constructor(objCanvas, width, height) {
		/**
		 * @property WebGLコンテキスト 
		 * @type {WebGLRenderingContext}
		 */
		this.gl = null;

		/** @property メッセージキュー */
		this._message = new Array();

		/** @property canvasコンテキスト */
		this._canvas = null;
		/** @property 登録モデル */
		this._models = null;
		/** @property バインド中モデル */
		this._bindingModel = null;

		/** ビュー×プロジェクション */
		this._matViewProj = null;
		/** カメラ */
		this._cam = null;
		/** 光源 */
		this._light = null;

		// canvasを初期化
		this._canvas = objCanvas;
		this._canvas.width = width;
		this._canvas.height = height;

		// webglコンテキストを取得
		this.gl = this._canvas.getContext('webgl') || this._canvas.getContext('experimental-webgl');

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

		this._loadModel();
	}

	/** シェーダを生成します */
	_create_shader(id) {
		// シェーダを格納する変数
		let shader;

		// HTMLからscriptタグへの参照を取得
		let scriptElement = document.getElementById(id);

		// scriptタグが存在しない場合は抜ける
		if (!scriptElement) {
			return;
		}

		// scriptタグのtype属性をチェック
		switch (scriptElement.type) {
			// 頂点シェーダの場合
			case 'x-shader/x-vertex':
				shader = this.gl.createShader(this.gl.VERTEX_SHADER);
				break;

			// フラグメントシェーダの場合
			case 'x-shader/x-fragment':
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
		}
		else {
			// 失敗していたらエラーログをアラートする
			alert(this.gl.getShaderInfoLog(shader));
		}
	}

	/** プログラムオブジェクトを生成しシェーダをリンクします */
	_create_program(vs, fs) {
		// プログラムオブジェクトの生成
		let program = this.gl.createProgram();

		// プログラムオブジェクトにシェーダを割り当てる
		this.gl.attachShader(program, vs);
		this.gl.attachShader(program, fs);

		// シェーダをリンク
		this.gl.linkProgram(program);

		// シェーダのリンクが正しく行なわれたかチェック
		if (this.gl.getProgramParameter(program, this.gl.LINK_STATUS)) {
			// 成功していたらプログラムオブジェクトを有効にする
			this.gl.useProgram(program);

			// プログラムオブジェクトを返して終了
			return program;
		}
		else {
			// 失敗していたらエラーログをアラートする
			alert(this.gl.getProgramInfoLog(program));
		}
	}

	/** VBOを生成します */
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

	/** IBOを生成します */
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

	/** モデルデータ読み込み */
	_loadModel() {
		let torusData = torus(256, 256, 1.0, 2.0);
		let sphereData = sphere(256, 256, 1.0);

		this._models = [
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

	/** モデルデータのバインドをします */
	_bindModel(modelIndex) {
		this._bindingModel = this._models[modelIndex];

		// VBOをバインドする
		this.mAttr.pos.bindBuffer(this._bindingModel.position);
		this.mAttr.nor.bindBuffer(this._bindingModel.normal);
		this.mAttr.col.bindBuffer(this._bindingModel.color);

		// IBOをバインドする
		this.gl.bindBuffer(this.gl.ELEMENT_ARRAY_BUFFER, this._bindingModel.index);
	}

	/** モデルを描画します */
	_drawModel(matModel) {
		// モデル×ビュー×プロジェクション
		let matMVP = new Mat();
		Mat.multiply(this._matViewProj, matModel, matMVP);

		// モデル逆行列
		let matInv = new Mat();
		Mat.inverse(matModel, matInv);

		// uniformへ座標変換行列を登録し描画する
		this.gl.uniformMatrix4fv(this.mUniLoc.matMVP, false, matMVP.Array);
		this.gl.uniformMatrix4fv(this.mUniLoc.matModel, false, matModel.Array);
		this.gl.uniformMatrix4fv(this.mUniLoc.matInvModel, false, matInv.Array);
		this.gl.uniform3fv(this.mUniLoc.eyeDirection, this._cam.position);
		this.gl.uniform3fv(this.mUniLoc.lightDirection, this._light.direction);
		this.gl.uniform4fv(this.mUniLoc.ambientColor, this._light.ambientColor);
		this.gl.drawElements(this.gl.TRIANGLES, this._bindingModel.indexCount, this.gl.UNSIGNED_SHORT, 0);
	}

	/** 
	 * @param {RenderMsgId}id
	 * @param {any}value
	 */
	pushMessage(id, value) {
		this._message.push({id:id, value:value});
	}

	/** */
	main() {
		// canvasを初期化
		this.gl.clearColor(0.0, 0.0, 0.0, 1.0);
		this.gl.clearDepth(1.0);
		this.gl.clear(this.gl.COLOR_BUFFER_BIT | this.gl.DEPTH_BUFFER_BIT);

		// カメラの位置カメラの向き
		this._cam = {
			position: [0.0, 0.0, 80.0],
			upDirection: [0.0, 1.0, 0.0]
		};

		// 光源の向き, 環境光の色
		this._light = {
			direction: [1.0, 1.0, 1.0],
			ambientColor: [0.1, 0.1, 0.1, 1.0]
		};

		// ビュー×プロジェクション座標変換行列
		let matView = new Mat();
		let matProj = new Mat();
		this._matViewProj = new Mat();
		matView.lookAt(this._cam.position, [0, 0, 0], this._cam.upDirection);
		matProj.perspective(45, 0.1, 200, this._canvas.width / this._canvas.height);
		Mat.multiply(matProj, matView, this._matViewProj);

		// メッセージキューの実行
		while(0 < this._message.length) {
			let msg = this._message.shift();
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
