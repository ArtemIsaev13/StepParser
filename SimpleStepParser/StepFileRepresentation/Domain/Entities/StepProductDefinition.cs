namespace SimpleStepParser.StepFileRepresentation.Domain.Entities;
/// <summary>
/// https://www.steptools.com/docs/stp_aim/html/t_product_definition.html
/// </summary>
internal class StepProductDefinition : AbstractStepEntity
{
    public StepProductDefinition(int id) : base(id)
    {
    }

    public string? Identifier { get; init; }

    public string? Description { get; init; }

    public int Formation { get; init; }

    public int FrameOfReference { get; init; }
}
