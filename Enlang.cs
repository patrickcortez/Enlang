using Enlang.Components;
using System.ComponentModel.Design;
using Terminal.Gui;
using static Enlang.Utils.Utility;

namespace Enlang;


// Main Enlang class
public class Enlang // this class acts as a entry point to the interpreter and a configurer
{

    private static readonly float enlangver = 1.0F; //Enlang Version
    string[] commands = { "help","run","debug" };
    static DirectoryInfo currentDir = new DirectoryInfo(Environment.CurrentDirectory);
    static readonly string Status = $@"Version: {enlangver},
                                       Status : W.I.P,
                                       Develoer: Tezz2026";
    static int width = Console.WindowWidth;
    static int height = Console.WindowHeight;
    static string currentFile = string.Empty;
    internal static void PrintHelp() //for user convenience
    {
        string msg = $@"
            Enlang v{enlangver}:
            ------------------------------------
            run <.enl file> : Runs Enlang Script
            debug <.enl file> : Debugs Enlang Script
            help or --help  : Displays help
            status : Shows the current status of Enlang

            ------------------------------------
            Interactive: 
            ------------------------------------
            ls : Lists all files and directories in pwd
            cd : Changes Directory
            clear : Clears Screen
            edit <file> : Opens a TUI Text Editor to edit your files.
            exit : Exits interactive mode

        ";
        Console.WriteLine(msg);
    }

    private static void PrintStatus()
    {
        Console.WriteLine(Status);
    }

    internal static void Run(string filepath,bool isdebug=false) // runs all  enl files
    {

        if(Path.GetExtension(filepath) != ".enl")
        {
            Debug($"File: {Path.GetFileName(filepath)} is not a .enl file!", true);
            return;
        }

        Core start = new Core(filepath,isdebug); // initialize our Interpreter core
        start.BeginInterpret();
    }

    private static void ListFiles(string tar = "")
    {
        if(tar != string.Empty)
        {
            string target = Path.Combine(currentDir.FullName, tar);
            var listd = Directory.GetDirectories(target);
            var listf = Directory.GetFiles(target);
            foreach (var dir in listd)
            {
                Console.WriteLine($"[DIR] {Path.GetFileName(dir)}");
            }

            foreach(string file in listf)
            {
                Console.WriteLine($"[File] {Path.GetFileName(file)}");
            }

            return;
        }

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

    private static void SetFile(string src)
    {
        if (!Path.Exists(src))
        {
            Debug($"File: {Path.GetFileName(src)} does not Exist!",true);
            return;
        }

        if(Path.GetExtension(src) != ".enl")
        {
            Debug($"File: {Path.GetFileName(src)} does not exist!",true);
            return;
        }

        currentFile = src;
    }

    private static void ClearScreen()
    {
        Console.Clear();
       // DrawFooter();
        Console.SetCursorPosition(0, 2);

    }

    private static void RunInteractive() // Enlang Interactive Shell for user convenience
    {
        try
        {
            string input = string.Empty;
        //    ConsoleKey key = new ConsoleKey();

            Console.WriteLine($"Welcome to Enlang v{enlangver}! type 'help' to get started\n");

            DrawHeader();
           // DrawFooter();
            Console.SetCursorPosition(0, 2);

            while (true)
            {


                Console.ResetColor();
                Console.Write($"[{currentDir.FullName}@Enlang]:"); input = Console.ReadLine(); // user prompt
                input = Environment.ExpandEnvironmentVariables(input); // expand any environement variables just incase

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
                    if(args.Length < 2)
                    {
                        ListFiles();
                    }
                    else
                    {
                        ListFiles(args[1]);
                    }


                }
                else if (args[0] == "exit") // exit the interactive shell
                {
                    Environment.Exit(0);
                }
                else if (args[0] == "cd") //basic navigation.
                {
                    ChangeDirectory(args[1]);
                }
                else if (args[0] == "clear")
                {
                    ClearScreen();
                }
                else if (args[0] == "status")
                {
                    PrintStatus();
                }
                else if (args[0] == "set")
                {
                    if (Path.IsPathRooted(args[1]))
                    {
                        SetFile(args[1]);
                    }
                    else
                    {
                        args[1] = Path.Combine(currentDir.FullName, args[1]);
                        SetFile(args[1]);
                    }


                }
                else if (args[0] == "edit")
                {
                    Application.Init();
                    if (args.Length < 2)
                    {
                        Application.Run(new Interactive());
                    }
                    else
                    {
                        string src = Path.Combine(currentDir.FullName,args[1]);
                        Application.Run(new Interactive(src));
                    }
                    
                    Application.Shutdown();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Command: {args[0]} does not exist!");
                }

            }
        }
         catch (Exception ex)
        {
            Debug(ex.Message, true);
        }
    }

    private static void Initialize()
    {
        Console.Clear();
        Console.Title = "Enlang Interpreter";
    }
    

    private static void DrawHeader()
    {
        Console.SetCursorPosition(0, 0);
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(new string('-', width));
        Console.SetCursorPosition((width - "[Enlang Interactive Shell]".Length) / 2, 0);
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("[Enlang Interactive Shell]");

    }

    /* Disabling Footers for now, up until I can find a way to implement scrolling
    private static void DrawFooter()
    {
        Console.SetCursorPosition(0, height - 1);
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(new string('-', width));
        Console.SetCursorPosition((width - "[By Tezz2026]".Length) / 2, height - 2);
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"[By Tezz2026]");
    }
    */

    public static void Main(string[] args)
    {

        Initialize();

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
        }else if(input == "status")
        {
            PrintStatus();
        }
    }
}