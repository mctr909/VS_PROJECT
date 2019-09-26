class To2d {
	constructor(scale=1, offsetX=0, offsetY=0) {
		this._SCALE = scale;
		this._OFFSET_X = offsetX;
		this._OFFSET_Y = offsetY;
		this._far = 0.0;
		this._near = 0.0;
		this._zScale = 0.0;
		this._xyScale = 0.0;

		this.far = 3.0;
		this.near = -2.0;
		this.angle = 110.0;
		this.eyePosX = 0.0;
		this.eyePosY = 0.0;
		this.eyePosZ = 0.0;
		this.azimuth = 0.7;
	}

	set far(val) {
		this._far = val;
		this._zScale = 1.0 / (this._far - this._near) * this._far;
	}
	set near(val) {
		this._near = val;
		this._zScale = 1.0 / (this._far - this._near) * this._far;
	}
	set angle(val) {
		this._xyScale = this._SCALE / Math.tan(Math.atan(1)*val/90);
	}

	exe(v3=[0.0, 0.0, 0.0], v2={x:0.0, y:0.0}) {
		const sx = v3[0] - this.eyePosX;
		const sy = v3[1] - this.eyePosY;
		const sz = v3[2] - this.eyePosZ;
		const x2_z2 = sx*sx + sz*sz;
		const r  = Math.sqrt(x2_z2 + sy*sy);
		const ph = Math.atan2(sz, sx) + this.azimuth;
		const th = Math.atan2(sy, Math.sqrt(x2_z2));
		const rx = r * Math.cos(th) * Math.cos(ph);
		const ry = r * Math.sin(th);
		const rz = r * Math.cos(th) * Math.sin(ph);
		const tz = (rz - this._near) * this._zScale;
		v2.x = this._OFFSET_X + this._xyScale * rx/tz;
		v2.y = this._OFFSET_Y - this._xyScale * ry/tz;
	}
}
