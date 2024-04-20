namespace SimpleStepParser.StepFileRepresentation._1.Domain.Entities;

internal class StepCartesianPoint : AbstractStepEntity
{
    public string? Name { get; init; }
    public float X { get; init; }
    public float Y { get; init; }
    public float Z { get; init; }
}
