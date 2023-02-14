using Xunit;
using FluentAssertions;
using smallApiExample.Interfaces;
using Moq;
using smallApiExample.Model;

namespace smallApiExample.Core.Tests
{
    public class DatabaseTests
    {
        [Fact()]
        public void DatabaseTest()
        {
            // arrange
            var mock = new Mock<IIdGenerator>();

            // act
            var database = new Database(mock.Object);

            // asset
            database.Should().NotBeNull();
        }

        [Fact()]
        public void CreateCustomerTest()
        {
            // arrange
            var id = 1234;
            var idGeneratorMock = new Mock<IIdGenerator>();
            idGeneratorMock.Setup(x => x.Get()).Returns(id);

            var database = new Database(idGeneratorMock.Object);
            var customer = new CreateCustomer()
            {
                FirstName = "FirstNameTest",
                SurName = "SurNameTest"
            };

            // act
            var result = database.CreateCustomer(customer);

            // asset
            result.Should().NotBeNull();
            result.status.Should().Be(CreateCustomerEnum.Created);
            result.customer.FirstName.Should().Be(customer.FirstName);
            result.customer.SurName.Should().Be(customer.SurName);
            result.customer.Id.Should().Be(id);
        }

        [Fact()]
        public void CreateCustomerErrorTest()
        {
            // arrange
            var id = 1234;
            var idGeneratorMock = new Mock<IIdGenerator>();
            idGeneratorMock.Setup(x => x.Get()).Returns(id);

            var database = new Database(idGeneratorMock.Object);
            var firstCustomer = new CreateCustomer()
            {
                FirstName = "FirstNameTest",
                SurName = "FirstSurNameTest"
            };

            var secendCustomer = new CreateCustomer()
            {
                FirstName = "SecendNameTest",
                SurName = "SecendNameTest"
            };

            // act
            var resultFirst = database.CreateCustomer(firstCustomer);
            var resultSecend = database.CreateCustomer(secendCustomer);

            // asset
            resultSecend.Should().NotBeNull();
            resultSecend.status.Should().Be(CreateCustomerEnum.Error);
        }

        [Fact()]
        public void CreateCustomerExistTest()
        {
            // arrange
            var id = 1234;
            var idGeneratorMock = new Mock<IIdGenerator>();
            idGeneratorMock.Setup(x => x.Get()).Returns(id);

            var database = new Database(idGeneratorMock.Object);
            var customer = new CreateCustomer()
            {
                FirstName = "FirstNameTest",
                SurName = "SurNameTest"
            };

            // act
            var resultFirst = database.CreateCustomer(customer);
            var resultSecend = database.CreateCustomer(customer);

            // asset
            resultSecend.Should().NotBeNull();
            resultSecend.status.Should().Be(CreateCustomerEnum.CustomerExist);
        }

        [Fact()]
        public void GetAllCustomersTest()
        {
            // arrage
            var database = new Database(new IdGenerator());
            database.CreateCustomer(new CreateCustomer() { FirstName = "fn1", SurName = "sn1" });
            database.CreateCustomer(new CreateCustomer() { FirstName = "fn2", SurName = "sn2" });

            // act
            var result = database.GetAllCustomers();

            // asset
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact()]
        public void RemoveCustomerTest()
        {
            // arrage
            var database = new Database(new IdGenerator());
            var firstId = database.CreateCustomer(new CreateCustomer() { FirstName = "fn1", SurName = "sn1" });
            var secendId = database.CreateCustomer(new CreateCustomer() { FirstName = "fn2", SurName = "sn2" });

            // act
            var delete = database.RemoveCustomer(firstId.customer.Id);
            var result = database.GetAllCustomers();

            // asset
            delete.Should().BeTrue();
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.Any(x => x.Id == firstId.customer.Id).Should().BeFalse();
            result.Any(x => x.Id == secendId.customer.Id).Should().BeTrue();
        }
    }
}