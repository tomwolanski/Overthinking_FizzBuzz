using BenchmarkDotNet.Attributes;

namespace FizzBuzz.SIMD
{
    [MemoryDiagnoser]
    [BenchmarkCategory("simd")]
    public class SIMD_BenchmarkCollection
    {
        //[Params(100, 10_000, 1_000_000)]

        [Params(1_000_000_000)]
        public int N { get; set; }

        private int[] _values = Array.Empty<int>();


        private BaselineFizzBuzzer _baseline = new BaselineFizzBuzzer();
        private Vector256FizzBuzzer _vector = new Vector256FizzBuzzer();
        private ILGPUFizzBuzzer _gpu = new ILGPUFizzBuzzer();
        
        [GlobalSetup]
        public void Setup()
        {
            _values = Enumerable.Range(1, N).ToArray();
        }

        [Benchmark(Baseline = true)]
        public object BaseLineCPULoop()
        {
            return _baseline.Execute(_values);
        }

        [Benchmark]
        public object Vector256()
        {
            return _vector.Execute(_values);
        }

        [Benchmark()]
        public object GPU()
        {
            return _gpu.Execute(_values);
        }
    }




}