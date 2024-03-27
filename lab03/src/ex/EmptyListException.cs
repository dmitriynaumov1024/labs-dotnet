namespace Lab03;
using System;

public class EmptyListException : Exception
{
    public EmptyListException (string message) 
    : base(message) { }
}
