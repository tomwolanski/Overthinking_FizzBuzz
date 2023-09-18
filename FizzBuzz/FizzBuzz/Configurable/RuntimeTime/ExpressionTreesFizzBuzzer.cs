using System.Linq.Expressions;

namespace FizzBuzz.Configurable
{
    public sealed class ExpressionTreesFizzBuzzer
    {
        private readonly Func<int, string> _impl;

        public ExpressionTreesFizzBuzzer(params ModuloOption[] options)
        {
            _impl = GenerateExpression(options).Compile();
        }

        public string Execute(int value) => _impl(value);

        private static Expression<Func<int, string>> GenerateExpression(IReadOnlyList<ModuloOption> options)
        {
            // define input parameter of our method
            var parameterExpression = Expression.Parameter(typeof(int), "value");

            // according to System.Linq.Expressions.Expression a return statement is just a glorified goto,
            // it needs to have a label and target we can reference later
            var returnTarget = Expression.Label(typeof(string));
            var returnExpression = Expression.Label(
                returnTarget,
                Expression.Constant(string.Empty)); // return empty string as default value, needed just to satisfy Expression.Label(...) method

            var bodyExpression = Expression.Block(
                GenerateOptionsMatchingExpression(options, parameterExpression, returnTarget, 0, null),
                returnExpression);

            return Expression.Lambda<Func<int, string>>(bodyExpression, parameterExpression);
        }

        // Recursively visit all options and build expression tree containing nested If Else statements:
        private static Expression GenerateOptionsMatchingExpression(
            IReadOnlyList<ModuloOption> options,
            Expression parameterExpr,
            LabelTarget returnTarget,
            int currentIndex,
            string? returnString)
        {
            // we are not finished with options, keep building the tree, concatenating the texts of matched options
            if (currentIndex < options.Count)
            {
                var (divider, text) = options[currentIndex];

                var nextIndex = currentIndex + 1;

                return Expression.IfThenElse(
                    GetIsDivisibleBy(parameterExpr, divider),
                    GenerateOptionsMatchingExpression(options, parameterExpr, returnTarget, nextIndex, returnString + text),
                    GenerateOptionsMatchingExpression(options, parameterExpr, returnTarget, nextIndex, returnString));
            }
            // we finished all options, return string that contains concatenated option texts
            else if (returnString is not null)
            {
                return Expression.Return(
                    returnTarget,
                    Expression.Constant(returnString));
            }
            // we did not matched any options, return value as string
            else
            {
                return Expression.Return(
                    returnTarget,
                    GetValueToStringExpression(parameterExpr));
            }
        }

        // returns an expression of "value % divider == 0"
        private static Expression GetIsDivisibleBy(Expression parameterExpr, int divider)
        {
            return  Expression.Equal(
                        Expression.Modulo(
                            parameterExpr,
                            Expression.Constant(divider)),
                        Expression.Constant(0));
        }

        // returns an expression of "value.ToString()"
        private static Expression GetValueToStringExpression(Expression parameterExpr)
        {
            var toStringMethod = typeof(int).GetMethod("ToString", Array.Empty<Type>())
                ?? throw new InvalidOperationException("ToString() not found");
            return Expression.Call(parameterExpr, toStringMethod);
        }
    }
}