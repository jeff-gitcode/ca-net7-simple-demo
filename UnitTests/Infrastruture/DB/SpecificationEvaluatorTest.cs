using Application.Interface.SPI;

using Domain;

using FluentAssertions;

using FluentValidation.TestHelper;

using Infrastructure.Authentication;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Moq;
using AutoFixture;
using Infrastructure.DB;

namespace UnitTests.Infrastructure.DB;

public class SpecificationEvaluatorTest
{
    private readonly IFixture _fixture = new Fixture();

    public SpecificationEvaluatorTest()
    {
    }

    [Fact]
    public void GetQuery_WithNullSpecification_ReturnsQuery()
    {
        var users = _fixture.CreateMany<UserDTO>(25);

        var query = new UserSpecification(users.FirstOrDefault().Email);

        var result = SpecificationEvaluator<UserDTO>.GetQuery(users.AsQueryable(), query);

        result.FirstOrDefault().Should().BeEquivalentTo(users.FirstOrDefault());

    }
}