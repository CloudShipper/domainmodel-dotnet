namespace CloudShipper.DomainModel.Infrastructure;

public interface ITransactionable
{
    Task<ITransaction> BeginTransactionAsync();
}
