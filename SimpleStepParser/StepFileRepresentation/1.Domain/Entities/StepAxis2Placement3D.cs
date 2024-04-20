namespace SimpleStepParser.StepFileRepresentation._1.Domain.Entities;

internal class StepAxis2Placement3D : AbstractStepEntity
{
    public string? Name { get; init; }
    public int LocationPointId { get; init; }
    public int ZAxisId { get; init; }
    public int XAxisId { get; init; }
}
