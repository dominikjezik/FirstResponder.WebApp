using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace FirstResponder.WebTests;

public class LoginTests : IDisposable
{
    private readonly IWebDriver _driver = new FirefoxDriver();
    private readonly string _baseAddress = "http://localhost:5159";

    private readonly string _testEmail = "dominik@example.com";
    private readonly string _testPassword = "Password1";
    private readonly string _testFullName = "Dominik Ježík";
    
    [Fact] 
    public void LoginWithValidCredentials_RedirectsToHomePage() 
    { 
        // Arrange
        
        // Act
        _driver.Navigate()
            .GoToUrl($"{_baseAddress}/login"); 
            
        _driver.FindElement(By.Id("Email"))
            .SendKeys(_testEmail); 
        
        _driver.FindElement(By.Id("Password"))
            .SendKeys(_testPassword); 
        
        _driver.FindElement(By.CssSelector("button[type='submit']"))
            .Click();

        // Assert
        _driver.Title.Should().BeEquivalentTo("Home Page | First Responder");
        
        _driver.FindElement(By.ClassName("logged-user"))
            .Text.Should().Contain(_testFullName);
    }

    [Fact]
    public void LoginWithValidEmailAndInvalidPassword_ShowsErrorMessage()
    {
        // Arrange

        // Act
        _driver.Navigate()
            .GoToUrl("http://localhost:5159/login");

        _driver.FindElement(By.Id("Email"))
            .SendKeys(_testEmail);

        _driver.FindElement(By.Id("Password"))
            .SendKeys("UrciteJeTotoNespravneHeslo");
        
        _driver.FindElement(By.CssSelector("button[type='submit']"))
            .Click();
        
        // Assert
        _driver.FindElement(By.CssSelector(".is-danger"))
            .Text.Should().Contain("Nesprávne prihlasovacie údaje");
        
        _driver.FindElement(By.Id("Email"))
            .GetAttribute("value").Should().Be(_testEmail);
        
        _driver.FindElement(By.Id("Password"))
            .GetAttribute("value").Should().BeEmpty();
    }

    [Fact]
    public void LoginWithInvalidEmailAndPassword_ShowsErrorMessage()
    {
        // Arrange

        // Act
        _driver.Navigate()
            .GoToUrl("http://localhost:5159/login");

        _driver.FindElement(By.Id("Email"))
            .SendKeys("neexistujucipouzivatel@example.com");

        _driver.FindElement(By.Id("Password"))
            .SendKeys("UrciteJeTotoNespravneHeslo");

        _driver.FindElement(By.CssSelector("button[type='submit']"))
            .Click();

        // Assert
        _driver.FindElement(By.CssSelector(".is-danger"))
            .Text.Should().Contain("Nesprávne prihlasovacie údaje");

        _driver.FindElement(By.Id("Email"))
            .GetAttribute("value").Should().Be("neexistujucipouzivatel@example.com");

        _driver.FindElement(By.Id("Password"))
            .GetAttribute("value").Should().BeEmpty();
    }

    public void Dispose()
    {
        _driver.Quit(); 
        _driver.Dispose();
    }
}