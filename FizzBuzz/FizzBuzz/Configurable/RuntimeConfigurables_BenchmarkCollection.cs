using BenchmarkDotNet.Attributes;

namespace FizzBuzz.Configurable
{
    [MemoryDiagnoser]
    [BenchmarkCategory("configurable")]
    [HtmlExporter, RPlotExporter, MarkdownExporter]
    public class RuntimeConfigurables_BenchmarkCollection
    {
        [Params(100, 10_000, 10_000_000)]
        public int N { get; set; }

        private int[] _values = Array.Empty<int>();

        [ParamsSource(nameof(ValuesForOptions))]
        public ModuloOption[] Options { get; set; }

        public static IEnumerable<ModuloOption[]> ValuesForOptions()
        {
            yield return new ModuloOption[] { new(3, "Fizz"), new(5, "Buzz") };
            yield return new ModuloOption[] { new(3, "Fizz"), new(5, "Buzz"), new(7, "Fazz") };
            yield return new ModuloOption[] { new(3, "Fizz"), new(5, "Buzz"), new(7, "Fazz"), new(11, "Yazz"), new(13, "Zazz") };
        }


        private StringsAppendFizzBuzzer _stringAppendImpl;
        private StringBuilderFizzBuzzer _stringBuilderImpl;
        private PooledStringBuilderFizzBuzzer _pooledStringBuilderImpl;
        private StackAllocFizzBuzzer _stackAllockImpl;
        private ExpressionTreesFizzBuzzer _expressionTreeImpl;
        private CommunityToolkitStringPoolFizzBuzzer _comuunityToolkitStringPoolImpl;
        
        [GlobalSetup]
        public void Setup()
        {
            _values = Enumerable.Range(1, N).ToArray();

            _stringAppendImpl = new StringsAppendFizzBuzzer(Options);
            _stringBuilderImpl = new StringBuilderFizzBuzzer(Options);
            _pooledStringBuilderImpl = new PooledStringBuilderFizzBuzzer(Options);
            _stackAllockImpl = new StackAllocFizzBuzzer(Options);
            _expressionTreeImpl = new ExpressionTreesFizzBuzzer(Options);
            _comuunityToolkitStringPoolImpl = new CommunityToolkitStringPoolFizzBuzzer(Options);
        }

        [Benchmark(Baseline = true)]
        public int StringsAppend()
        {
            return _values.Select(_stringAppendImpl.Execute).Count();
        }

        [Benchmark]
        public int StringBuilder()
        {
            return _values.Select(_stringBuilderImpl.Execute).Count();
        }

        [Benchmark()]
        public int PooledStringBuilder()
        {
            return _values.Select(_pooledStringBuilderImpl.Execute).Count();
        }

        [Benchmark()]
        public int StackAlloc()
        {
            return _values.Select(_stackAllockImpl.Execute).Count();
        }

        [Benchmark()]
        public int StackAllocWithToolkitStringPool()
        {
            return _values.Select(_comuunityToolkitStringPoolImpl.Execute).Count();
        }


        [Benchmark()]
        public int ExpressionTrees()
        {
            return _values.Select(_expressionTreeImpl.Execute).Count();
        }

    }




}