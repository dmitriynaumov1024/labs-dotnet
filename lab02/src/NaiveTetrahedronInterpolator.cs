namespace Lab02;
using System;
using System.Collections.Generic;
using System.Linq;

// The simplest interpolator that just 
// returns value at closest vert to given point
class NaiveTetrahedronInterpolator : TetrahedronInterpolator
{
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
        double[] ds = new double[VertCount];
        for (int i=0; i<VertCount; i+=1) {
            ds[i] = 1.0 / this.GetSquaredDistance(i, p);
            D += ds[i];
        }
        for (int i=0; i<VertCount; i+=1) {
            result += this.Values[i] * ds[i];
        }
        return result / D;
    }
}
