// See https://aka.ms/new-console-template for more information
using SimpleStepParser.SimplifiedModelRepresentation.Domain;
using SimpleStepParser.SimplifiedModelRepresentation.Application;
using System.Diagnostics;

Console.WriteLine("Hello, World!");

string StepExamplesFolder = "StepExamples";
string[] testFiles = { 
    Path.Combine(StepExamplesFolder, "myfirstassembly_asm.stp"),

    Path.Combine(StepExamplesFolder, "SiemensNx11_step_ap203.stp"),
    Path.Combine(StepExamplesFolder, "SiemensNx11_step_ap214.stp"),
    Path.Combine(StepExamplesFolder, "SiemensNx11_step_ap242.stp"),
    Path.Combine(StepExamplesFolder, "SiemensNx11_step_ap242_cyrillic.stp"),

    Path.Combine(StepExamplesFolder, "Solid - 3(2)_Assembly_STEP242.STP"),
    Path.Combine(StepExamplesFolder, "Assembly_map.stp"),
    Path.Combine(StepExamplesFolder, "Solid - 3(2)_Assembly_STEP214.STEP")
};

Stopwatch stopwatch = new Stopwatch();

Assembly? assembly = null;

int numOfTests = 1;
stopwatch.Start();
for (int i = 0; i < numOfTests; i++)
{
    foreach (var testFile in testFiles)
    {
        assembly = SimpleStepParser.SimpleStepParser.ReadStepFile(testFile);
        Console.WriteLine(assembly?.Root?.GetModelTreeString() ?? string.Empty);
    }
}
stopwatch.Stop();

Console.WriteLine($"Execution time in ms: {stopwatch.ElapsedMilliseconds/numOfTests}");

Console.ReadLine();