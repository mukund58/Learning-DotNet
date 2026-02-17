using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

	
 interface IUserService
{
    Task AddUserAsync(int id, string name);
   string GetUserName(int id);
   Task<List<string>> GetAllUserNamesAsync();
}

class UserService : IUserService {

private Dictionary<int, string> dict = new Dictionary<int, string>();

public async Task  AddUserAsync(int id, string name){
	await Task.Delay(500);

	if(dict.ContainsKey(id)){
	//	throw new Exception("User Alredy Exists");		
	throw new InvalidOperationException("User already exists");

	}else{
	 	dict[id]=name; // using Indexer
	}
}

public string GetUserName(int id){
//    	if(dict.TryGetValue(id,int out value)) return value;
	return dict.ContainsKey(id) ? dict[id] : "User Not Found" ;
}

public async Task<List<string>>  GetAllUserNamesAsync(){
	await Task.Delay(500);
	return  dict
		.OrderBy(pair => pair.Key)
		.Select (pair => pair.Value)
		.ToList();
}

}
class Program
{
    static async Task Main()
    {
        IUserService service = new UserService();

        await service.AddUserAsync(1, "Alice");
        await service.AddUserAsync(2, "Bob");
        await service.AddUserAsync(3, "Charlie");

        Console.WriteLine(service.GetUserName(2));
        Console.WriteLine(service.GetUserName(99));

        var users = await service.GetAllUserNamesAsync();
        foreach (var u in users)
            Console.Write(u + " ");
    }
}
