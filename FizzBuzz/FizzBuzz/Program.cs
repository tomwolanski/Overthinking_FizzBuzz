using System.Reflection;
using BenchmarkDotNet.Running;

namespace FizzBuzz
{
    public class Program
    {
        static void Main(string[] args)
        {
            //using var fb = new ILGPUFizzBuzzer();
            //var fb = new Vector256FizzBuzzer();
            //var fb = new BaselineFizzBuzzer();

            //foreach (var f in  fb.Execute(100))
            //{
            //    Console.WriteLine(f);
            //}

            BenchmarkSwitcher.FromAssembly(Assembly.GetExecutingAssembly()).Run(args);
        }
    }
}