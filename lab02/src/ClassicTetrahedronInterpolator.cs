namespace Lab02;
using System;
using System.Collections.Generic;
using System.Linq;

// Builds shape functions for gradient originating at each vert
public class ClassicTetrahedronInterpolator : TetrahedronInterpolator
{
    public double[,] C;

    public ClassicTetrahedronInterpolator (Point[] verts, double[] values) 
    {
        this.Verts = verts;
        this.Values = values;
    }

    public override TetrahedronInterpolator Build()
    {
        Point[] P = this.Verts;

        this.C = new double[VertCount, VertCount];

        // reusable matrix
        double[,] M = new double[VertCount, VertCount + 1];
        double[] R = new double[VertCount];


        // for each row of coeficients
        for (int v=0; v<VertCount; v++) {
            for (int i=0; i<VertCount; i++) {
                M[i, 0] = 1;
                M[i, 1] = P[i].X;
                M[i, 2] = P[i].Y;
                M[i, 3] = P[i].Z;
                M[i, 4] = (v == i)? 1 : 0;
            }

            Gauss.Solve(M, R);
            for (int i=0; i<VertCount; i++) {
                C[v, i] = R[i];
            }
        }

        return this;
    }

    // Get weight of i-th vert for given point p
    public double GetWeightAt (int i, Point p)
    {
        return this.C[i, 0] + 
               this.C[i, 1] * p.X + 
               this.C[i, 2] * p.Y + 
               this.C[i, 3] * p.Z;
    }

    public override double GetValueAt (Point p)
    {
        return this.Values.Select((v, i) => v * this.GetWeightAt(i, p)).Sum();
    }
}
