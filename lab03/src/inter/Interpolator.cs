namespace Lab03;
using System;

public abstract class Interpolator 
{
    public Shape<ValuedPoint> Shape { get; set; }

    // Perform additional calculations if necessary
    public abstract Interpolator Build ();

    // Get interpolated value at given point 
    public abstract double GetValueAt (Point p);
}
