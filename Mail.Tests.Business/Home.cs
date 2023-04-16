using Mail.Tests.Business.Models;
using Mail.Tests.Core.Pages;

namespace Mail.Tests.Business
{
    public class Home
    {
        private readonly HomePage _homePage;

        public Home(HomePage homePage)
        {
            _homePage = homePage;
        }

        public SignInPage? StartSignIn()
        {
            return _homePage.OpenSignIn();
        }
    }
}
