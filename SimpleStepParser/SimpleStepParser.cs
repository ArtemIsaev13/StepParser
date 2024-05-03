using SimpleStepParser.SimplifiedModelRepresentation._1.Domain;
using SimpleStepParser.SimplifiedModelRepresentation._2.Application;
using SimpleStepParser.StepFileRepresentation._1.Domain.StepRepresentation;
using SimpleStepParser.StepFileRepresentation._2.Application.Parser;
using System.Diagnostics;
using System.Text;

namespace SimpleStepParser
{
    public static class SimpleStepParser
    {
        public static Model? ReadStepFile(string path)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }
            var file = File.ReadAllLines(path, Encoding.UTF8);
            Console.WriteLine($"File is readed {stopwatch.ElapsedMilliseconds}");
            stopwatch.Restart();

            StepRepresentation representation 
                = StepRepresentationParser.GetStepRepresentation(file);
            Console.WriteLine($"Step representation created {stopwatch.ElapsedMilliseconds}");
            stopwatch.Restart();

            Model? result = ModelInterpretator.GetModelTree(representation);
            Console.WriteLine($"Model created {stopwatch.ElapsedMilliseconds}\n");
            return result;
        }
    }
}
