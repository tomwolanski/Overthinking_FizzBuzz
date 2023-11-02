using FizzBuzz.SIMD;

namespace FizzBuzz.Tests
{
    static class AssertEx
    {
        public static void AssertFizzBuzzSequence(this IEnumerable<string> input)
        {
            var indexed = input.Select((v, i) => (v, i));

            foreach (var (v, i) in indexed)
            {
                if (i % 3 == 0 && i % 5 == 0)
                {
                    Assert.AreEqual("FizzBuzz", v);
                }
                else if (i % 3 == 0)
                {
                    Assert.AreEqual("Fizz", v);
                }
                else if (i % 5 == 0)
                {
                    Assert.AreEqual("Buzz", v);
                }
                else
                {
                    Assert.AreEqual(i.ToString(), v);
                }
            }
        }

        public static void AssertFizzBuzzSequence(this IEnumerable<FizzBuzzResultEnum> input)
        {
            var indexed = input.Select((v, i) => (v, i));

            foreach (var (v, i) in indexed)
            {
                if (i % 3 == 0 && i % 5 == 0)
                {
                    Assert.AreEqual(FizzBuzzResultEnum.FizzBuzz, v);
                }
                else if (i % 3 == 0)
                {
                    Assert.AreEqual(FizzBuzzResultEnum.Fizz, v);
                }
                else if (i % 5 == 0)
                {
                    Assert.AreEqual(FizzBuzzResultEnum.Buzz, v);
                }
                else
                {
                    Assert.AreEqual((FizzBuzzResultEnum)i, v);
                }
            }
        }
    }

}