using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Single;

namespace Neural
{
    class Program
    {
        private static void Main(string[] args)
        {
            Matrix<float> A = DenseMatrix.OfArray(new float[,]
                                                  {
                                                      {1, 1, 1, 1},
                                                      {1, 2, 3, 4},
                                                      {4, 3, 2, 1}
                                                  });

            Matrix<float> B = DenseMatrix.OfArray(new float[,]
                                                  {
                                                      {1, 1, 1, 1},
                                                      {1, 2, 3, 4},
                                                      {4, 3, 2, 1}
                                                  });

            var C = A + B;
        }
    }
}
