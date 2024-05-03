namespace SimpleStepParser.StepFileRepresentation._1.Domain.Entities;

internal class UndefinedStepEntity : AbstractStepEntity
{
    public UndefinedStepEntity(int id) : base(id)
    {
    }

    public string EntityTypeText { get; init; } = string.Empty;
    public string Body { get; init; } = string.Empty;

}
