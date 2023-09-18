namespace FizzBuzz.SIMD
{
    public sealed class BaselineFizzBuzzer
    {
        public FizzBuzzResultEnum[] Execute(int count)
        {
            var output = new FizzBuzzResultEnum[count];

            for (int i=0; i< count; i++)
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
    }
}