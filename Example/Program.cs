// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

Console.WriteLine("Hello, World!");

string StepExamplesFolder = "StepExamples";
string SiemensNx11StepAp242 = Path.Combine(StepExamplesFolder, "SiemensNx11_step_ap242.stp");

Stopwatch stopwatch = new Stopwatch();

int numOfTests = 10;
stopwatch.Start();
for (int i = 0; i < numOfTests; i++)
{
    SimpleStepParser.StepFileRepresentation.Parser.SimpleStepParser.ReadStepFile(SiemensNx11StepAp242);
}
stopwatch.Stop();

Console.WriteLine($"Execution time in ms: {stopwatch.ElapsedMilliseconds/numOfTests}");

Console.ReadLine();