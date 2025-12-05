using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeeth.Domain.Entities.Exceptions
{
    public class BusinessRuleException : Exception
    {
        public BusinessRuleException(String message): base(message)
        {
            
        }
    }
}
