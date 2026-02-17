using System;
#nullable enable
class Tool{
private  String? _name{ get; set; }
private  String? _id{ get; set; }

	public  void add(string id, string name)
	{
		Console.WriteLine("Adding Issue...");
		// Add issue logic here
		String issuerId = id;
		String issuerName = name;
		// Simulate adding issue 
		//(issuerId != null) ? Tool._id = id : Tool._id = null;
		//(issuerName != null) ? Tool._name = issuerName : Tool._name = "Unknown";
		_id = issuerId;
		_name = issuerName;
		Console.WriteLine($"Added Issue with Id: {_id}, Name: {_name}");
	}
	public  void remove()
	{
		Console.WriteLine("Removing Issue...");
		// Remove issue logic here
	}
	public void list()
	{
		Console.WriteLine("Listing all Issues...");
		// List issues logic here
	}

	public void version()
	{
		Console.WriteLine("Tool version 1.0.0");
	}

	public void help()
	{
		Console.WriteLine("Help: This tool allows you to manage Issues.");
		Console.WriteLine("Usage: Tool [options]");
		Console.WriteLine("Options:");
		Console.WriteLine("  --help       Show this help message");
		Console.WriteLine("  --version    Show version information");
		Console.WriteLine(" --add <Id> <Name>   Add Issue with Id and Name");
		Console.WriteLine(" --remove <Id>       Remove Issue with Id");
		Console.WriteLine(" --list             List all Issues");
	}
	public void showHelp(String[] args){
		Tool tool = new Tool();
	switch (args[0])
	{
		case "--help":
			// Show help
			help();
			break;
		case "--version":
			// Show version
			version();
			break;
		case "--add":
			// Add issue 
			add(args[1], args[2]);
			break;
		case "--remove":
			// Remove issue
			remove();
			break;
		case "--list":
			// List issues
			list();
			break;
	    default:
			// Unknown option 
			Console.WriteLine("Unknown option. Use --help for usage information.");

			break;

	}
	/*
	if(args.Length < 0)
	{
		Console.WriteLine("No arguments provided. Use --help for usage information.");
	}
	else if(args[0] == "--help")
	{
	}
	else if(args[0] == "--version")
	{
		Console.WriteLine("Tool version 1.0.0");
	}
	else if(args[0] == "--add" && args.Length == 3)
	{
		string id = args[1];
		string name = args[2];
		Console.WriteLine($"Adding Issue with Id: {id}, Name: {name}");
		// Add issue logic here
	}
	else if(args[0] == "--remove" && args.Length == 2)
	{
		string id = args[1];
		Console.WriteLine($"Removing Issue with Id: {id}");
		// Remove issue logic here
	}
	else if(args[0] == "--list")
	{
		Console.WriteLine("Listing all Issues:");
		// List issues logic here
	}
	else
	{
	}
	}
	*/
}
	public static void Main(String[] args){
		showHelp(args);
		for(int i = 0 ;i<args.Length;i++){
			Console.WriteLine($"{i} Argument  {args[i]}");
		}
	}
}
