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
            var entity = Parse(_rawData.First(en => en.Id == id));
            if (entity != null)
            {
                _parsedData.Add(entity);
            }
            return entity;
        }
        return null;
    }

    public List<T> GetAll()
    {
        foreach(var rawDatum in _rawData)
        {
            if(!_parsedData.Exists(en => en.Id == rawDatum.Id))
            {
                T newEntity = GetEntity(rawDatum.Id);
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
        var wasParsed = StepEntityParser.TryParse(uninitializedStepEntity, out var result);

        if (wasParsed)
        {
            return result as T;
        }
        return null;
    }
}
