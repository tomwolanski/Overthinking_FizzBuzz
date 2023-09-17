using System.Linq.Expressions;

namespace FizzBuzz.Configurable
{
    public sealed class ExpressionTreesFizzBuzzer
    {
        private readonly Func<int, string> _impl;

        public ExpressionTreesFizzBuzzer(params ModuloOption[] options)
        {
            _impl = GenExpression(options).Compile();
        }

        public string Execute(int value) => _impl(value);

        private static Expression<Func<int, string>> GenExpression(IReadOnlyList<ModuloOption> options)
        {
            var parameterExpression = Expression.Parameter(typeof(int), "value");
            var returnTarget = Expression.Label(typeof(string));

            var returnExpression = Expression.Label(
                returnTarget,
                Expression.Constant(string.Empty));

            var bodyExpression = Expression.Block(
                GenExpression(options, parameterExpression, returnTarget, 0, null),
                returnExpression);

            return Expression.Lambda<Func<int, string>>(bodyExpression, parameterExpression);
        }



        private static Expression GenExpression(
            IReadOnlyList<ModuloOption> options,
            Expression parameterExpression,
            LabelTarget returnTarget,
            int currentIndex,
            string? returnString)
        {
            if (currentIndex < options.Count)
            {
                var (divider, text) = options[currentIndex];

                var nextIndex = currentIndex + 1;

                return Expression.IfThenElse(
                    GetIsDivisibleBy(parameterExpression, divider),
                    GenExpression(options, parameterExpression, returnTarget, nextIndex, returnString + text),
                    GenExpression(options, parameterExpression, returnTarget, nextIndex, returnString));
            }
            else if (returnString is not null)
            {
                return Expression.Return(
                    returnTarget,
                    Expression.Constant(returnString));
            }
            else
            {
                return Expression.Return(
                    returnTarget,
                    GetValueToStringExpression(parameterExpression));
            }
        }


        private static Expression GetIsDivisibleBy(Expression parameterExpression, int divider)
        {
            return Expression.Equal(
                        Expression.Modulo(
                            parameterExpression,
                            Expression.Constant(divider)),
                        Expression.Constant(0));
        }

        private static Expression GetValueToStringExpression(Expression parameterExpression)
        {
            var toStringMethod = typeof(int).GetMethod("ToString", Array.Empty<Type>()) ?? throw new InvalidOperationException("ToString not found");
            return Expression.Call(parameterExpression, toStringMethod);
        }


    }
}