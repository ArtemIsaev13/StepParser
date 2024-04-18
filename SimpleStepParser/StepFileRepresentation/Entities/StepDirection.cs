namespace SimpleStepParser.StepFileRepresentation.Entities;

internal class StepDirection : UndefinedStepEntity
{
    public string? Name { get; init; }
    public float I { get; init; }
    public float J { get; init; }
    public float K { get; init; }
}
