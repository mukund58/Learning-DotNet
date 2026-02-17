using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        List<int> nums = new List<int> { 1, 2, 3, 4, 5 };

        int result = SumEven(nums);
        Console.WriteLine(result);
    }

    static int SumEven(List<int> nums)
    {
        // YOU IMPLEMENT
	int sum = 0;
	foreach(int x in nums){
		if(x % 2 == 0){
			sum +=x;	
		}
	}
        return sum;
    }
}

