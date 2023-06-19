using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;

namespace Bookshelf.Models.Repositories
{
    public class BookRepository
    {
        private readonly NpgsqlConnection _connection;

        
        public BookRepository(string connectionString)
        {
            _connection = new NpgsqlConnection(connectionString);
        }
        public List<Book> GetAll()
        {
            _connection.Open();
            var adapter = new NpgsqlDataAdapter("SELECT * FROM books", _connection);

            var dataSet = new DataSet();
            adapter.Fill(dataSet);
            var table = dataSet.Tables[0];

            var queryResult = new List<Book>();
            foreach (DataRow row in table.Rows)
            {
                queryResult.Add(new Book
                {
                    Id = (int)row["id"],
                    Title = (string)row["title"],
                    Author = (string)row["author"],
                    PublishedDate = (DateTime)row["published_date"]
                });
            }

            _connection.Close();

            return queryResult;
        }
        public Book GetById(int id)
        {
            _connection.Open();
            var adapter = new NpgsqlDataAdapter("SELECT * FROM books WHERE id = :id", _connection);
            adapter.SelectCommand?.Parameters.AddWithValue(":id", id);

            var dataSet = new DataSet();
            adapter.Fill(dataSet);
            var table = dataSet.Tables[0];

            if (table.Rows.Count > 0)
            {
                DataRow row = table.Rows[0];
                return new Book
                {
                    Id = (int)row["id"],
                    Title = (string)row["title"],
                    Author = (string)row["author"],
                    PublishedDate = (DateTime)row["published_date"]
                };
            }

            _connection.Close();

            return null;
        }
        public int Create(Book book)
        {
            _connection.Open();
            var adapter = new NpgsqlDataAdapter(
                "INSERT INTO books (title, author, published_date) VALUES (:title, :author, :published_date) RETURNING id",
                _connection
            );
            adapter.SelectCommand?.Parameters.AddWithValue(":title", book.Title);
            adapter.SelectCommand?.Parameters.AddWithValue(":author", book.Author);
            adapter.SelectCommand?.Parameters.AddWithValue(":published_date", book.PublishedDate);

            var lastInsertId = (int)adapter.SelectCommand?.ExecuteScalar();
            _connection.Close();

            return lastInsertId;
        }
        public void Update(Book book)
        {
            _connection.Open();
            var adapter = new NpgsqlDataAdapter(
                "UPDATE books SET title = :title, author = :author, published_date = :published_date WHERE id = :id",
                _connection
            );
            adapter.SelectCommand?.Parameters.AddWithValue(":title", book.Title);
            adapter.SelectCommand?.Parameters.AddWithValue(":author", book.Author);
            adapter.SelectCommand?.Parameters.AddWithValue(":published_date", book.PublishedDate);
            adapter.SelectCommand?.Parameters.AddWithValue(":id", book.Id);

            adapter.SelectCommand?.ExecuteNonQuery();
            _connection.Close();
        }

        public void Delete(int id)
        {
            _connection.Open();
            var adapter = new NpgsqlDataAdapter(
                "DELETE FROM books WHERE id = :id",
                _connection
            );
            adapter.SelectCommand?.Parameters.AddWithValue(":id", id);

            adapter.SelectCommand?.ExecuteNonQuery();
            _connection.Close();
        }
    }
}