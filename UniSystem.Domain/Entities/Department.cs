using UniSystem.Domain.ValueObjects.Department;

namespace UniSystem.Domain.Entities;

public class Department
{
    public int Id { get; private set; }

    public DepartmentName Name { get; private set; } = null!;
    
    protected Department() { }
    
    public Department(string name)
    {
        Name = new DepartmentName(name);
    }
}