namespace SimpleStepParser.StepFileRepresentation.Domain.Entities;

/// <summary>
/// https://www.steptools.com/stds/stp_aim/html/t_product.html
/// </summary>
internal class StepProduct : AbstractStepEntity
{
    public StepProduct(int id) : base(id)
    {
    }

    public string? Identifier { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? FrameOfReference { get; init; }
}
