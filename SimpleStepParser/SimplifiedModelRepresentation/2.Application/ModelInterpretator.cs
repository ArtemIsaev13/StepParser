using SimpleStepParser.SimplifiedModelRepresentation._1.Domain;
using SimpleStepParser.StepFileRepresentation._1.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            models[relationship.ParentId].Childs.Add(models[relationship.ChildId]);
            models[relationship.ChildId].Parent = models[relationship.ParentId];
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
        Model model = null;
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

    private record ModelWithId(int Id, Model Model);
}
