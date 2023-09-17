using ILGPU;
using ILGPU.Runtime;

namespace FizzBuzz.SIMD
{
    public sealed class ILGPUFizzBuzzer : IDisposable
    {
        private readonly Context _context;
        private readonly Accelerator _accelerator;

        private readonly Action<Index1D, ArrayView<int>, ArrayView<FizzBuzzResultEnum>> _kernel;

        public ILGPUFizzBuzzer()
        {
            // Initialize ILGPU.
            _context = Context.CreateDefault();
            _accelerator = _context.GetPreferredDevice(preferCPU: false).CreateAccelerator(_context);

            // load / precompile the kernel
            _kernel = _accelerator.LoadAutoGroupedStreamKernel<Index1D, ArrayView<int>, ArrayView<FizzBuzzResultEnum>>(Kernel);

            
        }

        public FizzBuzzResultEnum[] Execute(int[] input)
        {
            var diviceInput = _accelerator.Allocate1D<int>(input);
            var deviceOutput = _accelerator.Allocate1D<FizzBuzzResultEnum>(input.Length);

            _kernel((int)diviceInput.Length, diviceInput.View, deviceOutput.View);

            _accelerator.Synchronize();
            return deviceOutput.GetAsArray1D();
        }

        static void Kernel(Index1D i, ArrayView<int> input, ArrayView<FizzBuzzResultEnum> output)
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

        public void Dispose()
        {
            _accelerator.Dispose();
            _context.Dispose();
        }
    }
}