namespace FizzBuzz.Configurable
{
    public sealed class StringsAppendFizzBuzzer
    {
        private readonly ModuloOption[] _options;

        public StringsAppendFizzBuzzer(params ModuloOption[] options)
        {
            _options = options.OrderBy(op => op.Divider).ToArray();
        }

        public string Execute(int value)
        {
            var s = string.Empty;

            for (int i = 0; i < _options.Length; i++)
            {
                var option = _options[i];
                var modulo = value % option.Divider;
                if (modulo == 0)
                {
                    s += option.Text;
                }
            }

            var result = s.Length != 0
                ? s
                : value.ToString();

            return result;
        }
    }
}