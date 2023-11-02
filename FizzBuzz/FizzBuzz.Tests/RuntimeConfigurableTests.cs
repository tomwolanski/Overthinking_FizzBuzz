using FizzBuzz.Configurable;

namespace FizzBuzz.Tests
{
    [TestClass]
    public class RuntimeConfigurableTests
    {
        const int N = 100;
        ModuloOption[] _options = new ModuloOption[] { new(3, "Fizz"), new(5, "Buzz") };

        [TestMethod]
        public void ExpressionTreesFizzBuzzer()
        {
            var buzzer = new ExpressionTreesFizzBuzzer(_options);

            Enumerable.Range(0, N)
                .Select(buzzer.Execute)
                .AssertFizzBuzzSequence();
        }

        [TestMethod]
        public void CommunityToolkitStringPoolFizzBuzzer()
        {
            var buzzer = new CommunityToolkitStringPoolFizzBuzzer(_options);

            Enumerable.Range(0, N)
                .Select(buzzer.Execute)
                .AssertFizzBuzzSequence();
        }

        [TestMethod]
        public void PooledStringBuilderFizzBuzzer()
        {
            var buzzer = new PooledStringBuilderFizzBuzzer(_options);

            Enumerable.Range(0, N)
                .Select(buzzer.Execute)
                .AssertFizzBuzzSequence();
        }

        [TestMethod]
        public void StackAllocFizzBuzzer()
        {
            var buzzer = new StackAllocFizzBuzzer(_options);

            Enumerable.Range(0, N)
                .Select(buzzer.Execute)
                .AssertFizzBuzzSequence();
        }

        [TestMethod]
        public void StringBuilderFizzBuzzer()
        {
            var buzzer = new StringBuilderFizzBuzzer(_options);

            Enumerable.Range(0, N)
                .Select(buzzer.Execute)
                .AssertFizzBuzzSequence();
        }

        [TestMethod]
        public void StringsAppendFizzBuzzer()
        {
            var buzzer = new StringsAppendFizzBuzzer(_options);

            Enumerable.Range(0, N)
                .Select(buzzer.Execute)
                .AssertFizzBuzzSequence();
        }

    }

}