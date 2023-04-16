using Mail.Tests.Business.Models;
using Mail.Tests.Core.Pages;

namespace Mail.Tests.Business
{
    public class Mail
    {
        private readonly MailPage _mailPage;

        public Mail(MailPage mailPage)
        {
            _mailPage = mailPage;
        }

        public void Send(Message messageToSend)
        {
            _mailPage.OpenNewMail();

            _mailPage.SetTo(messageToSend.To);
            _mailPage.SetSubject(messageToSend.Subject);
            _mailPage.SetContent(messageToSend.Content);

            _mailPage.ConfirmSend();
        }

        public bool IsMessageSent(Message sentMessage)
        {
            _mailPage.OpenSentItems();

            return _mailPage.MessageExists(sentMessage.Subject);
        }

        public void CreateDraft(Message messageToDraft)
        {
            _mailPage.OpenNewMail();

            _mailPage.SetTo(messageToDraft.To);
            _mailPage.SetSubject(messageToDraft.Subject);
            _mailPage.SetContent(messageToDraft.Content);

            _mailPage.OpenMoreOptions();
            _mailPage.SaveDraft();
        }

        public bool IsDraftCreated(Message draftMessage)
        {
            _mailPage.OpenDrafts();

            return _mailPage.MessageExists(draftMessage.Subject);
        }
    }
}
