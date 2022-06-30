using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudShipper.DomainModel.Test.Domain
{
    [Aggregate(Constants.DomainObjectBTypeId)]
    internal class DomainObjectB : AggregateRoot<Guid>
    {
        public DomainObjectB(Guid id) : base(id)
        {
        }
    }
}
