using BenchmarkDotNet.Attributes;

namespace FizzBuzz.Configurable
{
    [MemoryDiagnoser]
    [BenchmarkCategory("compiletime")]
    [HtmlExporter, RPlotExporter, MarkdownExporter]
    public class FinalConfigurables_BenchmarkCollection
    {
        [Params(100, 10_000, 10_000_000)]
        public int N { get; set; }

        private int[] _values = Array.Empty<int>();

        private StringsAppendFizzBuzzer _stringAppendImpl;
        private ExpressionTreesFizzBuzzer _expressionTreeImpl;
        private SourceGeneratedFizzBuzzer _sourceGeneratedImpl;

        [GlobalSetup]
        public void Setup()
        {
            _values = Enumerable.Range(1, N).ToArray();

            var options = new ModuloOption[] { new(3, "Fizz"), new(5, "Buzz"), new(7, "Fazz") };

            _stringAppendImpl = new StringsAppendFizzBuzzer(options);
            _expressionTreeImpl = new ExpressionTreesFizzBuzzer(options);
            _sourceGeneratedImpl = new SourceGeneratedFizzBuzzer();
    }

        [Benchmark(Baseline = true)]
        public int StringsAppend()
        {
            return _values.Select(_stringAppendImpl.Execute).Count();
        }

        [Benchmark]
        public int SourceGenerated()
        {
            return _values.Select(_sourceGeneratedImpl.Execute).Count();
        }

        [Benchmark()]
        public int ExpressionTrees()
        {
            return _values.Select(_expressionTreeImpl.Execute).Count();
        }

    }
}