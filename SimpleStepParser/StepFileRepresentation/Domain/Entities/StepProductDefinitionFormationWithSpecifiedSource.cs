namespace SimpleStepParser.StepFileRepresentation.Domain.Entities;

/// <summary>
/// https://www.steptools.com/stds/stp_aim/html/t_product_definition_formation_with_specified_source.html
/// </summary>
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
