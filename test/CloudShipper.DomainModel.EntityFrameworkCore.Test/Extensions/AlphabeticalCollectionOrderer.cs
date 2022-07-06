using Xunit.Abstractions;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Test.Extensions;

public class AlphabeticalCollectionOrderer : ITestCollectionOrderer
{
    public IEnumerable<ITestCollection> OrderTestCollections(IEnumerable<ITestCollection> testCollections)
     => testCollections.OrderBy(c => c.DisplayName);
}
