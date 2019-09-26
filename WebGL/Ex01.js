/// <reference path="To2d.js"/>

/******************************************************************************/
/** @type {HTMLCanvasElement} */
let elmCanvas = document.getElementById("disp");
elmCanvas.width = 160;
elmCanvas.height = 160;

/** @type {CanvasRenderingContext2D} */
let ctx = elmCanvas.getContext("2d");

/******************************************************************************/
const AXIS_COLOR_X = "rgb(32, 167, 32)";
const AXIS_COLOR_Y = "rgb(32, 127, 241)";
const AXIS_COLOR_Z = "rgb(191, 32, 32)";
const COLOR_GLAY = "rgb(127, 127, 127)";
const COLOR_WHITE = "rgb(231, 231, 231)";

let gIsStop = true;
let gTo2d = new To2d(elmCanvas.width*0.8, elmCanvas.width/2, elmCanvas.height/2);
let gPhi = 0.0;
let gTheta = 0.0;

/******************************************************************************/
phi_oninput();
theta_oninput();
azimuth_oninput();
eyePosX_oninput();
eyePosY_oninput();
eyePosZ_oninput();

/******************************************************************************/
function phi_oninput() {
	const val = document.getElementById("phi").value;
	gPhi = 4*Math.atan(1)*val/180;
	document.getElementById("lbl_phi").innerHTML = val;
	draw();
}
function theta_oninput() {
	const val = document.getElementById("theta").value
	gTheta = 4*Math.atan(1)*val/180;
	document.getElementById("lbl_theta").innerHTML = val;
	draw();
}
function azimuth_oninput() {
	const val = document.getElementById("azimuth").value
	gTo2d.azimuth = 4*Math.atan(1)*val/180;
	document.getElementById("lbl_azimuth").innerHTML = val;
	draw();
}
function eyePosX_oninput() {
	const val = document.getElementById("eyePosX").value
	gTo2d.eyePosX = val/100;
	document.getElementById("lbl_eyePosX").innerHTML = gTo2d.eyePosX;
	draw();
}
function eyePosY_oninput() {
	const val = document.getElementById("eyePosY").value
	gTo2d.eyePosY = val/100;
	document.getElementById("lbl_eyePosY").innerHTML = gTo2d.eyePosY;
	draw();
}
function eyePosZ_oninput() {
	const val = document.getElementById("eyePosZ").value
	gTo2d.eyePosZ = val/100;
	document.getElementById("lbl_eyePosZ").innerHTML = gTo2d.eyePosZ;
	draw();
}
function btnPlayStop_onclick() {
	if ("Play" == document.getElementById("btnPlayStop").value) {
		document.getElementById("btnPlayStop").value = "Stop";
		gIsStop = false;
		mainLoop();
	} else {
		document.getElementById("btnPlayStop").value = "Play";
		gIsStop = true;
	}
}
function mainLoop() {
	if (gIsStop) {
		return;
	}
	draw();
	gPhi += 0.1*8*Math.atan(1)/60;
	requestAnimationFrame(mainLoop);
}

/******************************************************************************/
function draw() {
	ctx.clearRect(0, 0, elmCanvas.width, elmCanvas.height);
	ctx.fillRect(0, 0, elmCanvas.width, elmCanvas.height);

	drawAxis();

	let v2A = {x:0.0, y:0.0};
	let v2B = {x:0.0, y:0.0};

	let x = Math.cos(gTheta)*Math.cos(gPhi);
	let y = Math.sin(gTheta);
	let z = Math.cos(gTheta)*Math.sin(gPhi);

	// X
	gTo2d.exe([x, 0, 0], v2A);
	gTo2d.exe([x, 0, z], v2B);
	drawLine(v2A, v2B, AXIS_COLOR_X, 0.5);
	// Z
	gTo2d.exe([0, 0, z], v2A);
	gTo2d.exe([x, 0, z], v2B);
	drawLine(v2A, v2B, AXIS_COLOR_Z, 0.5);
	// Y
	gTo2d.exe([x, 0, z], v2A);
	gTo2d.exe([x, y, z], v2B);
	drawLine(v2A, v2B, AXIS_COLOR_Y, 0.5);
	// Phi
	gTo2d.exe([0, 0, 0], v2A);
	gTo2d.exe([x, 0, z], v2B);
	drawLine(v2A, v2B, COLOR_GLAY, 1);
	// Theta, Phi
	gTo2d.exe([0, 0, 0], v2A);
	gTo2d.exe([x, y, z], v2B);
	drawLine(v2A, v2B, COLOR_WHITE, 1);
}

function drawAxis() {
	let v2A = {x:0.0, y:0.0};
	let v2B = {x:0.0, y:0.0};
	// Y軸
	gTo2d.exe([ 0.00,-1.00, 0.00], v2A);
	gTo2d.exe([ 0.00, 1.00, 0.00], v2B);
	drawLine(v2A, v2B, AXIS_COLOR_Y, 1.5);
	gTo2d.exe([ 0.00, 1.00, 0.00], v2A);
	gTo2d.exe([ 0.03, 0.90, 0.00], v2B);
	drawLine(v2A, v2B, AXIS_COLOR_Y, 1.5);
	gTo2d.exe([-0.03, 0.90, 0.00], v2B);
	drawLine(v2A, v2B, AXIS_COLOR_Y, 1.5);
	// X軸
	gTo2d.exe([-1.00, 0.00, 0.00], v2A);
	gTo2d.exe([ 1.00, 0.00, 0.00], v2B);
	drawLine(v2A, v2B, AXIS_COLOR_X, 1.5);
	gTo2d.exe([ 1.00, 0.00, 0.00], v2A);
	gTo2d.exe([ 0.90, 0.03, 0.00], v2B);
	drawLine(v2A, v2B, AXIS_COLOR_X, 1.5);
	gTo2d.exe([ 0.90,-0.03, 0.00], v2B);
	drawLine(v2A, v2B, AXIS_COLOR_X, 1.5);
	// Z軸
	gTo2d.exe([ 0.00, 0.00,-1.00], v2A);
	gTo2d.exe([ 0.00, 0.00, 1.00], v2B);
	drawLine(v2A, v2B, AXIS_COLOR_Z, 1.5);
	gTo2d.exe([ 0.00, 0.00, 1.00], v2A);
	gTo2d.exe([ 0.00, 0.03, 0.90], v2B);
	drawLine(v2A, v2B, AXIS_COLOR_Z, 1.5);
	gTo2d.exe([ 0.00,-0.03, 0.90], v2B);
	drawLine(v2A, v2B, AXIS_COLOR_Z, 1.5);
}

function drawLine(posA= {x:0.0, y:0.0}, posB= {x:0.0, y:0.0}, strokeStyle="rgb(0, 0, 0)", width=1.0) {
	ctx.setLineDash([1, 0]);
	ctx.lineWidth = width;
	ctx.lineCap = "butt";
	ctx.strokeStyle = strokeStyle;
	ctx.beginPath();
	ctx.moveTo(posA.x, posA.y);
	ctx.lineTo(posB.x, posB.y);
	ctx.closePath();
	ctx.stroke();
}
