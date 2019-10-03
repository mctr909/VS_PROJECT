class RK4 {
	constructor(variables=2) {
		this.mTime = 0.0;

		/** @type {number[]} */
		this.mF = new Array(variables);

		/** @type {number[]} */
		this.mDF = new Array(variables);

		/** @protected */
		this.mDeltaTime = 0.01;

		/** @private */
		this._endTime = 1.0;

		/** @private @type {number[]} */
		this._f = new Array(variables);

		/** @private @type {number[][]} */
		this._dfdt = new Array(4);
		this._dfdt[0] = new Array(variables);
		this._dfdt[1] = new Array(variables);
		this._dfdt[2] = new Array(variables);
		this._dfdt[3] = new Array(variables);

		for (let i=0; i<variables; i++) {
			this.mF[i] = 0.0;
			this.mDF[i] = 0.0;
			this._f[i] = 0.0;
			this._dfdt[0][i] = 0.0;
			this._dfdt[1][i] = 0.0;
			this._dfdt[2][i] = 0.0;
			this._dfdt[3][i] = 0.0;
		}
	}

	init(beginTime=0, endTime=1.0, deltaTime=0.001) {
		this.mTime = beginTime;
		this._endTime = endTime;
		this.mDeltaTime = deltaTime;
		for (let i = 0; i < this.mF.length; i++) {
			this._f[i] = this.mF[i];
		}
	}

	step() {
		if (this.mTime <= this._endTime) {
			this._rk4();
			return true;
		} else {
			return false;
		}
	}

	/** @protected */
	updateDF() {
		this.mDF[0] = -this.mF[1] * 6.283185;
		this.mDF[1] =  this.mF[0] * 6.283185;
	}

	/** @private */
	_rk4() {
		let idxVar = 0;

		// dfdt[0]算出
		for (idxVar = 0; idxVar < this.mF.length; idxVar++) {
			this.mF[idxVar] = this._f[idxVar];
		}
		this.updateDF();
		for (idxVar = 0; idxVar < this.mDF.length; idxVar++) {
			this._dfdt[0][idxVar] = this.mDF[idxVar] * this.mDeltaTime;
		}

		// mTimeを進める
		this.mTime += this.mDeltaTime / 2;

		// dfdt[1]算出
		for (idxVar = 0; idxVar < this.mF.length; idxVar++) {
			this.mF[idxVar] = this._f[idxVar] + this._dfdt[0][idxVar] / 2;
		}
		this.updateDF();
		for (idxVar = 0; idxVar < this.mDF.length; idxVar++) {
			this._dfdt[1][idxVar] = this.mDF[idxVar] * this.mDeltaTime;
		}

		// dfdt[2]算出
		for (idxVar = 0; idxVar < this.mF.length; idxVar++) {
			this.mF[idxVar] = this._f[idxVar] + this._dfdt[1][idxVar] / 2;
		}
		this.updateDF();
		for (idxVar = 0; idxVar < this.mDF.length; idxVar++) {
			this._dfdt[2][idxVar] = this.mDF[idxVar] * this.mDeltaTime;
		}

		// mTimeを進める
		this.mTime += this.mDeltaTime / 2;

		// dfdt[3]算出
		for (idxVar = 0; idxVar < this.mF.length; idxVar++) {
			this.mF[idxVar] = this._f[idxVar] + this._dfdt[2][idxVar];
		}
		this.updateDF();
		for (idxVar = 0; idxVar < this.mDF.length; idxVar++) {
			this._dfdt[3][idxVar] = this.mDF[idxVar] * this.mDeltaTime;
			// dfdt[0]～dfdt[3]をもとに次のfの値を計算
			this._f[idxVar] += (this._dfdt[0][idxVar] + 2*(this._dfdt[1][idxVar] + this._dfdt[2][idxVar]) + this._dfdt[3][idxVar]) / 6;
		}
	}
}
