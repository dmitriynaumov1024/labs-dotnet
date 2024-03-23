namespace Lab03;
using System;
using System.Collections.Generic;
using System.Linq;

// The simplest interpolator that just 
// returns value at closest vert to given point
class ProximityInterpolator : Interpolator
{
    public ProximityInterpolator (Shape<ValuedPoint> shape)
    {
        this.Shape = shape;
    }

    public override Interpolator Build ()
    {
        return this;
    }

    public override double GetValueAt (Point p) 
    {
        return this.Shape.GetNearest(p).Value;
    }
}
