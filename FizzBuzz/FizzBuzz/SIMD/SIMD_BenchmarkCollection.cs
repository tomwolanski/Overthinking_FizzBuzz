using BenchmarkDotNet.Attributes;

namespace FizzBuzz.SIMD
{
    [BenchmarkCategory("simd")]
    [HtmlExporter, RPlotExporter, MarkdownExporter]
    public class SIMD_BenchmarkCollection 
    {
        [Params(100, 10_000, 1_000_000, 500_000_000)]
        public int N { get; set; }

        private BaselineFizzBuzzer _baseline = new BaselineFizzBuzzer();
        private Vector256FizzBuzzer _vector = new Vector256FizzBuzzer();
        private ILGPUFizzBuzzer _gpu = new ILGPUFizzBuzzer();

        [Benchmark(Baseline = true)]
        public object BaseLineCPULoop()
        {
            return _baseline.Execute(N);
        }

        [Benchmark]
        public object Vector256()
        {
            return _vector.Execute(N);
        }

        [Benchmark()]
        public object GPU()
        {
            return _gpu.Execute(N);
        }

    }
}