using SimpleStepParser.StepFileRepresentation._1.Domain.Entities;

namespace SimpleStepParser.StepFileRepresentation._1.Domain;

internal class StepRepresentation
{
    internal string? Header { get; init; }
    internal List<UndefinedStepEntity>? UndefinedStepEntities { get; } = new List<UndefinedStepEntity>();
    internal List<StepDirection>? StepDirections { get; } = new List<StepDirection>();
    internal List<StepCartesianPoint>? StepCartesianPoints { get; } = new List<StepCartesianPoint>();
    internal List<StepAxis2Placement3D>? StepAxis2Placements3D { get; } = new List<StepAxis2Placement3D>();
    internal List<StepItemDefinedTransformation>? StepItemDefinedTransformations { get; } = new List<StepItemDefinedTransformation>();
    internal List<StepShapeRepresentation>? StepShapeRepresentations { get; } = new List<StepShapeRepresentation>();
}
