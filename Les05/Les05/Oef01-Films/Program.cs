namespace Oef01_Films
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Do you want to [list] all movies or [add] a new one?");
            string keuze = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();

            if (keuze == "list")
                ToonFilms();
            else if (keuze == "add")
                VoegFilmToe();
            else
                Console.WriteLine("Kies 'list' of 'add'.");

            Console.ReadKey();
        }

        static void ToonFilms()
        {
            List<MovieMapper> movies = MovieMapper.GetMovies();

            foreach(var movie in movies)
                Console.WriteLine(movie.ToString());
        }

        static void VoegFilmToe()
        {            
            Console.Write("Which movie did you watch?");
            string name = Console.ReadLine() ?? "";

            Console.Write("How much would you rate this movie out of 5?");
            if (!int.TryParse(Console.ReadLine(), out int rating) || rating < 0 || rating > 5)
            {
                Console.WriteLine("Ongeldige score (0–5).");
                return;
            }

            try
            {
                var newMovie = new MovieMapper(name, rating);
                newMovie.CreateMovie();
                Console.WriteLine("Your movie was saved.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout: {ex.Message}");
            }
        }
    }
}
