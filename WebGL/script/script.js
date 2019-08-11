/// <reference path="math.js"/>
// sample_021
//
// WebGLでクォータニオンによる球面線形補間

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
	 * バッファをバインド
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
	 * @param {HTMLCanvasElement} cnv 
	 */
	constructor(cnv) {
		cnv.width = 500;
		cnv.height = 300;
		this.mWidth = cnv.width;
		this.mHeight = cnv.height;
		this.mGL = cnv.getContext('webgl') || cnv.getContext('experimental-webgl');
		this.mProgram = null;
		this.mAttrPos = null;
		this.mAttrNor = null;
		this.mAttrCol = null;
	}

	/**
	 * attributeを作成
	 */
	create_attribute() {
		this.mAttrPos = new ShaderAttribute(this.mGL, this.mProgram, this.mGL.FLOAT, 3, "position");
		this.mAttrNor = new ShaderAttribute(this.mGL, this.mProgram, this.mGL.FLOAT, 3, "normal");
		this.mAttrCol = new ShaderAttribute(this.mGL, this.mProgram, this.mGL.FLOAT, 4, "color");
	}

	/**
	 * シェーダを生成する
	 * @param {string} shaderElementId
	 * @return {WebGLShader}
	 */
	create_shader(shaderElementId) {
		// HTMLからscriptタグへの参照を取得
		let scriptElement = document.getElementById(shaderElementId);
		if (!scriptElement) {
			// scriptタグが存在しない場合は抜ける
			return;
		}

		// scriptタグのtype属性をチェックして格納
		let shader;
		switch (scriptElement.type) {
			// 頂点シェーダの場合
			case 'x-shader/x-vertex':
				shader = this.mGL.createShader(this.mGL.VERTEX_SHADER);
				break;
			// フラグメントシェーダの場合
			case 'x-shader/x-fragment':
				shader = this.mGL.createShader(this.mGL.FRAGMENT_SHADER);
				break;
			default:
				return;
		}

		// 生成されたシェーダにソースを割り当てる
		this.mGL.shaderSource(shader, scriptElement.text);

		// シェーダをコンパイルする
		this.mGL.compileShader(shader);

		// シェーダが正しくコンパイルされたかチェック
		if (this.mGL.getShaderParameter(shader, this.mGL.COMPILE_STATUS)) {
			// 成功していたらシェーダを返して終了
			return shader;
		} else {
			// 失敗していたらエラーログをアラートする
			alert(this.mGL.getShaderInfoLog(shader));
			return null;
		}
	}

	/**
	 * プログラムオブジェクトを生成しシェーダをリンクする
	 * @param  {...WebGLShader} shaders
	 */
	create_program(...shaders) {
		// プログラムオブジェクトの生成
		let program = this.mGL.createProgram();

		for (let i in shaders) {
			// プログラムオブジェクトにシェーダを割り当てる
			this.mGL.attachShader(program, shaders[i]);
		}

		// シェーダをリンク
		this.mGL.linkProgram(program);

		// シェーダのリンクが正しく行なわれたかチェック
		if (this.mGL.getProgramParameter(program, this.mGL.LINK_STATUS)) {
			// 成功していたらプログラムオブジェクトを有効にする
			this.mGL.useProgram(program);
			// プログラムオブジェクトをセット
			this.mProgram = program;
			// attributeを作成
			this.create_attribute();
		} else {
			// 失敗していたらエラーログをアラートする
			alert(this.mGL.getProgramInfoLog(program));
		}
	}

	/**
	 * VBOを生成する
	 * @param {number[]} data
	 * @returns {WebGLBuffer}
	 */
	create_vbo(data) {
		// バッファオブジェクトの生成
		let vbo = this.mGL.createBuffer();
		// バッファをバインドする
		this.mGL.bindBuffer(this.mGL.ARRAY_BUFFER, vbo);
		// バッファにデータをセット
		this.mGL.bufferData(this.mGL.ARRAY_BUFFER, new Float32Array(data), this.mGL.STATIC_DRAW);
		// バッファのバインドを無効化
		this.mGL.bindBuffer(this.mGL.ARRAY_BUFFER, null);
		// 生成した VBO を返して終了
		return vbo;
	}

	/**
	 * IBOを生成する
	 * @param {number[]} data
	 * @returns {WebGLBuffer}
	 */
	create_ibo(data) {
		// バッファオブジェクトの生成
		let ibo = this.mGL.createBuffer();
		// バッファをバインドする
		this.mGL.bindBuffer(this.mGL.ELEMENT_ARRAY_BUFFER, ibo);
		// バッファにデータをセット
		this.mGL.bufferData(this.mGL.ELEMENT_ARRAY_BUFFER, new Int16Array(data), this.mGL.STATIC_DRAW);
		// バッファのバインドを無効化
		this.mGL.bindBuffer(this.mGL.ELEMENT_ARRAY_BUFFER, null);
		// 生成したIBOを返して終了
		return ibo;
	}
}

onload = function() {
	// canvasエレメントを取得
	let render = new Render(document.getElementById('canvas'));

	// input range エレメント
	var eRange = document.getElementById('range');

	// 頂点シェーダとフラグメントシェーダの生成
	var v_shader = render.create_shader('vs');
	var f_shader = render.create_shader('fs');

	// プログラムオブジェクトの生成とリンク
	render.create_program(v_shader, f_shader);

	// トーラスデータ
	var torusData = torus(6, 6, 0.5, 1.5, [0.5, 0.5, 0.5, 1.0]);
	var tPosition = render.create_vbo(torusData.p);
	var tNormal   = render.create_vbo(torusData.n);
	var tColor    = render.create_vbo(torusData.c);
	var tIndex = render.create_ibo(torusData.i);

	// バインド
	render.mAttrPos.bindBuffer(tPosition);
	render.mAttrNor.bindBuffer(tNormal);
	render.mAttrCol.bindBuffer(tColor);
	render.mGL.bindBuffer(render.mGL.ELEMENT_ARRAY_BUFFER, tIndex);

	// uniformLocationを配列に取得
	var uniLocation = new Array();
	uniLocation[0] = render.mGL.getUniformLocation(render.mProgram, 'mvpMatrix');
	uniLocation[1] = render.mGL.getUniformLocation(render.mProgram, 'mMatrix');
	uniLocation[2] = render.mGL.getUniformLocation(render.mProgram, 'invMatrix');
	uniLocation[3] = render.mGL.getUniformLocation(render.mProgram, 'lightPosition');
	uniLocation[4] = render.mGL.getUniformLocation(render.mProgram, 'eyeDirection');
	uniLocation[5] = render.mGL.getUniformLocation(render.mProgram, 'ambientColor');

	// 各種行列の生成と初期化
	var mMatrix   = new Mat();
	var vMatrix   = new Mat();
	var pMatrix   = new Mat();
	var tmpMatrix = new Mat();
	var mvpMatrix = new Mat();
	var invMatrix = new Mat();
	var qMatrix   = new Mat();
	
	// 各種クォータニオンの生成と初期化
	let aQuaternion = new Qtn();
	let bQuaternion = new Qtn();
	let sQuaternion = new Qtn();
	aQuaternion.identity();
	bQuaternion.identity();
	sQuaternion.identity();

	// 点光源の位置
	var lightPosition = [15.0, 10.0, 15.0];
	// 環境光の色
	var ambientColor = [0.1, 0.1, 0.1, 1.0];

	// カメラの座標
	var camPosition = [0.0, 0.0, 20.0];
	// カメラの上方向を表すベクトル
	var camUpDirection = [0.0, 1.0, 0.0];
	
	// ビュー×プロジェクション座標変換行列
	vMatrix.lookAt(camPosition, [0, 0, 0], camUpDirection);
	pMatrix.perspective(45, 0.1, 100, render.mWidth / render.mHeight);
	Mat.multiply(pMatrix, vMatrix, tmpMatrix);

	// カウンタの宣言
	var count = 0;
	
	// カリングと深度テストを有効にする
	render.mGL.enable(render.mGL.DEPTH_TEST);
	render.mGL.depthFunc(render.mGL.LEQUAL);
	render.mGL.enable(render.mGL.CULL_FACE);

	// 恒常ループ
	(function(){
		// canvasを初期化
		render.mGL.clearColor(0.0, 0.0, 0.0, 1.0);
		render.mGL.clearDepth(1.0);
		render.mGL.clear(render.mGL.COLOR_BUFFER_BIT | render.mGL.DEPTH_BUFFER_BIT);

		// カウンタをインクリメントしてラジアンを算出
		count++;
		var rad = (count % 360) * Math.PI / 180;
		
		// 経過時間係数を算出
		var time = eRange.value / 100;
		
		// 回転クォータニオンの生成
		Qtn.rotate(rad, [1.0, 0.0, 0.0], aQuaternion);
		Qtn.rotate(rad, [0.0, 1.0, 0.0], bQuaternion);
		Qtn.slerp(aQuaternion, bQuaternion, time, sQuaternion);
	
		// モデルのレンダリング
		ambientColor = [0.5, 0.0, 0.0, 1.0];
		draw(aQuaternion);
		ambientColor = [0.0, 0.5, 0.0, 1.0];
		draw(bQuaternion);
		ambientColor = [0.0, 0.0, 0.5, 1.0];
		draw(sQuaternion);

		function draw(qtn) {
			// モデル座標変換行列の生成
			qtn.toMat(qMatrix);
			mMatrix.identity();
			Mat.multiply(mMatrix, qMatrix, mMatrix);
			Mat.translate(mMatrix, [0.0, 0.0, -5.0], mMatrix);
			Mat.multiply(tmpMatrix, mMatrix, mvpMatrix);
			Mat.inverse(mMatrix, invMatrix);
			
			// uniform変数の登録と描画
			render.mGL.uniformMatrix4fv(uniLocation[0], false, mvpMatrix.Array);
			render.mGL.uniformMatrix4fv(uniLocation[1], false, mMatrix.Array);
			render.mGL.uniformMatrix4fv(uniLocation[2], false, invMatrix.Array);
			render.mGL.uniform3fv(uniLocation[3], lightPosition);
			render.mGL.uniform3fv(uniLocation[4], camPosition);
			render.mGL.uniform4fv(uniLocation[5], ambientColor);
			render.mGL.drawElements(render.mGL.TRIANGLES, torusData.i.length, render.mGL.UNSIGNED_SHORT, 0);
		}

		// コンテキストの再描画
		render.mGL.flush();
		
		// ループのために再帰呼び出し
		setTimeout(arguments.callee, 1000 / 30);
	})();
};

// トーラスを生成
function torus(row, column, irad, orad, color) {
	let pos = new Array();
	let nor = new Array();
	let col = new Array();
	let idx = new Array();
	for (let i = 0; i <= row; i++) {
		let r = Math.PI * 2 / row * i;
		let rr = Math.cos(r);
		let ry = Math.sin(r);
		for (let ii = 0; ii <= column; ii++) {
			let tr = Math.PI * 2 / column * ii;
			let tx = (rr * irad + orad) * Math.cos(tr);
			let ty = ry * irad;
			let tz = (rr * irad + orad) * Math.sin(tr);
			let rx = rr * Math.cos(tr);
			let rz = rr * Math.sin(tr);
			let tc;
			if (color) {
				tc = color;
			} else {
				tc = hsva(360 / column * ii, 1, 1, 1);
			}
			pos.push(tx, ty, tz);
			nor.push(rx, ry, rz);
			col.push(tc[0], tc[1], tc[2], tc[3]);
		}
	}
	for (let i = 0; i < row; i++) {
		for (let ii = 0; ii < column; ii++) {
			let r = (column + 1) * i + ii;
			idx.push(r, r + column + 1, r + 1);
			idx.push(r + column + 1, r + column + 2, r + 1);
		}
	}
	return {p : pos, n : nor, c : col, i : idx};
}

// HSVカラー取得
function hsva(h, s, v, a) {
	if (1 < s || 1 < v || 1 < a) {
		return;
	}
	var th = h % 360;
	var i = Math.floor(th / 60);
	var f = th / 60 - i;
	var m = v * (1 - s);
	var n = v * (1 - s * f);
	var k = v * (1 - s * (1 - f));
	var color = new Array();
	if (!s > 0 && !s < 0) {
		color.push(v, v, v, a); 
	} else {
		var r = new Array(v, n, m, m, k, v);
		var g = new Array(k, v, v, n, m, m);
		var b = new Array(m, m, k, v, v, n);
		color.push(r[i], g[i], b[i], a);
	}
	return color;
}
