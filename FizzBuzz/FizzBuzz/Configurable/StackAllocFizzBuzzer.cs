namespace FizzBuzz.Configurable
{
    public sealed class StackAllocFizzBuzzer
    {
        private readonly ModuloOption[] _options;
        private readonly int _capacity;

        public StackAllocFizzBuzzer(params ModuloOption[] options)
        {
            _options = options.OrderBy(op => op.Divider).ToArray();
            _capacity = _options.Sum(op => op.Text.Length);

            if (_capacity > 100) throw new InvalidOperationException("too big");
        }

        public string Execute(int value)
        {
            Span<char> buffer = stackalloc char[_capacity];
            int length = 0;

            for (int i = 0; i < _options.Length; i++)
            {
                var option = _options[i];

                var slice = buffer[length..];

                var modulo = value % option.Divider;
                if (modulo == 0)
                {
                    var txt = option.Text;
                    txt.AsSpan().CopyTo(slice);
                    length += txt.Length;
                }
            }

            return length > 0
                ? new string(buffer[..length])
                : value.ToString();

        }
    }
}