using MediatR;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Entities;
using UniSystem.Domain.Exceptions;
using UniSystem.Domain.ValueObjects.Department;

namespace UniSystem.Application.Departments;

public record CreateDepartmentCommand(string Name) : IRequest<int>;

public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateDepartmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = new Department(request.Name);
        _context.Departments.Add(department);
        await _context.SaveChangesAsync(cancellationToken);
        return department.Id;
    }
}

public record UpdateDepartmentCommand(int Id, string Name) : IRequest;

public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateDepartmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = await _context.Departments
            .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

        if (department is null)
            throw new DomainException("Department not found.");

        _context.Entry(department).Property("Name").CurrentValue = new DepartmentName(request.Name);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

public record DeleteDepartmentCommand(int Id) : IRequest;

public class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteDepartmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = await _context.Departments
            .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

        if (department is null)
            throw new DomainException("Department not found.");

        _context.Departments.Remove(department);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
