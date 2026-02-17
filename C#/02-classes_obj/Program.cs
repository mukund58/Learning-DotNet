using System;
class Student{
private int _id;
public int Id
	{
		get { return _id; }
		set {
			if(value < 0){
				throw new ArgumentException("Id cannot be negative");
			} else {
				_id = value;
			}
		}
	}
// private string _name; // no need as complier automatically create private member
public string? Name { get ; set; } = null;
public string GetInfo(){
	return $"{Id} - {Name}";
}

}
class Program
{
    static void Main()
    {
        Student s = new Student();
        s.Id = 101;
        s.Name = "Bun";

        Console.WriteLine(s.GetInfo());
    }
}

