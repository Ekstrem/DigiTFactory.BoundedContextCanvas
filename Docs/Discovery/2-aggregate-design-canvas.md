# Aggregate Design Canvas: BoundedContext

## 1. Name (Имя)

- **Имя:** BoundedContext
- **Описание:** Корень агрегата, представляющий модель «Ограниченного контекста» на основе Bounded Context Canvas. Координирует стратегическую классификацию, внутренние обязанности и внешние контракты микросервиса.
- **Создаётся когда:** архитектор инициирует проектирование нового микросервиса (CreateBoundedContext)
- **Завершается когда:** канвас архивируется при выводе контекста из эксплуатации (ArchiveBoundedContext)

## 2. Description (Описание)

### Value Objects

- **Root** (`BoundedContextRoot`):
  - `ContextId` (Guid) — уникальный идентификатор канваса
  - `TechnicalName` (string) — имя namespace, PascalCase, `^[A-Z][a-zA-Z0-9]*$`
  - `OwnerTeam` (string) — команда-владелец контекста
  - `Status` (enum) — Draft | Defined | Published | Archived

- **StrategicClassification** (`StrategicClassificationVO`):
  - `DomainType` (enum) — Core | Supporting | Generic
  - `BusinessModelRole` (enum) — RevenueGenerator | EngagementCreator | ComplianceEnforcer
  - `Evolution` (enum) — Genesis | CustomBuilt | Product | Commodity

- **Definition** (`DefinitionVO`):
  - `BusinessPurpose` (string) — зачем контекст существует (1–2 предложения)

- **DomainRole** (`DomainRoleVO`):
  - `RoleType` (enum) — Execution | Gateway | Analysis
  - `Description` (string) — пояснение роли

- **UbiquitousLanguageTerm** (`UbiquitousLanguageTermVO`, Entity Collection):
  - `Term` (string) — ключевой термин
  - `Definition` (string) — определение термина в контексте

- **Responsibility** (`ResponsibilityVO`, List):
  - `Description` (string) — описание обязанности
  - `Type` (enum) — Does | DoesNot (что делает / что явно НЕ делает)

- **ExternalConnection** (`ExternalConnectionVO`, List):
  - `TargetContextId` (Guid) — связанный контекст
  - `TargetContextName` (string) — имя связанного контекста
  - `Direction` (enum) — Upstream | Downstream | Mutual
  - `Pattern` (enum) — SharedKernel | Partners | CustomerSupplier | Conformist | ACL | OHS | PublishedLanguage | SeparateWays

- **PublicInterfaceItem** (`PublicInterfaceItemVO`, List):
  - `Name` (string) — имя команды/события/запроса
  - `Type` (enum) — Command | Query | DomainEvent
  - `Direction` (enum) — Inbound | Outbound
  - `LinkedResponsibility` (string) — ответственность, обосновывающая этот элемент

- **BusinessDecision** (`BusinessDecisionVO`, List):
  - `Rule` (string) — описание бизнес-правила
  - `Rationale` (string) — обоснование

- **Assumption** (`AssumptionVO`, List):
  - `Statement` (string) — утверждение
  - `Risk` (enum) — Low | Medium | High

## 3. State Transitions (Переходы состояний)

```
Draft → [DefineStrategicImportance] → Defined
Defined → [RefineBoundary] → Defined  (уточнение границ)
Defined → [AddTermToLanguage] → Defined  (обогащение глоссария)
Defined → [MapRelationship] → Defined  (установка связей)
Defined → [AddBusinessDecision] → Defined  (добавление правил)
Defined → [AddAssumption] → Defined  (фиксация предположений)
Defined → [PublishContract] → Published  (публикация контракта)
Published → [RefineBoundary] → Published  (эволюция → BoundaryShifted)
Published → [PublishContract] → Published  (эволюция контракта → PublicContractEvolved)
Published → [DefineStrategicImportance] → Published  (переклассификация → StrategicTypeChanged)
Published → [ArchiveBoundedContext] → Archived
```

## 4. Enforced Invariants (Обязательные инварианты)

1. **Strategic Alignment:** Core Domain не может иметь паттерн `Conformist` по отношению к Generic-контекстам. При попытке — отклонение с рекомендацией ACL.

2. **Responsibility Cohesion:** Описание каждой ответственности уникально в рамках агрегата (case-insensitive). Защита от дублей сигнализирует о нечётких границах.

3. **Interface Integrity:** Каждый элемент PublicInterface с Direction=Outbound и Type=DomainEvent обязан ссылаться на хотя бы одну Responsibility. Нельзя публиковать «бесхозное» событие.

4. **Naming Consistency:** `TechnicalName` соответствует `^[A-Z][a-zA-Z0-9]*$`. Проверяется при создании и недоступно для изменения после публикации.

5. **Status Guard:** Команды `AddTermToLanguage`, `MapRelationship`, `RefineBoundary`, `AddBusinessDecision`, `AddAssumption` доступны только в статусах Defined и Published. В Draft доступна только `DefineStrategicImportance`. В Archived — ничего.

## 5. Corrective Policies (Компенсирующие действия)

1. **OrganizationPolicy:** При изменении `OwnerTeam` — отправить событие в IAM-сервис через ACL для обновления прав доступа.

2. **GlobalContextMapPolicy:** При вызове `MapRelationship` — обновить глобальный граф зависимостей компании в ContextMap-сервисе (через событие `RelationshipMapped`).

3. **ContractCompatibilitySaga:** Если `PublicContractEvolved` содержит breaking changes — запустить процесс уведомления всех Downstream-потребителей. Ожидать подтверждение совместимости от каждого. Если хотя бы один не подтвердил в течение SLA — эскалация.

## 6. Handled Commands (Обрабатываемые команды)

| # | Команда | Входные VO | Описание | Допустимые статусы |
|---|---------|-----------|----------|-------------------|
| 1 | CreateBoundedContext | Root (TechnicalName, OwnerTeam), Definition | Создать канвас в статусе Draft | — (создание) |
| 2 | DefineStrategicImportance | StrategicClassification, DomainRole | Задать стратегическую классификацию и роль | Draft, Defined, Published |
| 3 | RefineBoundary | List\<Responsibility\> | Определить/обновить ответственности контекста | Defined, Published |
| 4 | AddTermToLanguage | UbiquitousLanguageTerm | Добавить термин в глоссарий | Defined, Published |
| 5 | RemoveTermFromLanguage | Term (string) | Удалить термин из глоссария | Defined, Published |
| 6 | MapRelationship | ExternalConnection | Установить связь с другим контекстом на карте | Defined, Published |
| 7 | RemoveRelationship | TargetContextId | Удалить связь с контекстом | Defined, Published |
| 8 | PublishContract | List\<PublicInterfaceItem\> | Опубликовать входящие/исходящие контракты | Defined, Published |
| 9 | AddBusinessDecision | BusinessDecision | Добавить бизнес-правило | Defined, Published |
| 10 | AddAssumption | Assumption | Зафиксировать предположение | Defined, Published |
| 11 | ArchiveBoundedContext | — | Архивировать канвас | Published |

## 7. Created Events (Порождаемые события)

| # | Событие | Изменённые VO | Когда порождается | Потребители |
|---|---------|--------------|-------------------|------------|
| 1 | BoundedContextCreated | Root, Definition | После CreateBoundedContext | ContextMap |
| 2 | StrategicImportanceDefined | Root, StrategicClassification, DomainRole | После DefineStrategicImportance | ContextMap |
| 3 | BoundaryRefined | Root, Responsibilities | После RefineBoundary | ContextMap |
| 4 | TermAddedToLanguage | Root, UbiquitousLanguageTerm | После AddTermToLanguage | — |
| 5 | TermRemovedFromLanguage | Root | После RemoveTermFromLanguage | — |
| 6 | RelationshipMapped | Root, ExternalConnection | После MapRelationship | ContextMap |
| 7 | RelationshipRemoved | Root | После RemoveRelationship | ContextMap |
| 8 | PublicContractPublished | Root, PublicInterface | После PublishContract | CodeGenerator, ContextMap |
| 9 | BusinessDecisionAdded | Root, BusinessDecision | После AddBusinessDecision | — |
| 10 | AssumptionAdded | Root, Assumption | После AddAssumption | — |
| 11 | StrategicTypeChanged | Root, StrategicClassification | После DefineStrategicImportance (при смене DomainType) | All Downstream |
| 12 | BoundedContextArchived | Root | После ArchiveBoundedContext | ContextMap, CodeGenerator |

## 8. Throughput (Пропускная способность)

- **Ожидаемая частота команд:** < 1 tps (архитектурный инструмент, используется десятками людей)
- **Конкурентность:** низкая. Один канвас обычно редактируется одним архитектором. Optimistic concurrency через Version достаточно.
- **Один пользователь — один агрегат?** Нет. Один архитектор может работать с несколькими канвасами, но одновременно один канвас редактирует один человек.
- **Пиковая нагрузка:** Workshop-сессии — до 10 команд в минуту от одного пользователя.

## 9. Size (Размер)

- **Ожидаемое количество событий за жизнь:** 20–100 (создание + многократное уточнение границ, языка, связей)
- **Время жизни агрегата:** месяцы–годы (пока контекст существует в организации)
- **Необходимость снапшотов:** Нет (< 100 событий — Event Sourcing без снапшотов)
- **Рекомендация:** FullEventSourcing, профиль Single или BudgetCqrs (низкая нагрузка)
