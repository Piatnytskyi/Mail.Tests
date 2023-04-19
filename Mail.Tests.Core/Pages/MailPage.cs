using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using System.IO;
using OpenQA.Selenium.Support.UI;

namespace Mail.Tests.Core.Pages
{
    public class MailPage
    {
        private readonly WebDriver _webDriver;

        private readonly By _newMailBy = By.XPath("//button[@aria-label='New mail']");
        private readonly By _sendBy = By.XPath("//button[@aria-label='Send']");
        private readonly By _saveDraftBy = By.Name("Save draft");

        private readonly By _toBy = By.XPath("//div[@aria-label='To']");
        private readonly By _subjectBy = By.XPath("//input[@aria-label='Add a subject']");
        private readonly By _editorBy = By.XPath("//*[contains(@id, 'editorParent')]/div");
        private readonly By _moveToNewFolderBy = By.XPath("//input[@title='Create new folder and move to it']");

        private readonly By _moreOptionsBy = By.XPath("//button[@aria-label='More options']");
        private readonly By _moveToBy = By.XPath("//button[@aria-label='Move to']");

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

        public void Select(string subject)
        {
            Actions actions = new Actions(_webDriver);
            actions.MoveToElement(_webDriver.FindElement(By.XPath($"//div[contains(@aria-label, '{subject}')]")))
                .Build()
                .Perform();
            
            _webDriver.FindElement(By.XPath($"//div[contains(@aria-label, '{subject}')]/descendant::div[contains(@role, 'checkbox')]")).Click();
        }

        public bool MessageExists(string subject)
        {
            WebDriverWait wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(60));

            try
            {
                wait.Until(e => e.FindElement(By.XPath($"//*[contains(.,'{subject}')]")));
            }
            catch (NoSuchElementException)
            {
                return false;
            }

            return true;
        }

        public void OpenFolder(string folderName)
        {
            _webDriver.FindElement(By.XPath($"//div[@title='{folderName}']")).Click();
        }

        public void OpenMoreOptions()
        {
            _webDriver.FindElement(_moreOptionsBy).Click();
        }

        public void SaveDraft()
        {
            _webDriver.FindElement(_saveDraftBy).Click();
        }

        public void OpenMoveToOptions()
        {
            _webDriver.FindElement(_moveToBy).Click();
        }

        public bool FolderExists(string folderName)
        {
            try
            {
                _webDriver.FindElement(By.XPath($"//div[@title='{folderName}']"));
            }
            catch (NoSuchElementException)
            {
                return false;
            }

            return true;
        }

        public void MoveToNewFolder(string toFolderName)
        {
            _webDriver.FindElement(_moveToNewFolderBy).SendKeys(toFolderName);
        }

        public void MoveToFolder(string toFolderName)
        {
            Actions actions = new Actions(_webDriver);
            actions.MoveToElement(_webDriver.FindElement(By.XPath($"//button[@name='{toFolderName}']")))
                .Build()
                .Perform();
            _webDriver.FindElement(By.XPath($"//button[@name='{toFolderName}']")).Click();
        }
    }
}
