namespace SimpleStepParser.StepFileRepresentation.Domain.Entities;

/// <summary>
/// https://www.steptools.com/docs/stp_aim/html/t_product_definition_shape.html
/// </summary>
internal class StepProductDefinitionShape : AbstractStepEntity
{
    public StepProductDefinitionShape(int id) : base(id)
    {
    }

    public string Name = string.Empty;

    public string? Description = string.Empty;

    public int Definition;
}
