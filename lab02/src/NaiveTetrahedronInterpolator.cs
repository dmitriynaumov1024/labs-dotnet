namespace Lab02;
using System;
using System.Collections.Generic;
using System.Linq;

// The simplest interpolator that just 
// returns value at closest vert to given point
class NaiveTetrahedronInterpolator : TetrahedronInterpolator
{
    static double Eps = 1e-9;

    public NaiveTetrahedronInterpolator (Point[] verts, double[] values)
    {
        this.Verts = verts;
        this.Values = values;
    }

    // Weighted average, weight of i-th vert is inv.square distance
    // from i-th vert to given point p
    public override double GetValueAt (Point p) 
    {
        double D = 0, result = 0;
        Span<double> ds = stackalloc double[VertCount];
        for (int i=0; i<VertCount; i+=1) {
            double d = this.GetSquaredDistance(i, p);
            if (Math.Abs(d) < Eps) return this.Values[i];
            ds[i] = 1 / d;
            D += ds[i];
        }
        for (int i=0; i<VertCount; i+=1) {
            result += this.Values[i] * ds[i];
        }
        return result / D;
    }
}
