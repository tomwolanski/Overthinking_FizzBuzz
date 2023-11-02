using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using ILGPU.Runtime;

namespace FizzBuzz.SIMD
{
    [BenchmarkCategory("simd")]
    [HtmlExporter, RPlotExporter, MarkdownExporter]
    [SimpleJob(RuntimeMoniker.Net60, baseline:true)]
    [SimpleJob(RuntimeMoniker.Net70)]
    [SimpleJob(RuntimeMoniker.Net80)]
    public class SIMD_BenchmarkCollection 
    {
        //[Params(/*100*/, 10_000, 1_000_000, 500_000_000)]
        public int N { get; set; } = 100;

        private BaselineFizzBuzzer _baseline = new BaselineFizzBuzzer();

#if NET7_0_OR_GREATER
        private Vector256FizzBuzzer _vector256 = new Vector256FizzBuzzer();
#endif

#if NET8_0_OR_GREATER
        private Vector512FizzBuzzer _vector512 = new Vector512FizzBuzzer();
#endif
        private ILGPUFizzBuzzer _gpu = new ILGPUFizzBuzzer(AcceleratorType.OpenCL);

        [Benchmark(Baseline = true)]
        public object BaseLineCPULoop()
        {
            return _baseline.Execute(N);
        }

#if NET7_0_OR_GREATER
        [Benchmark]
        public object Vector256()
        {
            return _vector256.Execute(N);
        }
#endif

#if NET8_0_OR_GREATER
        [Benchmark]
        public object Vector512()
        {
            return _vector512.Execute(N);
        }
#endif

        [Benchmark()]
        public object GPU()
        {
            return _gpu.Execute(N);
        }

    }
}