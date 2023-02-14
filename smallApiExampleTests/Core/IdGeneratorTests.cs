using Xunit;
using FluentAssertions;

namespace smallApiExample.Core.Tests
{
    public class IdGeneratorTests
    {
        [Fact()]
        public void GetTest()
        {
            // arrange
            var idGenerator = new IdGenerator();

            // act
            var id = idGenerator.Get();

            // asset
            id.Should().BeInRange(1, int.MaxValue);
        }

        [Fact()]
        public void GetTwoValuesTest()
        {
            // arrange
            var idGenerator = new IdGenerator();

            // act
            var idFirst = idGenerator.Get();
            var idSecond = idGenerator.Get();

            // asset
            idFirst.Should().NotBe(idSecond);
        }
    }
}