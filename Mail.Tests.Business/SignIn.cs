using Mail.Tests.Business.Models;
using Mail.Tests.Core.Pages;

namespace Mail.Tests.Business
{
    public class SignIn
    {
        private readonly SignInPage _signInPage;

        public SignIn(SignInPage signInPage)
        {
            _signInPage = signInPage;
        }

        public MailPage Login(User user)
        {
            _signInPage.SetUserName(user.Email);
            _signInPage.ConfirmUserName();
            _signInPage.SetPassword(user.Password);

            MailPage? mailPage = _signInPage.ConfirmPassword();
            if (mailPage is null)
                mailPage = _signInPage.ConfirmNoStaySignIn();

            return mailPage;
        }
    }
}
