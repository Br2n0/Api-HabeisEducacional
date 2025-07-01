using System;

namespace Api_HabeisEducacional.Models.Common
{
    // Base para todos os eventos de domínio
    public abstract class DomainEvent
    {
        public DateTime OcorreuEm { get; } = DateTime.UtcNow;
    }
} 