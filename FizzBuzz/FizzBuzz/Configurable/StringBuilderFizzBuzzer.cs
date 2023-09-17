using System.Text;

namespace FizzBuzz.Configurable
{
    public sealed class StringBuilderFizzBuzzer
    {
        private readonly ModuloOption[] _options;

        public StringBuilderFizzBuzzer(params ModuloOption[] options)
        {
            _options = options.OrderBy(op => op.Divider).ToArray();
        }

        public string Execute(int value)
        {
            StringBuilder? sb = null;

            foreach (var option in _options)
            {
                var modulo = value % option.Divider;
                if (modulo == 0)
                {
                    sb ??= new StringBuilder();

                    sb.Append(option.Text);
                }
            }

            var result = sb is not null
                ? sb.ToString()
                : value.ToString();

            return result;
        }
    }
}