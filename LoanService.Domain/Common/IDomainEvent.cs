using System;
using System.Collections.Generic;
using System.Text;

namespace LoanService.Domain.Common
{
    public interface IDomainEvent
    {
        Guid EventId {  get; }
        DateTime Occurred { get; }
    }
}
