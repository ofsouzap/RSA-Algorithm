using System;
using System.Collections.Generic;
using System.Linq;

namespace RSA_Algorithm
{
    static class EuclideanAlgorithmMath
    {

        public static ulong GetMultiplicativeInverseModN(ulong a, long n)
        {

            EEAReturnValues returnValues = ExtendedEuclideanAlgorithm(Convert.ToUInt64(n), a);
            long[] eeaMultiples = new long[] { returnValues.aMultiplier, returnValues.bMultiplier };

            //Incase of negative, add n then re-modulo by n
            ulong b = Convert.ToUInt64(eeaMultiples[1] + n) % Convert.ToUInt64(n);

            return b;

        }

        /// <summary>
        /// Run the Extended Euclidean Algorithm on two numbers to find their highest common factor as a multiple of the two numbers
        /// </summary>
        /// <param name="a">First number (must be coprime with b)</param>
        /// <param name="b">Second number (must be coprime with a)</param>
        /// <returns>ulong[3] with the the highest common factor then two multipliers that a and b should be multiplied by (in respective order) to be summed to their highest common factor</returns>
        public static EEAReturnValues ExtendedEuclideanAlgorithm(ulong a, ulong b)
        {

            //Swap if b > a so division works well
            if (b > a)
            {
                ulong bMem = b;
                b = a;
                a = bMem;
            }

            EuclideanAlgorithmStep[] rawSteps = RecordEuclideanAlgorithm(a, b);
            ulong hcf = rawSteps[rawSteps.Length - 1].x;

            //Take all steps but final where 0 is reached
            EuclideanAlgorithmStep[] steps = new EuclideanAlgorithmStep[rawSteps.Length - 1];
            Array.Copy(rawSteps, steps, steps.Length);
            steps = steps.Reverse().ToArray();

            ExtendedEuclideanAlgorithmState state = new ExtendedEuclideanAlgorithmState(steps[0].a,
                1,
                steps[0].x,
                -Convert.ToInt64(steps[0].xCount));

            for (int stepIndex = 1; stepIndex < steps.Length; stepIndex++)
            {

                EuclideanAlgorithmStep step = steps[stepIndex];

                long newACount = state.aCount;
                newACount += -Convert.ToInt64(step.xCount) * state.bCount;

                ulong newB = step.a;

                state.aCount = newACount;
                state.b = newB;

                //state.b will always be the one to substitute
                state.SwapAB();

            }

            EEAReturnValues returnValues = new EEAReturnValues()
            {
                hcf = hcf,
                //Usage of state.SwapAB() taken into account
                aMultiplier = state.a == a ? state.aCount : state.bCount,
                bMultiplier = state.b == b ? state.bCount : state.aCount,
            };

            return returnValues;

        }

        public struct EEAReturnValues
        {
            public ulong hcf;
            public long aMultiplier;
            public long bMultiplier;
        }

        class ExtendedEuclideanAlgorithmState
        {

            /*
             * Format:
             * (HCF =) {a}({aCount}) + {b}({bCount})
             */

            public ulong a;
            public long aCount;
            public ulong b;
            public long bCount;

            public ExtendedEuclideanAlgorithmState(ulong a,
                long aCount,
                ulong b,
                long bCount)
            {
                this.a = a;
                this.aCount = aCount;
                this.b = b;
                this.bCount = bCount;
            }

            public void SwapAB()
            {

                ulong bMem = b;
                long bCountMem = bCount;

                b = a;
                bCount = aCount;

                a = bMem;
                aCount = bCountMem;

            }
        }

        public static EuclideanAlgorithmStep[] RecordEuclideanAlgorithm(ulong a, ulong b)
        {

            //Swap if b > a so division works well
            if (b > a)
            {
                ulong bMem = b;
                b = a;
                a = bMem;
            }

            List<EuclideanAlgorithmStep> steps = new List<EuclideanAlgorithmStep>();

            while (true)
            {

                ulong stepA = a;
                ulong stepX = b;
                ulong stepXCount = a / b; //Will return rounded-down ulong
                ulong stepExtra = a % b;

                EuclideanAlgorithmStep step = new EuclideanAlgorithmStep(stepA,
                    stepX,
                    stepXCount,
                    stepExtra);

                steps.Add(step);

                a = stepX;
                b = stepExtra;

                if (stepExtra == 0)
                {
                    break;
                }

            }

            return steps.ToArray();

        }

        public struct EuclideanAlgorithmStep
        {

            /*
             * Format:
             * {a} = {x}({xCount}) + {extra}
             */

            public ulong a;
            public ulong x;
            public ulong xCount;
            public ulong extra;

            public EuclideanAlgorithmStep(ulong a,
                ulong x,
                ulong xCount,
                ulong extra)
            {

                this.a = a;
                this.x = x;
                this.xCount = xCount;
                this.extra = extra;

                if (this.xCount > long.MaxValue)
                {
                    throw new Exception("xCount must be within long range to avoid overflow errors later on");
                }

            }
        }

        /// <summary>
        /// Gets the highest common factor of two numbers using the Euclidean Algorithm
        /// </summary>
        /// <param name="a">First number</param>
        /// <param name="b">Second number</param>
        /// <returns>Highest common factor of the two numbers</returns>
        public static ulong EuclideanAlgorithm(ulong a, ulong b)
        {

            //Swap if b > a so division works well
            if (b > a)
            {
                ulong bMem = b;
                b = a;
                a = bMem;
            }

            ulong rem = a % b;

            if (rem == 0)
            {
                return b;
            }
            else
            {
                return EuclideanAlgorithm(b, rem);
            }

        }

    }
}
