using System;
using System.Reflection;
using System.Transactions;
using BenchmarkDotNet.Running;
using FizzBuzz.SIMD;

namespace FizzBuzz
{
    internal class Program
    {
        static void Main(string[] args)
        {


            BenchmarkSwitcher.FromAssembly(Assembly.GetExecutingAssembly()).Run(args);

            //BenchmarkRunner.Run<BenchmarkCollection>();

            //BenchmarkRunner.Run<SIMD.BenchmarkCollection>();
        }
    }





    
}