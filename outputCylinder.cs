void outputCylinder(string path, string objName, List<double[]> crossSection, int poles) {
    var sw = new StreamWriter(path);
    sw.WriteLine("g " + objName);
    // vertex
    for (var h = 0; h < crossSection.Count; h++) {
        var outer = crossSection[h][0];
        var inner = crossSection[h][1];
        var y = crossSection[h][2];
        for (int p = 0; p < poles; p++) {
            var th = 2 * Math.PI * p / P;
            var x = outer * Math.Cos(th);
            var z = outer * Math.Sin(th);
            sw.WriteLine(string.Format(
                "v {0} {1} {2}",
                x.ToString("0.00"),
                y.ToString("0.00"),
                z.ToString("0.00")));
        }
        for (int p = 0; p < poles; p++) {
            var th = 2 * Math.PI * p / poles;
            var x = inner * Math.Cos(th);
            var z = inner * Math.Sin(th);
            sw.WriteLine(string.Format(
                "v {0} {1} {2}",
                x.ToString("0.00"),
                y.ToString("0.00"),
                z.ToString("0.00")));
        }
    }
    // top index
    for (int p0 = 0; p0 < poles; p0++) {
        var o0 = 1 + p0;
        var o1 = 1 + (p0 + 1) % poles;
        var i0 = 1 + poles + p0;
        var i1 = 1 + poles + (p0 + 1) % poles;
        sw.WriteLine(string.Format("f {0} {1} {2}", o0, o1, i0));
        sw.WriteLine(string.Format("f {0} {1} {2}", o1, i1, i0));
    }
    // middle index
    for (var y = 0; y < crossSection.Count - 1; y++) {
        for (int p0 = 0; p0 < poles; p0++) {
            var p1 = (p0 + 1) % poles;
            var ofs1 = 2 * poles * y;
            var ofs2 = 2 * poles * (y + 1);
            var ou0 = 1 + ofs1 + p0;
            var ou1 = 1 + ofs1 + p1;
            var ob0 = 1 + ofs2 + p0;
            var ob1 = 1 + ofs2 + p1;
            var iu0 = 1 + ofs1 + p0 + poles;
            var iu1 = 1 + ofs1 + p1 + poles;
            var ib0 = 1 + ofs2 + p0 + poles;
            var ib1 = 1 + ofs2 + p1 + poles;
            sw.WriteLine(string.Format("f {0} {1} {2}", ou0, ob0, ob1));
            sw.WriteLine(string.Format("f {0} {1} {2}", ou0, ob1, ou1));
            sw.WriteLine(string.Format("f {0} {1} {2}", iu0, ib1, ib0));
            sw.WriteLine(string.Format("f {0} {1} {2}", iu0, iu1, ib1));
        }
    }
    // bottom index
    var bottomY = crossSection.Count - 1;
    for (int p0 = 0; p0 < poles; p0++) {
        var p1 = (p0 + 1) % poles;
        var ofs = 2 * poles * bottomY;
        var o0 = 1 + ofs + p0;
        var o1 = 1 + ofs + p1;
        var i0 = 1 + ofs + p0 + poles;
        var i1 = 1 + ofs + p1 + poles;
        sw.WriteLine(string.Format("f {0} {1} {2}", o1, o0, i0));
        sw.WriteLine(string.Format("f {0} {1} {2}", o1, i0, i1));
    }
    sw.Close();
    sw.Dispose();
}