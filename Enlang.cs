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
            debug <.enl file> : Debugs Enlang Script

        ";
        Console.WriteLine(msg);
    }

    public static void Run(string filepath,bool isdebug=false) // runs all  enl files
    {
        Core start = new Core(filepath,isdebug); // initialize our Interpreter core
        start.BeginInterpret();
    }

    public static void Main(string[] args)
    {
        if(args.Length < 1) // print help if there are no arguments
        {
            PrintHelp();
        }

        string input = args[0];

        if (input == "run")
        {
            if (Path.IsPathRooted(args[1]))
            {
                Run(args[1]);
            }
            else
            {
                string fpath = Path.Combine(Environment.CurrentDirectory, args[1]);
                Run(fpath);
            }
            
        }else if (input == "debug")
        {
            if (Path.IsPathRooted(args[1]))
            {
                Run(args[1],true);
            }
            else
            {
                string fpath = Path.Combine(Environment.CurrentDirectory, args[1]);
                Run(fpath,true);
            }
        }
    }
}