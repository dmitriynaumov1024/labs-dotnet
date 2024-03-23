namespace Lab03;
using System;

public class Point 
{
    public double X;
    public double Y;
    public double Z;

    public Point (double x, double y, double z) 
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
    }

    public double GetSquaredDistance (Point other) 
    {
        return Square(this.X - other.X) +
               Square(this.Y - other.Y) +
               Square(this.Z - other.Z);
    }

    public double GetDistance (Point other) 
    {
        return Math.Sqrt(this.GetSquaredDistance(other));
    }

    static double Square (double x) => x * x;
}
