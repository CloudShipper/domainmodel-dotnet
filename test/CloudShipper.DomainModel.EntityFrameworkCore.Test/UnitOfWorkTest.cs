using CloudShipper.DomainModel.Aggregate;
using CloudShipper.DomainModel.EntityFrameworkCore.Infrastructure;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Domain;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Domain.Events.DomainObjectA;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Extensions;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Infrastructure;
using CloudShipper.DomainModel.Events;
using CloudShipper.DomainModel.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
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
        dispatcher.Setup(x => x.Publish(It.IsAny<CreatedEvent>(), default)).Returns(Task.CompletedTask);
        dispatcher.Setup(x => x.Publish(It.IsAny<Value1ChangedEvent>(), default)).Returns(Task.CompletedTask);
        var unitOfWork = new UnitOfWork<TestDbContext>(context, dispatcher.Object);
        var dbSet = unitOfWork.Context.Set<DomainObjectA>();

        context.Database.OpenConnection();
        var x = context.Database.EnsureCreated();

        var aggregate = new DomainObjectA(Guid.NewGuid());
        aggregate.SetValue1(20);
        dbSet.Add(aggregate);

        unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();

        dispatcher.Verify(x => x.Publish(It.IsAny<IDomainEvent>(), default), Times.Exactly(2));
        dispatcher.Verify(x => x.Publish(It.IsAny<CreatedEvent>(), default), Times.Once());
        dispatcher.Verify(x => x.Publish(It.IsAny<Value1ChangedEvent>(), default), Times.Once());

        context.Database.CloseConnection();
    }

    [Fact]
    public void Test_002_RaiseEventsWithTransaction()
    {
        AggregateTypeIdProvider.ReadAllTypes(new[] { typeof(DomainObjectA) });
        DomainEventTypeIdProvider.ReadAllTypes(new[] { typeof(DomainObjectA) });

        // Setup
        var context = new TestDbContext();
        var dispatcher = new Mock<IDomainEventDispatcher>();
        dispatcher.Setup(x => x.Publish(It.IsAny<CreatedEvent>(), default)).Returns(Task.CompletedTask);
        dispatcher.Setup(x => x.Publish(It.IsAny<Value1ChangedEvent>(), default)).Returns(Task.CompletedTask);
        var unitOfWork = new UnitOfWork<TestDbContext>(context, dispatcher.Object);

        context.Database.OpenConnection();
        var x = context.Database.EnsureCreated();

        var dbSet = unitOfWork.Context.Set<DomainObjectA>();

        var transaction = unitOfWork.BeginTransactionAsync().Result;
        Assert.NotNull(transaction);
        Assert.True(unitOfWork.HasActiveTransaction());

        var aggregate = new DomainObjectA(Guid.NewGuid());
        aggregate.SetValue1(20);
        dbSet.Add(aggregate);
        unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();

        transaction.CommitAsync().GetAwaiter().GetResult();

        dispatcher.Verify(x => x.Publish(It.IsAny<IDomainEvent>(), default), Times.Exactly(2));
        dispatcher.Verify(x => x.Publish(It.IsAny<CreatedEvent>(), default), Times.Once());
        dispatcher.Verify(x => x.Publish(It.IsAny<Value1ChangedEvent>(), default), Times.Once());

        context.Database.CloseConnection();
    }

    [Fact]
    public void Test_003_CommitTransactionThrowsException()
    {
        var context = new Mock<TestDbContext>();
        var tx = new Mock<IDbContextTransaction>();
        var dbFacade = new Mock<DatabaseFacade>(context.Object);
        var dispatcher = new Mock<IDomainEventDispatcher>();
        var unitOfWork = new UnitOfWork<TestDbContext>(context.Object, dispatcher.Object);

        context.Setup(x => x.Database).Returns(dbFacade.Object);
        context.Setup(x => x.SaveChangesAsync(default)).Returns(Task.FromResult(0));
        dbFacade.Setup(x => x.BeginTransactionAsync(default)).Returns(Task.FromResult(tx.Object));

        tx.Setup(x => x.CommitAsync(default)).Throws<Exception>();
        tx.Setup(x => x.Rollback());

        var contextTx = unitOfWork.BeginTransactionAsync(default).Result;

        Assert.ThrowsAsync<Exception>(() => contextTx.CommitAsync(default));

        tx.Verify(x => x.Rollback(), Times.Once);
        
        // now test a commit again
        Assert.ThrowsAsync<InvalidOperationException>(() => contextTx.CommitAsync(default));

        tx.Verify(x => x.Rollback(), Times.Once);
    }

    [Fact]
    public void Test_004_RollbackTransactionThrowsException()
    {
        var context = new Mock<TestDbContext>();
        var tx = new Mock<IDbContextTransaction>();
        var dbFacade = new Mock<DatabaseFacade>(context.Object);
        var dispatcher = new Mock<IDomainEventDispatcher>();
        var unitOfWork = new UnitOfWork<TestDbContext>(context.Object, dispatcher.Object);

        context.Setup(x => x.Database).Returns(dbFacade.Object);
        context.Setup(x => x.SaveChangesAsync(default)).Returns(Task.FromResult(0));
        dbFacade.Setup(x => x.BeginTransactionAsync(default)).Returns(Task.FromResult(tx.Object));

        tx.Setup(x => x.CommitAsync(default)).Throws<Exception>();
        tx.Setup(x => x.Rollback()).Throws<Exception>();

        var contextTx = unitOfWork.BeginTransactionAsync(default).Result;

        Assert.Throws<Exception>(() => contextTx.Rollback());

        tx.Verify(x => x.Rollback(), Times.Once);
        context.Verify(x => x.SaveChangesAsync(default), Times.Never);

        // now try to Rollback agin, we expect that nothing happens
        Assert.Throws<InvalidOperationException>(() => contextTx.Rollback());

        tx.Verify(x => x.Rollback(), Times.Once);
        context.Verify(x => x.SaveChangesAsync(default), Times.Never);
    }

    [Fact]
    public void Test_004_UseWrongITransactionTypeImpl()
    {
        var context = new Mock<TestDbContext>();
        var dispatcher = new Mock<IDomainEventDispatcher>();
        var tx = new Mock<ITransaction>();
        var unitOfWork = new UnitOfWork<TestDbContext>(context.Object, dispatcher.Object);

        Assert.Throws<InvalidOperationException>(() => unitOfWork.UseTransaction(tx.Object));
    }
}
