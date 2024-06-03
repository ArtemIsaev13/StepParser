using MathNet.Spatial.Euclidean;
using SimpleStepParser.SimplifiedModelRepresentation.Domain;
using SimpleStepParser.StepFileRepresentation.Domain.Exceptions;
using System.Text;
using System.Text.RegularExpressions;

namespace SimpleStepParser.SimplifiedModelRepresentation.Application;

public static class ModelExtention
{
    /// <summary>
    /// Returns a transformation from current model to its child
    /// </summary>
    /// <param name="child"></param>
    /// <returns></returns>
    public static CoordinateSystem? GetTransformationTo(this Model model, Model child)
    {
        if(child?.CoordinateSystem == null)
        {
            return null;
        }

        Model current = child;
        CoordinateSystem result = child.CoordinateSystem;
        if(result == null)
        {
            return null;
        }

        while(true)
        {
            if(current.Parent == null)
            {
                throw new UnableToFindPathFromModelToChildException();
            }
            if(current.Parent == model)
            {
                return result;
            }
            current = current.Parent;
            if(current.CoordinateSystem == null)
            {
                return null;
            }
            result = new CoordinateSystem(current.CoordinateSystem.Multiply(result));
        }
    }

    /// <summary>
    /// Returns all child elements if their name matches the regular expression.
    /// Can returns themself.
    /// </summary>
    /// <param name="model">Root</param>
    /// <param name="regex">Regex</param>
    /// <param name="recursive">Should children's children be checked?</param>
    /// <returns></returns>
    public static List<Model> GetChildByName(this Model model, Regex regex, bool recursive = true)
    {
        List<Model> result = new();

        foreach (var child in model.Childs)
        {
            if (!string.IsNullOrEmpty(child.Name))
            {
                var match = regex.Match(child.Name);
                if (match.Success)
                {
                    result.Add(child);
                }
            }

            if (recursive)
            {
                List<Model> models = child.GetChildByName(regex);
                result.AddRange(models);
            }
        }

        return result;
    }

    /// <summary>
    /// Returns a text version of the model hierarchy
    /// </summary>
    /// <param name="model">Root model</param>
    /// <returns></returns>
    public static string GetModelTreeString(this Model model)
    {
        if(model == null)
        {
            return string.Empty;
        }

        StringBuilder sb = GetModelTreeRecursive(model, 0);

        return sb.ToString();
    }

    private static StringBuilder GetModelTreeRecursive(Model model, int level)
    {
        StringBuilder result = new StringBuilder();
        result.AppendLine($"{new String('\t', level)}+{model.Name}");
        foreach(var child in model.Childs)
        {
            result.Append(GetModelTreeRecursive(child, level+1));
        }
        return result;
    }
}
