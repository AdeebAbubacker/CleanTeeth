using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeeth.Domain.Entities.ValueObjects
{
    public record Email
    {
        public String Value { get; } = null!;

        private Email() { }
        public Email(String email)
        {
            if (String.IsNullOrWhiteSpace(email))
            {
                throw new Exceptions.BusinessRuleException("Patient email cannot be empty.");
            }

            if (!email.Contains("@"))
            {
                throw new Exceptions.BusinessRuleException("Patient email is not valid.");
            }

            Value = email;
        }
    }
}
