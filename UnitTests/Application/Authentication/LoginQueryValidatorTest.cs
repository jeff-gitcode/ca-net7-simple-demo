using Domain;

using FluentValidation.TestHelper;

namespace UnitTests.Application.Authentication;

public class LoginQueryValidatorTest
{
    private readonly LoginQueryValidator _loginQueryValidator;

    public LoginQueryValidatorTest()
    {
        _loginQueryValidator = new LoginQueryValidator();
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("test", "")]
    [InlineData("", "test")]
    public void LoginQueryValidator_Should_ReturnError(string email, string password)
    {
        var request = new LoginQuery(new LoginDTO { Email = email, Password = password });

        var result = _loginQueryValidator.TestValidate(request);

        if (email == "")
        {
            result.ShouldHaveValidationErrorFor(x => x.loginDTO.Email);

        }
        if (password == "")
        {
            result.ShouldHaveValidationErrorFor(x => x.loginDTO.Password);

        }
    }

    [Theory]
    [InlineData("test", "test")]
    public void LoginQueryValidator_Should_ReturnNoError(string email, string password)
    {
        var request = new LoginQuery(new LoginDTO { Email = email, Password = password });

        var result = _loginQueryValidator.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(x => x.loginDTO.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.loginDTO.Password);
    }
}