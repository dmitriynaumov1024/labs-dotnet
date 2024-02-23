namespace Lab08;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Sqlite;

public class MovieRepoSqlite : MovieRepo
{
    private SqliteConnection con;

    private SqliteConnection InitConnection (SqliteConfig config)
    {
        // create connection
        var con = new SqliteConnection($"DataSource={config.DbPath}");
        con.Open();

        // create tables
        var createStmt = con.CreateCommand();
        createStmt.CommandText = "create table if not exists movies ( id integer auto_increment, name text, duration integer, genre text, author text, released_at text, primary key(id) );";
        createStmt.ExecuteNonQuery();

        return con;
    }

    public MovieRepoSqlite (SqliteConfig config)
    {
        this.con = this.InitConnection(config);
    }

    private int NextId ()
    {
        var selectStmt = con.CreateCommand();
        selectStmt.CommandText = "select (max(id)+1) from movies";
        var result = selectStmt.ExecuteReader();
        result.GetEnumerator().MoveNext();
        if (!(result[0] is DBNull)) return Convert.ToInt32(result[0]);
        return 1;
    }

    public override void Add (Movie movie)
    {
        var insertStmt = con.CreateCommand();
        insertStmt.CommandText = "insert into movies (id, name, duration, genre, author, released_at) values ($0, $1, $2, $3, $4, $5);";
        var p = insertStmt.Parameters;
        if (movie.Id <= 0) movie.Id = this.NextId();
        p.AddWithValue("$0", movie.Id);
        p.AddWithValue("$1", movie.Name);
        p.AddWithValue("$2", movie.Duration);
        p.AddWithValue("$3", movie.Genre);
        p.AddWithValue("$4", movie.Author);
        p.AddWithValue("$5", movie.ReleasedAt.ToString("yyyy-MM-dd"));
        insertStmt.ExecuteNonQuery();
    }

    public override void Update (Movie movie)
    {
        var updateStmt = con.CreateCommand();
        updateStmt.CommandText = "update movies set name=$1, duration=$2, genre=$3, author=$4, released_at=$5 where id=$0;";
        var p = updateStmt.Parameters;
        p.AddWithValue("$0", movie.Id);
        p.AddWithValue("$1", movie.Name);
        p.AddWithValue("$2", movie.Duration);
        p.AddWithValue("$3", movie.Genre);
        p.AddWithValue("$4", movie.Author);
        p.AddWithValue("$5", movie.ReleasedAt.ToString("yyyy-MM-dd"));
        updateStmt.ExecuteNonQuery();
    }

    public override void Remove (int id)
    {
        var removeStmt = con.CreateCommand();
        removeStmt.CommandText = "delete from movies where id=$0;";
        var p = removeStmt.Parameters;
        p.AddWithValue("$0", id);
        removeStmt.ExecuteNonQuery();
    }

    public override Movie Get (int id)
    {
        var selectStmt = con.CreateCommand();
        selectStmt.CommandText = "select id, name, duration, genre, author, released_at from movies where id=$0;";
        var p = selectStmt.Parameters;
        p.AddWithValue("$0", id);
        var result = selectStmt.ExecuteReader();
        foreach (var row in result) {
            // scuffed but working
            return new Movie() {
                Id = Convert.ToInt32(result[0]),
                Name = (string)result[1],
                Duration = Convert.ToInt32(result[2]),
                Genre = (string)result[3],
                Author = (string)result[4],
                ReleasedAt = DateTime.ParseExact((string)result[5], "yyyy-MM-dd", default)
            };
        }
        return null;
    }

    public override IEnumerable<Movie> Filter (string propName, string value, int page)
    {
        int pageSize = 10;
        propName = propName.ToLower();
        if (propName == "year") propName = "released_at";
        if (page < 0) page = 0;
        bool isNumericQuery = (propName == "id");
        bool isTextQuery = (propName == "name" || propName == "genre" || propName == "author" || propName == "released_at");

        var selectStmt = con.CreateCommand();
        if (isNumericQuery) selectStmt.CommandText = $"select id, name, duration, genre, author, released_at from movies where {propName}=$val limit $limit offset $offset;";
        else if (isTextQuery) selectStmt.CommandText = $"select id, name, duration, genre, author, released_at from movies where {propName} like $val limit $limit offset $offset;";
        else {
            Console.WriteLine("aa");
            return new Movie[0];
        }

        var p = selectStmt.Parameters;
        p.AddWithValue("$offset", pageSize * page);
        p.AddWithValue("$limit", pageSize);

        try {
            if (isNumericQuery) p.AddWithValue("$val", Convert.ToInt32(value));
            if (isTextQuery) p.AddWithValue("$val", $"%{value}%");
        }
        catch (Exception) {
            Console.WriteLine("aaa");
            return new Movie[0];
        }

        var result = selectStmt.ExecuteReader();
        var items = new List<Movie>();
        foreach (var row in result) {
            items.Add(new Movie() {
                Id = Convert.ToInt32(result[0]),
                Name = (string)result[1],
                Duration = Convert.ToInt32(result[2]),
                Genre = (string)result[3],
                Author = (string)result[4],
                ReleasedAt = DateTime.ParseExact((string)result[5], "yyyy-MM-dd", default)
            });
        }
        return items;
    }

    public override void Dispose()
    {
        try {
            this.con.Close();
        }
        catch (Exception) {
            // ignore
        }
    }
}
