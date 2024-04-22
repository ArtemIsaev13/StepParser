using SimpleStepParser.StepFileRepresentation._1.Domain.Entities;
using SimpleStepParser.StepFileRepresentation._2.Application.Parser;

namespace SimpleStepParser.StepFileRepresentation._2.Application.Extensions;

internal static class LazyStepEntityExtension
{
    public static StepCartesianPoint? GetEntity(this LazyStepEntity<StepCartesianPoint> entity)
    {
        if(!entity.IsInitiolized)
        {
            entity.StepEntity = StepEntityParser.TryParseToStepCartesianPoint(entity.Id, entity.Body);
        }

        return entity.StepEntity;
    }

    public static StepDirection? GetEntity(this LazyStepEntity<StepDirection> entity)
    {
        if (!entity.IsInitiolized)
        {
            entity.StepEntity = StepEntityParser.TryParseToStepDirection(entity.Id, entity.Body);
        }

        return entity.StepEntity;
    }

    public static StepAxis2Placement3D? GetEntity(this LazyStepEntity<StepAxis2Placement3D> entity)
    {
        if (!entity.IsInitiolized)
        {
            entity.StepEntity = StepEntityParser.TryParseToStepAxis2Placement3D(entity.Id, entity.Body);
        }

        return entity.StepEntity;
    }
}
