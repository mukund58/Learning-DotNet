using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        var nums = new List<int> { 1, 2, 3, 4, 5 };

        var result = GetSquaresOfEven(nums);

        foreach (var x in result)
            Console.Write(x + " ");
    }

    static List<int> GetSquaresOfEven(List<int> nums)
    {
        // YOU IMPLEMENT
	var even = from n in nums
		  where n % 2 == 0
		  select   n*n;
	return  even.ToList();
}
}

