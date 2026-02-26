namespace UniSystem.Domain;

public class Statement
{ 
    public Guid Id { get; private set; }
    public string Type { get; private set; }
    
    public void Sign() => Id = Guid.NewGuid();
}