using OpenQA.Selenium;

namespace Mail.Tests.Core.Pages
{
    public class HomePage
    {
        private readonly WebDriver _webDriver;

        private readonly By _signInBy = By.XPath("//a[@data-task='signin']");

        public HomePage(WebDriver webDriver)
        {
            _webDriver = webDriver;
            if (!webDriver.Url.Contains("outlook.live.com/owa"))
                throw new InvalidDataException("This is not Outlook home, current page is: " + webDriver.Url);
        }
        public SignInPage? OpenSignIn()
        {
            _webDriver.FindElement(_signInBy).Click();
            if (!_webDriver.Url.Contains("login.live.com"))
                return null;
               
            return new SignInPage(_webDriver);
        }
    }
}
