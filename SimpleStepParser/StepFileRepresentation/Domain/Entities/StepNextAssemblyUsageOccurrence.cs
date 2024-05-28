namespace SimpleStepParser.StepFileRepresentation.Domain.Entities;

/// <summary>
/// https://www.steptools.com/stds/stp_aim/html/t_next_assembly_usage_occurrence.html
/// </summary>
internal class StepNextAssemblyUsageOccurrence : AbstractStepEntity
{
    public StepNextAssemblyUsageOccurrence(int id) : base(id)
    {
    }

    public string Identifier = string.Empty;

    public string Name = string.Empty;

    public string Description = string.Empty;

    public int RelatingProductDefinition;

    public int RelatedProductDefinition;

    public string ReferenceDesignator = string.Empty;
}
