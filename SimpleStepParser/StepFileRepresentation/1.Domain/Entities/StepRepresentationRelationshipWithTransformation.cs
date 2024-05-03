namespace SimpleStepParser.StepFileRepresentation._1.Domain.Entities;
/// <summary>
/// https://www.steptools.com/docs/stp_aim/html/t_representation_relationship.html
/// and
/// https://www.steptools.com/stds/stp_aim/html/t_representation_relationship_with_transformation.html
/// </summary>
internal class StepRepresentationRelationshipWithTransformation : AbstractStepEntity
{
    public StepRepresentationRelationshipWithTransformation(int id) : base(id)
    {
    }

    public string? Name { get; init; }
    public string? Description { get; init; }
    public int ParentId { get; init; }
    public int ChildId { get; init; }
    public int TransformationId { get; init; }
}
