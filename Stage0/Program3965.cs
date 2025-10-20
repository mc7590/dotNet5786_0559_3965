using Microsoft.Win32.SafeHandles;

partial class Program
{
    static void Main(string[] args)
    {
        Welcome3965();
        Welcome0559();
        Console.ReadKey();
        string s;

    }

    static partial void Welcome0559();

    private static void Welcome3965()
    {
        Console.WriteLine("Enter your name: ");
        string name = Console.ReadLine();
        Console.WriteLine("{0} , Welcome to my first console application", name);
    }
}