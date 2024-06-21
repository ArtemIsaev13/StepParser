using MathNet.Spatial.Euclidean;
using SimpleStepParser.SimplifiedModelRepresentation.Domain;
using SimpleStepParser.StepFileRepresentation.Domain.Entities;
using SimpleStepParser.StepFileRepresentation.Domain.Exceptions;
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

        Model? result = GetModelTree(stepFileRepresentation, false);
        if(result is null)
        {
            result = GetModelTree(stepFileRepresentation, true);
        }

        if(result is null)
        {
            throw new UnableToBuildModelTreeException();
        }

        return result;
    }

    private static Model? GetModelTree(StepRepresentation stepFileRepresentation, bool isRep2IsParent)
    {
        List<ModelType> modelTypes = new();
        List<ModelEntity> modelEntities = new();

        List<StepRepresentationRelationshipWithTransformation> relationships
            = stepFileRepresentation.StepRepresentationsRelationshipWithTransformation.GetAll();

        foreach (var relationship in relationships)
        {
            if (relationship == null)
            {
                continue;
            }

            int localParentId = isRep2IsParent ? relationship.Rep2Id : relationship.Rep1Id;
            int localChildId = isRep2IsParent ? relationship.Rep1Id : relationship.Rep2Id;

            //Creating model type for parent if necessary 
            ModelType? parentModelType = null;

            if (modelTypes.Exists(mt => mt.Id == localParentId))
            {
                parentModelType = modelTypes.First(mt => mt.Id == localParentId);
            }
            else
            {
                parentModelType = GetModelType(localParentId, stepFileRepresentation);
                modelTypes.Add(parentModelType);
            }

            //Creating model type for child if necessary 
            ModelType? childModelType = null;

            if (modelTypes.Exists(mt => mt.Id == localChildId))
            {
                childModelType = modelTypes.First(mt => mt.Id == localChildId);
            }
            else
            {
                childModelType = GetModelType(localChildId, stepFileRepresentation);
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
        Model? result = ToModelRoot(modelTypes);

        return result;
    }

    private static Model? ToModelRoot(List<ModelType> modelTypes)
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
            return null;
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

    private static ModelType GetModelType(int id, StepRepresentation stepFileRepresentation)
    {
        ModelType model = new(id);
        //Finding model name
        model.Name = SiemensNxModelNameFixer
            .FixString(GetModelNameByProduct(id, stepFileRepresentation));

        return model;
    }

    private static string GetModelNameByProduct(int shapeRepresentationId, StepRepresentation stepRepresentation)
    {
        List<StepShapeDefinitionRepresentation> allShapeRepresentation 
            = stepRepresentation.StepShapeDefinitionRepresentations.GetAll();
        if(!allShapeRepresentation
            .Exists(sr => sr.UsedRepresentation == shapeRepresentationId))
        {
            return string.Empty;
        }

        int properProductDefinitionShapeId 
            = allShapeRepresentation
            .Find(sr => sr.UsedRepresentation == shapeRepresentationId)?
            .Definition ?? 0;
        if(properProductDefinitionShapeId == 0)
        {
            return string.Empty;
        }

        int properProductDefinitionId 
            = stepRepresentation
            .StepProductDefinitionShapes
            .GetEntity(properProductDefinitionShapeId)?.Definition ?? 0;
        if(properProductDefinitionId == 0)
        {
            return string.Empty;
        }

        int prodDefFormWithSpecSource 
            = stepRepresentation
            .StepProductDefinitions
            .GetEntity(properProductDefinitionId)?
            .Formation ?? 0;
        if(prodDefFormWithSpecSource == 0)
        {
            return string.Empty;
        }

        int properProdId 
            = stepRepresentation
            .StepProductDefinitionFormationWithSpecifiedSources
            .GetEntity(prodDefFormWithSpecSource)?.OfProduct ?? 0;
        if(properProdId == 0)
        {
            return string.Empty;
        }

        string name 
            = stepRepresentation
            .StepProducts.GetEntity(properProdId)?
            .Name ?? string.Empty;

        if (string.IsNullOrEmpty(name))
        {
            name = "Unnamed model";
        }

        return name;
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
