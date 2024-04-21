using SimpleStepParser.StepFileRepresentation._1.Domain.Entities;
using SimpleStepParser.StepFileRepresentation._1.Domain;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace SimpleStepParser.StepFileRepresentation._2.Application.Parser;

internal static class StepRepresentationParser
{
    internal static StepRepresentation GetStepRepresentation(string[] stepFile)
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
        if (stepFile[i].Contains("ENDSEC;"))
        { 
            return stepRepresentation;
        }

        Regex entityStart = new Regex(@"^(?s)#(?<id>\d*)=\(*\n*(?<type>[A-Z0-9_]*)(?<body>.*)");
        for (; i < stepFile.Length; i++)
        {
            if (stepFile[i].Contains('='))
            {
                StringBuilder entitySb = new(stepFile[i]);

                while (true)
                {
                    if(stepFile[i + 1].Contains('='))
                    {
                        break;
                    }
                    else if(stepFile[i + 1].Contains("ENDSEC;"))
                    {
                        break;
                    }
                    else
                    {
                        i++;
                        entitySb.Append(stepFile[i]);
                    }
                }

                string entity = entitySb.ToString();

                var m = entityStart.Match(entity);
                if (!m.Success)
                {
                    throw new Exception("Error during parsing");
                }

                int id = int.Parse(m.Groups["id"].Value);
                string entityType = m.Groups["type"].Value;
                string entityBody = m.Groups["body"].Value;

                switch (entityType)
                {
                    case "CARTESIAN_POINT":
                        {
                            var currentEntity = StepEntityParser.TryParseToStepCartesianPoint(id, entityBody);
                            if (currentEntity != null)
                            {
                                stepRepresentation.StepCartesianPoints!.Add(currentEntity);
                            }
                            break;
                        }
                    case "AXIS2_PLACEMENT_3D":
                        {
                            var currentEntity = StepEntityParser.TryParseToStepAxis2Placement3D(id, entityBody);
                            if (currentEntity != null)
                            {
                                stepRepresentation.StepAxis2Placements3D!.Add(currentEntity);
                            }
                            break;
                        }
                    case "DIRECTION":
                        {
                            var currentEntity = StepEntityParser.TryParseToStepDirection(id, entityBody);
                            if(currentEntity != null)
                            {
                                stepRepresentation.StepDirections!.Add(currentEntity);
                            }
                            break;
                        }
                    case "ITEM_DEFINED_TRANSFORMATION":
                        {
                            var currentEntity = StepEntityParser.TryParseToStepItemDefinedTransformation(id, entityBody);
                            if (currentEntity != null)
                            {
                                stepRepresentation.StepItemDefinedTransformations!.Add(currentEntity);
                            }
                            break;
                        }
                    case "REPRESENTATION_RELATIONSHIP":
                        {
                            var currentEntity = StepEntityParser.TryParseToStepRepresentationRelationshipWithTransformation(id, entityBody);
                            if (currentEntity != null)
                            {
                                stepRepresentation.StepRepresentationsRelationshipWithTransformation!.Add(currentEntity);
                            }
                            break;
                        }
                    case "SHAPE_REPRESENTATION":
                        {
                            var currentEntity = StepEntityParser.TryParseToStepShapeRepresentation(id, entityBody);
                            if (currentEntity != null)
                            {
                                stepRepresentation.StepShapeRepresentations!.Add(currentEntity);
                            }
                            break;
                        }
                }

            }
            else if(stepFile[i].Contains("ENDSEC;"))
            {
                break;
            }
            else
            {
                continue;
            }
        }

        return stepRepresentation;
    }
}
