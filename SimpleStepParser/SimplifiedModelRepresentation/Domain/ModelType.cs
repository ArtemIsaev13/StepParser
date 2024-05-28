namespace SimpleStepParser.SimplifiedModelRepresentation.Domain;

public class ModelType
{
    public int Id { get; }
    public string? Name { get; set; }
    public List<ModelEntity> Childs { get; } = new ();

    public ModelType(int id)
    {
        Id = id;
    }
}
