// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

Console.WriteLine("Hello, World!");

Stopwatch stopwatch = new Stopwatch();
stopwatch.Start();
SimpleStepParser.Parser.SimpleStepParser.ReadStepFile(@"Doc\OurTubeSecondAssembly.stp");
stopwatch.Stop();

Console.WriteLine($"Execution time in ms: {stopwatch.ElapsedMilliseconds}");

Console.ReadLine();