namespace SimpleStepParser.StepFileRepresentation.Domain.Entities;
/// <summary>
/// https://www.steptools.com/docs/stp_aim/html/t_shape_representation.html
/// </summary>
internal class StepShapeRepresentation : AbstractStepEntity
{
    public StepShapeRepresentation(int id) : base(id)
    {
    }

    public string? Name { get; init; }
    public string? Items { get; init; } //we don't parse this string because we don't use it
    public int ContextOfItemsId { get; init; }
}
