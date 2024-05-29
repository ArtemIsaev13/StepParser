using MathNet.Spatial.Euclidean;
using SimpleStepParser.SimplifiedModelRepresentation.Domain;
using SimpleStepParser.StepFileRepresentation.Domain.Entities;
using SimpleStepParser.StepFileRepresentation.Domain.StepRepresentation;

namespace SimpleStepParser.SimplifiedModelRepresentation.Application;

internal static class ModelInterpretator
{
    internal static Model? GetModelTree(StepRepresentation stepFileRepresentation)
    {
        if(stepFileRepresentation == null)
        {
            return null;
        }

        List<ModelType> modelTypes = new ();
        List<ModelEntity> modelEntities = new ();

        List<StepRepresentationRelationshipWithTransformation> relationships 
            = stepFileRepresentation.StepRepresentationsRelationshipWithTransformation.GetAll();

        foreach (var relationship in relationships)
        {
            if(relationship == null)
            {
                continue;
            }

            //Creating model type for parent if necessary 
            ModelType parentModelType = null;

            if(modelTypes.Exists(mt => mt.Id == relationship.ParentId))
            {
                parentModelType = modelTypes.First(mt => mt.Id == relationship.ParentId);
            }
            else
            {
                parentModelType = GetModelType(relationship.ParentId, stepFileRepresentation);
                modelTypes.Add(parentModelType);
            }

            //Creating model type for child if necessary 
            ModelType childModelType = null;

            if (modelTypes.Exists(mt => mt.Id == relationship.ChildId))
            {
                childModelType = modelTypes.First(mt => mt.Id == relationship.ChildId);
            }
            else
            {
                childModelType = GetModelType(relationship.ChildId, stepFileRepresentation);
                if (string.IsNullOrEmpty(childModelType.Name))
                {
                    childModelType.Name = GetModelNameByNextAssemblyUsageOccurance(relationship.Id, stepFileRepresentation);
                    if (string.IsNullOrEmpty(childModelType.Name))
                    {
                        childModelType.Name = "Unnamed model";
                    }
                }
                modelTypes.Add(childModelType);
            }

            //Creating model entity for child
            ModelEntity childModelEntity = new ModelEntity(relationship.Id, childModelType);
            modelEntities.Add(childModelEntity);
            childModelEntity.Parent = parentModelType;
            childModelEntity.CoordinateSystem = GetCoordinateSystem(relationship.TransformationId, stepFileRepresentation);

            parentModelType.Childs.Add(childModelEntity);
        }

        //Root it is parentless model
        Model? result = ToModelRoot(modelTypes, modelEntities);

        if (string.IsNullOrEmpty(result.Name))
        {
            result.Name = "Unnamed model";
        }

        return result;
    }

    private static Model ToModelRoot(List<ModelType> modelTypes, List<ModelEntity> modelEntities)
    {
        List<ModelType> modelTypesResult = modelTypes.ToList();
        foreach(var modelType in modelTypes)
        {
            foreach (var child in modelType.Childs)
            {
                if (modelTypesResult.Contains(child.Type))
                {
                    modelTypesResult.Remove(child.Type);
                }
            }
        }
        if(modelTypesResult.Count != 1)
        {
            return new Model();
        }
        ModelEntity root = new(0, modelTypesResult[0]);
        return ToModel(root);
    }

    private static Model ToModel(ModelEntity modelEntity)
    {
        Model result = new() 
        {
            CoordinateSystem = modelEntity.CoordinateSystem,
            Name = modelEntity.Type.Name,
        };

        List<Model> resultChilds = new ();
        foreach(var child in modelEntity.Type.Childs)
        {
            var newChild = ToModel(child);
            newChild.Parent = result;
            resultChilds.Add(newChild);
        }

        result.Childs.AddRange(resultChilds);
        return result;
    }

    private static Model GetModel(int id, StepRepresentation stepFileRepresentation)
    {
        //Adding model if necessary
        Model? model = new ();
        //Finding model name
        model.Name = GetModelNameByShapeRepresentation(id, stepFileRepresentation);

        return model;
    }

    private static ModelType GetModelType(int id, StepRepresentation stepFileRepresentation)
    {
        ModelType model = new(id);
        //Finding model name
        model.Name = GetModelNameByShapeRepresentation(id, stepFileRepresentation);
        
        return model;
    }

    private static string GetModelNameByShapeRepresentation(int id, StepRepresentation stepFileRepresentation)
    {
        string result = string.Empty;
        var entity = stepFileRepresentation.StepShapeRepresentations.GetEntity(id);
        if (entity != null && entity.Name != null)
        {
            result = entity.Name!;
        }
        return result;
    }

    private static string GetModelNameByNextAssemblyUsageOccurance(int id, StepRepresentation stepRepresentation)
    {
        //finding CONTEXT_DEPENDENT_SHAPE_REPRESENTATION 
        var shapeRepr = stepRepresentation.StepContextDependentShapeRepresentations.GetEntity(id);
        //finding PRODUCT_DEFINITION_SHAPE
        int prodDefShapeId = shapeRepr?.RepresentedProductRelation ?? 0;
        if(prodDefShapeId == 0)
        {
            return string.Empty;
        }

        var pdsCollection = stepRepresentation.StepProductDefinitionShapes?.GetEntity(prodDefShapeId);

        //finding NEXT_ASSEMBLY_USAGE_OCCURRENCE
        int nextAssUsageOccId = pdsCollection?.Id ?? 0;
        if (nextAssUsageOccId == 0)
        {
            return string.Empty;
        }

        var nextAssUsageOcc = stepRepresentation
            .StepNextAssemblyUsageOccurrences?.GetEntity(nextAssUsageOccId);

        //finding name
        if (nextAssUsageOcc == null)
        {
            return string.Empty;
        }
        if (!string.IsNullOrWhiteSpace(nextAssUsageOcc.Description))
        {
            return nextAssUsageOcc.Description;
        }
        if (!string.IsNullOrWhiteSpace(nextAssUsageOcc.Identifier))
        {
            return nextAssUsageOcc.Identifier;
        }
        if (!string.IsNullOrWhiteSpace(nextAssUsageOcc.Name))
        {
            return nextAssUsageOcc.Name;
        }

        return string.Empty;
    }

    private static CoordinateSystem? GetCoordinateSystem(int id, StepRepresentation stepFileRepresentation)
    {
        // Оnly child CS is matter by some reason
        int currentCsId = 
            stepFileRepresentation.StepItemDefinedTransformations!
            .GetEntity(id)?.ChildId ?? 0;
        if (currentCsId == 0)
        {
            return null;
        }

        StepAxis2Placement3D? currentStepAxis2Placement3D
            = stepFileRepresentation.StepAxis2Placements3D!
            .GetEntity(currentCsId);

        if(currentStepAxis2Placement3D == null)
        {
            return null;
        }

        StepDirection? axisZ 
            = stepFileRepresentation.StepDirections!
            .GetEntity(currentStepAxis2Placement3D.ZAxisId);

        StepDirection? axisX
            = stepFileRepresentation.StepDirections!
            .GetEntity(currentStepAxis2Placement3D.XAxisId);
        
        StepCartesianPoint? origin
            = stepFileRepresentation.StepCartesianPoints!
            .GetEntity(currentStepAxis2Placement3D.LocationPointId);
        
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
