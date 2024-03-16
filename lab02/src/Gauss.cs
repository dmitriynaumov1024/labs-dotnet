namespace Lab02;
using System;

public class Gauss
{
    static double Eps = 1e-8;

    public static double[] Solve (double[,] M, double[] result = null)
    {
        double[,] matrix = (double[,])M.Clone();
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        result ??= new double[rows];
        if (cols != rows + 1) {
            return result;
        }

        // liquidation of lower triangle
        for (int j=0; j<cols-2; j++) {
            for (int i=j+1; i<rows; i++) {
                if (Math.Abs(matrix[i, j]) > Eps) {
                    // matrix[i, j] - target element
                    // matrix[j, j] - liquidator element
                    double d = matrix[j, j] / matrix[i, j];
                    for (int k=0; k<cols; k++) {
                        matrix[i, k] -= matrix[j, k] / d;
                    }
                }
                matrix[i, j] = 0;
            }
        }

        // liquidation of upper triangle
        for (int j=cols-2; j>=0; j--) {
            // Gathering result by the way
            double x = matrix[j, cols-1] / matrix[j, j];
            result[j] = x;
            for (int i=j-1; i>=0; i--) {
                if (Math.Abs(matrix[i, j]) > Eps) {
                    // matrix[i, j] - target element
                    // matrix[i+1, j] - liquidator element
                    matrix[i, cols-1] -= x * matrix[i, j];
                    matrix[i, j] = 0;
                }
                matrix[i, j] = 0;
            }
        }
        // The end
        return result;
    }
}
