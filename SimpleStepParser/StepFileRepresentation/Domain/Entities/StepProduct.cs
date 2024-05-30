namespace SimpleStepParser.StepFileRepresentation.Domain.Entities;

internal class StepProduct : AbstractStepEntity
{
    public StepProduct(int id) : base(id)
    {
    }

    public string? Identifier { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public int FrameOfReference { get; init; }
}
