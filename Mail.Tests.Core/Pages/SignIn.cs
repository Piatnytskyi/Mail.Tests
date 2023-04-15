namespace Mail.Tests.Core.Pages
{
    public class SignIn
    {
        private readonly WebDriver _webDriver;

        private readonly By _usernameBy = By.Name("loginfmt");
        private readonly By _passwordBy = By.Name("passwd");
        private readonly By _nextBy = By.Id("idSIButton9");

        public SignIn(WebDriver webDriver)
        {
            _webDriver = webDriver;
            if (!webDriver.Url.Contains("login.live.com/login") || !webDriver.Url.Contains("outlook.live.com"))
                throw new InvalidDataException("This is not Sign In Page, current page is: " + webDriver.Url);
        }

        public Mail Login(string userName, string password)
        {
            _webDriver.FindElement(_usernameBy).SendKeys(userName);
            _webDriver.FindElement(_nextBy).Click();

            _webDriver.FindElement(_passwordBy).SendKeys(password);
            _webDriver.FindElement(_nextBy).Click();

            return new Mail(_webDriver);
        }
    }
}
