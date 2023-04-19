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

        public bool Send(Message messageToSend)
        {
            _mailPage.OpenNewMail();

            _mailPage.SetTo(messageToSend.To);
            _mailPage.SetSubject(messageToSend.Subject);
            _mailPage.SetContent(messageToSend.Content);

            _mailPage.ConfirmSend();

            bool result = !_mailPage.InvalidToWarning();
            if (!result)
                _mailPage.ConfirmInvalidToWarning();

            return result;
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

        public bool IsMessageInFolder(string folderName, Message message)
        {
            _mailPage.OpenFolder(folderName);

            return _mailPage.MessageExists(message.Subject);
        }

        public bool IsMessageInFolder(string folderName, Message[] messages)
        {
            Thread.Sleep(3000);
            _mailPage.OpenFolder(folderName);

            return messages.All(m => _mailPage.MessageExists(m.Subject));
        }

        public void MoveToFolder(string fromFolderName, string toFolderName, Message message)
        {
            _mailPage.OpenFolder(fromFolderName);

            _mailPage.Select(message.Subject);
            _mailPage.OpenMoveToOptions();
            if (_mailPage.FolderExists(toFolderName))
                _mailPage.MoveToFolder(toFolderName);
            else
                _mailPage.MoveToNewFolder(toFolderName);
        }

        public void MoveToFolder(string fromFolderName, string toFolderName, Message[] messages)
        {
            _mailPage.OpenFolder(fromFolderName);

            foreach (var message in messages)
            {
                _mailPage.Select(message.Subject);
            }

            _mailPage.OpenMoveToOptions();
            if (_mailPage.FolderExists(toFolderName))
            {
                _mailPage.MoveToFolder(toFolderName);
            }
            else
                _mailPage.MoveToNewFolder(toFolderName);
        }
    }
}
