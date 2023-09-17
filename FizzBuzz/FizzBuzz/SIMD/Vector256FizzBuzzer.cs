using ILGPU;
using ILGPU.Runtime;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace FizzBuzz.SIMD
{
    public sealed class Vector256FizzBuzzer
    {
        // Depending on CPY this will vary. Should contain 8 integers on 64bit OS
        static readonly int VectorSize = Vector256<int>.Count;

        // a sequence of [ 0, 1, 2, 3, 4, 5, 6, 7 ] used to generate next vectors
        static readonly Vector256<int> Sequence = Vector256.Create(Enumerable.Range(0, VectorSize).ToArray());

        public Vector256FizzBuzzer()
        {
            if (!Vector256.IsHardwareAccelerated)
            {
                throw new InvalidOperationException("Vector256 not supported");
            }
        }

        public FizzBuzzResultEnum[] Execute(int n)
        {
            var output = new FizzBuzzResultEnum[n];
            
            int i;

            // Iterate over vectors that contain vectorSize of integers. Perform CalculateFizBuzz on each of them;
            for (i = 0; i < n - VectorSize; i += VectorSize)
            {
                var outSpan = output.AsSpan().Slice(i, VectorSize);

                CalculateFizBuzz(i, outSpan);
            }

            // If any value was left, perform a failback
            for (; i < n; i++)
            {
                output[i] = (i % 3, i % 5) switch
                {
                    (0, 0) => FizzBuzzResultEnum.FizzBuzz,
                    (0, _) => FizzBuzzResultEnum.Fizz,
                    (_, 0) => FizzBuzzResultEnum.Buzz,
                    _ => (FizzBuzzResultEnum)i
                };
            }

            return output;
        }

        static void CalculateFizBuzz(int index, Span<FizzBuzzResultEnum> output)
        {
            // Generate next vector by 
            var input = Vector256.Create(index) + Sequence;

            // Calculate new vectors, multiply them by our enum value
            // Fizz calculation:
            // input:  [1, 2, 3, 4, 5, 6], divider: 3, FizzBuzzEnum.Fizz: -1
            // result: [0, 0,-1, 0, 0,-1]
            // Bazz calculation:
            // input:  [1, 2, 3, 4, 5, 6], divider: 5, FizzBuzzEnum.Bazz: -2
            // result: [0, 0, 0, 0,-2, 0]
            Vector256<int> fizzValues = Math.Abs((int)FizzBuzzResultEnum.Fizz) * CalcIsDivisible(input, 3);
            Vector256<int> buzzValues = Math.Abs((int)FizzBuzzResultEnum.Buzz) * CalcIsDivisible(input, 5);

            // Our enum is designed in such a way that Fizz + Bazz = FizzBazz
            // To calculate results, we can just add moth vectors
            // input:      [1, 2, 3, 4, 5, 6, .. 10]
            // fizzValues: [0, 0,-1, 0, 0,-1, .. -1]
            // bazzValues: [0, 0, 0, 0,-2, 0, .. -2]
            // result:     [0, 0,-1, 0,-2, 0, ..,-3]
            var fizzBuzzValues = Vector256.Add(fizzValues, buzzValues);

            // Select values depending on the fizzBuzzValues vector.
            // If 0 select original, value, otherwise select value from fizzBuzzValues
            var result = Vector256.ConditionalSelect(
                Vector256.Equals(fizzBuzzValues, Vector256<int>.Zero),
                input,
                fizzBuzzValues);

            // Transform vector to Span<FizzBuzzEnum> and copy to output
            Span<int> resultSpan = stackalloc int[Vector256<int>.Count];
            result.CopyTo(resultSpan);
            MemoryMarshal.Cast<int, FizzBuzzResultEnum>(resultSpan).CopyTo(output);
        }

        /// <summary>
        /// Using formula 'x - m * floor(x / m)' a new vector is calculated with values set if input is divisible by divider.
        /// </summary>
        /// for instance:
        /// input: [1, 2, 3, 4, 5, 6], divider: 3
        /// result [0, 0,-1, 0, 0,-1]
        static Vector256<int> CalcIsDivisible(Vector256<int> input, int divider)
        {
            var x = Vector256.ConvertToSingle(input);
            var m = Vector256.Create((float)divider);

            var r = Vector256.ConvertToInt32(x - m * Vector256.Floor(x / m));

            return Vector256.Equals(r, Vector256<int>.Zero);
        }
    }
}