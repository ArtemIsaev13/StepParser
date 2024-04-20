﻿namespace SimpleStepParser.StepFileRepresentation._1.Domain.Entities;
/// <summary>
/// https://www.steptools.com/stds/stp_aim/html/t_axis2_placement_3d.html
/// </summary>
internal class StepAxis2Placement3D : AbstractStepEntity
{
    public string? Name { get; init; }
    public int LocationPointId { get; init; }
    public int ZAxisId { get; init; }
    public int XAxisId { get; init; }
}