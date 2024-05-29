using SimpleStepParser.StepFileRepresentation.Application;
using SimpleStepParser.StepFileRepresentation.Domain.Entities;

namespace SimpleStepParser.StepFileRepresentation.Domain.StepRepresentation;

internal class StepRepresentation
{
    internal string? Header { get; init; }

    internal StepEntityStorage<UndefinedStepEntity> 
        UndefinedStepEntities { get; } = new();
    
    internal StepEntityStorage<StepItemDefinedTransformation> 
        StepItemDefinedTransformations { get; } = new();
    
    internal StepEntityStorage<StepShapeRepresentation> 
        StepShapeRepresentations { get; } = new();
    
    internal StepEntityStorage<StepRepresentationRelationshipWithTransformation> 
        StepRepresentationsRelationshipWithTransformation { get; } = new();

    internal StepEntityStorage<StepContextDependentShapeRepresentation>
        StepContextDependentShapeRepresentations { get; } = new();

    internal StepEntityStorage<StepDirection> 
        StepDirections { get; } = new();
    
    internal StepEntityStorage<StepCartesianPoint> 
        StepCartesianPoints { get; } = new();
    
    internal StepEntityStorage<StepAxis2Placement3D> 
        StepAxis2Placements3D { get; } = new();

    internal StepEntityStorage<StepProductDefinitionShape>
        StepProductDefinitionShapes { get; } = new();

    internal StepEntityStorage<StepNextAssemblyUsageOccurrence>
        StepNextAssemblyUsageOccurrences { get; } = new();
}
