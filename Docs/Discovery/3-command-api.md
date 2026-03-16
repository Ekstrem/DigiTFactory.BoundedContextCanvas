# Command API: BoundedContextCanvas v1

> API и события, вызывающие изменения состояния агрегата BoundedContext.
> Вход: секция 6 (Handled Commands) Aggregate Design Canvas.
> Атрибуты Request DTO коррелируют с входными Value Objects команды.

## HTTP-эндпоинты

| # | Метод | URI | Команда агрегата | Request DTO | Допустимые статусы |
|---|-------|-----|-----------------|-------------|-------------------|
| 1 | POST | /api/v1/bounded-context | CreateBoundedContext | CreateBoundedContextRequest | — (создание) |
| 2 | POST | /api/v1/bounded-context/{id}/strategic-importance | DefineStrategicImportance | DefineStrategicImportanceRequest | Draft, Defined, Published |
| 3 | POST | /api/v1/bounded-context/{id}/boundary | RefineBoundary | RefineBoundaryRequest | Defined, Published |
| 4 | POST | /api/v1/bounded-context/{id}/language/terms | AddTermToLanguage | AddTermToLanguageRequest | Defined, Published |
| 5 | DELETE | /api/v1/bounded-context/{id}/language/terms/{term} | RemoveTermFromLanguage | — (term из URI) | Defined, Published |
| 6 | POST | /api/v1/bounded-context/{id}/relationships | MapRelationship | MapRelationshipRequest | Defined, Published |
| 7 | DELETE | /api/v1/bounded-context/{id}/relationships/{targetId} | RemoveRelationship | — (targetId из URI) | Defined, Published |
| 8 | POST | /api/v1/bounded-context/{id}/contract | PublishContract | PublishContractRequest | Defined, Published |
| 9 | POST | /api/v1/bounded-context/{id}/business-decisions | AddBusinessDecision | AddBusinessDecisionRequest | Defined, Published |
| 10 | POST | /api/v1/bounded-context/{id}/assumptions | AddAssumption | AddAssumptionRequest | Defined, Published |
| 11 | POST | /api/v1/bounded-context/{id}/archive | ArchiveBoundedContext | — | Published |

## Входящие события из шины

| # | Событие | Команда агрегата | Источник |
|---|---------|-----------------|---------|
| 1 | CanvasImportRequested | CreateBoundedContext + DefineStrategicImportance + ... | CI/CD Pipeline |

## Request DTO

### CreateBoundedContextRequest
```json
{
  "technicalName": "string",     // ← Root.TechnicalName (PascalCase, ^[A-Z][a-zA-Z0-9]*$)
  "ownerTeam": "string",         // ← Root.OwnerTeam
  "businessPurpose": "string"    // ← Definition.BusinessPurpose
}
```
**Корреляция с VO:** Root (TechnicalName, OwnerTeam), Definition (BusinessPurpose)

### DefineStrategicImportanceRequest
```json
{
  "domainType": "string",           // ← StrategicClassification.DomainType: "Core" | "Supporting" | "Generic"
  "businessModelRole": "string",    // ← StrategicClassification.BusinessModelRole: "RevenueGenerator" | "EngagementCreator" | "ComplianceEnforcer"
  "evolution": "string",            // ← StrategicClassification.Evolution: "Genesis" | "CustomBuilt" | "Product" | "Commodity"
  "roleType": "string",            // ← DomainRole.RoleType: "Execution" | "Gateway" | "Analysis"
  "roleDescription": "string"      // ← DomainRole.Description
}
```
**Корреляция с VO:** StrategicClassification (DomainType, BusinessModelRole, Evolution), DomainRole (RoleType, Description)

### RefineBoundaryRequest
```json
{
  "responsibilities": [
    {
      "description": "string",     // ← Responsibility.Description
      "type": "string"             // ← Responsibility.Type: "Does" | "DoesNot"
    }
  ]
}
```
**Корреляция с VO:** List\<Responsibility\> (Description, Type)

### AddTermToLanguageRequest
```json
{
  "term": "string",                // ← UbiquitousLanguageTerm.Term
  "definition": "string"          // ← UbiquitousLanguageTerm.Definition
}
```
**Корреляция с VO:** UbiquitousLanguageTerm (Term, Definition)

### MapRelationshipRequest
```json
{
  "targetContextId": "guid",       // ← ExternalConnection.TargetContextId
  "targetContextName": "string",   // ← ExternalConnection.TargetContextName
  "direction": "string",           // ← ExternalConnection.Direction: "Upstream" | "Downstream" | "Mutual"
  "pattern": "string"              // ← ExternalConnection.Pattern: "SharedKernel" | "Partners" | "CustomerSupplier" | "Conformist" | "ACL" | "OHS" | "PublishedLanguage" | "SeparateWays"
}
```
**Корреляция с VO:** ExternalConnection (TargetContextId, TargetContextName, Direction, Pattern)

### PublishContractRequest
```json
{
  "items": [
    {
      "name": "string",              // ← PublicInterfaceItem.Name
      "type": "string",              // ← PublicInterfaceItem.Type: "Command" | "Query" | "DomainEvent"
      "direction": "string",         // ← PublicInterfaceItem.Direction: "Inbound" | "Outbound"
      "linkedResponsibility": "string" // ← PublicInterfaceItem.LinkedResponsibility
    }
  ]
}
```
**Корреляция с VO:** List\<PublicInterfaceItem\> (Name, Type, Direction, LinkedResponsibility)

### AddBusinessDecisionRequest
```json
{
  "rule": "string",                // ← BusinessDecision.Rule
  "rationale": "string"           // ← BusinessDecision.Rationale
}
```
**Корреляция с VO:** BusinessDecision (Rule, Rationale)

### AddAssumptionRequest
```json
{
  "statement": "string",           // ← Assumption.Statement
  "risk": "string"                 // ← Assumption.Risk: "Low" | "Medium" | "High"
}
```
**Корреляция с VO:** Assumption (Statement, Risk)

## Response DTO (единый для всех команд)

### BoundedContextOperationResponse
```json
{
  "aggregateId": "guid",
  "version": 0,
  "result": "string",              // "Success" | "Exception" | "WithWarnings"
  "reasons": ["string"] | null
}
```

**HTTP-статусы:**
- `200 OK` — Success или WithWarnings
- `400 Bad Request` — Exception (нарушение инварианта)
- `404 Not Found` — агрегат не найден
- `409 Conflict` — Optimistic Concurrency (version mismatch)

## Цепочка корреляции имён

```
Request DTO                    → MediatR Command                   → метод агрегата                    → доменное событие
CreateBoundedContext-          → CreateBoundedContext-              → aggregate.Create-                 → BoundedContext-
  Request                        Command                             BoundedContext(model)                Created

DefineStrategicImportance-     → DefineStrategicImportance-        → aggregate.Define-                 → StrategicImportance-
  Request                        Command                             StrategicImportance(model)          Defined

RefineBoundary-                → RefineBoundary-                   → aggregate.Refine-                 → Boundary-
  Request                        Command                             Boundary(model)                     Refined

AddTermToLanguage-             → AddTermToLanguage-                → aggregate.AddTerm-                → TermAddedTo-
  Request                        Command                             ToLanguage(model)                   Language

MapRelationship-               → MapRelationship-                  → aggregate.Map-                    → Relationship-
  Request                        Command                             Relationship(model)                 Mapped

PublishContract-               → PublishContract-                  → aggregate.Publish-                → PublicContract-
  Request                        Command                             Contract(model)                     Published

AddBusinessDecision-           → AddBusinessDecision-              → aggregate.Add-                    → BusinessDecision-
  Request                        Command                             BusinessDecision(model)             Added

AddAssumption-                 → AddAssumption-                    → aggregate.Add-                    → Assumption-
  Request                        Command                             Assumption(model)                   Added

ArchiveBoundedContext          → ArchiveBoundedContext-            → aggregate.Archive-                → BoundedContext-
  (no body)                      Command                             BoundedContext()                    Archived
```

## Версионирование

- Версия в URI: `/api/v1/bounded-context`
- Добавление опционального поля в Request — обратно совместимо (v1)
- Удаление/переименование поля — breaking change (v2)
- Поддерживать максимум 2 версии одновременно
