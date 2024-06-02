namespace SimpleStepParser.StepFileRepresentation.Domain.Entities;

/// <summary>
/// https://www.steptools.com/docs/stp_aim/html/t_product_definition_shape.html
/// </summary>
internal class StepProductDefinitionShape : AbstractStepEntity
{
    public StepProductDefinitionShape(int id) : base(id)
    {
    }

    public string? Name { get; init; }

    public string? Description { get; init; }

    public int Definition { get; init; }
}
