using System.Reflection;
using BenchmarkDotNet.Running;

namespace FizzBuzz
{
    public class Program
    {
        static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(Assembly.GetExecutingAssembly()).Run(args);
        }
    }
}