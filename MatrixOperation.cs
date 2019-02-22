using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixMultParallel
{
    static class MatrixOperation
    {
        public static int[,] GenerateRandMatrix(int row, int column)
        {
            var matrix = new int[row, column];
            var rand = new Random();
            for(int r=0; r<row; r++)
            {
                for (int c = 0; c < column; c++)
                {
                    matrix[r, c] = rand.Next(0, 10);
                }
            }
            return matrix;
        }

        public static int[,] Multiply(int[,] a, int[,] b)
        {
            if (a is null)
                throw new ArgumentNullException(nameof(a));
            if (b is null)
                throw new ArgumentNullException(nameof(b));

            int rowA = a.GetLength(0);
            int columnA = a.GetLength(1);
            int rowB = b.GetLength(0);
            int columnB = b.GetLength(1);

            if (columnA != rowB)
                throw new ArgumentException("Cannot be multiplied! Column size of "
                    + nameof(a) + " not equal to row size of " + nameof(b));

            var result = new int[rowA, columnB];

            var tasks = new List<Task>();
            for (int i = 0; i < columnA; i++)
            {
                int j = i;
                tasks.Add(new Task(() => MultTask(result, a, b, j)));
                tasks.Last().Start();
            }
            
            Task.WaitAll(tasks.ToArray());

            return result;
        }

        private static void MultTask(int[,] result, int[,] a, int[,] b, int m)
        {
            for (int n = 0; n < a.GetLength(0); n++)
            {
                for (int k = 0; k < b.GetLength(1); k++)
                {
                    result[n, k] += a[n, m] * b[m, k];
                }
            }
        }
    }
}
