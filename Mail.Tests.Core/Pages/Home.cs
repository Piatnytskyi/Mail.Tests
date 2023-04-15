namespace Mail.Tests.Core.Pages
{
    public class Home
    {
        private readonly WebDriver _webDriver;

        public Home(WebDriver webDriver)
        {
            this._webDriver = webDriver;
            if (!webDriver.Title.Equals("Sign In Page"))
                throw new InvalidDataException("This is not Sign In Page, current page is: " + webDriver.Url);
        }
    }
}
