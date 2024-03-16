namespace Lab02;
using System;
using System.Collections.Generic;
using System.Linq;

// The simplest interpolator that just 
// returns value at closest vert to given point
class ProximityTetrahedronInterpolator : TetrahedronInterpolator
{
    public ProximityTetrahedronInterpolator (Point[] verts, double[] values)
    {
        this.Verts = verts;
        this.Values = values;
    }

    public override double GetValueAt (Point p) 
    {
        int i = this.GetIndexOfClosest(p);
        return this.Values[i];
    }
}
