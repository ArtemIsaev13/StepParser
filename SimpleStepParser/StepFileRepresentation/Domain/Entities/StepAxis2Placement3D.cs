using SimpleStepParser.StepFileRepresentation.Domain.Enums;
namespace SimpleStepParser.StepFileRepresentation.Domain.Entities;
/// <summary>
/// https://www.steptools.com/stds/stp_aim/html/t_axis2_placement_3d.html
/// </summary>
internal class StepAxis2Placement3D : AbstractStepEntity
{
    public StepAxis2Placement3D(int id) : base(id)
    {
    }

    public string? Name { get; init; }
    public int LocationPointId { get; init; }
    public int ZAxisId { get; init; }
    public int XAxisId { get; init; }
}
