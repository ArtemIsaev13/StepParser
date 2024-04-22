using SimpleStepParser.StepFileRepresentation._1.Domain.Entities;

namespace SimpleStepParser.StepFileRepresentation._1.Domain;

internal class StepRepresentation
{
    internal string? Header { get; init; }
    internal List<UndefinedStepEntity>? UndefinedStepEntities { get; } = new();
    internal List<StepItemDefinedTransformation>? StepItemDefinedTransformations { get; } = new();
    internal List<StepShapeRepresentation>? StepShapeRepresentations { get; } = new();
    internal List<StepRepresentationRelationshipWithTransformation>? StepRepresentationsRelationshipWithTransformation { get; } = new();
    #region Lazy
    internal List<LazyStepEntity<StepDirection>>? StepDirections { get; } = new();
    internal List<LazyStepEntity<StepCartesianPoint>>? StepCartesianPoints { get; } = new();
    internal List<LazyStepEntity<StepAxis2Placement3D>>? StepAxis2Placements3D { get; } = new();
    #endregion
}
