/// <reference path="RK4.js"/>
class MRK4 extends RK4 {
    constructor() {
        super(2);
    }
    init() {
        this.mF[0] = 1.0;
        this.mF[1] = 0.0;
        super.init();
    }
    updateDF() {
        const L = 0.2;
        const C = 0.000470;
        const R = 1;
        const E = 0;
        this.mDF[0] = this.mF[1];
        this.mDF[1] = E/L - R/L*this.mF[1] - 1/(L*C)*this.mF[0];
    }
}

let rk4 = new MRK4();
rk4.init();

document.write("<table style=\"font-size:9px; font-family:MS Gothic\">");
while (rk4.mTime < 1.0) {
    rk4.step();
    let i = rk4.mF[0];
    let v = rk4.mF[1];
    document.write("<tr><td>" + i.toExponential(2) + "</td><td>" + v.toExponential(2) + "</td></tr>");
}
document.write("</table>");