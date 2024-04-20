using SimpleStepParser.StepFileRepresentation._1.Domain.Entities;

namespace SimpleStepParser.StepFileRepresentation._1.Domain;

internal class StepRepresentation
{
    internal string? Header { get; init; }
    internal List<UndefinedStepEntity>? UndefinedStepEntities { get; } = new List<UndefinedStepEntity>();
    internal List<StepDirection>? StepDirections { get; } = new List<StepDirection>();
    internal List<StepCartesianPoint>? StepCartesianPoints { get; } = new List<StepCartesianPoint>();
}
