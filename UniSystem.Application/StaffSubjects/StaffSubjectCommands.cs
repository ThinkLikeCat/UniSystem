using MediatR;
using Microsoft.EntityFrameworkCore;
using UniSystem.Application.Common.Interfaces;
using UniSystem.Domain.Entities;
using UniSystem.Domain.Exceptions;

namespace UniSystem.Application.StaffSubjects;

public record CreateStaffSubjectCommand(Guid StaffId, int SubjectId) : IRequest;

public class CreateStaffSubjectCommandHandler : IRequestHandler<CreateStaffSubjectCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateStaffSubjectCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(CreateStaffSubjectCommand request, CancellationToken cancellationToken)
    {
        var exists = await _context.StaffSubjects
            .AnyAsync(s => s.StaffId == request.StaffId && s.SubjectId == request.SubjectId, cancellationToken);

        if (exists)
            throw new DomainException("Staff member is already linked to this subject.");

        var staffExists = await _context.StaffProfiles
            .AnyAsync(p => p.Id == request.StaffId, cancellationToken);

        if (!staffExists)
            throw new DomainException("Staff profile not found.");

        var subjectExists = await _context.Subjects
            .AnyAsync(s => s.Id == request.SubjectId, cancellationToken);

        if (!subjectExists)
            throw new DomainException("Subject not found.");

        var entity = new StaffSubject(request.StaffId, request.SubjectId);
        _context.StaffSubjects.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

public record DeleteStaffSubjectCommand(Guid StaffId, int SubjectId) : IRequest;

public class DeleteStaffSubjectCommandHandler : IRequestHandler<DeleteStaffSubjectCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteStaffSubjectCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteStaffSubjectCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.StaffSubjects
            .FirstOrDefaultAsync(s => s.StaffId == request.StaffId && s.SubjectId == request.SubjectId, cancellationToken);

        if (entity is null)
            throw new DomainException("Staff-subject link not found.");

        _context.StaffSubjects.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
