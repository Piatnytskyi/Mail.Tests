using Mail.Tests.Business;
using Mail.Tests.Business.Models;
using Mail.Tests.Core.Pages;
using Mail.Tests.Options;
using Mapster;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace Mail.Tests
{
    public class MarkImportantTests : IDisposable
    {
        private readonly WebDriver _webDriver;
        private readonly IConfiguration _configuration;

        public MarkImportantTests()
        {
            _configuration = TestConfigHelper.GetConfiguration();
            WebDriverOptions webDriverOptions = _configuration.GetSection(nameof(WebDriverOptions)).Get<WebDriverOptions>()!;

            var edgeOptions = new EdgeOptions();
            edgeOptions.AddArgument("inprivate");
            _webDriver = new EdgeDriver(webDriverOptions.WebDriverPath, edgeOptions);
            _webDriver.Manage().Timeouts().ImplicitWait = webDriverOptions.CommandTimeout;
            _webDriver.Manage().Window.Maximize();
        }

        public void Dispose()
        {
            _webDriver.Quit();
        }

        public static IEnumerable<object[]> MarkImportant_TestCases()
        {
            IConfiguration configuration = TestConfigHelper.GetConfiguration();
            UserOptions userOptions = configuration.GetSection(nameof(UserOptions)).Get<UserOptions>()!;
            yield return new object[]
            {
                new [] {
                    new Message() { To = userOptions.Email,  Subject = "Test 1 " + Guid.NewGuid(), Content = "Hello World!" },
                    new Message() { To = userOptions.Email, Subject = "Test 2 " + Guid.NewGuid(), Content = "Hello World!" },
                    new Message() { To = userOptions.Email, Subject = "Test 3 " + Guid.NewGuid(), Content = "Hello World!" }
                }
            };
        }

        [Theory]
        [MemberData(nameof(MarkImportant_TestCases))]
        public void MarkImportant(Message[] messages)
        {
            _webDriver.Navigate().GoToUrl(_configuration["BaseUrl"]);

            var home = new Home(new HomePage(_webDriver));
            MailPage mailPage;
            SignInPage? signInPage = home.StartSignIn();
            if (signInPage is not null)
            {
                var signIn = new SignIn(signInPage);
                UserOptions userOptions = _configuration.GetSection(nameof(UserOptions)).Get<UserOptions>()!;
                var user = userOptions.Adapt<User>();

                mailPage = signIn.Login(user);
            }
            else
            {
                mailPage = new MailPage(_webDriver);
            }
            var mail = new Business.Mail(mailPage);
            var inboxFolderName = "Inbox";
            var deletedItemsFolder = "Deleted Items";
            var importantFolderName = "Important";

            mail.Send(messages[0]);
            mail.Send(messages[1]);
            mail.Send(messages[2]);
            mail.IsMessageInFolder(inboxFolderName, messages[2]);

            //Act
            mail.MoveToFolder(inboxFolderName, importantFolderName, messages);

            //Assert
            Assert.True(mail.IsMessageInFolder(importantFolderName, messages));

            mail.MoveToFolder(importantFolderName, deletedItemsFolder, messages);

            Assert.True(mail.IsMessageInFolder(deletedItemsFolder, messages));
        }
    }
}
