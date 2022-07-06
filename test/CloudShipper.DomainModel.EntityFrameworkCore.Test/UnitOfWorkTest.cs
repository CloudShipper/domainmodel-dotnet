using CloudShipper.DomainModel.Aggregate;
using CloudShipper.DomainModel.EntityFrameworkCore.Infrastructure;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Domain;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Domain.Events.DomainObjectA;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Extensions;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Infrastructure;
using CloudShipper.DomainModel.Events;
using Moq;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Test;

[Collection("C_003")]
[TestCaseOrderer(AlphabeticalTestCaseOrderer.TypeName, AlphabeticalTestCaseOrderer.AssemblyName)]
public class UnitOfWorkTest
{
    [Fact]
    public void Test_001_RaiseEvents()
    {
        AggregateTypeIdProvider.ReadAllTypes(new[] { typeof(DomainObjectA) });
        DomainEventTypeIdProvider.ReadAllTypes(new[] { typeof(DomainObjectA) });

        // Setup
        var context = new TestDbContext();
        var dispatcher = new Mock<IDomainEventDispatcher>();
        dispatcher.Setup(x => x.Publish(It.IsAny<CreatedEvent>())).Returns(Task.CompletedTask);
        dispatcher.Setup(x => x.Publish(It.IsAny<Value1ChangedEvent>())).Returns(Task.CompletedTask);
        var unitOfWork = new UnitOfWork<TestDbContext>(context, dispatcher.Object);
        var dbSet = unitOfWork.Context.Set<DomainObjectA>();

        var aggregate = new DomainObjectA(Guid.NewGuid());
        aggregate.SetValue1(20);
        dbSet.Add(aggregate);

        unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();

        dispatcher.Verify(x => x.Publish(It.IsAny<IDomainEvent>()), Times.Exactly(2));
        dispatcher.Verify(x => x.Publish(It.IsAny<CreatedEvent>()), Times.Once());
        dispatcher.Verify(x => x.Publish(It.IsAny<Value1ChangedEvent>()), Times.Once());
    }

    [Fact]
    public void Test_002_RaiseEventsWithTransaction()
    {
        AggregateTypeIdProvider.ReadAllTypes(new[] { typeof(DomainObjectA) });
        DomainEventTypeIdProvider.ReadAllTypes(new[] { typeof(DomainObjectA) });

        // Setup
        var context = new TestDbContext();
        var dispatcher = new Mock<IDomainEventDispatcher>();
        dispatcher.Setup(x => x.Publish(It.IsAny<CreatedEvent>())).Returns(Task.CompletedTask);
        dispatcher.Setup(x => x.Publish(It.IsAny<Value1ChangedEvent>())).Returns(Task.CompletedTask);
        var unitOfWork = new UnitOfWork<TestDbContext>(context, dispatcher.Object);
        var dbSet = unitOfWork.Context.Set<DomainObjectA>();

        var transaction = unitOfWork.BeginTransactionAsync().Result;
        Assert.NotNull(transaction);

        var aggregate = new DomainObjectA(Guid.NewGuid());
        aggregate.SetValue1(20);
        dbSet.Add(aggregate);
        unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();

        transaction.CommitAsync().GetAwaiter().GetResult();

        dispatcher.Verify(x => x.Publish(It.IsAny<IDomainEvent>()), Times.Exactly(2));
        dispatcher.Verify(x => x.Publish(It.IsAny<CreatedEvent>()), Times.Once());
        dispatcher.Verify(x => x.Publish(It.IsAny<Value1ChangedEvent>()), Times.Once());
    }
}
