﻿namespace SimpleStepParser.StepFileRepresentation.Domain.Entities;
/// <summary>
/// https://www.steptools.com/stds/stp_aim/html/t_direction.html
/// </summary>
internal class StepDirection : UndefinedStepEntity
{
    public StepDirection(int id) : base(id)
    {
    }

    public string? Name { get; init; }
    public float I { get; init; }
    public float J { get; init; }
    public float K { get; init; }
}
