namespace SimpleStepParser.SimplifiedModelRepresentation.Domain;

public class Assembly
{
    public Model? Root { get; init; }

    public CadName CadName { get; init; }

    public StepVersion StepVersion { get; init; }
}
