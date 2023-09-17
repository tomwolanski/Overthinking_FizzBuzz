using ILGPU;
using ILGPU.Runtime;

namespace FizzBuzz.SIMD
{
    // any positive value should be threated as numeric value
    public enum FizzBuzzResultEnum
    {
        Fizz = -1,
        Buzz = -2,
        FizzBuzz = -3
    }
}