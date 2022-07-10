using CloudShipper.DomainModel.EntityFrameworkCore.Infrastructure;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Domain;
using CloudShipper.DomainModel.Infrastructure;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Test.Infrastructure;

internal class DomainObjectCRepository : AggregateRootRepository<TestDbContext, DomainObjectC, Guid>, IDomainObjectCRepository
{
    public DomainObjectCRepository(IUnitOfWork<TestDbContext> unitOfWork) : base(unitOfWork)
    {
    }
}
