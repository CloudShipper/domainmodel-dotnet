﻿using CloudShipper.DomainModel.Events;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Test.Domain.Events.AuditableDomainObjectA;

[DomainEvent("668501A4-A9AC-4B28-86BF-8F6DDD51429B")]
internal class Value1ChangedEvent : AuditableDomainEvent<Guid, Guid>
{
    public Value1ChangedEvent(
        Guid aggregateId, Type aggregateType, Guid principalId) 
        : base(aggregateId, aggregateType, principalId)
    {
    }

    public int Value1 { get; set; }
}
