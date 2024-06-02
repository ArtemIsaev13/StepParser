namespace SimpleStepParser.StepFileRepresentation.Domain.Exceptions;

public class UnableToBuildModelTreeException : Exception
{
    public override string Message => "Unable to build model tree.";
}