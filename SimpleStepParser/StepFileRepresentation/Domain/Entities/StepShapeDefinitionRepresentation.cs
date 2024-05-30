namespace SimpleStepParser.StepFileRepresentation.Domain.Entities;
/// <summary>
/// https://www.steptools.com/stds/stp_aim/html/t_shape_definition_representation.html
/// </summary>
internal class StepShapeDefinitionRepresentation : AbstractStepEntity
{
    public StepShapeDefinitionRepresentation(int id) : base(id)
    {
    }

    public int Definition { get; init; }
    public int UsedRepresentation { get; init; }
}
