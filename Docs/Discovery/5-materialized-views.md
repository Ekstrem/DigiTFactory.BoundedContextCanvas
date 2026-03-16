# Materialized Views: BoundedContextCanvas

> Сущности на уровне БД, проецируемые из доменных событий.
> Вход: секция 7 (Created Events) Aggregate Design Canvas.
> Атрибуты колонок коррелируют с Query API Response DTO (артефакт 4).

## Обзор проекций

| # | Таблица | Тип | Обслуживает Query API | Read Model класс |
|---|---------|-----|----------------------|-----------------|
| 1 | bounded_context_details | Основная | GET /bounded-context/{id} | BoundedContextDetailReadModel |
| 2 | bounded_context_list | Специализированная | GET /bounded-context?filter= | BoundedContextListReadModel |
| 3 | bounded_context_language | Append/Delete | GET /bounded-context/{id}/language | BoundedContextLanguageReadModel |
| 4 | bounded_context_responsibilities | Replace-all | GET /bounded-context/{id}/boundary | BoundedContextResponsibilityReadModel |
| 5 | bounded_context_relationships | Append/Delete | GET /bounded-context/{id}/relationships | BoundedContextRelationshipReadModel |
| 6 | bounded_context_interface | Replace-all | GET /bounded-context/{id}/interface | BoundedContextInterfaceReadModel |
| 7 | context_relationships | Append/Delete | GET /bounded-context/relationships/graph | ContextRelationshipReadModel |
| 8 | context_stats | Агрегированная | GET /bounded-context/stats | ContextStatsReadModel |

---

## 1. bounded_context_details

**Схема:** read_model
**Read Model:** `BoundedContextDetailReadModel : IReadModel<IBoundedContextCanvas>`
**Тип:** Основная проекция — полное состояние канваса для детального просмотра.

### Колонки

| Колонка | Тип | Источник (событие → поле) | Query API DTO |
|---------|-----|--------------------------|--------------|
| id | uuid PK | BoundedContextCreated → ContextId | BoundedContextDetailResponse.id |
| technical_name | varchar(100) | BoundedContextCreated → TechnicalName | .technicalName |
| owner_team | varchar(100) | BoundedContextCreated → OwnerTeam | .ownerTeam |
| status | varchar(20) | BoundedContextCreated → "Draft", StrategicImportanceDefined → "Defined", PublicContractPublished → "Published", BoundedContextArchived → "Archived" | .status |
| business_purpose | text | BoundedContextCreated → BusinessPurpose | .businessPurpose |
| domain_type | varchar(20) | StrategicImportanceDefined → DomainType | .strategicClassification.domainType |
| business_model_role | varchar(30) | StrategicImportanceDefined → BusinessModelRole | .strategicClassification.businessModelRole |
| evolution | varchar(20) | StrategicImportanceDefined → Evolution | .strategicClassification.evolution |
| role_type | varchar(20) | StrategicImportanceDefined → RoleType | .domainRole.roleType |
| role_description | text | StrategicImportanceDefined → RoleDescription | .domainRole.description |
| business_decisions | jsonb | BusinessDecisionAdded → append to array | .businessDecisions[] |
| assumptions | jsonb | AssumptionAdded → append to array | .assumptions[] |
| version | bigint | Каждое событие → Version | .version |
| created_at | timestamptz | BoundedContextCreated → Timestamp | .createdAt |
| updated_at | timestamptz | Каждое событие → Timestamp | .updatedAt |

### Проекционные правила

| Доменное событие | Действие | Затронутые колонки |
|-----------------|---------|-------------------|
| BoundedContextCreated | INSERT | id, technical_name, owner_team, status='Draft', business_purpose, version, created_at, updated_at |
| StrategicImportanceDefined | UPDATE | status='Defined', domain_type, business_model_role, evolution, role_type, role_description, version, updated_at |
| BoundaryRefined | UPDATE | version, updated_at |
| TermAddedToLanguage | UPDATE | version, updated_at |
| TermRemovedFromLanguage | UPDATE | version, updated_at |
| RelationshipMapped | UPDATE | version, updated_at |
| RelationshipRemoved | UPDATE | version, updated_at |
| PublicContractPublished | UPDATE | status='Published', version, updated_at |
| BusinessDecisionAdded | UPDATE | business_decisions (jsonb append), version, updated_at |
| AssumptionAdded | UPDATE | assumptions (jsonb append), version, updated_at |
| StrategicTypeChanged | UPDATE | domain_type, business_model_role, evolution, version, updated_at |
| BoundedContextArchived | UPDATE | status='Archived', version, updated_at |

### Индексы

- PK: `id`
- IX_bounded_context_details_technical_name: `technical_name` (UNIQUE)
- IX_bounded_context_details_owner_team: `owner_team`

---

## 2. bounded_context_list

**Схема:** read_model
**Read Model:** `BoundedContextListReadModel : IReadModel<IBoundedContextCanvas>`
**Тип:** Специализированная проекция — список с денормализованными счётчиками для фильтрации и пагинации.

### Колонки

| Колонка | Тип | Источник (событие → поле) | Query API DTO |
|---------|-----|--------------------------|--------------|
| id | uuid PK | BoundedContextCreated → ContextId | BoundedContextListResponse.items[].id |
| technical_name | varchar(100) | BoundedContextCreated → TechnicalName | .technicalName |
| owner_team | varchar(100) | BoundedContextCreated → OwnerTeam | .ownerTeam |
| status | varchar(20) | Различные события → статус | .status |
| business_purpose | text | BoundedContextCreated → BusinessPurpose | .businessPurpose |
| domain_type | varchar(20) | StrategicImportanceDefined → DomainType | .domainType |
| role_type | varchar(20) | StrategicImportanceDefined → RoleType | .roleType |
| responsibility_count | int | BoundaryRefined → count(Responsibilities) | .responsibilityCount |
| relationship_count | int | RelationshipMapped/Removed → ±1 | .relationshipCount |
| updated_at | timestamptz | Каждое событие → Timestamp | .updatedAt |

### Проекционные правила

| Доменное событие | Действие | Затронутые колонки |
|-----------------|---------|-------------------|
| BoundedContextCreated | INSERT | id, technical_name, owner_team, status='Draft', business_purpose, responsibility_count=0, relationship_count=0, updated_at |
| StrategicImportanceDefined | UPDATE | status='Defined', domain_type, role_type, updated_at |
| BoundaryRefined | UPDATE | responsibility_count=len(Responsibilities), updated_at |
| RelationshipMapped | UPDATE | relationship_count++, updated_at |
| RelationshipRemoved | UPDATE | relationship_count--, updated_at |
| PublicContractPublished | UPDATE | status='Published', updated_at |
| BoundedContextArchived | UPDATE | status='Archived', updated_at |

### Индексы

- PK: `id`
- IX_bounded_context_list_owner_team: `owner_team`
- IX_bounded_context_list_domain_type: `domain_type`
- IX_bounded_context_list_status: `status`
- IX_bounded_context_list_updated_at: `updated_at DESC` (сортировка по умолчанию)
- IX_bounded_context_list_search: GIN-индекс на `technical_name, business_purpose` (для полнотекстового поиска)

---

## 3. bounded_context_language

**Схема:** read_model
**Read Model:** `BoundedContextLanguageReadModel : IReadModel<IBoundedContextCanvas>`
**Тип:** Append/Delete — термины добавляются и удаляются по одному.

### Колонки

| Колонка | Тип | Источник (событие → поле) | Query API DTO |
|---------|-----|--------------------------|--------------|
| context_id | uuid | TermAddedToLanguage → ContextId | LanguageResponse.contextId |
| term | varchar(200) | TermAddedToLanguage → Term | .terms[].term |
| definition | text | TermAddedToLanguage → Definition | .terms[].definition |

### Проекционные правила

| Доменное событие | Действие | Затронутые колонки |
|-----------------|---------|-------------------|
| TermAddedToLanguage | INSERT | context_id, term, definition |
| TermRemovedFromLanguage | DELETE | WHERE context_id = @id AND term = @term |
| BoundedContextArchived | DELETE | WHERE context_id = @id (все термины) |

### Индексы

- PK: `(context_id, term)` — композитный
- IX_bounded_context_language_context_id: `context_id`

---

## 4. bounded_context_responsibilities

**Схема:** read_model
**Read Model:** `BoundedContextResponsibilityReadModel : IReadModel<IBoundedContextCanvas>`
**Тип:** Replace-all — при каждом RefineBoundary полностью заменяется список.

### Колонки

| Колонка | Тип | Источник (событие → поле) | Query API DTO |
|---------|-----|--------------------------|--------------|
| context_id | uuid | BoundaryRefined → ContextId | BoundedContextDetailResponse.responsibilities[].contextId |
| ordinal | int | Порядковый номер в списке | — (порядок) |
| description | text | BoundaryRefined → Responsibility.Description | .responsibilities[].description |
| type | varchar(10) | BoundaryRefined → Responsibility.Type | .responsibilities[].type |

### Проекционные правила

| Доменное событие | Действие | Затронутые колонки |
|-----------------|---------|-------------------|
| BoundaryRefined | DELETE + INSERT (replace all) | DELETE WHERE context_id = @id; INSERT для каждой Responsibility |
| BoundedContextArchived | DELETE | WHERE context_id = @id |

### Индексы

- PK: `(context_id, ordinal)` — композитный
- IX_bounded_context_responsibilities_context_id: `context_id`

---

## 5. bounded_context_relationships

**Схема:** read_model
**Read Model:** `BoundedContextRelationshipReadModel : IReadModel<IBoundedContextCanvas>`
**Тип:** Append/Delete — связи добавляются и удаляются по одной.

### Колонки

| Колонка | Тип | Источник (событие → поле) | Query API DTO |
|---------|-----|--------------------------|--------------|
| context_id | uuid | RelationshipMapped → ContextId | RelationshipsResponse.contextId |
| target_context_id | uuid | RelationshipMapped → TargetContextId | .relationships[].targetContextId |
| target_context_name | varchar(100) | RelationshipMapped → TargetContextName | .relationships[].targetContextName |
| direction | varchar(20) | RelationshipMapped → Direction | .relationships[].direction |
| pattern | varchar(30) | RelationshipMapped → Pattern | .relationships[].pattern |

### Проекционные правила

| Доменное событие | Действие | Затронутые колонки |
|-----------------|---------|-------------------|
| RelationshipMapped | UPSERT | context_id, target_context_id, target_context_name, direction, pattern |
| RelationshipRemoved | DELETE | WHERE context_id = @id AND target_context_id = @targetId |
| BoundedContextArchived | DELETE | WHERE context_id = @id (все связи) |

### Индексы

- PK: `(context_id, target_context_id)` — композитный
- IX_bounded_context_relationships_context_id: `context_id`
- IX_bounded_context_relationships_target: `target_context_id`

---

## 6. bounded_context_interface

**Схема:** read_model
**Read Model:** `BoundedContextInterfaceReadModel : IReadModel<IBoundedContextCanvas>`
**Тип:** Replace-all — при публикации контракта полностью заменяется.

### Колонки

| Колонка | Тип | Источник (событие → поле) | Query API DTO |
|---------|-----|--------------------------|--------------|
| context_id | uuid | PublicContractPublished → ContextId | PublicInterfaceResponse.contextId |
| ordinal | int | Порядковый номер | — (порядок) |
| name | varchar(200) | PublicContractPublished → Item.Name | .items[].name |
| type | varchar(20) | PublicContractPublished → Item.Type | .items[].type |
| direction | varchar(10) | PublicContractPublished → Item.Direction | .items[].direction |
| linked_responsibility | text | PublicContractPublished → Item.LinkedResponsibility | .items[].linkedResponsibility |

### Проекционные правила

| Доменное событие | Действие | Затронутые колонки |
|-----------------|---------|-------------------|
| PublicContractPublished | DELETE + INSERT (replace all) | DELETE WHERE context_id = @id; INSERT для каждого Item |
| BoundedContextArchived | DELETE | WHERE context_id = @id |

### Индексы

- PK: `(context_id, ordinal)` — композитный
- IX_bounded_context_interface_context_id: `context_id`
- IX_bounded_context_interface_type_direction: `(type, direction)` — для фильтрации по Inbound/Outbound

---

## 7. context_relationships

**Схема:** read_model
**Read Model:** `ContextRelationshipReadModel : IReadModel<IBoundedContextCanvas>`
**Тип:** Append/Delete — глобальный граф всех связей для Context Map.

### Колонки

| Колонка | Тип | Источник (событие → поле) | Query API DTO |
|---------|-----|--------------------------|--------------|
| source_context_id | uuid | RelationshipMapped → ContextId | ContextMapGraphResponse.edges[].sourceContextId |
| source_context_name | varchar(100) | (lookup из bounded_context_details) | .nodes[].technicalName |
| source_domain_type | varchar(20) | (lookup из bounded_context_details) | .nodes[].domainType |
| target_context_id | uuid | RelationshipMapped → TargetContextId | .edges[].targetContextId |
| target_context_name | varchar(100) | RelationshipMapped → TargetContextName | — |
| direction | varchar(20) | RelationshipMapped → Direction | .edges[].direction |
| pattern | varchar(30) | RelationshipMapped → Pattern | .edges[].pattern |

### Проекционные правила

| Доменное событие | Действие | Затронутые колонки |
|-----------------|---------|-------------------|
| RelationshipMapped | UPSERT | Все колонки. source_context_name и source_domain_type берутся из bounded_context_details |
| RelationshipRemoved | DELETE | WHERE source_context_id = @id AND target_context_id = @targetId |
| StrategicTypeChanged | UPDATE | source_domain_type WHERE source_context_id = @id |
| BoundedContextArchived | DELETE | WHERE source_context_id = @id |

### Индексы

- PK: `(source_context_id, target_context_id)` — композитный
- IX_context_relationships_target: `target_context_id`

---

## 8. context_stats

**Схема:** read_model
**Read Model:** `ContextStatsReadModel : IReadModel<IBoundedContextCanvas>`
**Тип:** Агрегированная — singleton-запись с глобальными счётчиками.

### Колонки

| Колонка | Тип | Источник (событие → поле) | Query API DTO |
|---------|-----|--------------------------|--------------|
| id | varchar(10) PK | 'singleton' (фиксированный) | — |
| total_contexts | int | BoundedContextCreated → +1, Archived → -1 | ContextStatsResponse.totalContexts |
| core_count | int | StrategicImportanceDefined/StrategicTypeChanged → ±1 | .byDomainType.core |
| supporting_count | int | StrategicImportanceDefined/StrategicTypeChanged → ±1 | .byDomainType.supporting |
| generic_count | int | StrategicImportanceDefined/StrategicTypeChanged → ±1 | .byDomainType.generic |
| draft_count | int | BoundedContextCreated → +1, StrategicImportanceDefined → -1 | .byStatus.draft |
| defined_count | int | StrategicImportanceDefined → +1, PublicContractPublished → -1 | .byStatus.defined |
| published_count | int | PublicContractPublished → +1, Archived → -1 | .byStatus.published |
| archived_count | int | BoundedContextArchived → +1 | .byStatus.archived |
| total_responsibilities | int | BoundaryRefined → delta (new_count - old_count) | (для avg) |
| total_relationships | int | RelationshipMapped → +1, RelationshipRemoved → -1 | .totalRelationships |

### Проекционные правила

| Доменное событие | Действие | Затронутые колонки |
|-----------------|---------|-------------------|
| BoundedContextCreated | UPSERT | total_contexts++, draft_count++ |
| StrategicImportanceDefined | UPSERT | draft_count-- (если был Draft), defined_count++ (если стал Defined), {domainType}_count++ |
| StrategicTypeChanged | UPSERT | {old_domainType}_count--, {new_domainType}_count++ |
| BoundaryRefined | UPSERT | total_responsibilities += delta |
| RelationshipMapped | UPSERT | total_relationships++ |
| RelationshipRemoved | UPSERT | total_relationships-- |
| PublicContractPublished | UPSERT | defined_count-- (если был Defined), published_count++ |
| BoundedContextArchived | UPSERT | total_contexts--, published_count--, archived_count++, {domainType}_count-- |

### Индексы

- PK: `id` ('singleton')

---

## Корреляция Command API → MV → Query API

| Команда (артефакт 3) | Событие (артефакт 2) | MV действие | Query API (артефакт 4) |
|----------------------|---------------------|------------|----------------------|
| POST /bounded-context | BoundedContextCreated | details: INSERT, list: INSERT, stats: UPSERT | GET /{id}, GET /?filter |
| POST /{id}/strategic-importance | StrategicImportanceDefined | details: UPDATE, list: UPDATE, stats: UPSERT | GET /{id}, GET /?filter, GET /stats |
| POST /{id}/boundary | BoundaryRefined | details: UPDATE, responsibilities: REPLACE, list: UPDATE, stats: UPSERT | GET /{id}, GET /{id}/boundary, GET /stats |
| POST /{id}/language/terms | TermAddedToLanguage | details: UPDATE, language: INSERT | GET /{id}, GET /{id}/language |
| DELETE /{id}/language/terms/{t} | TermRemovedFromLanguage | details: UPDATE, language: DELETE | GET /{id}, GET /{id}/language |
| POST /{id}/relationships | RelationshipMapped | details: UPDATE, relationships: UPSERT, list: UPDATE, context_relationships: UPSERT, stats: UPSERT | GET /{id}, GET /{id}/relationships, GET /relationships/graph, GET /stats |
| DELETE /{id}/relationships/{t} | RelationshipRemoved | details: UPDATE, relationships: DELETE, list: UPDATE, context_relationships: DELETE, stats: UPSERT | GET /{id}, GET /{id}/relationships, GET /relationships/graph, GET /stats |
| POST /{id}/contract | PublicContractPublished | details: UPDATE, interface: REPLACE, list: UPDATE, stats: UPSERT | GET /{id}, GET /{id}/interface, GET /stats |
| POST /{id}/business-decisions | BusinessDecisionAdded | details: UPDATE (jsonb append) | GET /{id} |
| POST /{id}/assumptions | AssumptionAdded | details: UPDATE (jsonb append) | GET /{id} |
| POST /{id}/archive | BoundedContextArchived | details: UPDATE, list: UPDATE, language: DELETE, responsibilities: DELETE, relationships: DELETE, interface: DELETE, context_relationships: DELETE, stats: UPSERT | GET /?filter, GET /stats |

## Правила идемпотентности

Каждое проекционное правило использует `Version` из доменного события как idempotency key:
- `bounded_context_details`: `WHERE id = @id AND version < @newVersion`
- Коллекционные таблицы (language, responsibilities, relationships, interface): `context_id + event version` для проверки дубликатов
- `context_stats`: атомарные инкременты/декременты с идемпотентностью через журнал обработанных событий
