using CleanTeeth.Domain.Entities.Exceptions;
using CleanTeeth.Domain.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeeth.Tests.Domain.ValueObjects
{
    [TestClass]
    public class EmailTests
    {
        [TestMethod]
        [ExpectedException(typeof(BusinessRuleException))]
        public void Should_Not_Allow_Invalid_Email()
        {
            new Email("invalid-email");
            // Assert is handled by ExpectedException
        }
    }
}
