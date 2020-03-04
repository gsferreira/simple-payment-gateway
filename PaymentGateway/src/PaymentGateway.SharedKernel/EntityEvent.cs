using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace PaymentGateway.SharedKernel
{
    public abstract class EntityEvent : INotification
    {
        public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
        public int Version { get; set; }
    }

}
