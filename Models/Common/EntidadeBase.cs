using System.Collections.Generic;

namespace Api_HabeisEducacional.Models.Common
{
    // Classe base para entidades que suportam eventos de domínio
    public abstract class EntidadeBase
    {
        private readonly List<DomainEvent> _eventos = new();
        public IReadOnlyCollection<DomainEvent> Eventos => _eventos.AsReadOnly();

        protected void AdicionarEvento(DomainEvent evento)
        {
            _eventos.Add(evento);
        }

        public void LimparEventos()
        {
            _eventos.Clear();
        }
    }
} 