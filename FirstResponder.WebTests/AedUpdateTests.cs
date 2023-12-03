using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace FirstResponder.WebTests;

public class AedUpdateTests : IDisposable
{
    private readonly IWebDriver _driver = new FirefoxDriver();
    private readonly string _baseAddress = "http://localhost:5159";

    private readonly string _testEmail = "dominik@example.com";
    private readonly string _testPassword = "Password1";
    private readonly string _testFullName = "Dominik Ježík";
    
    private readonly string _targetAedGuid = "4cc7d74c-335c-4e98-9599-08dbf3f95942";

    [Fact]
    public void PersonalAedUpdateWithoutSelectedOwner_SubmitButtonIsDisabled()
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
            .GoToUrl($"{_baseAddress}/aed/{_targetAedGuid}");
        
        _driver.FindElement(By.Id("remove-selected-owner"))
            .Click();
        
        _driver.FindElement(By.Id("search-box"))
            .SendKeys("John Doe");
        
        // Assert
        _driver.FindElement(By.CssSelector("button[type='submit']"))
            .GetAttribute("disabled").Should().Be("true");

    }

    [Fact]
    public void PersonalAedUpdateWithSelectedOwner_SubmitButtonIsEnabled()
    {
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
            .GoToUrl($"{_baseAddress}/aed/{_targetAedGuid}");
        
        _driver.FindElement(By.Id("remove-selected-owner"))
            .Click();
        
        _driver.FindElement(By.Id("search-box"))
            .SendKeys("John Doe");
        
        // Wait for search results to load
        new WebDriverWait(_driver, TimeSpan.FromSeconds(5))
            .Until(d => d.FindElement(By.CssSelector(".search-result")) != null);
        
        // Get first search result
        _driver.FindElement(By.CssSelector(".search-result"))
            .Click();
        
        // Assert
        _driver.FindElement(By.CssSelector("button[type='submit']"))
            .GetAttribute("disabled").Should().Be(null);

    }
    
    public void Dispose()
    {
        _driver.Quit(); 
        _driver.Dispose();
    }
}