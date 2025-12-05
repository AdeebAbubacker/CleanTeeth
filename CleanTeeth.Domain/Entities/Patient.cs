using CleanTeeth.Domain.Entities.Exceptions;
using CleanTeeth.Domain.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeeth.Domain.Entities
{
    public class Patient
    {
        public Guid Id { get;private set; }

        public string Name { get; private set; } = null!;

        public Email Email { get; private set; } = null!;
        public Patient() { }

        public Patient(String name, Email email)
        {
            EnforceBusinessRule(name);
            EnforceEmailBusinessRule(email);
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new Exceptions.BusinessRuleException("Patient name cannot be empty.");
            }
            if (email == null)
            {
                throw new Exceptions.BusinessRuleException("Patient email cannot be null.");
            }
            Name = name;
            Email = email;
            Id = Guid.NewGuid();
        }

        public void UpdateName(String name) {
        EnforceBusinessRule(name);
        Name = name;
        }

        private void EnforceBusinessRule(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BusinessRuleException($"The {nameof(name)} is required");
            }
        }

        public void UpdateEmail(Email email)
        {
            EnforceEmailBusinessRule(email);
            Email = email;
        }

        private void EnforceEmailBusinessRule(Email email)
        {
            if (email is null)
            {
                throw new BusinessRuleException($"The {nameof(email)} is required");
            }
        }
    }
}
