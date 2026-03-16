using FluentAssertions;
using BoundedContextCanvas.Domain;
using BoundedContextCanvas.Domain.Enums;
using BoundedContextCanvas.Domain.Implementation;

namespace BoundedContextCanvas.Domain.Tests;

public class AggregateTests
{
    [Fact]
    public void CreateBoundedContext_WithValidData_ShouldSucceed()
    {
        // Arrange
        var model = TestDataBuilder.CreateDefaultModel();
        var aggregate = Aggregate.Create(model);

        // Act
        var result = aggregate.CreateBoundedContext(model);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void DefineStrategicImportance_FromDraft_ShouldTransitionToDefined()
    {
        // Arrange
        var model = TestDataBuilder.CreateDefaultModel();
        var aggregate = Aggregate.Create(model);
        aggregate.CreateBoundedContext(model);

        var classification = TestDataBuilder.CreateStrategicClassification();
        var role = TestDataBuilder.CreateDomainRole();
        var inputModel = BoundedContextCanvasAnemicModel.Create(
            model.Root,
            classification,
            domainRole: role);

        // Act
        var result = aggregate.DefineStrategicImportance(inputModel);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void RefineBoundary_WithResponsibilities_ShouldUpdateState()
    {
        // Arrange
        var definedModel = TestDataBuilder.CreateDefinedModel();
        var aggregate = Aggregate.Create(definedModel);

        var responsibilities = new[]
        {
            TestDataBuilder.CreateResponsibility("Manage canvas lifecycle"),
            TestDataBuilder.CreateResponsibility("Validate strategic alignment", ResponsibilityTypeEnum.Does),
            TestDataBuilder.CreateResponsibility("Generate code", ResponsibilityTypeEnum.DoesNot)
        };

        var inputModel = BoundedContextCanvasAnemicModel.Create(
            definedModel.Root,
            responsibilities: responsibilities);

        // Act
        var result = aggregate.RefineBoundary(inputModel);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void AddTermToLanguage_ShouldAppendTerm()
    {
        // Arrange
        var definedModel = TestDataBuilder.CreateDefinedModel();
        var aggregate = Aggregate.Create(definedModel);

        var newTerm = TestDataBuilder.CreateTerm("BoundedContext", "An autonomous business area");
        var inputModel = BoundedContextCanvasAnemicModel.Create(
            definedModel.Root,
            language: new[] { newTerm });

        // Act
        var result = aggregate.AddTermToLanguage(inputModel);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void MapRelationship_ShouldAddConnection()
    {
        // Arrange
        var definedModel = TestDataBuilder.CreateDefinedModel();
        var aggregate = Aggregate.Create(definedModel);

        var connection = TestDataBuilder.CreateConnection(
            targetName: "ContextMap",
            pattern: IntegrationPatternEnum.Partners);
        var inputModel = BoundedContextCanvasAnemicModel.Create(
            definedModel.Root,
            relationships: new[] { connection });

        // Act
        var result = aggregate.MapRelationship(inputModel);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void PublishContract_ShouldTransitionToPublished()
    {
        // Arrange
        var definedModel = TestDataBuilder.CreateDefinedModel();
        var aggregate = Aggregate.Create(definedModel);

        var items = new[]
        {
            TestDataBuilder.CreateInterfaceItem(
                "BoundedContextCreated",
                InterfaceItemTypeEnum.DomainEvent,
                InterfaceDirectionEnum.Outbound,
                "Handle business logic")
        };
        var inputModel = BoundedContextCanvasAnemicModel.Create(
            definedModel.Root,
            publicInterface: items);

        // Act
        var result = aggregate.PublishContract(inputModel);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void ArchiveBoundedContext_FromPublished_ShouldSucceed()
    {
        // Arrange
        var root = TestDataBuilder.CreateRoot(status: CanvasStatusEnum.Published);
        var model = BoundedContextCanvasAnemicModel.Create(root, definition: TestDataBuilder.CreateDefinition());
        var aggregate = Aggregate.Create(model);

        // Act
        var result = aggregate.ArchiveBoundedContext(model);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void FullLifecycle_Draft_Defined_Published_Archived()
    {
        // Arrange — Create
        var createModel = TestDataBuilder.CreateDefaultModel();
        var aggregate = Aggregate.Create(createModel);
        aggregate.CreateBoundedContext(createModel);

        // Act — Define
        var classificationModel = BoundedContextCanvasAnemicModel.Create(
            createModel.Root,
            TestDataBuilder.CreateStrategicClassification(),
            domainRole: TestDataBuilder.CreateDomainRole());
        aggregate.DefineStrategicImportance(classificationModel);

        // Act — Publish
        var publishModel = BoundedContextCanvasAnemicModel.Create(
            createModel.Root,
            publicInterface: new[]
            {
                TestDataBuilder.CreateInterfaceItem(
                    "BoundedContextCreated",
                    InterfaceItemTypeEnum.DomainEvent,
                    InterfaceDirectionEnum.Outbound,
                    "Handle business logic")
            });
        aggregate.PublishContract(publishModel);

        // Act — Archive
        var archiveResult = aggregate.ArchiveBoundedContext(createModel);

        // Assert
        archiveResult.Should().NotBeNull();
    }
}
