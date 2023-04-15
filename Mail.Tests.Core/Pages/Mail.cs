namespace Mail.Tests.Core.Pages
{
    public class Mail
    {
        private readonly WebDriver _webDriver;

        public Mail(WebDriver webDriver)
        {
            _webDriver = webDriver;
            if (!webDriver.Url.Contains("outlook.live.com/mail"))
                throw new InvalidDataException("This is not Outlook mail, current page is: " + webDriver.Url);
        }
    }
}
