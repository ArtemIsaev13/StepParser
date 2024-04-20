using SimpleStepParser.SimplifiedModelRepresentation._1.Domain;
using SimpleStepParser.SimplifiedModelRepresentation._2.Application;
using SimpleStepParser.StepFileRepresentation._1.Domain;
using SimpleStepParser.StepFileRepresentation._2.Application.Parser;
using System.Text;

namespace SimpleStepParser
{
    public static class SimpleStepParser
    {
        public static Model? ReadStepFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }
            var file = File.ReadAllLines(path, Encoding.UTF8);
            StepRepresentation representation 
                = StepRepresentationParser.GetStepRepresentation(file);
            Model? result = ModelInterpretator.GetModelTree(representation);
            return result;
        }
    }
}
