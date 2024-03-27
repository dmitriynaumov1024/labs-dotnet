namespace Lab03;
using System;

public class InputFormatException : Exception
{
    public InputFormatException (string message) 
    : base(message) { }

    public InputFormatException (int line, string message) 
    : base($"line {line}: {message}") { }
}
