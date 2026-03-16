using BoundedContextCanvas.Domain.Abstraction;
using BoundedContextCanvas.Domain.Enums;

namespace BoundedContextCanvas.Domain.Implementation;

public sealed class Assumption : IAssumption
{
    public string Statement { get; }
    public RiskLevelEnum Risk { get; }

    private Assumption(string statement, RiskLevelEnum risk)
    {
        Statement = statement;
        Risk = risk;
    }

    public static Assumption CreateInstance(string statement, RiskLevelEnum risk)
        => new(statement, risk);
}
