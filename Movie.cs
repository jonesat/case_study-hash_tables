using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using static System.Array;
using static System.Math;
using System.Runtime.Remoting.Messaging;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel.Design;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Numerics;

namespace HashTableExploration
{   
    public class MovieFactory
    {
        private int idCounter = 0;
        private static readonly string[] genres = { "Drama", "Adventure", "Family", "Action", "Scifi", "Comedy", "Animated", "Thriller", "Other" };
        private static readonly string[] classifications = { "General (G)", "Parental Guidance (PG)", "Mature (M15+)", "Mature Accompanied (MA15+)" };

        public MovieFactory() { }
        public Movie CreateMovie(string title, string genre, string classification, int duration)
        {
            Movie film = new Movie(this.idCounter,title,genre,classification,duration);
            this.idCounter++;
            return film;
        }
        public Movie CreateMovieFromUserInput()
        {
            WriteLine("Enter the details for the movie:");       
            string title = RequestUserInput("Title");

            string genre = GetValidOption(genres, "Genre");
            string classification = GetValidOption(classifications, "Classification");

            int duration;
            bool validDuration;

            do
            {
                string durationInput = RequestUserInput("Duration");

                validDuration = int.TryParse(durationInput, out duration);

                if (!validDuration)
                {
                    Console.WriteLine("Invalid input. Please enter an integer value for the duration.");
                }

            } while (!validDuration);

            Movie movie = CreateMovie(title, genre, classification, duration);
            return movie;
        }
        public string RequestUserInput(string inputName)
        {
            string userInput;
            bool confirmed = false;

            do
            {
                Write($"{inputName}: ");
                userInput = ReadLine();

                Console.Write("Are you sure (y/n)? ");
                string confirmation = Console.ReadLine();

                if (confirmation.Equals("y", StringComparison.OrdinalIgnoreCase))
                {
                    confirmed = true;
                }

            } while (!confirmed);

            return userInput;
        }

        private string GetValidOption(string[] options, string inputName)
        {
            int optionIndex;
            bool validOption = false;
            do
            {
                WriteLine($"Select the {inputName} by entering the corresponding number:");
                for (int i = 0; i < options.Length; i++)
                {
                    WriteLine($"{i + 1}. {options[i]}");
                }

                string userInput = RequestUserInput(inputName);

                if (int.TryParse(userInput, out optionIndex) && optionIndex >= 1 && optionIndex <= options.Length)
                {
                    validOption = true;
                }
                else
                {
                    WriteLine($"Invalid {inputName}. Please select a valid option by entering the corresponding number.");
                }

            } while (!validOption);

            return options[optionIndex - 1];
        }
    }

    public class Movie: IComparable<Movie>
    {
        private int id;
        private string title;
        private string genre;
        private string classification;
        private int duration;
        private int rentedCount;
        private bool withdrawn;

        public Movie(int id, string title, string genre, string classification, int duration)
        {
            //Automatically Assigned
            this.id = id;
            
            //User Assigned
            this.title = title;
            this.genre = genre;
            this.classification = classification;
            this.duration = duration;

            // State Assigned
            this.rentedCount = 0;
            this.withdrawn = false;
            
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string Genre
        {
            get { return genre; }
            set { genre = value; }
        }
        public string Classification
        {
            get { return classification; }
            set { classification = value; }
        }       
        public int Duration
        {
            get { return duration; }
            set { duration = value; }
        }
        public bool Withdrawn
        {
            get { return withdrawn; }
              
        }
        public int Id
        {
            get { return id; }
        }
        public int RentedCount
        {
            get { return rentedCount; }
            set { rentedCount = value; }
        }
        public void IncrementRented()
        {
            this.rentedCount++;
        }
        public void DecreaseRented()
        {
            this.rentedCount--;
        }
        public void RentMovie()
        {
            this.withdrawn = true;
            this.IncrementRented();
        }
        public void ReturnMovie()
        {
            this.withdrawn = false;
        }
        public int CompareTo(Movie other)
        {   
            int result = this.Title.CompareTo(other.Title);            
            return result;            
        }
        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Movie other = (Movie)obj;
            return string.Equals(Title, other.Title);
        }
        public override int GetHashCode()
        {
            return Title.GetHashCode();
        }
        public static bool operator <(Movie movie1, Movie movie2)
        {
            return string.Compare(movie1.Title, movie2.Title) < 0;
        }
        public static bool operator <=(Movie movie1, Movie movie2)
        {            
            return string.Compare(movie1.Title, movie2.Title) <= 0;
        }
        public static bool operator ==(Movie movie1, Movie movie2)
        {
            if (ReferenceEquals(movie1, movie2))
                return true;
            if (movie1 is null || movie2 is null)
                return false;

            return string.Equals(movie1.Title, movie2.Title);
        }
        public static bool operator !=(Movie movie1, Movie movie2)
        {
            return !(movie1 == movie2);
        }
        public static bool operator >=(Movie movie1, Movie movie2)
        {
            return string.Compare(movie1.Title, movie2.Title) >= 0;
        }
        public static bool operator >(Movie movie1, Movie movie2)
        {
            return string.Compare(movie1.Title, movie2.Title) > 0;
        }
        public override string ToString()
        {
            string message = $"\t{this.title} - Movie Collection #{this.id}\n\t- Genre: {this.genre}\n\t- Classification: {this.classification}\n\t- Duration: {this.duration}\n\t- Currently Rented: {this.withdrawn}\n\t- Rented {this.rentedCount} times";
            return message;
        }
    }
}
