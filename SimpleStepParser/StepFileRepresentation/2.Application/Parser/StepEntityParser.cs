﻿using SimpleStepParser.StepFileRepresentation._1.Domain.Entities;
using SimpleStepParser.StepFileRepresentation._1.Domain.Enums;
using System.Text.RegularExpressions;

namespace SimpleStepParser.StepFileRepresentation._2.Application.Parser;

internal static class StepEntityParser
{
    internal static bool TryParse(UninitializedStepEntity from, out AbstractStepEntity? result)
    {
        result = null;
        switch (from.StepEntityType)
        {
            case StepEntityType.CARTESIAN_POINT:
                {
                    result = TryParseToStepCartesianPoint(from.Id, from.Body);
                    break;
                }
            case StepEntityType.AXIS2_PLACEMENT_3D:
                {
                    result = TryParseToStepAxis2Placement3D(from.Id, from.Body);
                    break;
                }
            case StepEntityType.DIRECTION:
                {
                    result = TryParseToStepDirection(from.Id, from.Body);
                    break;
                }
            case StepEntityType.ITEM_DEFINED_TRANSFORMATION:
                {
                    result = TryParseToStepItemDefinedTransformation(from.Id, from.Body);
                    break;
                }
            case StepEntityType.REPRESENTATION_RELATIONSHIP:
                {
                    result = TryParseToStepRepresentationRelationshipWithTransformation(from.Id, from.Body);
                    break;
                }
            case StepEntityType.SHAPE_REPRESENTATION:
                {
                    result = TryParseToStepShapeRepresentation(from.Id, from.Body);
                    break;
                }
        }
        return (result == null);
    }

    private static readonly Regex _stepDirectionRegex
        = new Regex(@"^\('(?<name>.*)',\((?<i>.*),(?<j>.*),(?<k>.*)\)\);");

    internal static StepDirection? TryParseToStepDirection(int id, string body)
    {
        if (body == null)
        {
            return null;
        }

        var match = _stepDirectionRegex.Match(body);
        if (!match.Success)
        {
            return null;
        }

        StepDirection result = new StepDirection(id)
        {
            Name = match.Groups["name"].Value ?? string.Empty,
            I = float.Parse(match.Groups["i"].Value),
            J = float.Parse(match.Groups["j"].Value),
            K = float.Parse(match.Groups["k"].Value),
        };

        return result;
    }

    private static readonly Regex _stepCartesianPoint
        = new Regex(@"^\('(?<name>.*)',\((?<x>.*),(?<y>.*),(?<z>.*)\)\);");

    internal static StepCartesianPoint? TryParseToStepCartesianPoint(int id, string body)
    {
        if (body == null)
        {
            return null;
        }

        var match = _stepCartesianPoint.Match(body);
        if (!match.Success)
        {
            return null;
        }

        StepCartesianPoint result = new StepCartesianPoint(id)
        {
            Name = match.Groups["name"].Value ?? string.Empty,
            X = float.Parse(match.Groups["x"].Value),
            Y = float.Parse(match.Groups["y"].Value),
            Z = float.Parse(match.Groups["z"].Value),
        };

        return result;
    }

    private static readonly Regex _stepAxis2Placement3D
        = new Regex(@"^\('(?<name>.*)',#(?<point>\d*),#(?<zAxis>\d*),#(?<xAxis>\d*)\);");

    internal static StepAxis2Placement3D? TryParseToStepAxis2Placement3D(int id, string body)
    {
        if (body == null)
        {
            return null;
        }

        var match = _stepAxis2Placement3D.Match(body);
        if (!match.Success)
        {
            return null;
        }

        StepAxis2Placement3D result = new StepAxis2Placement3D(id)
        {
            Name = match.Groups["name"].Value ?? string.Empty,
            LocationPointId = int.Parse(match.Groups["point"].Value),
            ZAxisId = int.Parse(match.Groups["zAxis"].Value),
            XAxisId = int.Parse(match.Groups["xAxis"].Value),
        };

        return result;
    }

    private static readonly Regex _stepItemDefinedTransformation
        = new Regex(@"^\('(?<name>.*)','(?<description>.*)',#(?<parent>\d*),#(?<child>\d*)\);");

    internal static StepItemDefinedTransformation? TryParseToStepItemDefinedTransformation(int id, string body)
    {
        if (body == null)
        {
            return null;
        }

        var match = _stepItemDefinedTransformation.Match(body);
        if (!match.Success)
        {
            return null;
        }

        StepItemDefinedTransformation result = new StepItemDefinedTransformation(id)
        {
            Name = match.Groups["name"].Value ?? string.Empty,
            Description = match.Groups["description"].Value ?? string.Empty,
            ParentId = int.Parse(match.Groups["parent"].Value),
            ChildId = int.Parse(match.Groups["child"].Value),
        };

        return result;
    }

    private static readonly Regex _stepShapeRepresentation
        = new Regex(@"(?s)^\('(?<name>.*)',\((?<items>.*)\),#(?<context>\d*)\);");

    internal static StepShapeRepresentation? TryParseToStepShapeRepresentation(int id, string body)
    {
        if (body == null)
        {
            return null;
        }

        var match = _stepShapeRepresentation.Match(body);
        if (!match.Success)
        {
            return null;
        }

        StepShapeRepresentation result = new StepShapeRepresentation(id)
        {
            Name = match.Groups["name"].Value ?? string.Empty,
            Items = match.Groups["items"].Value ?? string.Empty,
            ContextOfItemsId = int.Parse(match.Groups["context"].Value)
        };

        return result;
    }

    private static readonly Regex _stepRepresentationRelationshipWithTransformation
        = new Regex(@"^(?s)\('(?<name>.*)','(?<description>.*)',#(?<child>\d*),#(?<parent>\d*)\).*REPRESENTATION_RELATIONSHIP_WITH_TRANSFORMATION\(#(?<transformation>\d*)\)");

    internal static StepRepresentationRelationshipWithTransformation? 
        TryParseToStepRepresentationRelationshipWithTransformation(int id, string body)
    {
        if (body == null)
        {
            return null;
        }

        var match = _stepRepresentationRelationshipWithTransformation.Match(body);
        if (!match.Success)
        {
            return null;
        }

        StepRepresentationRelationshipWithTransformation result
            = new StepRepresentationRelationshipWithTransformation(id)
            {
                Name = match.Groups["name"].Value ?? string.Empty,
                Description = match.Groups["description"].Value ?? string.Empty,
                ChildId = int.Parse(match.Groups["child"].Value),
                ParentId = int.Parse(match.Groups["parent"].Value),
                TransformationId = int.Parse(match.Groups["transformation"].Value)
            };
        return result;
    }
}
