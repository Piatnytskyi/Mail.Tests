using System.IO;

namespace Mail.Tests.Core.Pages
{
    public class MailPage
    {
        private readonly WebDriver _webDriver;

        private readonly By _newMailBy = By.XPath("//button[@aria-label='New mail']");
        private readonly By _toBy = By.XPath("//div[@aria-label='To']");
        private readonly By _subjectBy = By.XPath("//input[@aria-label='Add a subject']");
        private readonly By _editorBy = By.XPath("//*[@id='editorParent_1']/div");
        private readonly By _sendBy = By.XPath("//button[@aria-label='Send']");
        private readonly By _sentItemsBy = By.XPath("//div[@title='Sent Items']");
        private readonly By _draftsBy = By.XPath("//div[@title='Drafts']");

        public MailPage(WebDriver webDriver)
        {
            _webDriver = webDriver;
            if (!webDriver.Url.Contains("outlook.live.com/mail"))
                throw new InvalidDataException("This is not Outlook mail, current page is: " + webDriver.Url);
        }

        public void OpenNewMail()
        {
            try
            {
                _webDriver.FindElement(_newMailBy).Click();
            }
            catch (StaleElementReferenceException)
            {
                _webDriver.FindElement(_newMailBy).Click();
            }
        }

        public void SetTo(string to)
        {
            _webDriver.FindElement(_toBy).SendKeys(to);
        }

        public void SetSubject(string subject)
        {
            _webDriver.FindElement(_subjectBy).SendKeys(subject);
        }

        public void SetContent(string content)
        {
            _webDriver.FindElement(_editorBy).SendKeys(content);
        }

        public void ConfirmSend()
        {
            _webDriver.FindElement(_sendBy).Click();
        }

        public void OpenSentItems()
        {
            _webDriver.FindElement(_sentItemsBy).Click();
        }

        public bool MessageExists(string subject)
        {
            try
            {
                _webDriver.FindElement(By.XPath($"//*[contains(.,'{subject}')]"));
            }
            catch (NoSuchElementException)
            {
                return false;
            }

            return true;
        }

        public void OpenDrafts()
        {
            _webDriver.FindElement(_draftsBy).Click();
        }
    }
}
