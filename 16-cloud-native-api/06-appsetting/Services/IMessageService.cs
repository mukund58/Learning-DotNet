public interface IMessageServices
{
    string GetMessage();
}
public class MessageServices : IMessageServices
{
    public string GetMessage()
    {
        return "hi";
    }
}
