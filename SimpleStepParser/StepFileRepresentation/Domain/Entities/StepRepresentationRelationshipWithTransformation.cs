namespace SimpleStepParser.StepFileRepresentation.Domain.Entities;
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
    public int Rep1Id { get; init; }
    public int Rep2Id { get; init; }
    public int TransformationId { get; init; }
}
