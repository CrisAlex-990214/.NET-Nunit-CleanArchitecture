using Bogus;
using CleanArchitecture.Application;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Domain.Entities;
using Moq;
using Shouldly;

namespace CleanArchitecture.UnitTests
{
    public class Tests
    {
        private Mock<IDatabaseService> dbService;
        private Mock<IEmailService> emailService;
        private UserService userService;

        [SetUp]
        public void Setup()
        {
            dbService = new Mock<IDatabaseService>();
            dbService.Setup(x => x.CreateUser(It.IsAny<User>())).Returns(1);

            emailService = new Mock<IEmailService>();
            emailService.Setup(x => x.SendConfirmationEmail(It.IsAny<User>())).Returns(true);

            userService = new UserService(dbService.Object, emailService.Object);
        }

        [Test]
        public void Should_RegisterUser()
        {
            //Arrange
            var dto = new CreateUserDto { Name = new Faker().Person.FullName };

            //Act
            var id = userService.RegisterUser(dto);

            //Assert
            id.ShouldBeGreaterThan(0);
        }

        [Test]
        public void ShouldNot_RegisterUser_ByValidationError()
        {
            //Arrange
            var dto = new CreateUserDto { Name = string.Empty };

            //Act
            void action() => userService.RegisterUser(dto);

            //Assert
            Should.Throw<Exception>(action).Message.ShouldBe("ValidationError");
        }

        [Test]
        public void Should_RegisterUser_ByConfirmationEmailError()
        {
            //Arrange
            var dto = new CreateUserDto { Name = new Faker().Person.FullName };
            emailService.Setup(x => x.SendConfirmationEmail(It.IsAny<User>())).Returns(false);

            //Act
            void action() => userService.RegisterUser(dto);

            //Assert
            Should.Throw<Exception>(action).Message.ShouldBe("ConfirmationEmailError");
        }
    }
}