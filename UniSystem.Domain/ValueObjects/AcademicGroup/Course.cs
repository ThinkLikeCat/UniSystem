using UniSystem.Domain.Exceptions;

namespace UniSystem.Domain.ValueObjects.AcademicGroup;

public record Course
{
    public const short MinCourse = 1;
    public const short MaxCourse = 6;
    
    public short Value { get; }

    public Course(short value)
    {
        if (value < MinCourse || value > MaxCourse)
            throw new DomainException($"Курс не может быть меньше {MinCourse} или больше {MaxCourse}.");
        
        Value = value;
    }
    
    public static implicit operator short(Course course) => course.Value;
}