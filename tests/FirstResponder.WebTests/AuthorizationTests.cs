using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace FirstResponder.WebTests;

public class AuthorizationTests : IDisposable
{
    private readonly IWebDriver _driver = new FirefoxDriver();
    private readonly string _baseAddress = "http://localhost:5159";

    private readonly string _testEmail = "dominik@example.com";
    private readonly string _testPassword = "Password1";
    
    
    
    public void Dispose()
    {
        _driver.Quit(); 
        _driver.Dispose();
    }
}