using System;

class Program
{
    static void Main()
    {
	    
        Console.WriteLine(SafeDivide(10, 2));
        Console.WriteLine(SafeDivide(10, 0));
    }

    static int SafeDivide(int a, int b)
    {
        // YOU IMPLEMENT
	try {
		return a/b;
	}catch(DivideByZeroException){
		return -1;
	}
    }
}

