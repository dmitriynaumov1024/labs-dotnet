namespace Lab08;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class Program
{
    public static void Main (string[] args)
    {
        Directory.CreateDirectory("./var");

        MovieRepo movieRepo = new MovieRepoSqlite (
            new SqliteConfig() { DbPath = "./var/movies.db" }
        );

        MovieTui movieTui = new MovieTui() { 
            Repo = movieRepo
        };

        Console.CancelKeyPress += (_, _) => {
            movieRepo.Dispose();  
            Console.WriteLine("\n^C");
        };

        Console.WriteLine("Наумов Дмитро Павлович, гр.8.1213");
        Console.WriteLine("Бібліотека фільмів");
        Console.WriteLine("");

        while (true) {
            try {
                var result = movieTui.ReadAndHandleCommand();
                if (result.Status == TuiActionStatus.Stop) {
                    break;
                }
                else if (result.Status == TuiActionStatus.Error) {
                    Console.Write("\u001b[00;31m");
                    Console.Write(result.Message);
                    Console.Write("\u001b[00m\n");
                }
                else if (result.Message != null) {
                    Console.Write("\u001b[00;36m");
                    Console.Write(result.Message);
                    Console.Write("\u001b[00m\n");
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
            }
            Console.WriteLine();
        }

        Console.WriteLine();

        movieRepo.Dispose();
    }
}
