﻿using SimpleStepParser.StepFileRepresentation.Domain.Entities;
using System.Text;
using System.Text.RegularExpressions;
using SimpleStepParser.StepFileRepresentation.Domain.StepRepresentation;
using SimpleStepParser.StepFileRepresentation.Domain.Enums;

namespace SimpleStepParser.StepFileRepresentation.Application.Parser;

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
        while (!stepFile[i].Contains('='))
        {
            if (stepFile[i + 1].Contains("ENDSEC;"))
            {
                return stepRepresentation;
            }
            else
            {
                i++;
            }
        }

        Regex entityStart = new Regex(@"^(?s)#(?<id>\d*)=\(*\n*(?<type>[A-Z0-9_]*)(?<body>.*)",
            RegexOptions.Compiled);
        for (; i < stepFile.Length; i++)
        {
            StringBuilder? entitySb = null;

            bool endOfFile = false;
            while (true)
            {
                if (stepFile[i + 1].Contains('='))
                {
                    break;
                }
                else if (stepFile[i + 1].Contains("ENDSEC;"))
                {
                    endOfFile = true;
                    break;
                }
                else
                {
                    if(entitySb == null)
                    {
                        entitySb = new(stepFile[i]);
                    }
                    i++;
                    entitySb.Append(stepFile[i]);
                }
            }

            if(endOfFile)
            {
                break;
            }

            //Does the command have one line or many lines?
            string entity = entitySb == null ? stepFile[i] : entitySb.ToString();
            entity = DeleteWhiteSpaces(entity);

            var m = entityStart.Match(entity);
            if (!m.Success)
            {
                //TODO: add new Exception class
                throw new Exception("Error during parsing");
            }

            int id = int.Parse(m.Groups["id"].Value);
            string entityType = m.Groups["type"].Value;
            string entityBody = m.Groups["body"].Value;

            switch (entityType)
            {
                case "CARTESIAN_POINT":
                    {
                        //TODO: double type identification
                        stepRepresentation.StepCartesianPoints!.Add(
                            new LazyStepEntityContainer<StepCartesianPoint>(
                                new UninitializedStepEntity(
                                    id, 
                                    entityBody, 
                                    StepEntityType.CARTESIAN_POINT)));
                        break;
                    }
                case "AXIS2_PLACEMENT_3D":
                    {
                        stepRepresentation.StepAxis2Placements3D!.Add(
                            new LazyStepEntityContainer<StepAxis2Placement3D>(
                                new UninitializedStepEntity(
                                    id,
                                    entityBody,
                                    StepEntityType.AXIS2_PLACEMENT_3D)));
                        break;
                    }
                case "DIRECTION":
                    {
                        stepRepresentation.StepDirections!.Add(
                            new LazyStepEntityContainer<StepDirection>(
                                new UninitializedStepEntity(
                                    id,
                                    entityBody,
                                    StepEntityType.DIRECTION)));
                        break;
                    }
                case "PRODUCT_DEFINITION_SHAPE":
                    {
                        stepRepresentation.StepProductDefinitionShapes!.Add(
                          new LazyStepEntityContainer<StepProductDefinitionShape>(
                            new UninitializedStepEntity(
                                id,
                                entityBody,
                                StepEntityType.PRODUCT_DEFINITION_SHAPE)));
                        break;
                    }
                case "NEXT_ASSEMBLY_USAGE_OCCURRENCE":
                    {
                        stepRepresentation.StepNextAssemblyUsageOccurrences!.Add(
                          new LazyStepEntityContainer<StepNextAssemblyUsageOccurrence>(
                            new UninitializedStepEntity(
                                id,
                                entityBody,
                                StepEntityType.NEXT_ASSEMBLY_USAGE_OCCURRENCE)));
                        break;
                    }
                case "ITEM_DEFINED_TRANSFORMATION":
                    {
                        var currentEntity = StepEntityParser.TryParseToStepItemDefinedTransformation(id, entityBody);
                        if (currentEntity != null)
                        {
                            stepRepresentation.StepItemDefinedTransformations!.Add(
                              new LazyStepEntityContainer<StepItemDefinedTransformation>(
                                new UninitializedStepEntity(
                                    id,
                                    entityBody,
                                    StepEntityType.ITEM_DEFINED_TRANSFORMATION)));
                        }
                        break;
                    }
                case "REPRESENTATION_RELATIONSHIP":
                    {
                        var currentEntity = StepEntityParser.TryParseToStepRepresentationRelationshipWithTransformation(id, entityBody);
                        if (currentEntity != null)
                        {
                            stepRepresentation.StepRepresentationsRelationshipWithTransformation!.Add(
                            new LazyStepEntityContainer<StepRepresentationRelationshipWithTransformation>(
                                new UninitializedStepEntity(
                                    id,
                                    entityBody,
                                    StepEntityType.REPRESENTATION_RELATIONSHIP)));
                        }
                        break;
                    }
                case "SHAPE_REPRESENTATION":
                    {
                        var currentEntity = StepEntityParser.TryParseToStepShapeRepresentation(id, entityBody);
                        if (currentEntity != null)
                        {
                            stepRepresentation.StepShapeRepresentations!.Add(
                              new LazyStepEntityContainer<StepShapeRepresentation>(
                                new UninitializedStepEntity(
                                    id,
                                    entityBody,
                                    StepEntityType.SHAPE_REPRESENTATION)));
                        }
                        break;
                    }
                case "CONTEXT_DEPENDENT_SHAPE_REPRESENTATION":
                    {
                        var currentEntity = StepEntityParser.TryParseToStepContextDependentShapeRepresentation(id, entityBody);
                        if (currentEntity != null)
                        {
                            stepRepresentation.StepContextDependentShapeRepresentations!.Add(
                              new LazyStepEntityContainer<StepContextDependentShapeRepresentation>(
                                new UninitializedStepEntity(
                                    id,
                                    entityBody,
                                    StepEntityType.CONTEXT_DEPENDENT_SHAPE_REPRESENTATION)));
                        }
                        break;
                    }
            }
        }

        return stepRepresentation;
    }

    private static string DeleteWhiteSpaces(string entity)
    {
        if(entity.Contains('\''))
        {
            if (entity.Contains(' '))
            {
                StringBuilder sb = new StringBuilder();
                string[] groups = entity.Split('\'');
                for(int i = 0; i < groups.Count(); i++)
                {
                    if(i%2 == 0)
                    {
                        sb.Append(groups[i].Replace(" ", ""));
                    }
                    else
                    {
                        sb.Append(groups[i]);
                    }
                    if (i != groups.Count() - 1)
                    {
                        sb.Append("'");
                    }
                }
                entity = sb.ToString();
            }
        }
        else
        {
            if (entity.Contains(' ')) 
            {
                entity = entity.Replace(" ", "");
            }
        }
        return entity;
    }
}