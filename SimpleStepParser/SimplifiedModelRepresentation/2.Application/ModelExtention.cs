using SimpleStepParser.SimplifiedModelRepresentation._1.Domain;
using System.Text;

namespace SimpleStepParser.SimplifiedModelRepresentation._2.Application;

public static class ModelExtention
{
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
