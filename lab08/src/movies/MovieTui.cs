namespace Lab08;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;


public class MovieQuery
{
    public string PropName { get; set; }
    public string Value { get; set; }
    public int Page { get; set; }
}

public class TuiActionResult
{
    public TuiActionStatus Status { get; set; }
    public string Message { get; set; }
}

[Flags]
public enum TuiActionStatus
{
    Continue = 1,
    Error = 2,
    Stop = 4
}

// text user interface for movies
public class MovieTui
{
    public MovieQuery LastQuery { get; set; } = new MovieQuery();
    public MovieRepo Repo { get; set; } = null;

    public void UseRepo (MovieRepo repo)
    {
        this.Repo = repo;
    }

    public TuiActionResult ReadAndHandleCommand ()
    {
        Console.Write("> ");
        var line = ReadString();
        
        if (line == null || line.Length == 0) {
            return new TuiActionResult() {
                Status = TuiActionStatus.Continue
            };
        }

        if (line.StartsWith("exit") || line.StartsWith("quit") || line.StartsWith("stop")) {
            return new TuiActionResult() { 
                Status = TuiActionStatus.Stop 
            };
        }

        if (line.StartsWith("help")) {
            try {
                Console.WriteLine(File.ReadAllText("./science/commands.txt"));
                return new TuiActionResult() {
                    Status = TuiActionStatus.Continue
                };
            }
            catch (Exception) {
                return new TuiActionResult() {
                    Status = TuiActionStatus.Error,
                    Message = "Error: User manual not available. Sorry for this inconvenience."
                };
            }
        }

        if (this.Repo == null) {
            return new TuiActionResult() { 
                Status = TuiActionStatus.Error,
                Message = "Error: not attached to any Movie Repo"
            };
        }

        if (line.StartsWith("add")) {
            var movie = new Movie();
            this.Read(movie);
            this.Repo.Add(movie);
            return new TuiActionResult() {
                Status = TuiActionStatus.Continue,
                Message = $"Added new Movie #{movie.Id}"
            };
        }

        if (line.StartsWith("find")) {
            var query = new MovieQuery();
            try {
                var words = line.Split(' ', 3, StringSplitOptions.RemoveEmptyEntries);
                query.PropName = words[1];
                query.Value = words[2];
                query.Page = 0;
            }
            catch (Exception) {
                return new TuiActionResult() {
                    Status = TuiActionStatus.Error,
                    Message = "Usage: find id|name|author|year|genre <value>"
                };
            }
            this.LastQuery = query;
            return this.HandlePageQuery();
        }

        if (line.StartsWith("prev") && this.LastQuery.Page > 0) {
            this.LastQuery.Page -= 1;
            return this.HandlePageQuery();
        }

        if (line.StartsWith("next") && this.LastQuery.Page < 99999) {
            this.LastQuery.Page += 1;
            return this.HandlePageQuery();
        }

        if (line.StartsWith("get")) {
            int id = 0;
            try {
                id = int.Parse(line.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]);
            }
            catch (Exception) {
                return new TuiActionResult() {
                    Status = TuiActionStatus.Error,
                    Message = "Usage: get <id: integer>"
                };
            }
            var movie = this.Repo.Get(id);
            if (movie == null) return new TuiActionResult() {
                Status = TuiActionStatus.Error,
                Message = $"Error: could not find Movie #{id}"
            };
            this.Print(movie);
            return new TuiActionResult() {
                Status = TuiActionStatus.Continue
            };
        }

        if (line.StartsWith("edit")) {
            int id = 0;
            try {
                id = int.Parse(line.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]);
            }
            catch (Exception) {
                return new TuiActionResult() {
                    Status = TuiActionStatus.Error,
                    Message = "Usage: edit <id: integer>"
                };
            }
            var movie = this.Repo.Get(id);
            if (movie == null) return new TuiActionResult() {
                Status = TuiActionStatus.Error,
                Message = $"Error: could not find Movie #{id}"
            };
            this.Read(movie);
            this.Repo.Update(movie);
            return new TuiActionResult() {
                Status = TuiActionStatus.Continue,
                Message = $"Updated Movie #{movie.Id}"
            };
        }

        if (line.StartsWith("remove")) {
            int id = 0;
            try {
                id = int.Parse(line.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]);
            }
            catch (Exception) {
                return new TuiActionResult() {
                    Status = TuiActionStatus.Error,
                    Message = "Usage: remove <id: integer>"
                };
            }
            var movie = this.Repo.Get(id);
            if (movie == null) return new TuiActionResult() {
                Status = TuiActionStatus.Error,
                Message = $"Error: could not find Movie #{id}"
            };
            this.Print(movie);
            this.Repo.Remove(movie.Id);
            return new TuiActionResult() {
                Status = TuiActionStatus.Continue,
                Message = $"Removed Movie #{movie.Id}"
            };
        }

        if (line.StartsWith("seed")) {
            string filename = null;
            try {
                filename = line.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1];
            }
            catch (Exception) {
                return new TuiActionResult() {
                    Status = TuiActionStatus.Error,
                    Message = "Usage: seed <filename: *.csv>"
                };
            }
            string[] lines = null;
            try {
                lines = File.ReadAllLines(filename);
            }
            catch (Exception) {
                return new TuiActionResult() {
                    Status = TuiActionStatus.Error,
                    Message = $"Error: Could not open {filename}"
                };
            }
            CsvReader<Movie> reader = new CsvReader<Movie>(line => new Movie() {
                Id = 0,
                Name = line[1],
                Duration = int.Parse(line[2]),
                Genre = line[3],
                Author = line[4],
                ReleasedAt = DateTime.ParseExact(line[5], "yyyy-MM-dd", default)
            });

            CsvOptions csvOptions = new CsvOptions() {
                SkipFirstRow = lines[0].StartsWith("id"),
                IgnoreErrors = true,
                IgnoreNulls = true
            };

            try {
                var movies = reader.Parse(lines, csvOptions).ToList();
                foreach (var movie in movies) {
                    this.Repo.Add(movie);
                }
                return new TuiActionResult() {
                    Status = TuiActionStatus.Continue,
                    Message = $"Added {movies.Count} Movies"
                };
            }
            catch (Exception) {
                return new TuiActionResult() {
                    Status = TuiActionStatus.Error,
                    Message = $"Error: Could not parse input file. Expected format: \nid;name;duration;genre;author;released_at"
                };
            }
        }

        return new TuiActionResult() {
            Status = TuiActionStatus.Error,
            Message = $"Not implemented yet"
        };
    }

    public TuiActionResult HandlePageQuery()
    {
        var query = this.LastQuery;
        var movies = this.Repo.Filter(query.PropName, query.Value, query.Page).ToList();
        foreach (var movie in movies) {
            this.Print(movie);
        }
        Console.Write($"[ < prev ] page {query.Page + 1} [ next > ]");
        if (movies.Count == 0) {
            return new TuiActionResult() {
                Status = TuiActionStatus.Continue,
                Message = "No movies match your query"
            };
        }
        else {
            return new TuiActionResult() {
                Status = TuiActionStatus.Continue
            };
        }
    }

    public void Print (Movie movie) 
    {
        Console.WriteLine($"Movie #{movie.Id}");
        Console.WriteLine($" - name: {movie.Name}");
        Console.WriteLine($" - duration: {movie.Duration}");
        Console.WriteLine($" - genre: {movie.Genre}");
        Console.WriteLine($" - author: {movie.Author}");
        Console.WriteLine($" - release date: {movie.ReleasedAt:yyyy-MM-dd}");
    }

    public void Read (Movie movie)
    {
        Console.WriteLine($"Editing movie #{movie.Id}");
        Console.Write($" - name [{movie.Name}]: ");
        movie.Name = ReadString(movie.Name);
        Console.Write($" - duration [{movie.Duration}]: ");
        movie.Duration = ReadInt(movie.Duration);
        Console.Write($" - genre [{movie.Genre}]: ");
        movie.Genre = ReadString(movie.Genre);
        Console.Write($" - author [{movie.Author}]: ");
        movie.Author = ReadString(movie.Author);
        Console.Write($" - release date: [{movie.ReleasedAt:yyyy-MM-dd}]: ");
        movie.ReleasedAt = ReadDate(movie.ReleasedAt);
        Console.WriteLine();
    }

    private static string ReadString (string fallback = "")
    {
        var line = Console.ReadLine();
        if (line == null || line.Length == 0) return fallback;
        return line.Trim();
    }

    private static int ReadInt (int fallback = 0)
    {
        try {
            var line = Console.ReadLine();
            if (line == null || line.Length == 0) return fallback;
            return int.Parse(line.Trim());
        }
        catch (Exception) {
            return fallback;
        }
    }

    private static DateTime ReadDate (DateTime fallback = default)
    {
        try {
            var line = Console.ReadLine();
            if (line == null || line.Length == 0) return fallback;
            return DateTime.Parse(line.TrimStart().Substring(0, 10));
        }
        catch (Exception) {
            return fallback;
        }
    }
}
