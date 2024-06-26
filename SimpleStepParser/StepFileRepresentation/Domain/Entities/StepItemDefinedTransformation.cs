﻿namespace SimpleStepParser.StepFileRepresentation.Domain.Entities;
/// <summary>
/// https://www.steptools.com/stds/stp_aim/html/t_item_defined_transformation.html
/// </summary>
internal class StepItemDefinedTransformation : AbstractStepEntity
{
    public StepItemDefinedTransformation(int id) : base(id)
    {
    }

    public string? Name { get; init; }
    public string? Description { get; init; }
    public int ParentId { get; init; }
    public int ChildId { get; init; }
}
