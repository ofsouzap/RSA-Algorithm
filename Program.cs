using System;
using System.Collections.Generic;

namespace RSA_Algorithm
{
    class Program
    {

        static void Main(string[] args)
        {

            string mode;
            string[] modeArgs;

            if (args.Length == 0)
            {

                Console.Write("Mode> ");
                mode = Console.ReadLine();
                modeArgs = new string[0];

            }
            else
            {

                mode = args[0];

                modeArgs = new string[args.Length - 1];
                Array.Copy(args, 1, modeArgs, 0, modeArgs.Length);

            }

            switch (mode.ToLower())
            {
                case "generate":
                case "generatekeys":
                    Generate(modeArgs);
                    break;

                case "encrypt":
                case "decrypt":
                    Encrypt(modeArgs);
                    break;

                default:
                    Console.WriteLine("Invalid Mode");
                    break;
            }

            Console.WriteLine("Press any key to close...");
            Console.ReadKey();

        }

        static void Generate(string[] args)
        {

            ulong p, q, n, phiN, e, d;

            p = 0;
            q = 0;

            if (args.Length > 0)
            {
                GetGenerateParametresFromArgs(args,
                    ref p,
                    ref q);
            }
            else
            {
                GetGenerateParametresFromInput(ref p,
                    ref q);
            }

            n = p * q;
            phiN = (p - 1) * (q - 1);

            if (phiN > long.MaxValue)
            {
                throw new Exception("phiN must be within long range to avoid overflow errors later on");
            }

            e = RSAMath.GenerateEValue(phiN);

            d = EuclideanAlgorithmMath.GetMultiplicativeInverseModN(e, Convert.ToInt64(phiN));

            Console.WriteLine($"\nP = {p}");
            Console.WriteLine($"Q = {q}");
            Console.WriteLine($"N = {n}");
            Console.WriteLine($"phiN = {phiN}");
            Console.WriteLine($"E = {e}");
            Console.WriteLine($"D = {d}\n");

        }

        static void GetGenerateParametresFromArgs(string[] args,
            ref ulong p,
            ref ulong q)
        {

            p = ulong.Parse(args[0]);
            q = ulong.Parse(args[1]);

        }

        static void GetGenerateParametresFromInput(ref ulong p,
            ref ulong q)
        {

            Console.Write("P> ");
            p = ulong.Parse(Console.ReadLine());

            Console.Write("Q> ");
            q = ulong.Parse(Console.ReadLine());

        }

        static void Encrypt(string[] args)
        {

            ulong key, modSize, message;

            key = 0;
            modSize = 0;
            message = 0;

            if (args.Length > 0)
            {
                GetEncryptParametresFromArgs(args,
                    ref key,
                    ref modSize,
                    ref message);
            }
            else
            {
                GetEncryptParametresFromInput(ref key,
                    ref modSize,
                    ref message);
            }

            Console.WriteLine(RSAMath.Encrypt(key,
                modSize,
                message));

        }

        static void GetEncryptParametresFromArgs(string[] args,
            ref ulong key,
            ref ulong modSize,
            ref ulong message)
        {
            key = ulong.Parse(args[0]);
            modSize = ulong.Parse(args[1]);
            message = ulong.Parse(args[2]);
        }

        static void GetEncryptParametresFromInput(ref ulong key,
            ref ulong modSize,
            ref ulong message)
        {

            Console.Write("Key> ");
            key = ulong.Parse(Console.ReadLine());

            Console.Write("Modulo Number> ");
            modSize = ulong.Parse(Console.ReadLine());

            Console.Write("Message> ");
            message = ulong.Parse(Console.ReadLine());

        }

    }
}
