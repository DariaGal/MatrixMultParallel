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

            var m1 = MatrixOperation.GenerateRandMatrix(2, 3);
            var m2 = MatrixOperation.GenerateRandMatrix(3, 4);
            Console.WriteLine("matrix1: ");
            PrintMatrix(m1);
            Console.WriteLine("matrix2: ");
            PrintMatrix(m2);

            var result = MatrixOperation.Multiply(m1, m2);

            Console.WriteLine("result: ");
            PrintMatrix(result);
        }

        static void PrintMatrix(int[,] a)
        {
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    Console.Write("{0} ", a[i, j]);
                }
                Console.WriteLine();
            }
        }
    }
}
