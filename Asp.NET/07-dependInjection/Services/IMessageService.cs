public interface IMessageService
{
    string GetMessage();
}
public class MessageService : IMessageService
{
    public string GetMessage()
    {
        return "Injected successfully";
    }
}
