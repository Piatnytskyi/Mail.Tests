namespace Mail.Tests.Core.Pages
{
    public class SignIn
    {
        private readonly WebDriver _webDriver;

        private readonly By usernameBy = By.Name("user_name");
        private readonly By passwordBy = By.Name("password");
        private readonly By signinBy = By.Name("sign_in");

        public SignIn(WebDriver webDriver)
        {
            this._webDriver = webDriver;
            if (!webDriver.Title.Equals("Sign In Page"))
                throw new InvalidDataException("This is not Sign In Page, current page is: " + webDriver.Url);
        }

        public Home loginValidUser(String userName, String password)
        {
            _webDriver.FindElement(usernameBy).SendKeys(userName);
            _webDriver.FindElement(passwordBy).SendKeys(password);
            _webDriver.FindElement(signinBy).Click();
            return new Home(_webDriver);
        }
    }
}
