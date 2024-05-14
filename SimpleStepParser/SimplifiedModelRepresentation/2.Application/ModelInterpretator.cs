using MathNet.Spatial.Euclidean;
using SimpleStepParser.SimplifiedModelRepresentation._1.Domain;
using SimpleStepParser.StepFileRepresentation._1.Domain.Entities;
using SimpleStepParser.StepFileRepresentation._1.Domain.StepRepresentation;

namespace SimpleStepParser.SimplifiedModelRepresentation._2.Application;

internal static class ModelInterpretator
{
    internal static Model? GetModelTree(StepRepresentation stepFileRepresentation)
    {
        if(stepFileRepresentation == null)
        {
            return null;
        }

        Dictionary<int, List<Model>> models = new();

        foreach(var relationship in stepFileRepresentation.StepRepresentationsRelationshipWithTransformation!.Select(v => v.Value))
        {
            if(relationship == null)
            {
                continue;
            }

            //Creating models for parent if necessary 
            if(!models.ContainsKey(relationship.ParentId))
            {
                Model parent = GetModel(relationship.ParentId, stepFileRepresentation);
                models.Add(relationship.ParentId, new() { parent });
            }
            List<Model> parents = models[relationship.ParentId];

            //Creating models for child
            Model child = null;

            //If there are another exemplars of child we need to copy some information
            if (models.ContainsKey(relationship.ChildId))
            {
                //If this is parentless model
                if(models[relationship.ChildId].Count == 1 && models[relationship.ChildId][0].Parent == null)
                {
                    child = models[relationship.ChildId][0];
                }
                //If the same models already exists
                else if (models[relationship.ChildId].Count > 0)
                {
                    child = models[relationship.ChildId][0].GetDeepCopy();
                }
            }
            else
            {
                models.Add(relationship.ChildId, new ());
                child = GetModel(relationship.ChildId, stepFileRepresentation);
                models[relationship.ChildId].Add(child);
            }

            //Adding CoordinateSystem
            child!.CoordinateSystem = GetCoordinateSystem(relationship.TransformationId, stepFileRepresentation);

            //Adding child to all parents:
            for(int i = 0; i < parents.Count; i++)
            {
                Model currentChild = child;
                if(i != 0)
                {
                    currentChild = child.GetDeepCopy();
                    models[relationship.ChildId].Add(currentChild);
                }
                //Adding parent to child
                currentChild.Parent = parents[i];
                parents[i].Childs.Add(currentChild);
            }
        }

        //Root it is parentless model
        Model? result = null;
        foreach(var modelList in models.Values)
        {
            foreach (var model in modelList)
            {
                if (model.Parent == null)
                {
                    result = model;
                    break;
                }
            }
        }

        return result;
    }

    private static Model GetModel(int id, StepRepresentation stepFileRepresentation)
    {
        //Adding model if necessary
        Model? model = new ();
        //Finding model name
        var collection = stepFileRepresentation.StepShapeRepresentations?.Where(f => (f.Id == id));
        if(collection?.Count() == 1 && collection.FirstOrDefault()?.Value?.Name != null)
        {
            model.Name = collection.First().Value!.Name!;
        }
        else
        {
            model.Name = "Unnamed model";
        }
        return model;
    }

    private static CoordinateSystem? GetCoordinateSystem(int id, StepRepresentation stepFileRepresentation)
    {
        // Оnly child CS is matter by some reason
        int currentCsId = 
            stepFileRepresentation.StepItemDefinedTransformations!
            .Find(s => s.Id == id)?.Value?.ChildId ?? 0;
        if (currentCsId == 0)
        {
            return null;
        }

        StepAxis2Placement3D? currentStepAxis2Placement3D
            = stepFileRepresentation.StepAxis2Placements3D!.Find(s => (s.Id == currentCsId))?.Value;

        if(currentStepAxis2Placement3D == null)
        {
            return null;
        }

        StepDirection? axisZ 
            = stepFileRepresentation.StepDirections!
            .First(s => (s.Id == currentStepAxis2Placement3D.ZAxisId)).Value;

        StepDirection? axisX
            = stepFileRepresentation.StepDirections!
            .First(s => (s.Id == currentStepAxis2Placement3D.XAxisId)).Value;
        
        StepCartesianPoint? origin
            = stepFileRepresentation.StepCartesianPoints!
            .First(s => (s.Id == currentStepAxis2Placement3D.LocationPointId)).Value;
        
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
