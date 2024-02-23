namespace Lab08;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

public class MovieRepoSqlite : MovieRepo
{
    private MovieDbContext Db = null;

    public MovieRepoSqlite (SqliteConfig config) : base ()
    {
        this.Db = new MovieDbContext(optionsBuilder => {
            optionsBuilder.UseSnakeCaseNamingConvention();
            optionsBuilder.UseSqlite($"DataSource={config.DbPath}");
        });
        this.Db.Database.EnsureCreated();
    }

    public override void Add (Movie movie) 
    {
        this.Db.Movies.Add(movie);
        this.Db.SaveChanges();
    }
    
    public override void Update (Movie movie)
    {
        this.Db.Movies.Update(movie);
        this.Db.SaveChanges();
    }

    public override void Remove (int id)
    {
        var movie = this.Get(id);
        if (movie == null) return;
        this.Db.Movies.Remove(movie);
        this.Db.SaveChanges();
    }

    public override Movie Get (int id)
    {
        return this.Db.Movies.Where(m => m.Id == id).FirstOrDefault();
    }

    public override IEnumerable<Movie> Filter (string propName, string value, int page)
    {
        int pageSize = 10;

        propName = propName.ToLower();
        value = value.ToLower();
        
        IQueryable<Movie> movies = null;

        try {
            if (propName == "id") {
                int numValue = Convert.ToInt32(value);
                movies = this.Db.Movies.Where(m => m.Id == numValue);
            }
            else if (propName == "name") {
                movies = this.Db.Movies.Where(m => m.Name.ToLower().Contains(value));
            }
            else if (propName == "author") {
                movies = this.Db.Movies.Where(m => m.Author.ToLower().Contains(value));
            }
            else if (propName == "genre") {
                movies = this.Db.Movies.Where(m => m.Genre.ToLower().Contains(value));
            }
            else if (propName == "year") {
                movies = this.Db.Movies.FromSqlRaw (
                    "select * from movies where released_at like {0}", 
                    value + '%'
                );
            }
            else {
                return new Movie[0];
            }
            return movies.Skip(page * pageSize).Take(pageSize).ToList();
        }
        catch (Exception) {
            return new Movie[0];
        }
    }

    public override void Dispose()
    {
        try {
            this.Db.Dispose();
        }
        catch (Exception) {
            // ignore
        }
    }
}
