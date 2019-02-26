using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixMultParallel
{
    public enum MatrixTypes
    {
        ZeroMatrix,
        RandomMatrix
    }

    class Matrix
    {
        public int RowsCount { get; }
        public int ColumnsCount { get; }

        private readonly int[,] matrix;
        private string matrixString;

        public Matrix(int rowsCount, int columnsCount, MatrixTypes matrixType = MatrixTypes.ZeroMatrix)
        {
            RowsCount = rowsCount;
            ColumnsCount = columnsCount;
            switch(matrixType)
            {
                case MatrixTypes.ZeroMatrix:
                    matrix = new int[rowsCount, columnsCount];
                    break;
                case MatrixTypes.RandomMatrix:
                    matrix = CreateRandomMatrix(rowsCount, columnsCount);
                    break;
                default:
                    throw new ArgumentException("Undefined " + nameof(matrixType));
            }
        }

        public Matrix(int[,] matrix)
        {
            if (matrix is null)
                throw new ArgumentNullException(nameof(matrix));

            RowsCount = matrix.GetLength(0);
            ColumnsCount = matrix.GetLength(1);
            this.matrix = new int[RowsCount, ColumnsCount];
            this.matrix = matrix;
        }

        public override string ToString()
        {
            if(matrixString is null)
            {
                var str = new StringBuilder();
                str.Append("\n");
                for (int i = 0; i < RowsCount; i++)
                {
                    for (int j = 0; j < ColumnsCount; j++)
                    {
                        str.Append(matrix[i, j] + " ");
                    }
                    str.Append("\n");
                }
                matrixString = str.ToString();
            }

            return matrixString;
        }

        public Matrix Multiply(Matrix other)
        {
            if (ColumnsCount != other.RowsCount)
                throw new ArgumentException("Cannot be multiplied! Column size of "
                    + nameof(matrix) + " not equal to row size of " + nameof(other));

            var result = new int[RowsCount, other.ColumnsCount];

            var tasks = new Task[ColumnsCount];

            for (int i = 0; i < ColumnsCount; i++)
            {
                int firstMatrixColumn = i;
                tasks[i] = Task.Factory.StartNew(() => MultTask(result, matrix, other.matrix, firstMatrixColumn));
            }

            Task.WaitAll(tasks);

            return new Matrix(result);
        }

        public static Matrix operator* (Matrix m1, Matrix m2)
        {
            return m1.Multiply(m2);
        }

        private void MultTask(int[,] result, int[,] m1, int[,] m2, int n)
        {
            int m1RowsCount = m1.GetLength(0);
            int m2ColumnCount = m2.GetLength(1);

            var tasks = new Task[m1RowsCount];
            for (int i = 0; i < m1RowsCount; i++)
            {
                int row = i;

                tasks[row] = Task.Factory.StartNew(() =>
                    {
                        for(int col = 0;col< m2ColumnCount; col++)
                        {
                            result[row, col] += m1[row, n] * m2[n, col];
                        }
                    }
                );
            }

            Task.WaitAll(tasks);
        }

        private int[,] CreateRandomMatrix(int rowsCount, int columnsCount)
        {
            var matrix = new int[rowsCount, columnsCount];
            var rand = new Random();

            for (int row = 0; row < rowsCount; row++)
            {
                for (int col = 0; col < columnsCount; col++)
                {
                    matrix[row, col] = rand.Next(0, 10);
                }
            }

            return matrix;
        }
    }
}
