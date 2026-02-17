public interface IConfiguration
{
    
}
public class GetConfigServices : IConfiguration
{
    public string GetConfig()
    {
        return "hi";
    }
}
