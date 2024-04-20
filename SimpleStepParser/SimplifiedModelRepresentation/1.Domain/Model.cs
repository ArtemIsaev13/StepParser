using MathNet.Spatial.Euclidean;

namespace SimpleStepParser.SimplifiedModelRepresentation._1.Domain;

public class Model
{
    public string? Name { get; set; }
    public List<Model> Childs { get; } = new List<Model>();
    public Model? Parent { get; set; }
    public CoordinateSystem? CoordinateSystem { get; set; }
}
