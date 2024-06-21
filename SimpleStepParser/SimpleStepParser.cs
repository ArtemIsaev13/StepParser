using SimpleStepParser.SimplifiedModelRepresentation.Domain;
using SimpleStepParser.SimplifiedModelRepresentation.Application;
using SimpleStepParser.StepFileRepresentation.Domain.StepRepresentation;
using SimpleStepParser.StepFileRepresentation.Application.Parser;
using System.Text;

namespace SimpleStepParser
{
    public static class SimpleStepParser
    {
        /// <summary>
        /// Get parsed step file as a model tree with coordinate systems
        /// </summary>
        /// <param name="fullStepFile">Full step file as an array of strings</param>
        /// <returns></returns>
        public static Assembly? Parse(string[] fullStepFile)
        {
            StepRepresentation representation
                = StepRepresentationParser.GetStepRepresentation(fullStepFile);

            CadName resultCadName = CadNameInterpretator.GetCadNameByHeader(representation.Header);
            StepVersion resultStepVersion = CadNameInterpretator.GetStepVersionByHeader(representation.Header);
            Model? resultRoot = ModelInterpretator.GetModelTree(representation);

            Assembly? result = new()
            {
                Root = resultRoot,
                CadName = resultCadName,
                StepVersion = resultStepVersion
            };

            return result;
        }

        /// <summary>
        /// Get parsed step file as a model tree with coordinate systems
        /// </summary>
        /// <param name="path">Path to step file</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static Assembly? ReadStepFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }
            var file = File.ReadAllLines(path, Encoding.UTF8);

            return Parse(file);
        }
    }
}
