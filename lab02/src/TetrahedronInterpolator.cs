namespace Lab02;
using System;
using System.Linq;

public abstract class TetrahedronInterpolator : Interpolator 
{
    public static readonly int VertCount = 4;

    public Point[] Verts;
    public double[] Values;

    // Get distance from vert with given index to given point
    public double GetDistance (int i, Point p) 
    {
        return Math.Sqrt (
            GetSquaredDistance(i, p)
        );
    }

    // Get squared distance from vert with given index to given point
    public double GetSquaredDistance (int i, Point p)
    {
        Point v = this.Verts[i];
        return (
            Square(v.X - p.X) + 
            Square(v.Y - p.Y) + 
            Square(v.Z - p.Z)
        );
    }

    // Get distances from each vert to given point
    public double[] GetDistances (Point p)
    {
        double[] result = new double[VertCount];
        for (int i=0; i<VertCount; i+=1) {
            result[i] = this.GetDistance(i, p);
        }
        return result;
    }

    // Get sum of distances to given point
    public double GetDistanceSum (Point p)
    {
        double result = 0;
        for (int i=0; i<VertCount; i+=1) {
            result += this.GetDistance(i, p);
        }
        return result;
    }

    // Get index of vert closest to given point
    public int GetIndexOfClosest (Point p) 
    {
        int minIndex = 0;
        double minDistance = double.MaxValue;
        for (int i=0; i<VertCount; i+=1) {
            double d = GetDistance(i, p);
            if (d < minDistance) {
                minIndex = i;
                minDistance = d;
            }
        }
        return minIndex;
    }

    // Quickly get a square of given number
    private static double Square (double x) => x * x;

    // Use this method to calculate auxiliary properties
    // after initialization or change of Points or Values
    public virtual TetrahedronInterpolator Build()
    {
        return this;
    }
}
