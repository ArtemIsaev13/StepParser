namespace SimpleStepParser.StepFileRepresentation._1.Domain.Entities;

/// <summary>
/// https://www.steptools.com/stds/stp_aim/html/t_context_dependent_shape_representation.html
/// </summary>
internal class StepContextDependentShapeRepresentation : AbstractStepEntity
{
    public StepContextDependentShapeRepresentation(int id) : base(id)
    {
    }

    public int RepresentationRelation;

    public int RepresentedProductRelation;
}
