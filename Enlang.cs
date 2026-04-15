using Enlang.Components;

namespace Enlang;



// Main Enlang class
public class Enlang // this class acts as a entry point to the interpreter and a configurer
{

    private static readonly float enlangver = 1.0F; //Enlang Version
    string[] commands = { "help","run","debug" };
    static DirectoryInfo currentDir = new DirectoryInfo(Environment.CurrentDirectory);

    internal static void PrintHelp() //for user convenience
    {
        string msg = $@"
            Enlang v{enlangver}:
            ------------------------------------
            run <.enl file> : Runs Enlang Script
            debug <.enl file> : Debugs Enlang Script
            help or --help  : Displays help

            ------------------------------------
            Interactive: 
            ------------------------------------
            ls : Lists all files and directories in pwd
            cd : Changes Directory
            exit : Exits interactive mode

        ";
        Console.WriteLine(msg);
    }

    internal static void Run(string filepath,bool isdebug=false) // runs all  enl files
    {
        Core start = new Core(filepath,isdebug); // initialize our Interpreter core
        start.BeginInterpret();
    }

    private static void ListFiles()
    {


        Console.WriteLine("Files & Directories:");
        foreach(DirectoryInfo dir in currentDir.GetDirectories())
        {
            Console.WriteLine($"[DIR] {dir.Name}");
        }

        foreach(FileInfo file in currentDir.GetFiles())
        {
            Console.WriteLine($"[FILE] {file.Name}");
        }
    }

    private static void ChangeDirectory(string newDir)
    {
        string nDir = Path.Combine(currentDir.FullName, newDir);
        var newpath = new DirectoryInfo(nDir);

        if (Directory.Exists(nDir) && !Path.HasExtension(nDir))
        {
            currentDir = newpath;
            return;
        }
        else if(Path.HasExtension(nDir))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Path: {nDir} is a File!");
            return;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Path: {nDir} does not exist");
            return;
        }
    }

    private static void RunInteractive() // Enlang Interactive Shell for user convenience
    {
        string input = string.Empty;

        Console.WriteLine($"Welcome to Enlang v{enlangver}! type 'help' to get started\n");

        while (true)
        {
            Console.ResetColor();
            Console.Write($"[{currentDir.FullName}@Enlang]:"); input = Console.ReadLine(); // user prompt
            Environment.ExpandEnvironmentVariables(input); // expand any environement variables just incase

            string[] args = input.Split(' ');

            //evaluator 
            if (args[0] == "run") // running .enl script
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

            }
            else if (args[0] == "debug") // running .enl scripts in debug mode
            {
                if (Path.IsPathRooted(args[1]))
                {
                    Run(args[1], true);
                }
                else
                {
                    string fpath = Path.Combine(Environment.CurrentDirectory, args[1]);
                    Run(fpath, true);
                }
            }
            else if (args[0] == "help" || args[0] == "--help")
            {
                PrintHelp();
            }
            else if (args[0] == "ls") // list files & directories
            {
                ListFiles();
            }else if (args[0] == "exit") // exit the interactive shell
            {
                Environment.Exit(0);
            }else if (args[0] == "cd") //basic navigation.
            {
                ChangeDirectory(args[1]);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Command: {args[0]} does not exist!");
            }
        }
    }

    public static void Main(string[] args)
    {
        if(args.Length < 1) // print help if there are no arguments
        {
            RunInteractive();
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
        }else if(input == "help")
        {
            PrintHelp();
        }
    }
}