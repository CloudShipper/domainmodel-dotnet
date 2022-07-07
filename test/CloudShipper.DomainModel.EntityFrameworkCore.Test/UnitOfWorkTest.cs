using CloudShipper.DomainModel.Aggregate;
using CloudShipper.DomainModel.EntityFrameworkCore.Infrastructure;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Domain;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Domain.Events.DomainObjectA;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Extensions;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Infrastructure;
using CloudShipper.DomainModel.Events;
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

    [Fact]
    public void Test_003_TestNestedTransactions()
    {
        var context = new Mock<TestDbContext>();
        var tx = new Mock<IDbContextTransaction>();
        var dbFacade = new Mock<DatabaseFacade>(context.Object);
        var dispatcher = new Mock<IDomainEventDispatcher>();
        var unitOfWork = new UnitOfWork<TestDbContext>(context.Object, dispatcher.Object);

        context.Setup(x => x.Database).Returns(dbFacade.Object);
        dbFacade.Setup(x => x.BeginTransactionAsync(default)).Returns(Task.FromResult(tx.Object));

        tx.Setup(x => x.CommitAsync(default)).Returns(Task.CompletedTask);

        var innerTx = unitOfWork.BeginTransactionAsync().Result;
        var outerTx = unitOfWork.BeginTransactionAsync().Result;

        context.Verify(x => x.Database.BeginTransactionAsync(default), Times.Once);

        outerTx.CommitAsync().GetAwaiter().GetResult();
        tx.Verify(x => x.CommitAsync(default), Times.Never);

        innerTx.CommitAsync().GetAwaiter().GetResult();
        tx.Verify(x => x.CommitAsync(default), Times.Once);
    }

    [Fact]
    public void Test_004_TestCommitNullTransaction()
    {
        var context = new Mock<TestDbContext>();
        var dispatcher = new Mock<IDomainEventDispatcher>();
        var unitOfWork = new UnitOfWork<TestDbContext>(context.Object, dispatcher.Object);

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.ThrowsAsync<ArgumentNullException>(() => ((ITransactionHandler)unitOfWork).CommitTransactionAsync(null, default));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }

    [Fact]
    public void Test_005_WrongTransactionContext()
    {
        var context = new Mock<TestDbContext>();
        var anotherContext = new Mock<TestDbContext>();
        var tx = new Mock<IDbContextTransaction>();
        var anotherTx = new Mock<IDbContextTransaction>();
        var dbFacade = new Mock<DatabaseFacade>(context.Object);
        var anotherDbFacade = new Mock<DatabaseFacade>(anotherContext.Object);
        var dispatcher = new Mock<IDomainEventDispatcher>();
        var unitOfWork = new UnitOfWork<TestDbContext>(context.Object, dispatcher.Object);
        var anotherUnitOfWork = new UnitOfWork<TestDbContext>(anotherContext.Object, dispatcher.Object);

        context.Setup(x => x.Database).Returns(dbFacade.Object);
        anotherContext.Setup(x => x.Database).Returns(anotherDbFacade.Object);
        dbFacade.Setup(x => x.BeginTransactionAsync(default)).Returns(Task.FromResult(tx.Object));
        anotherDbFacade.Setup(x => x.BeginTransactionAsync(default)).Returns(Task.FromResult(anotherTx.Object));

        tx.Setup(x => x.CommitAsync(default)).Returns(Task.CompletedTask);
        anotherTx.Setup(x => x.CommitAsync(default)).Returns(Task.CompletedTask);

        var contextTx = unitOfWork.BeginTransactionAsync(default).Result;
        var anotherContextTx = anotherUnitOfWork.BeginTransactionAsync(default).Result;

        Assert.ThrowsAsync<InvalidOperationException>(() => ((ITransactionHandler)unitOfWork).CommitTransactionAsync((Transaction)anotherContextTx, default));
    }

    [Fact]
    public void Test_006_CommitTransactionThrowsException()
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
        context.Verify(x => x.SaveChangesAsync(default), Times.Once);

        // now test a commit again
        Assert.ThrowsAsync<InvalidOperationException>(() => contextTx.CommitAsync(default));

        tx.Verify(x => x.Rollback(), Times.Once);
        context.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public void Test_007_RollbackNullTransaction()
    {
        var context = new Mock<TestDbContext>();
        var dispatcher = new Mock<IDomainEventDispatcher>();
        var unitOfWork = new UnitOfWork<TestDbContext>(context.Object, dispatcher.Object);

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<ArgumentNullException>(() => ((ITransactionHandler)unitOfWork).RollbackTransaction(null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }

    [Fact]
    public void Test_008_RollbackTransactionThrowsException()
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
    public void Test_009_RollbackNestedTransaction()
    {
        var context = new Mock<TestDbContext>();
        var tx = new Mock<IDbContextTransaction>();
        var dbFacade = new Mock<DatabaseFacade>(context.Object);
        var dispatcher = new Mock<IDomainEventDispatcher>();
        var unitOfWork = new UnitOfWork<TestDbContext>(context.Object, dispatcher.Object);

        context.Setup(x => x.Database).Returns(dbFacade.Object);
        dbFacade.Setup(x => x.BeginTransactionAsync(default)).Returns(Task.FromResult(tx.Object));

        tx.Setup(x => x.Rollback());

        var innerTx = unitOfWork.BeginTransactionAsync().Result;
        var outerTx = unitOfWork.BeginTransactionAsync().Result;

        context.Verify(x => x.Database.BeginTransactionAsync(default), Times.Once);

        outerTx.Rollback();
        tx.Verify(x => x.Rollback(), Times.Never);

        innerTx.Rollback();
        tx.Verify(x => x.Rollback(), Times.Once);
    }

    [Fact]
    public void Test_010_RollbackWithWrongContext()
    {
        var context = new Mock<TestDbContext>();
        var anotherContext = new Mock<TestDbContext>();
        var tx = new Mock<IDbContextTransaction>();
        var anotherTx = new Mock<IDbContextTransaction>();
        var dbFacade = new Mock<DatabaseFacade>(context.Object);
        var anotherDbFacade = new Mock<DatabaseFacade>(anotherContext.Object);
        var dispatcher = new Mock<IDomainEventDispatcher>();
        var unitOfWork = new UnitOfWork<TestDbContext>(context.Object, dispatcher.Object);
        var anotherUnitOfWork = new UnitOfWork<TestDbContext>(anotherContext.Object, dispatcher.Object);

        context.Setup(x => x.Database).Returns(dbFacade.Object);
        anotherContext.Setup(x => x.Database).Returns(anotherDbFacade.Object);
        dbFacade.Setup(x => x.BeginTransactionAsync(default)).Returns(Task.FromResult(tx.Object));
        anotherDbFacade.Setup(x => x.BeginTransactionAsync(default)).Returns(Task.FromResult(anotherTx.Object));

        tx.Setup(x => x.Rollback());
        anotherTx.Setup(x => x.Rollback());

        var contextTx = unitOfWork.BeginTransactionAsync(default).Result;
        var anotherContextTx = anotherUnitOfWork.BeginTransactionAsync(default).Result;

        Assert.Throws<InvalidOperationException>(() => ((ITransactionHandler)unitOfWork).RollbackTransaction((Transaction)anotherContextTx));        
    }
}
