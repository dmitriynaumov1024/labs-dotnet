namespace Lab03;
using System;
using System.Collections.Generic;
using System.Linq;

public class InterpolatorArray
{
    public List<Interpolator> Interpolators { get; set; }

    public double GetValueAt (Point p)
    {
        return this.GetNearest(p).GetValueAt(p);
    }

    public Interpolator GetNearest (Point p)
    {
        return this.Interpolators.MinBy (
            it => it.Shape.Verts.Sum (
                v => v.GetDistance(p)
            )
        );
    }
}
