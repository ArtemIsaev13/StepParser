using SimpleStepParser.StepFileRepresentation.Application.Parser;
using SimpleStepParser.StepFileRepresentation.Domain.Entities;

namespace SimpleStepParser.StepFileRepresentation.Application;

internal class StepEntityStorage<T> where T : AbstractStepEntity
{
    private readonly List<T> _parsedData = new ();

    private readonly List<UninitializedStepEntity> _rawData = new ();

    public T? GetEntity(int id)
    {
        if(_parsedData.Exists(en => en.Id == id))
        {
            return _parsedData.First(en => en.Id == id);
        }
        else if(_rawData.Exists(en => en.Id == id))
        {
            return Parse(_rawData.First(en => en.Id == id));
        }
        return null;
    }

    public List<T> GetAll()
    {
        foreach(var rawDatum in _rawData)
        {
            if(_parsedData.Exists(en => en.Id == rawDatum.Id))
            {
                T newEntity = GetEntity(rawDatum.Id);
                if (newEntity != null)
                {
                    _parsedData.Add(newEntity);
                }
            }
        }
        return _parsedData.ToList();
    }

    public void AddEntity(UninitializedStepEntity entity)
    {
        _rawData.Add(entity);
    }

    private T? Parse(UninitializedStepEntity uninitializedStepEntity)
    {
        if(StepEntityParser.TryParse(uninitializedStepEntity, out var result))
        {
            return result as T;
        }
        return null;
    }
}
