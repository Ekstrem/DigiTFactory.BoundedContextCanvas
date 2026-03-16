using Hive.SeedWorks.TacticalPatterns;

namespace BoundedContextCanvas.Domain.Abstraction;

public interface IUbiquitousLanguageTerm : IValueObject
{
    string Term { get; }
    string Definition { get; }
}
