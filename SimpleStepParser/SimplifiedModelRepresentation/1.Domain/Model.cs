using MathNet.Spatial.Euclidean;

namespace SimpleStepParser.SimplifiedModelRepresentation._1.Domain;

public class Model
{
    public string? Name { get; set; }
    public List<Model> Childs { get; } = new List<Model>();
    public Model? Parent { get; set; }
    public CoordinateSystem? CoordinateSystem { get; set; }

    public Model GetDeepCopy()
    {
        Model result = new();
        result.Name = Name;
        result.Parent = Parent;
        result.CoordinateSystem = CoordinateSystem;
        foreach (var child in Childs)
        {
            result.Childs.Add(child.GetDeepCopy());
        }
        return result;
    }
}
