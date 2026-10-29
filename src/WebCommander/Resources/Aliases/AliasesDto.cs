namespace WebCommander.Resources.Aliases.AliasesDto;

/// <summary>One domain alias.</summary>
public sealed class DomainAlias : Dto
{
    protected override string? EnvelopeKey => "alias";

    public long? Id { get; set; }

    public string? Alias { get; set; }

    public string? CreatedAt { get; set; }

    public string? UpdatedAt { get; set; }
}

/// <summary>The create body, <c>{"alias": "&lt;hostname&gt;"}</c>, with the field bare.</summary>
public sealed class AliasInput : Dto
{
    public string? Alias { get; set; }
}

/// <summary>The listing's five-key <c>pagination</c> block.</summary>
public sealed class Pagination : Dto
{
    public long? Total { get; set; }

    public long? Limit { get; set; }

    public long? Offset { get; set; }

    public bool? HasNext { get; set; }

    public bool? HasPrevious { get; set; }
}

/// <summary>One page of the alias listing.</summary>
public sealed class AliasPage : Dto
{
    public IReadOnlyList<DomainAlias?>? Aliases { get; set; }

    public Pagination? Pagination { get; set; }
}
