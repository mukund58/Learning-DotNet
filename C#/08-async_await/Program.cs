using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        int result = await GetDataAsync();
        Console.WriteLine(result);
    }

     static async  Task<int> GetDataAsync()
    {
        // YOU IMPLEMENT
	await Task.Delay(4000);

        return 42;
    }
}

