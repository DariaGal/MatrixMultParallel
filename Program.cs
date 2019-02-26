using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixMultParallel
{
    class Program
    {
        static void Main(string[] args)
        {

            var matrix1 = new Matrix(2, 3, MatrixTypes.RandomMatrix);
            var matrix2 = new Matrix(3, 4, MatrixTypes.RandomMatrix);

            var matrixNew = matrix1 * matrix2;
            Console.WriteLine("matrix1: {0}", matrix1.ToString());

            Console.WriteLine("matrix2: {0}", matrix2.ToString());

            Console.WriteLine("result: {0}", matrixNew.ToString());
        }
    }
}
