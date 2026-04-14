using Enlang.Components;

namespace Enlang;



// Main Enlang class
public class Enlang // this class acts as a entry point to the interpreter and a configurer
{

    private static readonly float enlangver = 1.0F; //Enlang Version

    public static void PrintHelp() //for user convenience
    {
        string msg = $@"
            Enlang v{enlangver}:
            ------------------------------------
            run <.enl file> : Runs Enlang Script

        ";
        Console.WriteLine(msg);
    }

    public static void Run(string filepath) // runs all  enl files
    {
        Core start = new Core(filepath); // initialize our Interpreter core
        start.BeginInterpret();
    }

    public static void Main(string[] args)
    {
        if(args.Length < 1) // print help if there are no arguments
        {
            PrintHelp();
        }
    }
}