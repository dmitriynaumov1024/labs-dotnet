namespace Lab08;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

public class MovieDbContext : DbContext
{
    private Action<DbContextOptionsBuilder> configure = null;

    public DbSet<Movie> Movies { get; set; }

    public MovieDbContext (Action<DbContextOptionsBuilder> configure) : base ()
    {
        this.configure = configure;
    }

    protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder) 
    {
        if (this.configure != null) this.configure(optionsBuilder);
    }
}
