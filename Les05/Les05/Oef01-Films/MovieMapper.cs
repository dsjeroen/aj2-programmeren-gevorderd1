using Microsoft.Data.SqlClient;
using System.Data;

namespace Oef01_Films
{
    internal class MovieMapper
    {
        private const string ConnStr = @"Data Source=.\SQLEXPRESS;Initial Catalog=OefeningenLes5;Integrated Security=True;TrustServerCertificate=True;";
        private const string TableName = "dbo.Movies";

        private string _name = string.Empty;
        public string Name 
        { 
            get => _name;
            set
            {
                if(string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Naam mag niet leeg zijn of enkel uit spaties bestaan.", nameof(Name));

                _name = value.Trim();
            }
        }

        private int _rating;
        public int Rating
        {
            get => _rating;
            set
            {
                if(value <0 || value > 5)
                    throw new ArgumentOutOfRangeException(nameof(Rating), value, "Rating moet tussen 0 en 5 liggen.");

                _rating = value;
            }
        }


        public MovieMapper(string name, int rating)
        {
            Name = name;
            Rating = rating;            
        }

        public static List<MovieMapper> GetMovies(int? rating)
        {
            var movieList = new List<MovieMapper>();

            var sb = new System.Text.StringBuilder($"SELECT Name, Rating FROM {TableName} ");
            bool filter = rating.HasValue && rating.Value != 0;
            if (filter)
                sb.Append("WHERE Rating = @rating ");
            sb.Append("ORDER BY Name;");

            try
            {
                using var connection = new SqlConnection(ConnStr);
                connection.Open();

                using var cmd = new SqlCommand(sb.ToString(), connection); 
                if (filter)
                    cmd.Parameters.Add("@rating", SqlDbType.TinyInt).Value = rating!.Value; // <-- parameter zetten

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var name = (string)reader["Name"];
                    var r = Convert.ToInt32(reader["Rating"]);   
                    movieList.Add(new MovieMapper(name, r));
                }

                return movieList;
            }
            catch (SqlException ex)
            {
                throw new DataException("Fout bij het ophalen van de films uit de database.", ex);
            }
        }

        public void CreateMovie()
        {
            var sql = $@"
                INSERT INTO {TableName} (Name, Rating, CreatedAt)
                VALUES (@name, @rating, @createdAt);";  // <-- kolommen + params matchen

            try
            {
                using var connection = new SqlConnection(ConnStr);
                connection.Open();

                using var cmd = new SqlCommand(sql, connection);
                cmd.Parameters.Add("@name", SqlDbType.NVarChar, 200).Value = _name;
                cmd.Parameters.Add("@rating", SqlDbType.TinyInt).Value = _rating;
                cmd.Parameters.Add("@createdAt", SqlDbType.DateTime2).Value = DateTime.Now;

                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new DataException("Fout bij het aanmaken van de film in de database.", ex);
            }
        }


        public override string ToString() => $"{Name} ({Rating}/5)";
    }
}
