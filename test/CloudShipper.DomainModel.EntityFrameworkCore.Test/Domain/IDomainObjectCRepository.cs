using CloudShipper.DomainModel.Repository;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Test.Domain;

internal interface IDomainObjectCRepository : IAggregateRootRepository<DomainObjectC, Guid>
{
}
