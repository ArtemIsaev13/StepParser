﻿namespace SimpleStepParser.StepFileRepresentation.Domain.Enums;

internal enum StepEntityType
{
    UNKNOWN = 0,
    CARTESIAN_POINT = 10,
    AXIS2_PLACEMENT_3D = 11,
    DIRECTION = 12,
    ITEM_DEFINED_TRANSFORMATION = 21,
    REPRESENTATION_RELATIONSHIP = 22,
    SHAPE_REPRESENTATION = 23,
    CONTEXT_DEPENDENT_SHAPE_REPRESENTATION = 31,
    PRODUCT_DEFINITION_SHAPE = 32,
    NEXT_ASSEMBLY_USAGE_OCCURRENCE = 33
}