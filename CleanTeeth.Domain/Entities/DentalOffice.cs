using CleanTeeth.Domain.Entities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeeth.Domain.Entities
{
    public class DentalOffice
    {
        public Guid Id { get; private set; }
        public string Name { get;private set; } = null!;

        public DentalOffice(String name)
        {
            EnforeNameBusinessRules(name);
            Name = name;
            Id = Guid.NewGuid();
        }

        public void UpdateName(String name) {
            EnforeNameBusinessRules(name);
            Name = name;
        }

        private void EnforeNameBusinessRules(String name) {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new Exceptions.BusinessRuleException("Dental office name cannot be empty.");
            }
        }

    }
}
