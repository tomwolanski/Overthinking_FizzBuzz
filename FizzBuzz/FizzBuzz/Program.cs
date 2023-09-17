using System;
using System.Transactions;
using BenchmarkDotNet.Running;
using FizzBuzz.SIMD;

namespace FizzBuzz
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //using var ff = new ILGPUFizzBuzzer();

            //var arr = Enumerable.Range(1, 10_000).ToArray();

            //var fff = ff.Execute(arr);

            BenchmarkRunner.Run<BenchmarkCollection>();
        }
    }





    
}