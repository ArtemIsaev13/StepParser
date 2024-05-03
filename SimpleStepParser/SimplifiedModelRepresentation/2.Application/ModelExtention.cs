﻿using MathNet.Spatial.Euclidean;
using SimpleStepParser.SimplifiedModelRepresentation._1.Domain;
using System.Text;
using System.Text.RegularExpressions;

namespace SimpleStepParser.SimplifiedModelRepresentation._2.Application;

public static class ModelExtention
{
    /// <summary>
    /// Returns a transformation from current model to its child
    /// </summary>
    /// <param name="child"></param>
    /// <returns></returns>
    public static CoordinateSystem? GetTransformationTo(this Model model, Model child)
    {
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
                throw new ArgumentException("This model isn't child.");
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
    /// <returns></returns>
    public static List<Model> GetChildByName(this Model model, Regex regex)
    {
        List<Model> result = new();
        if(!string.IsNullOrEmpty(model.Name))
        {
            var match = regex.Match(model.Name);
            if(match.Success)
            {
                result.Add(model);
            }
        }

        foreach(var child in model.Childs)
        {
            List<Model> models = child.GetChildByName(regex);
            result.AddRange(models);
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
        result.AppendLine($"{new String('\t', level)}{model.Name}");
        foreach(var child in model.Childs)
        {
            result.Append(GetModelTreeRecursive(child, level+1));
        }
        return result;
    }
}