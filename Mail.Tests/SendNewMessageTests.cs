using Mail.Tests.Business;
using Mail.Tests.Business.Models;
using Mail.Tests.Core.Pages;
using Mail.Tests.Options;
using Mapster;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using Xunit;

namespace Mail.Tests
{
    public class SendNewMessageTests : IDisposable
    {
        private readonly WebDriver _webDriver;
        private readonly IConfiguration _configuration;

        public SendNewMessageTests()
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

        public static IEnumerable<object[]> SendNewMessage_TestCases()
        {
            yield return new object[]
            {
                new Message() { To = "whatever@mailinator.com", Subject = "Test 1 " + Guid.NewGuid(), Content = "Hello World!" }
            };
        }

        [Theory]
        [MemberData(nameof(SendNewMessage_TestCases))]
        public void SendNewMessage(Message message)
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

            //Act
            mail.Send(message);

            //Assert
            Assert.True(mail.IsMessageInFolder("Sent Items", message));
        }

        public static IEnumerable<object[]> SendNewMessage_Invalid_TestCases()
        {
            yield return new object[]
            {
                new Message() { To = "invalid", Subject = "Test 1 " + Guid.NewGuid(), Content = "Hello World!" }
            };
            yield return new object[]
            {
                new Message() { To = "invalid", Subject = "Test 2 " + Guid.NewGuid(), Content = "Hello World!" }
            };
        }

        [Theory]
        [MemberData(nameof(SendNewMessage_Invalid_TestCases))]
        public void SendNewMessage_Invalid(Message message)
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

            //Act
            bool isSend = mail.Send(message);

            //Assert
            Assert.False(isSend);

            _webDriver.Navigate().Refresh();
            _webDriver.SwitchTo().Alert().Accept();
            message.To = "whatever@mailinator.com";
            mail.Send(message);

            Assert.True(mail.IsMessageInFolder("Sent Items", message));
        }
    }
}
