using CloudShipper.DomainModel.EntityFrameworkCore.Infrastructure;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Extensions;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Test;

[Collection("C_002")]
[TestCaseOrderer(AlphabeticalTestCaseOrderer.TypeName, AlphabeticalTestCaseOrderer.AssemblyName)]
public class TransactionTest
{
    [Fact]
    public void Test_001_Commit()
    {       
        var dbTransaction = new Mock<IDbContextTransaction>();
        var transaction = new Transaction(dbTransaction.Object);

        dbTransaction.Setup(x => x.CommitAsync(default)).Returns(Task.CompletedTask);
        dbTransaction.Setup(x => x.Rollback());

        transaction.CommitAsync().GetAwaiter().GetResult();
        dbTransaction.Verify(x => x.CommitAsync(default), Times.Once());

        transaction.Dispose();
        dbTransaction.Verify(x => x.Rollback(), Times.Never);
    }

    [Fact]
    public void Test_002_CommitThatThrowsException()
    {
        var dbTransaction = new Mock<IDbContextTransaction>();
        var transaction = new Transaction(dbTransaction.Object);

        dbTransaction.Setup(x => x.CommitAsync(default)).Throws<Exception>();
        dbTransaction.Setup(x => x.Rollback());

        Assert.Throws<Exception>(() => transaction.CommitAsync().GetAwaiter().GetResult());
        dbTransaction.Verify(x => x.CommitAsync(default), Times.Once());

        transaction.Dispose();
        dbTransaction.Verify(x => x.Rollback(), Times.Once);
    }

    [Fact]
    public void Test_003_Rollback()
    {
        var dbTransaction = new Mock<IDbContextTransaction>();
        var transaction = new Transaction(dbTransaction.Object);
        dbTransaction.Setup(x => x.Rollback());

        transaction.Rollback();
        transaction.Dispose();

        dbTransaction.Verify(x => x.Rollback(), Times.Once);
    }

    [Fact]
    public void Test_004_RollbackThatThrowsException()
    {
        var dbTransaction = new Mock<IDbContextTransaction>();
        var transaction = new Transaction(dbTransaction.Object);
        dbTransaction.Setup(x => x.Rollback()).Throws<Exception>();

        Assert.Throws<Exception>(() => transaction.Rollback());
        transaction.Dispose();

        dbTransaction.Verify(x => x.Rollback(), Times.Once);
    }

    [Fact]
    public void Test_005_Dispose()
    {
        var dbTransaction = new Mock<IDbContextTransaction>();
        var transaction = new Transaction(dbTransaction.Object);
        dbTransaction.Setup(x => x.Rollback());

        transaction.Dispose();
        dbTransaction.Verify(x => x.Rollback(), Times.Once);
    }
}
