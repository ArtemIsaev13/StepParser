namespace SimpleStepParser.StepFileRepresentation.Domain.Entities;

internal abstract class AbstractStepEntity
{
    public int Id { get; init; }

    protected AbstractStepEntity(int id)
    {
        Id = id;
    }
}
