using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace FirstResponder.WebTests;

public class UserUpdateTests : IDisposable
{
    private readonly IWebDriver _driver = new FirefoxDriver();
    private readonly string _baseAddress = "http://localhost:5159";

    private readonly string _testEmail = "dominik@example.com";
    private readonly string _testPassword = "Password1";
    private readonly string _testFullName = "Dominik Ježík";
    
    private readonly string _targetUserGuid = "21e0e51a-5eb1-40e8-199d-08dbf3ed49ef";

    [Fact]
    public void UpdateUserWithTakenEmail_ShowsErrorMessage()
    {
        // Arrange
        _driver.Navigate()
            .GoToUrl($"{_baseAddress}/login");

        _driver.FindElement(By.Id("Email"))
            .SendKeys(_testEmail);

        _driver.FindElement(By.Id("Password"))
            .SendKeys(_testPassword);

        _driver.FindElement(By.CssSelector("button[type='submit']"))
            .Click();


        // Act
        _driver.Navigate()
            .GoToUrl($"{_baseAddress}/users/{_targetUserGuid}");

        _driver.FindElement(By.Id("Email"))
            .Clear();

        _driver.FindElement(By.Id("Email"))
            .SendKeys(_testEmail);

        _driver.FindElement(By.CssSelector("button[type='submit']"))
            .Click();
        
        // Assert
        _driver.FindElement(By.CssSelector(".message"))
            .Text.Should().Contain("Email 'dominik@example.com' is already taken.");
    }

    [Fact]
    public void UpdateUserWithNotTakenEmail_UpdatesUser()
    {
        // Arrange
        _driver.Navigate()
            .GoToUrl($"{_baseAddress}/login");

        _driver.FindElement(By.Id("Email"))
            .SendKeys(_testEmail);

        _driver.FindElement(By.Id("Password"))
            .SendKeys(_testPassword);

        _driver.FindElement(By.CssSelector("button[type='submit']"))
            .Click();

        // Act
        _driver.Navigate()
            .GoToUrl($"{_baseAddress}/users/{_targetUserGuid}");

        var oldEmail = _driver.FindElement(By.Id("Email")).GetAttribute("value");
        var newEmail = "updated_" + oldEmail;
        
        if (oldEmail.StartsWith("updated_"))
        {
            newEmail = oldEmail.Replace("updated_", "");
        }
        
        _driver.FindElement(By.Id("Email"))
            .Clear();

        _driver.FindElement(By.Id("Email"))
            .SendKeys(newEmail);

        _driver.FindElement(By.CssSelector("button[type='submit']"))
            .Click();
        
        // Assert
        _driver.FindElement(By.CssSelector("body"))
            .Text.Should().NotContain($"Email '{newEmail}' is already taken.");
        
    }

    public void Dispose()
    {
        _driver.Quit(); 
        _driver.Dispose();
    }
}