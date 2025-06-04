using System.Collections.Generic;
using System.Linq;

namespace Api_HabeisEducacional.Models.Common
{
    // Base para todos os value objects
    public abstract class ValueObject
    {
        // Método que as classes filhas devem implementar para comparação
        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType()) return false;
            var other = (ValueObject)obj;
            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }
    }
} 