using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication.Exceptions
{
    public class MediatorException : Exception
    {
        public MediatorException(String message) : base(message) 
        { 
        
        }
    }
}
