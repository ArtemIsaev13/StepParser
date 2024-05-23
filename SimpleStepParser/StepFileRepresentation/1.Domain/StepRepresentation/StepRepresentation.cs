using SimpleStepParser.StepFileRepresentation._1.Domain.Entities;

namespace SimpleStepParser.StepFileRepresentation._1.Domain.StepRepresentation;

internal class StepRepresentation
{
    internal string? Header { get; init; }
    internal List<UndefinedStepEntity> UndefinedStepEntities { get; } = new();
    
    internal List<LazyStepEntityContainer<StepItemDefinedTransformation>> 
        StepItemDefinedTransformations { get; } = new();
    
    internal List<LazyStepEntityContainer<StepShapeRepresentation>> 
        StepShapeRepresentations { get; } = new();
    
    internal List<LazyStepEntityContainer<StepRepresentationRelationshipWithTransformation>> 
        StepRepresentationsRelationshipWithTransformation { get; } = new();

    internal List<LazyStepEntityContainer<StepContextDependentShapeRepresentation>>
        StepContextDependentShapeRepresentations { get; } = new();

    #region Lazy
    internal List<LazyStepEntityContainer<StepDirection>> 
        StepDirections { get; } = new();
    
    internal List<LazyStepEntityContainer<StepCartesianPoint>> 
        StepCartesianPoints { get; } = new();
    
    internal List<LazyStepEntityContainer<StepAxis2Placement3D>> 
        StepAxis2Placements3D { get; } = new();

    internal List<LazyStepEntityContainer<StepProductDefinitionShape>>
        StepProductDefinitionShapes { get; } = new();

    internal List<LazyStepEntityContainer<StepNextAssemblyUsageOccurrence>>
        StepNextAssemblyUsageOccurrences { get; } = new();
    #endregion
}
