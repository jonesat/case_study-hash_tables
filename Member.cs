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
using System.Security.Policy;

namespace HashTableExploration
{
    public class Member : IComparable<Member>
    {
        private string firstName;
        private string lastName;
        private string phoneNumber;
        private string userName;
        private string password;
        private string memberType;
        private const int MAX_RENTABLE = 5;
        private Movie[] rentedMovies = new Movie[MAX_RENTABLE];

        // Constructor
        public Member(string firstName, string lastName, string phoneNumber)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.phoneNumber = phoneNumber;
        }

        // Properties
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public string MemberType
        {
            get { return memberType; }  
        }

        public Movie[] RentedMovies
        {
            get { return this.rentedMovies; }
            set { rentedMovies = value; }
        }
        public void SetCredentials(string userName, string password,string memberType)
        {
            this.userName = userName;
            this.password = password;
            this.memberType = memberType;
        }

        public bool AddMovie(Movie movie)
        {
            // Check if the movie is already rented by the member
            if (IsRented(movie))
            {
                WriteLine("You have already rented this movie.");
                return false;
            }

            // Check if the member has reached the maximum number of rented movies
            if (IsRentLimitReached())
            {
                WriteLine("You have reached the maximum limit of rented movies.");
                return false;
            }

            // Find the first available slot in the rentedMovies array
            for (int i = 0; i < rentedMovies.Length; i++)
            {
                if (rentedMovies[i] == null)
                {
                    rentedMovies[i] = movie;
                    Console.WriteLine("Movie added to your rental list.");
                    return true;
                }
            }

            return false; 
        }
        public bool RemoveMovie(Movie movie)
        {
            // Remove a movie from the member's rentedMovies array
            // Find the movie in the rentedMovies array
            for (int i = 0; i < rentedMovies.Length; i++)
            {
                if (rentedMovies[i] == movie)
                {
                    rentedMovies[i] = null;                    
                    return true;
                }
            }

            WriteLine("Movie not found in your rental history.");
            return false;
        }
        public Movie RemoveMovie()
        {
            // Check if the member's rentedMovies array is empty
            if (rentedMovies.Count(movie => movie != null) == 0)
            {
                WriteLine("Your rental list is empty.");
                return null;
            }

            string movieTitle;
            Movie movieToRemove = null;
            bool movieFound = false;
            const string SENTINEL = "0";
            do
            {
                WriteLine("Enter the the name of the movie you wish to return (or 0 to quit):");
                movieTitle = ReadLine();

                if (movieTitle.Equals(SENTINEL))
                {
                    return null;
                }

                foreach (Movie movie in rentedMovies)
                {
                    if (movie != null && movie.Title == movieTitle)
                    {
                        movieToRemove = movie;
                        movieFound = true;
                        break;
                    }
                }

                if (!movieFound)
                {
                    WriteLine("You have not rented that movie. Please try another.");
                }
            } while (!movieFound);

            // Remove the movie from the rentedMovies array
            for (int i = 0; i < rentedMovies.Length; i++)
            {
                if (rentedMovies[i] == movieToRemove)
                {
                    rentedMovies[i] = null;
                    WriteLine($"Movie: {movieToRemove.Title} removed from your rental list.");
                    return movieToRemove;
                }
            }

            return null; // Should not reach this point, but added for completeness
        }
        private bool IsRented(Movie movie)
        {
            // Check if the movie is already rented by the member
            foreach (Movie rentedMovie in rentedMovies)
            {
                if (rentedMovie == movie)
                {
                    return true;
                }
            }
            return false;
        }
        private bool IsRentLimitReached()
        {
            // Check if the member has reached the maximum number of rented movies
            int rentedCount = rentedMovies.Count(movie => movie != null);
            return rentedCount >= MAX_RENTABLE;
        }
        public void PrintCollection()
        {
            WriteLine($"Hello {firstName} {lastName} you are currently renting the following movies:");
            foreach (Movie movie in rentedMovies)
            {
                if (movie != null)
                {
                    WriteLine(movie.ToString());
                }
            }
        }
        public int CompareTo(Member other)
        {
            // Compare the current member to another member based on last name and then first name
            int result = this.lastName.CompareTo(other.LastName);
            if (result == 0)
            {
                result = this.firstName.CompareTo(other.FirstName);
            }
            return result;
        }
        public override string ToString()
        {
            return $"{userName} is known as {firstName} {lastName} and has phone number: {phoneNumber}";
        }
    }
}
