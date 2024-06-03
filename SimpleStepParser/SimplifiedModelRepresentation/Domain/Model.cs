using MathNet.Spatial.Euclidean;

namespace SimpleStepParser.SimplifiedModelRepresentation.Domain;

public class Model
{
    public string? Name { get; set; }
    public List<Model> Childs { get; } = new ();
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
            Model newChild = child.GetDeepCopy();
            newChild.Parent = result;
            result.Childs.Add(newChild);
        }
        return result;
    }
}
