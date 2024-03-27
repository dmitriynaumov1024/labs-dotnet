namespace Lab03;
using System;

public class Gauss
{
    static double Eps = 1e-10;

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
            // fix zeros on main diagonal
            if (Math.Abs(matrix[j, j]) < Eps) {
                for (int k=j+1; k<rows; k++) {
                    if (Math.Abs(matrix[k, j]) > Eps) {
                        // fix it by swapping j-th row with first k-th row 
                        // where k > j and matrix[k, j] != 0
                        Swap(matrix, j, k);
                        break;
                    }
                }
            }
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

    public static void Swap (double[,] M, int row1, int row2)
    {
        int cols = M.GetLength(1);
        for (int i=0; i<cols; i++) {
            double tmp = M[row1, i];
            M[row1, i] = M[row2, i];
            M[row2, i] = tmp;
        }
    }
}
