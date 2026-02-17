using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        var scores = new Dictionary<string, int>
        {
            { "Alice", 90 },
            { "Bob", 75 },
            { "Charlie", 85 }
        };

        Console.WriteLine(GetScore(scores, "Bob"));
        Console.WriteLine(GetScore(scores, "Eve"));
    }

    static int GetScore(Dictionary<string, int> dict, string name)
    {
        // YOU IMPLEMENT
	//foreach(KeyValuePair<string, int> x in dict){
		
	//	if(x.Key == name){
	//		return x.Value;
	//	}
	//} 
//	foreach(var x in dict){ // auto assign complier will refer as keyvaluepair<TKey,TValue> x
		
//		if(x.Key == name){
//			return x.Value;
//		}
	return	dict.ContainsKey(name) ? dict[name] : -1;
//	    if(dict.TryGetValue(name,out int value)) return value;
    	
       // return -1;
    }
}

