using CleanTeeth.Domain.Entities;
using CleanTeethApplication.Contracts.Repositories;
using CleanTeethApplication.Exceptions;
using CleanTeethApplication.Features.DentalOffices.Queries.GetDentalOfficeDetail;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeeth.Tests.Application.Features.DentalOffices
{
    [TestClass]
    public class getDentalOfficeQueryHandlerTests
    {
        private IDentalOfficeRepository repository;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private GetDentalOfficeQueryHandler handler;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        [TestInitialize]
        public void Setup() {
        repository = Substitute.For<IDentalOfficeRepository>();
        handler = new GetDentalOfficeQueryHandler(repository);
        }
        [TestMethod]
        public async Task Handle_DentalOfficeExcist_ReturnsIt()
        {
            var dentalOffice = new DentalOffice("Dental Office A");
            var id = dentalOffice.Id;
            var query = new GetDentalOfficeDetailQuery { Id = id };
            repository.GetById(id).Returns(dentalOffice);
            var result = await handler.Handle(query);
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual("Dental Office A",result.Name);
        }
        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task Handle_DentalOFFiceDoesNotExists_Throws()
        {
            var id = Guid.NewGuid();
            var query = new GetDentalOfficeDetailQuery { Id= id };
            repository.GetById(id).ReturnsNull();
            await handler.Handle(query);
        }
    }
}
