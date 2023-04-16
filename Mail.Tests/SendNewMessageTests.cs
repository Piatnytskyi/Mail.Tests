using Mail.Tests.Business;
using Mail.Tests.Business.Models;
using Mail.Tests.Core.Pages;
using Mail.Tests.Options;
using Mapster;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Interactions;
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
        }

        public void Dispose()
        {
            _webDriver.Quit();
        }

        [Fact]
        public void SendNewMessage()
        {
            //Arrange
            Message message = new Message() { To = "v.v.piatnytskyi@gmail.com", Subject = "Test", Content = "Test" };

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
            Assert.Equal("https://outlook.live.com/mail/0/", _configuration["BaseUrl"]);
        }
    }
}
