using System.Text;
using Microsoft.Extensions.ObjectPool;

namespace FizzBuzz.Configurable
{
    public sealed class PooledStringBuilderFizzBuzzer
    {
        private static readonly ObjectPool<StringBuilder> _stringBuilderPool = new DefaultObjectPoolProvider().CreateStringBuilderPool();

        private readonly ModuloOption[] _options;

        public PooledStringBuilderFizzBuzzer(params ModuloOption[] options)
        {
            _options = options.OrderBy(op => op.Divider).ToArray();
        }

        public string Execute(int value)
        {
            var sb = _stringBuilderPool.Get();

            try
            {
                for (int i = 0; i < _options.Length; i++)
                {
                    var option = _options[i];
                    var modulo = value % option.Divider;
                    if (modulo == 0)
                    {
                        sb.Append(option.Text);
                    }
                }

                return sb.Length != 0
                    ? sb.ToString()
                    : value.ToString();
            }
            finally
            {
                _stringBuilderPool.Return(sb);
            }
        }
    }
}