using OpenQA.Selenium;

namespace Mail.Tests.Core.Pages
{
    public class SignInPage
    {
        private readonly WebDriver _webDriver;

        private readonly By _usernameBy = By.Name("loginfmt");
        private readonly By _passwordBy = By.Name("passwd");
        private readonly By _nextBy = By.XPath("//input[@id='idSIButton9' and @value='Next']");
        private readonly By _signInBy = By.XPath("//input[@id='idSIButton9' and @value='Sign in']");
        private readonly By _noStaySignInBy = By.Id("idBtn_Back");

        public SignInPage(WebDriver webDriver)
        {
            _webDriver = webDriver;
            if (!webDriver.Url.Contains("login.live.com"))
                throw new InvalidDataException("This is not Sign In Page, current page is: " + webDriver.Url);
        }

        public void SetUserName(string userName)
        {
            _webDriver.FindElement(_usernameBy).SendKeys(userName);
        }

        public void SetPassword(string password)
        {
            _webDriver.FindElement(_passwordBy).SendKeys(password);
        }

        public void ConfirmUserName()
        {
            _webDriver.FindElement(_nextBy).Click();
        }

        public MailPage? ConfirmPassword()
        {
            _webDriver.FindElement(_signInBy).Click();

            if (!_webDriver.Url.Contains("outlook.live.com/mail"))
                return null;

            return new MailPage(_webDriver);
        }

        public MailPage ConfirmNoStaySignIn()
        {
            _webDriver.FindElement(_noStaySignInBy).Click();
            return new MailPage(_webDriver);
        }
    }
}
