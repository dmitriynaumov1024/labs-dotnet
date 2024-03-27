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
        if (this.Interpolators.Count < 1) {
            throw new EmptyListException("InterpolatorArray should contain at least 1 interpolator");
        }
        if (this.Interpolators.Count == 1) {
            return this.Interpolators[0];
        }
        return this.Interpolators.MinBy (
            it => it.Shape.Verts.Sum (
                v => v.GetDistance(p)
            )
        );
    }
}
