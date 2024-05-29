using SimpleStepParser.StepFileRepresentation.Domain.Enums;

namespace SimpleStepParser.StepFileRepresentation.Domain.Entities;

internal class UninitializedStepEntity : AbstractStepEntity
{
    public UninitializedStepEntity(int id) : base(id)
    {
    }

    public UninitializedStepEntity(int id, string body, StepEntityType type) : base(id)
    {
        Body = body;
        StepEntityType = type;
    }

    public StepEntityType StepEntityType { get; init; }
    public string Body { get; init; } = string.Empty;
}
