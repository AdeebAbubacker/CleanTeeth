using CleanTeeth.Domain.Entities;
using CleanTeethApplication.Contracts.Persistance;
using CleanTeethApplication.Contracts.Repositories;
using CleanTeethApplication.Features.DentalOffices.Commands.CreateDentalOffice;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeeth.Tests.Application.Features.DentalOffices
{
    [TestClass]
    public class createDentalOfficeCommandHandlerTests
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private IDentalOfficeRepository repository;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private IUnitOfWork unitOfWork;
       private CreateDentalOfficeCommandHandler handler;



        [TestInitialize]
        public void Setup() { 
        repository = Substitute.For<IDentalOfficeRepository>();
        unitOfWork = Substitute.For<IUnitOfWork>();
        handler = new CreateDentalOfficeCommandHandler(repository,unitOfWork);
        }

        [TestMethod]
        public async Task Handle_ValidCommandReturnsDentalOfficeId()
        {
            var command = new CreateDentalOfficeCommand { Name = "Test" };
            var dentalOffice = new DentalOffice("Office");
            repository.Add(Arg.Any<DentalOffice>()).Returns(dentalOffice);
            var result = await handler.Handle(command);
            await repository.Received(1).Add(Arg.Any<DentalOffice>());
            await unitOfWork.Received(1).Commit();
            Assert.AreEqual(dentalOffice.Id, result);
        }

        [TestMethod]
        public async Task HandleWhenThereWasErrorRollBack()
        {
            var command = new CreateDentalOfficeCommand { Name="Test" };
            repository.Add(Arg.Any<DentalOffice>()).Throws<Exception>();
            await Assert.ThrowsExceptionAsync<Exception>(async() => await handler.Handle(command));
            await unitOfWork.Received(1).Rollback();
        }
    }
}
