using CleanTeethApplication.Exceptions;
using CleanTeethApplication.Utilities;
using FluentValidation;
using Microsoft.Testing.Platform.Requests;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeeth.Tests.Application.Utilities.Mediator
{
    [TestClass]
    public class SimpleMediatorTests
    {
        public class FalseRequest : IRequest<String> {
            public required String Name { get; set; }
        }

        public class FalseRequestValidator : AbstractValidator<FalseRequest> {
            public FalseRequestValidator()
            {
                RuleFor(x => x.Name).NotEmpty();
            }
        }
        [TestMethod]
        public async Task Send_WithRegisteredHandler_HandleIsExecuted()
        {
            var request = new FalseRequest()
            {
                Name = "test"
            };
            var handlerMock = Substitute.For<IRequestHandler<FalseRequest, String>>();
            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(typeof(IRequestHandler<FalseRequest,String>)).Returns(handlerMock);
            var mediator = new SimpleMediator(serviceProvider);
            var result = await mediator.Send(request);
            await handlerMock.Received(1).Handle(request);
        }

        [TestMethod]
        [ExpectedException(typeof(MediatorException))]
        public async Task Send_WithoutRegisteredHandler_Throws()
        {
            var request = new FalseRequest() { Name = "d"};
            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(typeof(IRequestHandler<FalseRequest, String>)).ReturnsNull();
            var mediator = new SimpleMediator(serviceProvider);
            var result = await mediator.Send(request);
        }


        [TestMethod]
        [ExpectedException(typeof(CustomValidationException))]
        public async Task Send_InvalidCommand_Throws()
        {
            var request = new FalseRequest() { Name = "" };
            var serviceProvider = Substitute.For<IServiceProvider>();
            var validator = new FalseRequestValidator();
            serviceProvider.GetService(typeof(IValidator<FalseRequest>)).Returns(validator);
            var mediator = new SimpleMediator(serviceProvider);

            await mediator.Send(request);
        }
    }
}
