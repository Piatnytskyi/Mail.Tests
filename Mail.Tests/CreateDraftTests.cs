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
    public class CreateDraftTests : IDisposable
    {
        private readonly WebDriver _webDriver;
        private readonly IConfiguration _configuration;

        public CreateDraftTests()
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

        public static IEnumerable<object[]> CreateDraft_TestCases()
        {
            yield return new object[]
            {
                new Message() { To = "whatever@mailinator.com", Subject = "Test 1 " + Guid.NewGuid(), Content = "Hello World!" }
            };
        }

        [Theory]
        [MemberData(nameof(CreateDraft_TestCases))]
        public void CreateDraft(Message message)
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
            mail.CreateDraft(message);

            //Assert
            Assert.True(mail.IsMessageInFolder("Drafts", message));
        }
    }
}
