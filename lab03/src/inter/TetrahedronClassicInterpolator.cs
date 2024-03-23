namespace Lab03;
using System;
using System.Collections.Generic;
using System.Linq;

// Builds shape functions for gradient originating at each vert
public class TetrahedronClassicInterpolator : Interpolator
{
    public double[,] C;

    public TetrahedronClassicInterpolator (Shape<ValuedPoint> shape) 
    {
        this.Shape = shape;
    }

    public override Interpolator Build()
    {
        int N = this.Shape.VertCount;
        List<ValuedPoint> P = this.Shape.Verts;

        this.C = new double[N, N];

        // reusable matrix
        double[,] M = new double[N, N + 1];
        double[] R = new double[N];


        // for each row of coeficients
        for (int v=0; v<N; v++) {
            for (int i=0; i<N; i++) {
                M[i, 0] = 1;
                M[i, 1] = P[i].X;
                M[i, 2] = P[i].Y;
                M[i, 3] = P[i].Z;
                M[i, 4] = (v == i)? 1 : 0;
            }

            Gauss.Solve(M, R);
            for (int i=0; i<N; i++) {
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
        return this.Shape.Verts.Select((v, i) => v.Value * this.GetWeightAt(i, p)).Sum();
    }
}
