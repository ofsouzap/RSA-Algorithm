using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSA_Algorithm
{
    static class RSAMath
    {

        public static ulong GenerateEValue(ulong phiN)
        {

            Random random = new Random();

            ulong e;

            List<ulong> eOptions = new List<ulong>();
            for (ulong i = 2; i < phiN; i++)
            {

                //Check if coprime
                if (EuclideanAlgorithmMath.EuclideanAlgorithm(i, phiN) == 1)
                {

                    eOptions.Add(i);

                }

            }

            if (eOptions.Count == 0)
            {
                throw new Exception("No valid e values");
            }

            int randEIndex = random.Next(0, eOptions.Count);
            e = eOptions[randEIndex];

            return e;

        }

        public static ulong Encrypt(ulong key, ulong modSize, ulong message)
        {

            ulong curr = 1;

            for (ulong i = 0; i < key; i++)
            {

                curr *= message;
                curr %= modSize;

            }

            return curr;

        }

    }
}
