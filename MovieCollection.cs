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
using System.ComponentModel;
using System.Numerics;

namespace HashTableExploration
{
    public class MovieCollection
    {
        private HashFunctionFactory factory;
        private MovieFactory movieFactory;
        private HashTable container;
        public MovieCollection(int hashTableLength)
        {
        
            // Uses the nearest prime below input hashTableLength as the hashTable Size.
            // Manually specify the kind of hash function the hash table will use, can bring the factory outside later.
            this.factory = new HashFunctionFactory();
            this.container = new HashTable(hashTableLength);
            int digitGroupSize = 3;
            HashFunction<string, int> f1 = this.factory.CreateFoldingHash(digitGroupSize, false, container.NoSlots);
            this.container.SetHashFunction(f1);

            //Sets a predetermined kind of hashFunction
            this.container.SetDoubleProbing(factory);

        }
        public HashTable Container
        {
            get { return container; }

        }
        public MovieFactory MovieFactory { 
            get { return movieFactory; } 
            set { movieFactory = value; }
        }
        public void Add(Movie movie)
        {
            this.container.Insert(new KeyValuePair(movie));
        }
        public Movie RentMovie(string title)
        {
            // Removes a movie from the hash table and increments its frequency.            
            Movie film = this.container.Withdraw(title);
            if (film == null)
            {
                WriteLine($"The film {title} was searched for but not found instead we found {film.Title}");
            }
            return film;


        }
        public void ReturnMovie(Movie movie,bool duplicate)
        {
            this.container.Return(movie,duplicate);
        }
        public void DuplicateMovie()
        {
            SearchResult userInput = UserInputMovie();
            if (userInput.Index != -1)
            {                
                Movie movieToDuplicate = RentMovie(userInput.Input);
                if (movieToDuplicate != null)
                {                    
                    Movie duplicateMovie = movieFactory.CreateMovie(movieToDuplicate.Title, movieToDuplicate.Genre, movieToDuplicate.Classification, movieToDuplicate.Duration);
                    Add(duplicateMovie);                    
                    ReturnMovie(movieToDuplicate, true);

                    WriteLine($"An additional copy of '{movieToDuplicate.Title}' was added to the DVD collection successfully!");
                }
                else
                {
                    WriteLine($"Movie '{userInput.Input}' cannot be found.");
                }
            }
            ReadKey();
        }
        public SearchResult UserInputMovie()
        {
            string input;
            const string SENTINEL = "0";
            int index;
            do
            {
                WriteLine("Please enter the name of a Movie:");
                Write(">>");
                input = ReadLine();
                if (input != SENTINEL && !String.IsNullOrEmpty(input.Trim()))
                {
                    index = this.container.Search(input.Trim());                    
                    if (index == -1)
                    {
                        WriteLine($"The movie {input} cannot be found, please try again.");
                    }
                }
                else
                {
                    index = 0;
                }

            } while ((String.IsNullOrEmpty(input.Trim()) && input != SENTINEL) || (index == -1));


            if (index != -1 && input != SENTINEL)
            {
                SearchResult output = new SearchResult(index, input.Trim());
                return output;
                
            }
            else if (input == SENTINEL)
            {
                SearchResult output = new SearchResult(-1, input.Trim());
                WriteLine("Thank you for searching...");
                ReadKey();
                return output;
            }
            else
            {
                SearchResult output = new SearchResult(-1, input.Trim());
                return output;
            }
        }
        public void ViewMovieDetails()
        {
            SearchResult userInput = UserInputMovie();
            this.container.ViewMovie(userInput.Index);
            ReadKey();
            
        }
        public void RemoveMovie()
        {
            // Finds the movie with the same title as input and deletes it from the hash table
            SearchResult userInput = UserInputMovie();
            this.container.Delete(new KeyValuePair(userInput.Input));
    
        }
        public void Browse()
        {            
            foreach (string movie in this.container.IterateHashTable())
            {
                WriteLine(movie);
            }
        }
        public void Top3()
        {
            foreach (string movie in this.container.IterateRentedMovies())
            {
                WriteLine(movie);
            }
        }

    }
}
