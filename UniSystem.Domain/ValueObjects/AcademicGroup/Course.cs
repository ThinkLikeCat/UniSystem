using UniSystem.Domain.Exceptions;

namespace UniSystem.Domain.ValueObjects.AcademicGroup;

public record Course
{
    public const short MinCourse = 1;
    public const short MaxCourse = 6;
    
    public short Value { get; private set; }

    public Course(short value)
    {
        if (value < MinCourse || value > MaxCourse)
            throw new CourseOutOfRangeException(value);
        
        Value = value;
    }
    
    public static implicit operator short(Course course) => course.Value;
}