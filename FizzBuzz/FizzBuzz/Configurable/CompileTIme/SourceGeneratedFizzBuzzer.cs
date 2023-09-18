

namespace FizzBuzz.Configurable
{
    public sealed partial class SourceGeneratedFizzBuzzer
    {
        [FizzBuzzGenerator.ModuloOption(3, "Fizz")]
        [FizzBuzzGenerator.ModuloOption(5, "Buzz")]
        [FizzBuzzGenerator.ModuloOption(7, "Fazz")]
        public partial string Execute(int value);
    }
}