﻿namespace FizzBuzz.Configurable
{
    public sealed class CommunityToolkitStringPoolFizzBuzzer
    {
        private readonly ModuloOption[] _options;
        private readonly int _capacity;

        private static readonly CommunityToolkit.HighPerformance.Buffers.StringPool _pool = new CommunityToolkit.HighPerformance.Buffers.StringPool();

        public CommunityToolkitStringPoolFizzBuzzer(params ModuloOption[] options)
        {
            _options = options.OrderBy(op => op.Divider).ToArray();
            _capacity = _options.Sum(op => op.Text.Length);

            // stack has limitations, so lets assume we do not have more than 100 characters max
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
                ? _pool.GetOrAdd(buffer[..length])
                : value.ToString();

        }
    }
}