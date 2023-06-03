namespace Mail.Tests.Business.Models
{
    public class Message
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public override string ToString()
        {
            return Subject;
        }
    }
}
