using FizzBuzz.SIMD;
using ILGPU.Runtime;

namespace FizzBuzz.Tests
{
    [TestClass]
    public class SIMDTests
    {
        const int N = 100;

        [TestMethod]
        public void BaselineFizzBuzzer()
        {
            var buzzer = new BaselineFizzBuzzer();

            buzzer.Execute(N)
                  .AssertFizzBuzzSequence();
        }

        [TestMethod]
        [DataRow(AcceleratorType.CPU)]
        [DataRow(AcceleratorType.OpenCL)]
        [DataRow(AcceleratorType.Cuda)]
        public void ILGPUFizzBuzzer(AcceleratorType acceleratorType)
        {
            using var buzzer = new ILGPUFizzBuzzer(acceleratorType);

            buzzer.Execute(N)
                  .AssertFizzBuzzSequence();
        }

        [TestMethod]
        public void Vector256FizzBuzzer()
        {
            var buzzer = new Vector256FizzBuzzer();

            buzzer.Execute(N)
                  .AssertFizzBuzzSequence();
        }
    }

}