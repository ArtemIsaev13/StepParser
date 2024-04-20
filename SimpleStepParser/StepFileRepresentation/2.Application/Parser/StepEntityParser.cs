﻿using SimpleStepParser.StepFileRepresentation._1.Domain.Entities;
using System.Text.RegularExpressions;

namespace SimpleStepParser.StepFileRepresentation._2.Application.Parser;

internal static class StepEntityParser
{
    private static readonly Regex _stepDirectionRegex
        = new Regex(@"^DIRECTION\('(?<name>.*)',\((?<i>\d*.\d*),(?<j>\d*.\d*),(?<k>\d*.\d*)\)\);");

    internal static StepDirection? TryParseToStepDirection(UndefinedStepEntity from)
    {
        if (from.Body == null)
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

    internal static StepCartesianPoint? TryParseToStepCartesianPoint(UndefinedStepEntity from)
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

    private static readonly Regex _stepAxis2Placement3D
        = new Regex(@"^AXIS2_PLACEMENT_3D\('(?<name>.*)',#(?<point>\d*),#(?<zAxis>\d*),#(?<xAxis>\d*)\);");

    internal static StepAxis2Placement3D? TryParseToStepAxis2Placement3D(UndefinedStepEntity from)
    {
        if (from.Body == null)
        {
            return null;
        }

        var match = _stepAxis2Placement3D.Match(from.Body);
        if (!match.Success)
        {
            return null;
        }

        StepAxis2Placement3D result = new StepAxis2Placement3D()
        {
            Id = from.Id,
            Name = match.Groups["name"].Value ?? string.Empty,
            LocationPointId = int.Parse(match.Groups["point"].Value),
            ZAxisId = int.Parse(match.Groups["zAxis"].Value),
            XAxisId = int.Parse(match.Groups["xAxis"].Value),
        };

        return result;
    }

    private static readonly Regex _stepItemDefinedTransformation
        = new Regex(@"^ITEM_DEFINED_TRANSFORMATION\('(?<name>.*)','(?<description>.*)',#(?<parent>\d*),#(?<child>\d*)\);");

    internal static StepItemDefinedTransformation? TryParseToStepItemDefinedTransformation(UndefinedStepEntity from)
    {
        if (from.Body == null)
        {
            return null;
        }

        var match = _stepItemDefinedTransformation.Match(from.Body);
        if (!match.Success)
        {
            return null;
        }

        StepItemDefinedTransformation result = new StepItemDefinedTransformation()
        {
            Id = from.Id,
            Name = match.Groups["name"].Value ?? string.Empty,
            Description = match.Groups["description"].Value ?? string.Empty,
            ParentId = int.Parse(match.Groups["parent"].Value),
            ChildId = int.Parse(match.Groups["child"].Value),
        };

        return result;
    }

    private static readonly Regex _stepShapeRepresentation
        = new Regex(@"(?s)^SHAPE_REPRESENTATION\('(?<name>.*)',\((?<items>.*)\),#(?<context>\d*)\);");

    internal static StepShapeRepresentation? TryParseToStepShapeRepresentation(UndefinedStepEntity from)
    {
        if (from.Body == null)
        {
            return null;
        }

        var match = _stepShapeRepresentation.Match(from.Body);
        if (!match.Success)
        {
            return null;
        }

        StepShapeRepresentation result = new StepShapeRepresentation()
        {
            Id = from.Id,
            Name = match.Groups["name"].Value ?? string.Empty,
            Items = match.Groups["items"].Value ?? string.Empty,
            ContextOfItemsId = int.Parse(match.Groups["context"].Value)
        };

        return result;
    }

    private static readonly Regex _stepRepresentationRelationshipWithTransformation
        = new Regex(@"^(?s)\(.*REPRESENTATION_RELATIONSHIP\('(?<name>.*)','(?<description>.*)',#(?<parent>\d*),#(?<child>\d*)\).*REPRESENTATION_RELATIONSHIP_WITH_TRANSFORMATION\(#(?<transformation>\d*)\)");

    internal static StepRepresentationRelationshipWithTransformation? 
        TryParseToStepRepresentationRelationshipWithTransformation(UndefinedStepEntity from)
    {
        if (from.Body == null)
        {
            return null;
        }

        var match = _stepRepresentationRelationshipWithTransformation.Match(from.Body);
        if (!match.Success)
        {
            return null;
        }
        
        StepRepresentationRelationshipWithTransformation result 
            = new StepRepresentationRelationshipWithTransformation()
        {
            Id = from.Id,
            Name = match.Groups["name"].Value ?? string.Empty,
            Description = match.Groups["description"].Value ?? string.Empty,
            ChildId = int.Parse(match.Groups["child"].Value),
            ParentId = int.Parse(match.Groups["parent"].Value),
            TransformationId = int.Parse(match.Groups["transformation"].Value)

            };
        return result;
    }
}
