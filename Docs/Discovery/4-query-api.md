# Query API: BoundedContextCanvas v1

> API для потребителей, запрашивающих состояние канвасов.
> Вход: Materialized Views (артефакт 5).
> Атрибуты Response DTO один-к-одному с колонками MV.
> Query Side НЕ обращается к Domain и Event Store — только к Read Store.

## Потребители

| Потребитель | Что ему нужно | MV-источник |
|------------|--------------|-------------|
| UI архитектора | Полные детали канваса, история изменений | bounded_context_details, bounded_context_language, bounded_context_responsibilities, bounded_context_relationships, bounded_context_interface |
| UI архитектора | Список канвасов с фильтрацией по команде, типу домена, статусу | bounded_context_list |
| Context Map визуализатор | Граф связей между контекстами с паттернами | context_relationships |
| Дашборд руководителя | Статистика: Core/Supporting/Generic, покрытие, среднее число ответственностей | context_stats |

## HTTP-эндпоинты

| # | Метод | URI | Описание | Response DTO | MV-источник |
|---|-------|-----|----------|-------------|------------|
| 1 | GET | /api/v1/bounded-context/{id} | Полные детали канваса | BoundedContextDetailResponse | bounded_context_details + joined |
| 2 | GET | /api/v1/bounded-context | Список канвасов с фильтрами | BoundedContextListResponse | bounded_context_list |
| 3 | GET | /api/v1/bounded-context/{id}/language | Глоссарий контекста | LanguageResponse | bounded_context_language |
| 4 | GET | /api/v1/bounded-context/{id}/relationships | Связи контекста | RelationshipsResponse | bounded_context_relationships |
| 5 | GET | /api/v1/bounded-context/{id}/interface | Публичный интерфейс | PublicInterfaceResponse | bounded_context_interface |
| 6 | GET | /api/v1/bounded-context/relationships/graph | Граф всех связей (Context Map) | ContextMapGraphResponse | context_relationships |
| 7 | GET | /api/v1/bounded-context/stats | Агрегированная статистика | ContextStatsResponse | context_stats |

**Параметры фильтрации эндпоинта #2:**
- `?ownerTeam={team}` — фильтр по команде-владельцу
- `?domainType={Core|Supporting|Generic}` — фильтр по типу домена
- `?status={Draft|Defined|Published|Archived}` — фильтр по статусу
- `?search={text}` — полнотекстовый поиск по имени и назначению
- `?page={N}&pageSize={N}` — пагинация (по умолчанию page=1, pageSize=20)
- `?sortBy={name|createdAt|updatedAt}&sortDir={asc|desc}` — сортировка

## Response DTO

### BoundedContextDetailResponse
```json
{
  "id": "guid",                                    // ← MV: bounded_context_details.id
  "technicalName": "string",                       // ← MV: bounded_context_details.technical_name
  "ownerTeam": "string",                           // ← MV: bounded_context_details.owner_team
  "status": "string",                              // ← MV: bounded_context_details.status
  "businessPurpose": "string",                     // ← MV: bounded_context_details.business_purpose
  "strategicClassification": {
    "domainType": "string",                        // ← MV: bounded_context_details.domain_type
    "businessModelRole": "string",                 // ← MV: bounded_context_details.business_model_role
    "evolution": "string"                          // ← MV: bounded_context_details.evolution
  },
  "domainRole": {
    "roleType": "string",                          // ← MV: bounded_context_details.role_type
    "description": "string"                        // ← MV: bounded_context_details.role_description
  },
  "responsibilities": [
    {
      "description": "string",                     // ← MV: bounded_context_responsibilities.description
      "type": "string"                             // ← MV: bounded_context_responsibilities.type
    }
  ],
  "language": [
    {
      "term": "string",                            // ← MV: bounded_context_language.term
      "definition": "string"                       // ← MV: bounded_context_language.definition
    }
  ],
  "relationships": [
    {
      "targetContextId": "guid",                   // ← MV: bounded_context_relationships.target_context_id
      "targetContextName": "string",               // ← MV: bounded_context_relationships.target_context_name
      "direction": "string",                       // ← MV: bounded_context_relationships.direction
      "pattern": "string"                          // ← MV: bounded_context_relationships.pattern
    }
  ],
  "publicInterface": [
    {
      "name": "string",                            // ← MV: bounded_context_interface.name
      "type": "string",                            // ← MV: bounded_context_interface.type
      "direction": "string",                       // ← MV: bounded_context_interface.direction
      "linkedResponsibility": "string"             // ← MV: bounded_context_interface.linked_responsibility
    }
  ],
  "businessDecisions": [
    {
      "rule": "string",                            // ← MV: bounded_context_details (JSON) .business_decisions[].rule
      "rationale": "string"                        // ← MV: bounded_context_details (JSON) .business_decisions[].rationale
    }
  ],
  "assumptions": [
    {
      "statement": "string",                       // ← MV: bounded_context_details (JSON) .assumptions[].statement
      "risk": "string"                             // ← MV: bounded_context_details (JSON) .assumptions[].risk
    }
  ],
  "version": 0,                                    // ← MV: bounded_context_details.version
  "createdAt": "datetime",                         // ← MV: bounded_context_details.created_at
  "updatedAt": "datetime"                          // ← MV: bounded_context_details.updated_at
}
```

### BoundedContextListResponse
```json
{
  "items": [
    {
      "id": "guid",                                // ← MV: bounded_context_list.id
      "technicalName": "string",                   // ← MV: bounded_context_list.technical_name
      "ownerTeam": "string",                       // ← MV: bounded_context_list.owner_team
      "status": "string",                          // ← MV: bounded_context_list.status
      "businessPurpose": "string",                 // ← MV: bounded_context_list.business_purpose
      "domainType": "string",                      // ← MV: bounded_context_list.domain_type
      "roleType": "string",                        // ← MV: bounded_context_list.role_type
      "responsibilityCount": 0,                    // ← MV: bounded_context_list.responsibility_count
      "relationshipCount": 0,                      // ← MV: bounded_context_list.relationship_count
      "updatedAt": "datetime"                      // ← MV: bounded_context_list.updated_at
    }
  ],
  "total": 0,
  "page": 1,
  "pageSize": 20
}
```

### LanguageResponse
```json
{
  "contextId": "guid",
  "terms": [
    {
      "term": "string",                            // ← MV: bounded_context_language.term
      "definition": "string"                       // ← MV: bounded_context_language.definition
    }
  ]
}
```

### RelationshipsResponse
```json
{
  "contextId": "guid",
  "relationships": [
    {
      "targetContextId": "guid",                   // ← MV: bounded_context_relationships.target_context_id
      "targetContextName": "string",               // ← MV: bounded_context_relationships.target_context_name
      "direction": "string",                       // ← MV: bounded_context_relationships.direction
      "pattern": "string"                          // ← MV: bounded_context_relationships.pattern
    }
  ]
}
```

### PublicInterfaceResponse
```json
{
  "contextId": "guid",
  "items": [
    {
      "name": "string",                            // ← MV: bounded_context_interface.name
      "type": "string",                            // ← MV: bounded_context_interface.type
      "direction": "string",                       // ← MV: bounded_context_interface.direction
      "linkedResponsibility": "string"             // ← MV: bounded_context_interface.linked_responsibility
    }
  ]
}
```

### ContextMapGraphResponse
```json
{
  "nodes": [
    {
      "contextId": "guid",                         // ← MV: context_relationships.source_context_id
      "technicalName": "string",                   // ← MV: context_relationships.source_context_name
      "domainType": "string"                       // ← MV: context_relationships.source_domain_type
    }
  ],
  "edges": [
    {
      "sourceContextId": "guid",                   // ← MV: context_relationships.source_context_id
      "targetContextId": "guid",                   // ← MV: context_relationships.target_context_id
      "direction": "string",                       // ← MV: context_relationships.direction
      "pattern": "string"                          // ← MV: context_relationships.pattern
    }
  ]
}
```

### ContextStatsResponse
```json
{
  "totalContexts": 0,                             // ← MV: context_stats.total_contexts
  "byDomainType": {
    "core": 0,                                     // ← MV: context_stats.core_count
    "supporting": 0,                               // ← MV: context_stats.supporting_count
    "generic": 0                                   // ← MV: context_stats.generic_count
  },
  "byStatus": {
    "draft": 0,                                    // ← MV: context_stats.draft_count
    "defined": 0,                                  // ← MV: context_stats.defined_count
    "published": 0,                                // ← MV: context_stats.published_count
    "archived": 0                                  // ← MV: context_stats.archived_count
  },
  "avgResponsibilitiesPerContext": 0.0,            // ← MV: context_stats.total_responsibilities / total_contexts
  "avgRelationshipsPerContext": 0.0,               // ← MV: context_stats.total_relationships / total_contexts
  "totalRelationships": 0                          // ← MV: context_stats.total_relationships
}
```

## Цепочка: MV → QueryHandler → Response

```
GET /api/v1/bounded-context/{id}
  → GetBoundedContextDetailQuery : IRequest<BoundedContextDetailResponse>
    → IReadRepository<IBoundedContextCanvas, BoundedContextDetailReadModel>.GetByIdAsync(id)
      → SELECT * FROM read_model.bounded_context_details WHERE id = @id
      + JOIN bounded_context_language, bounded_context_responsibilities, bounded_context_relationships, bounded_context_interface

GET /api/v1/bounded-context?domainType=Core&status=Published
  → GetBoundedContextListQuery : IRequest<BoundedContextListResponse>
    → IReadRepository<IBoundedContextCanvas, BoundedContextListReadModel>.QueryAsync(filter)
      → SELECT * FROM read_model.bounded_context_list WHERE domain_type = @domainType AND status = @status

GET /api/v1/bounded-context/relationships/graph
  → GetContextMapGraphQuery : IRequest<ContextMapGraphResponse>
    → IReadRepository<IBoundedContextCanvas, ContextRelationshipReadModel>.GetAllAsync()
      → SELECT * FROM read_model.context_relationships

GET /api/v1/bounded-context/stats
  → GetContextStatsQuery : IRequest<ContextStatsResponse>
    → IReadRepository<IBoundedContextCanvas, ContextStatsReadModel>.GetSingleAsync()
      → SELECT * FROM read_model.context_stats WHERE id = 'singleton'
```
