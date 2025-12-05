using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeeth.Domain.Entities.ValueObjects
{
    public class TimeInterval
    {
        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public TimeInterval(DateTime start, DateTime end)
        {
            if (start >= end)
            {
                throw new Exceptions.BusinessRuleException("TimeInterval start time must be before end time.");
            }
            Start = start;
            End = end;
        }
    }
}
