using SimpleStepParser.StepFileRepresentation._1.Domain;
using SimpleStepParser.StepFileRepresentation._1.Domain.Entities;
using System.Text;
using System.Text.RegularExpressions;

namespace SimpleStepParser.StepFileRepresentation.Parser
{
    public static class SimpleStepParser
    {
        public static void ReadStepFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }
            var file = File.ReadAllLines(path, Encoding.UTF8);
            var representation = GetStepRepresentation(file);
            ;
        }

        private static StepRepresentation GetStepRepresentation(string[] stepFile)
        {
            if (!stepFile[0].Contains("ISO-10303-21;") ||
                !stepFile[stepFile.Length - 1]
                .Contains("END-ISO-10303-21;"))
            {
                throw new ArgumentException("This is not a ISO-10303-21 file");
            }

            int i = 0;
            //Skip to header
            for (; i < stepFile.Length; i++)
            {
                if (stepFile[i].Contains("HEADER;"))
                {
                    i++;
                    break;
                }
            }
            //Saving header
            StringBuilder header = new();
            for (; i < stepFile.Length; i++)
            {
                if (stepFile[i].Contains("ENDSEC;"))
                {
                    i++;
                    break;
                }
                header.AppendLine(stepFile[i]);
            }
            //Skip to data
            for (; i < stepFile.Length; i++)
            {
                if (stepFile[i].Contains("DATA;"))
                {
                    i++;
                    break;
                }
            }
            //Saving data
            StepRepresentation stepRepresentation = new() { Header = header.ToString() };
            Regex entityStart = new Regex(@"^#(?<id>\d*)=(?<body>.*)");
            for (; i < stepFile.Length; i++)
            {
                //if the line is the end of data section
                if (stepFile[i].Contains("ENDSEC;"))
                {
                    break;
                }

                var m = entityStart.Match(stepFile[i]);
                if (!m.Success)
                {
                    continue;
                }
                int id = int.Parse(m.Groups["id"].Value);

                StringBuilder body = new();
                body.Append(m.Groups["body"].Value);

                int bracketNum =
                    stepFile[i].Count(x => x == '(') -
                    stepFile[i].Count(x => x == ')');
                while (bracketNum > 0)
                {
                    i++;
                    body.Append(stepFile[i]);
                    bracketNum +=
                    stepFile[i].Count(x => x == '(') -
                    stepFile[i].Count(x => x == ')');
                }

                var undefinedStepEntity =
                    new UndefinedStepEntity()
                    {
                        Id = id,
                        Body = body.ToString()
                    };
                AbstractStepEntity? currentEntity = null;

                if (!string.IsNullOrEmpty(undefinedStepEntity.Body))
                {
                    //Sorted by frequency of occurrence in the common step file
                    if ((currentEntity = TryParseToStepCartesianPoint(undefinedStepEntity)) != null)
                    {
                        stepRepresentation.StepCartesianPoints?.Add((StepCartesianPoint)currentEntity);
                    }
                    else if ((currentEntity = TryParseToStepDirection(undefinedStepEntity)) != null)
                    {
                        stepRepresentation.StepDirections?.Add((StepDirection)currentEntity);
                    }
                    else
                    {
                        stepRepresentation.UndefinedStepEntities?.Add(undefinedStepEntity);
                    }
                }
            }

            return stepRepresentation;
        }


        private static readonly Regex _stepDirectionRegex
            = new Regex(@"^DIRECTION\('(?<name>.*)',\((?<i>\d*.\d*),(?<j>\d*.\d*),(?<k>\d*.\d*)\)\);");

        private static StepDirection? TryParseToStepDirection(UndefinedStepEntity from)
        {
            if(from.Body == null)
            {
                return null;
            }

            var match = _stepDirectionRegex.Match(from.Body);
            if (!match.Success)
            {
                return null;
            }

            StepDirection result = new StepDirection()
            {
                Id = from.Id,
                Name = match.Groups["name"].Value ?? string.Empty,
                I = float.Parse(match.Groups["i"].Value),
                J = float.Parse(match.Groups["j"].Value),
                K = float.Parse(match.Groups["k"].Value),
            };

            return result;
        }

        private static readonly Regex _stepCartesianPoint
            = new Regex(@"^CARTESIAN_POINT\('(?<name>.*)',\((?<x>\d*.\d*),(?<y>\d*.\d*),(?<z>\d*.\d*)\)\);");
        
        private static StepCartesianPoint? TryParseToStepCartesianPoint(UndefinedStepEntity from)
        {
            if (from.Body == null)
            {
                return null;
            }

            var match = _stepCartesianPoint.Match(from.Body);
            if (!match.Success)
            {
                return null;
            }

            StepCartesianPoint result = new StepCartesianPoint()
            {
                Id = from.Id,
                Name = match.Groups["name"].Value ?? string.Empty,
                X = float.Parse(match.Groups["x"].Value),
                Y = float.Parse(match.Groups["y"].Value),
                Z = float.Parse(match.Groups["z"].Value),
            };

            return result;
        }

    }
}
