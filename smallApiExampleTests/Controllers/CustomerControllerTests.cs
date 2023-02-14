using Xunit;
using FluentAssertions;
using smallApiExample.Interfaces;
using Moq;
using Microsoft.Extensions.Logging;
using smallApiExample.Model;
using Microsoft.AspNetCore.Mvc;

namespace smallApiExample.Controllers.Tests
{
    public class CustomerControllerTests
    {
        [Fact()]
        public void CustomerControllerTest()
        {
            // arrange
            var databaseMock = new Mock<IDatabase>();
            var loggerMock = new Mock<ILogger<CustomerController>>();

            // act
            var controler = new CustomerController(loggerMock.Object, databaseMock.Object);

            // asset
            controler.Should().NotBeNull();
        }

        [Fact()]
        public void GetTest()
        {
            // arrange
            var customerList = new List<Customer>() { new Customer() { Id = 123, FirstName = "FirstName", SurName = "SurName" } };
            var databaseMock = new Mock<IDatabase>();
            var loggerMock = new Mock<ILogger<CustomerController>>();
            databaseMock.Setup(x => x.GetAllCustomers()).Returns(customerList);
            var controler = new CustomerController(loggerMock.Object, databaseMock.Object);

            // act
            var result = controler.Get();
            var okResult = result as OkObjectResult;

            // asset
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(customerList);
        }

        [Fact()]
        public void GetErrorTest()
        {
            // arrange
            var databaseMock = new Mock<IDatabase>();
            var loggerMock = new Mock<ILogger<CustomerController>>();
            databaseMock.Setup(x => x.GetAllCustomers()).Throws(new Exception());
            var controler = new CustomerController(loggerMock.Object, databaseMock.Object);

            // act
            var result = controler.Get();
            var statusCodeResult = result as StatusCodeResult;

            // asset
            statusCodeResult.Should().NotBeNull();
            statusCodeResult.StatusCode.Should().Be(500);
        }

        [Fact()]
        public void PostTest()
        {
            // arrange
            var customer = new Customer() { Id = 123, FirstName = "FirstName", SurName = "SurName" };
            var databaseMock = new Mock<IDatabase>();
            var loggerMock = new Mock<ILogger<CustomerController>>();
            databaseMock.Setup(x => x.CreateCustomer(It.IsAny<CreateCustomer>())).Returns((CreateCustomerEnum.Created, customer));
            var controler = new CustomerController(loggerMock.Object, databaseMock.Object);

            // act
            var result = controler.Post(new CreateCustomer() {  FirstName = "FirstName", SurName = "SurName" });
            var objectResult = result as ObjectResult;

            // asset
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(201);
            objectResult.Value.Should().BeEquivalentTo(customer);
        }

        [Fact()]
        public void PostCustomerExistTest()
        {
            // arrange
            var customer = new Customer() { Id = 123, FirstName = "FirstName", SurName = "SurName" };
            var databaseMock = new Mock<IDatabase>();
            var loggerMock = new Mock<ILogger<CustomerController>>();
            databaseMock.Setup(x => x.CreateCustomer(It.IsAny<CreateCustomer>())).Returns((CreateCustomerEnum.CustomerExist, customer));
            var controler = new CustomerController(loggerMock.Object, databaseMock.Object);

            // act
            var result = controler.Post(new CreateCustomer() { FirstName = "FirstName", SurName = "SurName" });
            var badRequestObjectResult = result as BadRequestObjectResult;

            // asset
            badRequestObjectResult.Should().NotBeNull();
            badRequestObjectResult.StatusCode.Should().Be(400);
            badRequestObjectResult.Value.Should().BeEquivalentTo("Customer exist");
        }

        [Fact()]
        public void PostReturnErrorTest()
        {
            // arrange
            var customer = new Customer() { Id = 123, FirstName = "FirstName", SurName = "SurName" };
            var databaseMock = new Mock<IDatabase>();
            var loggerMock = new Mock<ILogger<CustomerController>>();
            databaseMock.Setup(x => x.CreateCustomer(It.IsAny<CreateCustomer>())).Returns((CreateCustomerEnum.Error, customer));
            var controler = new CustomerController(loggerMock.Object, databaseMock.Object);

            // act
            var result = controler.Post(new CreateCustomer() { FirstName = "FirstName", SurName = "SurName" });
            var badRequestResult = result as BadRequestResult;

            // asset
            badRequestResult.Should().NotBeNull();
            badRequestResult.StatusCode.Should().Be(400);
        }

        [Fact()]
        public void PostErrorTest()
        {
            // arrange
            var customer = new Customer() { Id = 123, FirstName = "FirstName", SurName = "SurName" };
            var databaseMock = new Mock<IDatabase>();
            var loggerMock = new Mock<ILogger<CustomerController>>();
            databaseMock.Setup(x => x.CreateCustomer(It.IsAny<CreateCustomer>())).Throws(new Exception());
            var controler = new CustomerController(loggerMock.Object, databaseMock.Object);

            // act
            var result = controler.Post(new CreateCustomer() { FirstName = "FirstName", SurName = "SurName" });
            var statusCodeResult = result as StatusCodeResult;

            // asset
            statusCodeResult.Should().NotBeNull();
            statusCodeResult.StatusCode.Should().Be(500);
        }

        [Fact()]
        public void DeleteTest()
        {
            // arrange
            int id = 123;
            var databaseMock = new Mock<IDatabase>();
            var loggerMock = new Mock<ILogger<CustomerController>>();
            databaseMock.Setup(x => x.RemoveCustomer(It.IsAny<int>())).Returns(true);
            var controler = new CustomerController(loggerMock.Object, databaseMock.Object);

            // act
            var result = controler.Delete(id);
            var okResult = result as OkObjectResult;

            // asset
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().Be(id);
        }

        [Fact()]
        public void DeleteNoContentTest()
        {
            // arrange
            var databaseMock = new Mock<IDatabase>();
            var loggerMock = new Mock<ILogger<CustomerController>>();
            databaseMock.Setup(x => x.RemoveCustomer(It.IsAny<int>())).Returns(false);
            var controler = new CustomerController(loggerMock.Object, databaseMock.Object);

            // act
            var result = controler.Delete(123);
            var noContentResult = result as NoContentResult;

            // asset
            noContentResult.Should().NotBeNull();
            noContentResult.StatusCode.Should().Be(204);
        }

        [Fact()]
        public void DeleteErrorTest()
        {
            // arrange
            var databaseMock = new Mock<IDatabase>();
            var loggerMock = new Mock<ILogger<CustomerController>>();
            databaseMock.Setup(x => x.RemoveCustomer(It.IsAny<int>())).Throws(new Exception());
            var controler = new CustomerController(loggerMock.Object, databaseMock.Object);

            // act
            var result = controler.Delete(123);
            var statusCodeResult = result as StatusCodeResult;

            // asset
            statusCodeResult.Should().NotBeNull();
            statusCodeResult.StatusCode.Should().Be(500);
        }
    }
}