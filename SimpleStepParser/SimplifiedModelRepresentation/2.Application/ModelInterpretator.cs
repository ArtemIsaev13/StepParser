using MathNet.Spatial.Euclidean;
using SimpleStepParser.SimplifiedModelRepresentation._1.Domain;
using SimpleStepParser.StepFileRepresentation._1.Domain;
using SimpleStepParser.StepFileRepresentation._1.Domain.Entities;
using SimpleStepParser.StepFileRepresentation._2.Application.Extensions;

namespace SimpleStepParser.SimplifiedModelRepresentation._2.Application;

internal static class ModelInterpretator
{
    internal static Model? GetModelTree(StepRepresentation stepFileRepresentation)
    {
        if(stepFileRepresentation == null)
        {
            return null;
        }

        Dictionary<int, Model> models = new();

        foreach(var relationship in stepFileRepresentation.StepRepresentationsRelationshipWithTransformation!)
        {
            //Creating models for parent if necessary 
            AddModel(ref models, relationship.ParentId, stepFileRepresentation);
            //Creating models for child
            AddModel(ref models, relationship.ChildId, stepFileRepresentation);
            //Adding child to parent
            models[relationship.ParentId].Childs.Add(models[relationship.ChildId]);
            //Adding parent to child
            models[relationship.ChildId].Parent = models[relationship.ParentId];
            //Adding CoordinateSystem
            models[relationship.ChildId].CoordinateSystem 
                = GetCoordinateSystem(relationship.TransformationId, stepFileRepresentation);
        }

        //Root it is parentless model
        Model? result = null;
        foreach(var model in models.Values)
        {
            if(model.Parent == null)
            {
                result = model;
                break;
            }
        }

        return result;
    }

    private static void AddModel(ref Dictionary<int, Model> models, int id, StepRepresentation stepFileRepresentation)
    {
        //Adding model if necessary
        Model? model = null;
        if (models.ContainsKey(id))
        {
            return;
        }
        else
        {
            model = new Model();
            models.Add(id, model);
        }
        //Finding model name
        var collection = stepFileRepresentation.StepShapeRepresentations?.Where(f => (f.Id == id));
        if(collection?.Count() == 1 && collection.First()?.Name != null)
        {
            model.Name = collection.First().Name!;
        }
        else
        {
            model.Name = "Unnamed model";
        }
    }

    private static CoordinateSystem? GetCoordinateSystem(int id, StepRepresentation stepFileRepresentation)
    {
        // Оnly child CS is matter by some reason
        int currentCsId = 
            stepFileRepresentation.StepItemDefinedTransformations!.First(s => s.Id == id).ChildId;

        StepAxis2Placement3D? currentStepAxis2Placement3D
            = stepFileRepresentation.StepAxis2Placements3D!.First(s => (s.Id == currentCsId)).GetEntity();

        if(currentStepAxis2Placement3D == null)
        {
            return null;
        }

        StepDirection? axisZ 
            = stepFileRepresentation.StepDirections!
            .First(s => (s.Id == currentStepAxis2Placement3D.ZAxisId)).GetEntity();

        StepDirection? axisX
            = stepFileRepresentation.StepDirections!
            .First(s => (s.Id == currentStepAxis2Placement3D.XAxisId)).GetEntity();
        
        StepCartesianPoint? origin
            = stepFileRepresentation.StepCartesianPoints!
            .First(s => (s.Id == currentStepAxis2Placement3D.LocationPointId)).GetEntity();
        
        if (axisZ == null ||
            axisX == null ||
            origin == null)
        {
            return null;
        }

        Vector3D z = GetUnitVector3D(axisZ);
        Vector3D x = GetUnitVector3D(axisX);
        Vector3D y = z.CrossProduct(x);

        Point3D originPoint = GetPoint3D(origin);
        
        return new CoordinateSystem(originPoint, x, y, z);
    }

    private static Point3D GetPoint3D(StepCartesianPoint stepCartesianPoint)
    {
        return new Point3D(stepCartesianPoint.X, stepCartesianPoint.Y, stepCartesianPoint.Z);
    }

    private static Vector3D GetUnitVector3D(StepDirection stepDirection)
    {
        return new Vector3D(stepDirection.I, stepDirection.J, stepDirection.K);
    }
}
