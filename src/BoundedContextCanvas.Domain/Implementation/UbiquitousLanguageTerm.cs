using BoundedContextCanvas.Domain.Abstraction;

namespace BoundedContextCanvas.Domain.Implementation;

public sealed class UbiquitousLanguageTerm : IUbiquitousLanguageTerm
{
    public string Term { get; }
    public string Definition { get; }

    private UbiquitousLanguageTerm(string term, string definition)
    {
        Term = term;
        Definition = definition;
    }

    public static UbiquitousLanguageTerm CreateInstance(string term, string definition)
        => new(term, definition);
}
