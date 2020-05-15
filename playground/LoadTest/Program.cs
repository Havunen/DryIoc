using System;

namespace LoadTest
{
    public class Program
    {
        static /*async Task*/void Main(string[] args)
        {
            for (int i = 1; i < 100; i++)
            {
                Console.WriteLine("iteration = " + i + " InvalidProgramExceptionTest! ****NO FEC***" + DateTime.Now);

                InvalidProgramExceptionTest.Start();

                Console.WriteLine("iteration = " + i + " SplitDependencyGraphTest! ****NO FEC***" + DateTime.Now);

                SplitDependencyGraphTest.Start();

                Console.WriteLine("iteration = " + i + " LoadTestBenchmark! ****NO FEC***" + DateTime.Now);

                LoadTestBenchmark.Start();

                Console.WriteLine("iteration = " + i + " Success! ****NO FEC*****" + DateTime.Now);




                Console.WriteLine("iteration = " + i + " InvalidProgramExceptionTest! ****FEC***" + DateTime.Now);

                InvalidProgramExceptionTest.Start2();

                Console.WriteLine("iteration = " + i + " SplitDependencyGraphTest! ****FEC***" + DateTime.Now);

                SplitDependencyGraphTest.Start2();

                Console.WriteLine("iteration = " + i + " LoadTestBenchmark! ****FEC***" + DateTime.Now);

                LoadTestBenchmark.Start2();

                Console.WriteLine("iteration = " + i + " Success! ****FEC*****" + DateTime.Now);
            }
        }
    }
}