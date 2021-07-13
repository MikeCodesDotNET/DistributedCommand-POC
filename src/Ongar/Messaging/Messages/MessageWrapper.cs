namespace Ongar.Messaging.Messages
{
    internal class MessageWrapper 
    {
        public int MessageNumber { get; set; }

        public object Message { get; set; }

        public string TypeName { get; set; }
    }
}
