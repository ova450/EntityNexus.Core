using EntityNexus.Abstractions.Domain.Model;

namespace EntityNexus.Additionals.History;

public class HistoryEntry<TKey> : AEntity<int> //, IHistoryEntry
    where TKey : IEquatable<TKey>
{
    public TKey EntityId { get; set; } = default!;

    public string EntityName { get; set; } = default!;
    public string PropertyName { get; set; } = default!;

    public string? OldValue { get; set; }
    public string? NewValue { get; set; }

    public DateTimeOffset ChangedAt { get; set; }
}