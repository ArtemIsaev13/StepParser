using MathNet.Spatial.Euclidean;

namespace SimpleStepParser.SimplifiedModelRepresentation.Domain;

public class ModelEntity
{
    public int Id { get; }
    public ModelType? Parent { get; set; }
    public CoordinateSystem? CoordinateSystem { get; set; }

    public ModelType Type { get; }

    public ModelEntity(int id, ModelType type)
    {
        Id = id;
        Type = type;
    }
}
