namespace SimpleStepParser.StepFileRepresentation.Domain.Exceptions;

public class UnableToFindPathFromModelToChildException : Exception
{
    public override string Message => "Cannot find path from model to child element.";
}