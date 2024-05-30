namespace SimpleStepParser.StepFileRepresentation.Domain.Entities;

internal class StepProductDefinitionFormationWithSpecifiedSource : AbstractStepEntity
{
    public StepProductDefinitionFormationWithSpecifiedSource(int id) : base(id)
    {
    }

    public string? Identifier { get; init; }
    public string? Description { get; init; }
    public int OfProduct {  get; init; }
    public string? MakeOrBuy { get; init; }
}
