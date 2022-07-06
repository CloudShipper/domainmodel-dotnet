using CloudShipper.DomainModel.EntityFrameworkCore.Infrastructure;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Extensions;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Test;

[Collection("C_001")]
[TestCaseOrderer(AlphabeticalTestCaseOrderer.TypeName, AlphabeticalTestCaseOrderer.AssemblyName)]
public class TransactionTest
{
    [Fact]
    public void Test_001_Commit()
    {       
        var dbTransaction = new Mock<IDbContextTransaction>();
        var handler = new Mock<ITransactionHandler>();
        var transaction = new Transaction(dbTransaction.Object, handler.Object);

        handler.Setup(x => x.CommitTransactionAsync(transaction, default)).Returns(Task.CompletedTask);
        handler.Setup(x => x.RollbackTransaction(transaction));

        transaction.CommitAsync().GetAwaiter().GetResult();
        handler.Verify(x => x.CommitTransactionAsync(transaction, default), Times.Once());

        transaction.Dispose();
        handler.Verify(x => x.RollbackTransaction(transaction), Times.Never);
    }

    [Fact]
    public void Test_002_CommitThatThrowsException()
    {
        var dbTransaction = new Mock<IDbContextTransaction>();
        var handler = new Mock<ITransactionHandler>();
        var transaction = new Transaction(dbTransaction.Object, handler.Object);

        handler.Setup(x => x.CommitTransactionAsync(transaction, default)).Throws<Exception>();
        handler.Setup(x => x.RollbackTransaction(transaction));

        Assert.Throws<Exception>(() => transaction.CommitAsync().GetAwaiter().GetResult());
        handler.Verify(x => x.CommitTransactionAsync(transaction, default), Times.Once());

        transaction.Dispose();
        handler.Verify(x => x.RollbackTransaction(transaction), Times.Never);
    }

    [Fact]
    public void Test_003_Rollback()
    {
        var dbTransaction = new Mock<IDbContextTransaction>();
        var handler = new Mock<ITransactionHandler>();
        var transaction = new Transaction(dbTransaction.Object, handler.Object);
        handler.Setup(x => x.RollbackTransaction(transaction));

        transaction.Rollback();
        transaction.Dispose();

        handler.Verify(x => x.RollbackTransaction(transaction), Times.Once);
    }

    [Fact]
    public void Test_004_RollbackThatThrowsException()
    {
        var dbTransaction = new Mock<IDbContextTransaction>();
        var handler = new Mock<ITransactionHandler>();
        var transaction = new Transaction(dbTransaction.Object, handler.Object);
        handler.Setup(x => x.RollbackTransaction(transaction)).Throws<Exception>();

        Assert.Throws<Exception>(() => transaction.Rollback());
        transaction.Dispose();

        handler.Verify(x => x.RollbackTransaction(transaction), Times.Once);
    }

    [Fact]
    public void Test_005_Dispose()
    {
        var dbTransaction = new Mock<IDbContextTransaction>();
        var handler = new Mock<ITransactionHandler>();
        var transaction = new Transaction(dbTransaction.Object, handler.Object);
        handler.Setup(x => x.RollbackTransaction(transaction));

        transaction.Dispose();
        handler.Verify(x => x.RollbackTransaction(transaction), Times.Once);
    }
}
