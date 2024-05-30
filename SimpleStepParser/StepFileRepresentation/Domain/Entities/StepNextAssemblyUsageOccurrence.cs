namespace SimpleStepParser.StepFileRepresentation.Domain.Entities;

/// <summary>
/// https://www.steptools.com/stds/stp_aim/html/t_next_assembly_usage_occurrence.html
/// </summary>
internal class StepNextAssemblyUsageOccurrence : AbstractStepEntity
{
    public StepNextAssemblyUsageOccurrence(int id) : base(id)
    {
    }

    public string? Identifier { get; init; }

    public string? Name { get; init; }

    public string? Description { get; init; }

    public int RelatingProductDefinition { get; init; }

    public int RelatedProductDefinition { get; init; }

    public string? ReferenceDesignator { get; init; }
}
