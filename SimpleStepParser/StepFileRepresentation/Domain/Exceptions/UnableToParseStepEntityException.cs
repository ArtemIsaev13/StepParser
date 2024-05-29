namespace SimpleStepParser.StepFileRepresentation.Domain.Exceptions;

public class UnableToParseStepEntityException : Exception
{
    public override string Message => "Unable to parse step entity.";
}
