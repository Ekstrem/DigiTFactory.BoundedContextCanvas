using BoundedContextCanvas.Domain.Abstraction;

namespace BoundedContextCanvas.Domain.Implementation;

public sealed class BusinessDecision : IBusinessDecision
{
    public string Rule { get; }
    public string Rationale { get; }

    private BusinessDecision(string rule, string rationale)
    {
        Rule = rule;
        Rationale = rationale;
    }

    public static BusinessDecision CreateInstance(string rule, string rationale)
        => new(rule, rationale);
}
