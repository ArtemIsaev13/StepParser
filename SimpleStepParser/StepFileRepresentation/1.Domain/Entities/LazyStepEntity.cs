namespace SimpleStepParser.StepFileRepresentation._1.Domain.Entities;

internal class LazyStepEntity<T> : AbstractStepEntity where T : AbstractStepEntity
{
    public string? Body { get; set; }

    public bool IsInitiolized { get => StepEntity != null; }
    public T? StepEntity { internal get; set; }
}
