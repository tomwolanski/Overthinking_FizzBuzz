using FizzBuzz.Configurable;

namespace FizzBuzz.Tests
{
    [TestClass]
    public class CompileTimeConfigurableTests
    {
        const int N = 100;
        
        [TestMethod]
        public void ExpressionTreesFizzBuzzer()
        {
            var buzzer = new ExpressionTreesFizzBuzzer(new(3, "Fizz"), new(5, "Buzz"));

            Enumerable.Range(0, N)
                .Select(buzzer.Execute)
                .AssertFizzBuzzSequence();
        }

        [TestMethod]
        public void SourceGeneratedFizzBuzzer()
        {
            var buzzer = new SourceGeneratedFizzBuzzerTestImpl();

            Enumerable.Range(0, N)
                .Select(buzzer.Execute)
                .AssertFizzBuzzSequence();
        }
    }

    public partial class SourceGeneratedFizzBuzzerTestImpl
    {
        [FizzBuzzGenerator.ModuloOption(3, "Fizz")]
        [FizzBuzzGenerator.ModuloOption(5, "Buzz")]
        public partial string Execute(int value);
    }

}