namespace FizzBuzz.SIMD
{
    public sealed class BaselineFizzBuzzer
    {
        public FizzBuzzResultEnum[] Execute(int[] input)
        {
            var output = new FizzBuzzResultEnum[input.Length];

            for (int i=0; i< input.Length; i++)
            {
                var value = input[i];

                output[i] = (value % 3, value % 5) switch
                {
                    (0, 0) => FizzBuzzResultEnum.FizzBuzz,
                    (0, _) => FizzBuzzResultEnum.Fizz,
                    (_, 0) => FizzBuzzResultEnum.Buzz,
                    _ => (FizzBuzzResultEnum)value
                };
            }

            return output;
        }
    }
}