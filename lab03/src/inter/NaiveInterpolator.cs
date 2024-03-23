namespace Lab03;
using System;
using System.Collections.Generic;
using System.Linq;

// Naive interpolator for any shape returns weighed average 
// where weight is proportional to inverted squared distance to given point
class NaiveInterpolator : Interpolator
{
    static double Eps = 1e-9;

    public NaiveInterpolator (Shape<ValuedPoint> shape)
    {
        this.Shape = shape;
    }

    public override Interpolator Build ()
    {
        return this;
    }

    public override double GetValueAt (Point p) 
    {
        double N = this.Shape.VertCount, D = 0, result = 0;
        var verts = this.Shape.Verts;
        for (int i=0; i<N; i+=1) {
            double d = verts[i].GetSquaredDistance(p);
            if (Math.Abs(d) < Eps) return verts[i].Value;
            result += verts[i].Value / d;
            D += 1 / d;
        }
        return result / D;
    }
}
