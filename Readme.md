# Enlang

![Badge1](https://img.shields.io/badge/Enlang-Interpeter-blue) ![Badge2](https://img.shields.io/badge/Csharp-blue)

**Enlang** is a interpreted programming language with an interactive shell made in C# with the sole aim of education.
This software is not meant to replace industry standard programming languages but rather
its purely for educational attainment on how interpreters work. 

**Enlang** for now has 3 basic instructions output, input and variable declaration:

 - `print("text")` : This outputs "text" to the Console
 - 'input(var)' : This captures the users input and stores it on a variable named *var*
 - `name=value` : This declares a variable.

With these basic instructions, you can write simple programs:

```Enlang
name="Patrick"

print("Hello my name is: $name ");

age = 0

print("My age is:")

input(age)

print("$age Years old")

```

## Interactive Shell

It also has its own interactive shell with variable expansion for convenience 
with simple linux like commands:

- run : for running .enl normally
- debug : for debugging .enl script
- help : shows commands
- ls : list current directory
- cd : change directory
- clear : clears screen
- exit : exits interactive shell.

---

## Status

The current status of **Erlang** is still work in progress.
I have alot to implement:

Implemented:

- Tokenizer
- Interactive Shell
- Lexer
- Typecaster
- Parser
- Interpreter
- Arithmetic

Yet to be implemented:
- Functions
- Control Flow (if-else)
- Arrays
- Loops (While and For)

---

## Screenshots


![Preview](Assets/Preview1.png)

![Code](Assets/Preview2.png)

---

## LICENSE

This project is under GNU General Public License V3. Check LICENSE file for more information.