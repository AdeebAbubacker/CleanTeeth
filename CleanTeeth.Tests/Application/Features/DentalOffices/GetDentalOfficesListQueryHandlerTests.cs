using CleanTeeth.Domain.Entities;
using CleanTeethApplication.Contracts.Repositories;
using CleanTeethApplication.Features.DentalOffices.Queries.GetDentalOfficeList;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeeth.Tests.Application.Features.DentalOffices
{
    [TestClass]
    public class GetDentalOfficesListQueryHandlerTests
    {
#pragma warning disable IDE0044 // Add readonly modifier
        private IDentalOfficeRepository repository;
#pragma warning restore IDE0044 // Add readonly modifier
        private GetDentalOfficeListQueryHandler handler;

        [TestInitialize]
        public void Setup()
        {
            repository = Substitute.For<IDentalOfficeRepository>();
            handler = new GetDentalOfficeListQueryHandler(repository);
        }

        [TestMethod]
        public async Task Handle_WhenThereAreDentalOfficeReturnsListOfThem()
        {
            var dentalOffices = new List<DentalOffice>
            {
                new DentalOffice("Dental Office A"),
                new DentalOffice("Dental Office B")
            };
            repository.GetAll().Returns(dentalOffices);
            var expexcted = dentalOffices.Select(d => new GetDentalOfficeListDTO
            {
                Id = d.Id,
                Name = d.Name
            }).ToList();
            var result = await handler.Handle(new GetDentalOfficeListQuery());
            Assert.AreEqual(expexcted.Count, result.Count);
            for(int i=0; i < expexcted.Count; i++)
            {
                Assert.AreEqual(expexcted[i].Id, result[i].Id);
                Assert.AreEqual(expexcted[i].Name, result[i].Name);
            }
        }

        [TestMethod]
        public async Task Handle_WhenThereAreNoDentalOffices_ItReturnsAnEmptyList()
        {
            repository.GetAll().Returns(new List<DentalOffice>());
            var result = await handler.Handle(new GetDentalOfficeListQuery());
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }
    }
}
