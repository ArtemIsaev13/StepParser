using SimpleStepParser.StepFileRepresentation.Domain.Entities;
using SimpleStepParser.StepFileRepresentation.Application.Parser;

namespace SimpleStepParser.StepFileRepresentation.Domain.StepRepresentation;

internal class LazyStepEntityContainer<T> where T : AbstractStepEntity
{
    public int Id { get; }

    private readonly UninitializedStepEntity _source;

    private T? _value;
    public T? Value
    {
        get
        {
            if (_value == null)
            {
                StepEntityParser.TryParse(_source, out AbstractStepEntity? parsedEntity);
                if (parsedEntity is T)
                {
                    _value = (T)parsedEntity;
                }
            }
            return _value;
        }
    }

    public bool IsInitiolized { get => Value != null; }

    public LazyStepEntityContainer(UninitializedStepEntity source)
    {
        Id = source.Id;
        _source = source;
    }
}
