namespace SimpleStepParser.StepFileRepresentation._1.Domain.Entities;
/// <summary>
/// https://www.steptools.com/docs/stp_aim/html/t_cartesian_point.html
/// </summary>
internal class StepCartesianPoint : AbstractStepEntity
{
    public StepCartesianPoint(int id) : base(id)
    {
    }

    public string? Name { get; init; }
    public float X { get; init; }
    public float Y { get; init; }
    public float Z { get; init; }
}
