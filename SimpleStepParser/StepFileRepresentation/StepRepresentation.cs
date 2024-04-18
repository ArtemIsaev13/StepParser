using SimpleStepParser.StepFileRepresentation.Entities;

namespace SimpleStepParser.StepFileRepresentation;

internal class StepRepresentation
{
    internal string? Header { get; init; }
    internal List<UndefinedStepEntity>? UndefinedStepEntities { get; } = new List<UndefinedStepEntity>();
    internal List<StepDirection>? StepDirections { get; } = new List<StepDirection>();
}
