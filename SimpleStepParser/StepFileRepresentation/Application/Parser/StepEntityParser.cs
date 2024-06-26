﻿using SimpleStepParser.StepFileRepresentation.Domain.Entities;
using SimpleStepParser.StepFileRepresentation.Domain.Enums;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SimpleStepParser.StepFileRepresentation.Application.Parser;

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
            case StepEntityType.CONTEXT_DEPENDENT_SHAPE_REPRESENTATION:
                {
                    result = TryParseToStepContextDependentShapeRepresentation(from.Id, from.Body);
                    break;
                }
            case StepEntityType.PRODUCT_DEFINITION_SHAPE:
                {
                    result = TryParseToStepProductDefinitionShape(from.Id, from.Body);
                    break;
                }
            case StepEntityType.NEXT_ASSEMBLY_USAGE_OCCURRENCE:
                {
                    result = TryParseToStepNextAssemblyUsageOccurrence(from.Id, from.Body);
                    break;
                }
            case StepEntityType.SHAPE_DEFINITION_REPRESENTATION:
                {
                    result = TryParseToStepShapeDefinitionRepresentation(from.Id, from.Body);
                    break;
                }
            case StepEntityType.PRODUCT_DEFINITION:
                {
                    result = TryParseToStepProductDefinition(from.Id, from.Body);
                    break;
                }
            case StepEntityType.PRODUCT_DEFINITION_FORMATION_WITH_SPECIFIED_SOURCE:
                {
                    result = TryParseToStepProductDefinitionFormationWithSpecifiedSource(from.Id, from.Body);
                    break;
                }
            case StepEntityType.PRODUCT:
                {
                    result = TryParseToStepProduct(from.Id, from.Body);
                    break;
                }
        }
        return (result != null);
    }

    private static readonly Regex _stepDirectionRegex
        = new Regex(@"^\('(?<name>.*)',\((?<i>.*),(?<j>.*),(?<k>.*)\)\);", RegexOptions.Compiled);

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
            I = float.Parse(match.Groups["i"].Value, CultureInfo.InvariantCulture),
            J = float.Parse(match.Groups["j"].Value, CultureInfo.InvariantCulture),
            K = float.Parse(match.Groups["k"].Value, CultureInfo.InvariantCulture),
        };

        return result;
    }

    private static readonly Regex _stepCartesianPoint
        = new Regex(@"^\('(?<name>.*)',\((?<x>.*),(?<y>.*),(?<z>.*)\)\);",
            RegexOptions.Compiled);

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
            X = float.Parse(match.Groups["x"].Value, CultureInfo.InvariantCulture),
            Y = float.Parse(match.Groups["y"].Value, CultureInfo.InvariantCulture),
            Z = float.Parse(match.Groups["z"].Value, CultureInfo.InvariantCulture),
        };

        return result;
    }

    private static readonly Regex _stepAxis2Placement3D
        = new Regex(@"^\('(?<name>.*)',#(?<point>\d*),#(?<zAxis>\d*),#(?<xAxis>\d*)\);",
            RegexOptions.Compiled);

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
        = new Regex(@"^\('(?<name>.*)','(?<description>.*)',#(?<parent>\d*),#(?<child>\d*)\);", 
            RegexOptions.Compiled);

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
        = new Regex(@"(?s)^\('(?<name>.*)',\((?<items>.*)\),#(?<context>\d*)\);", 
            RegexOptions.Compiled);

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
        = new Regex(@"^(?s)\('(?<name>.*)','(?<description>.*)',#(?<child>\d*),#(?<parent>\d*)\).*REPRESENTATION_RELATIONSHIP_WITH_TRANSFORMATION\(#(?<transformation>\d*)\)", 
            RegexOptions.Compiled);

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
                Rep2Id = int.Parse(match.Groups["child"].Value),
                Rep1Id = int.Parse(match.Groups["parent"].Value),
                TransformationId = int.Parse(match.Groups["transformation"].Value)
            };
        return result;
    }

    private static readonly Regex _stepContexDependentShapeRepresentation
        = new Regex(@"^\(#(?<repRel>\d*),#(?<repProdRel>\d*)\);", RegexOptions.Compiled);

    internal static StepContextDependentShapeRepresentation?
        TryParseToStepContextDependentShapeRepresentation(int id, string body)
    {
        if (body == null)
        {
            return null;
        }

        var match = _stepContexDependentShapeRepresentation.Match(body);
        if (!match.Success)
        {
            return null;
        }

        StepContextDependentShapeRepresentation result
            = new StepContextDependentShapeRepresentation(id)
            {
                RepresentationRelation = int.Parse(match.Groups["repRel"].Value),
                RepresentedProductRelation = int.Parse(match.Groups["repProdRel"].Value)
            };
        return result;
    }

    private static readonly Regex _stepProductDefinitionShape
        = new Regex(@"^\('(?<name>.*)','(?<description>.*)',#(?<definition>\d*)\);", RegexOptions.Compiled);

    internal static StepProductDefinitionShape? 
        TryParseToStepProductDefinitionShape(int id, string body)
    {
        if (body == null)
        {
            return null;
        }

        var match = _stepProductDefinitionShape.Match(body);
        if (!match.Success)
        {
            return null;
        }

        StepProductDefinitionShape result
            = new StepProductDefinitionShape(id)
            {
                Name = match.Groups["name"].Value,
                Description = match.Groups["description"].Value,
                Definition = int.Parse(match.Groups["definition"].Value)
            };
        return result;
    }


    private static Regex _stepNextAssemblyUsageOccurrence
        = new Regex(@"^\(\W*'(?<id>.*)'\W*,\W*'(?<name>.*)'\W*,\W*'(?<description>.*)'\W*,\W*#(?<relatingProd>\d*)\W*,\W*#(?<relatedProd>\d*)\W*,\W*.*\W*\);",
            RegexOptions.Compiled);

    internal static StepNextAssemblyUsageOccurrence? 
        TryParseToStepNextAssemblyUsageOccurrence(int id, string body)
    {
        if (body == null)
        {
            return null;
        }

        var match = _stepNextAssemblyUsageOccurrence.Match(body);
        if (!match.Success)
        {
            return null;
        }

        StepNextAssemblyUsageOccurrence result
            = new StepNextAssemblyUsageOccurrence(id)
            {
                Identifier = match.Groups["id"].Value,
                Name = match.Groups["name"].Value,
                Description = match.Groups["description"].Value,
                RelatingProductDefinition = int.Parse(match.Groups["relatingProd"].Value),
                RelatedProductDefinition = int.Parse(match.Groups["relatedProd"].Value),
                ReferenceDesignator = match.Groups["ref"]?.Value ?? string.Empty
            };
        return result;
    }


    private static Regex _stepShapeDefinitionRepresentation
    = new Regex(@"^\(#(?<def>.*),#(?<rep>\d*)\);",
        RegexOptions.Compiled);

    internal static StepShapeDefinitionRepresentation?
        TryParseToStepShapeDefinitionRepresentation(int id, string body)
    {
        if (body == null)
        {
            return null;
        }

        var match = _stepShapeDefinitionRepresentation.Match(body);
        if (!match.Success)
        {
            return null;
        }

        StepShapeDefinitionRepresentation result
            = new StepShapeDefinitionRepresentation(id)
            {
                Definition = int.Parse(match.Groups["def"].Value),
                UsedRepresentation = int.Parse(match.Groups["rep"].Value)
            };
        return result;
    }


    private static Regex _stepProductDefinition
    = new Regex(@"^\('(?<iden>.*)','(?<descr>.*)',#(?<formation>\d*),#(?<frOfRef>\d*)\);",
        RegexOptions.Compiled);

    internal static StepProductDefinition?
        TryParseToStepProductDefinition(int id, string body)
    {
        if (body == null)
        {
            return null;
        }

        var match = _stepProductDefinition.Match(body);
        if (!match.Success)
        {
            return null;
        }

        StepProductDefinition result
            = new StepProductDefinition(id)
            {
                Identifier = match.Groups["iden"].Value,
                Description = match.Groups["descr"].Value,
                Formation = int.Parse(match.Groups["formation"].Value),
                FrameOfReference = int.Parse(match.Groups["frOfRef"].Value)
            };
        return result;
    }


    private static Regex _stepProductDefinitionFormationWithSpecifiedSource
    = new Regex(@"^\('(?<iden>.*)','(?<descr>.*)',#(?<ofProduct>\d*),(?<makeOrBuy>.*)\);",
        RegexOptions.Compiled);

    internal static StepProductDefinitionFormationWithSpecifiedSource?
        TryParseToStepProductDefinitionFormationWithSpecifiedSource(int id, string body)
    {
        if (body == null)
        {
            return null;
        }

        var match = _stepProductDefinitionFormationWithSpecifiedSource.Match(body);
        if (!match.Success)
        {
            return null;
        }

        StepProductDefinitionFormationWithSpecifiedSource result
            = new StepProductDefinitionFormationWithSpecifiedSource(id)
            {
                Identifier = match.Groups["iden"].Value,
                Description = match.Groups["descr"].Value,
                OfProduct = int.Parse(match.Groups["ofProduct"].Value),
                MakeOrBuy = match.Groups["makeOrBuy"].Value,
            };
        return result;
    }


    private static Regex _stepProduct
    = new Regex(@"^\('(?<iden>.*)','(?<name>.*)','(?<descr>.*)',(?<frOfRef>.*)\);",
        RegexOptions.Compiled);

    internal static StepProduct?
        TryParseToStepProduct(int id, string body)
    {
        if (body == null)
        {
            return null;
        }

        var match = _stepProduct.Match(body);
        if (!match.Success)
        {
            return null;
        }

        StepProduct result
            = new StepProduct(id)
            {
                Identifier = match.Groups["iden"].Value,
                Name = match.Groups["name"].Value,
                Description = match.Groups["descr"].Value,
                FrameOfReference = match.Groups["frOfRef"].Value,
            };
        return result;
    }
}
