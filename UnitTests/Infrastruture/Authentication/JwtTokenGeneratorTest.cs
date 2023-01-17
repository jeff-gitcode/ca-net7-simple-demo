using Application.Interface.SPI;

using Domain;

using FluentAssertions;

using FluentValidation.TestHelper;

using Infrastructure.Authentication;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Moq;

namespace UnitTests.Infrastructure.Authentication;

public class JwtTokenGeneratorTest
{
    private readonly Mock<IOptions<JwtSettings>> _configuration;

    private readonly Mock<IDateTimeService> _dateTimeService;

    private readonly JwtTokenGenerator _jwtTokenGenerator;

    public JwtTokenGeneratorTest()
    {
        _configuration = new Mock<IOptions<JwtSettings>>();
        _configuration.Setup(x => x.Value).Returns(new JwtSettings { Secret = "test-for-secret-key", Issuer = "test", Audience = "test", ExpiryInMinutes = 1 });

        _dateTimeService = new Mock<IDateTimeService>();

        _jwtTokenGenerator = new JwtTokenGenerator(_configuration.Object, _dateTimeService.Object);
    }

    [Theory]
    [InlineData("test", "test")]
    public void JwtTokenGenerator_Should_ReturnToken(string email, string password)
    {
        var request = new LoginDTO { Email = email, Password = password };


        _dateTimeService.Setup(x => x.UtcNow).Returns(DateTime.UtcNow);

        var result = _jwtTokenGenerator.CreateToken(request);

        result.Should().NotBeNull();

        result.Should().NotBeEmpty();

        result.Should().NotBeNullOrWhiteSpace();

        result.Should().BeOfType(typeof(string));
    }

    [Theory]
    [InlineData("test", "test")]
    public void JwtTokenGenerator_Should_ReturnTokenWithClaimsAndAudience(string email, string password)
    {
        var request = new LoginDTO { Email = email, Password = password };


        _dateTimeService.Setup(x => x.UtcNow).Returns(DateTime.UtcNow);

        var result = _jwtTokenGenerator.CreateToken(request);

        var jwt = _jwtTokenGenerator.ValidateToken(result);

        jwt.Audiences.FirstOrDefault().Should().Be("test");
        jwt.Issuer.Should().Be("test");

    }

}