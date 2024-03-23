namespace Lab03;
using System;

public class ValuedPoint : Point 
{
    // inherited X, Y, Z
    public double Value;

    public ValuedPoint (double x, double y, double z, double value) : base(x, y, z) 
    {
        this.Value = value;
    }
}
