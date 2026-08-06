namespace DefaultNamespace;

public class Equipment
{
    public Guid Id { get; set; } = Guid.newGuid();
    public string Name { get; set; }
    public string Type { get; set; } // sprava ili oprema neka
    public string Description { get; set; }
}