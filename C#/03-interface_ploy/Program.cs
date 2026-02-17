using System;

interface ILogger{
void Log(string msg);
}

class FileLogger : ILogger{
	public void Log(string msg){
		Console.WriteLine($"[FILE] {msg}");
	}
}
class Program
{
    static void Main()
    {
        ILogger logger = new FileLogger();
        logger.Log("System started");
    }
}

