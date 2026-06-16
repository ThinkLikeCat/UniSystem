using UniSystem.Domain.Common;

namespace UniSystem.Domain.ValueObjects;

public readonly record struct UserId(Guid Value) : IEntityId<UserId>;
public readonly record struct DocumentId(Guid Value) : IEntityId<DocumentId>;
public readonly record struct AttachmentId(Guid Value) : IEntityId<AttachmentId>;