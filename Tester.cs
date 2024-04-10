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
using System.ComponentModel.Design;
using System.Numerics;

namespace HashTableExploration
{
    class Tester
    {
        private string className;

        public Tester(string className)
        {
            this.className = className;
        }
        public MovieCollection SeedCollection()
        {
            int hashTableLength = 1000;
            MovieCollection movies = new MovieCollection(hashTableLength);
            string[] insertables = new string[]{
                    // Action Movies
                    "The Avengers",
                    "Die Hard",
                    "Mission: Impossible",
                    "Fast and Furious",
                    "The Dark Knight",
                    "Mad Max: Fury Road",
                    "John Wick",
                    "Terminator",
                    "Indiana Jones",
                    "The Matrix",
                    // Adventure Movies
                    "Jurassic Park",
                    "Pirates of the Caribbean",
                    "The Lord of the Rings",
                    "Harry Potter",
                    "Avatar",
                    "Star Wars",
                    "Jumanji",
                    "Indiana Jones",
                    "National Treasure",
                    "Back to the Future",
                    // Comedy Movies
                    "Anchorman",
                    "Superbad",
                    "Bridesmaids",
                    "Dumb and Dumber",
                    "The Hangover",
                    "Mean Girls",
                    "Pitch Perfect",
                    "Napoleon Dynamite",
                    "Step Brothers",
                    "Ted",
                    // Drama Movies
                    "The Shawshank Redemption",
                    "Fight Club",
                    "The Godfather",
                    "Schindler's List",
                    "Forrest Gump",
                    "The Social Network",
                    "Good Will Hunting",
                    "American Beauty",
                    "Pulp Fiction",
                    "A Beautiful Mind",
                    // Sci-Fi Movies
                    "Blade Runner",
                    "Inception",
                    "Interstellar",
                    "The Matrix",
                    "Star Wars",
                    "Star Wars",
                    "Star Wars",
                    "Avatar",
                    "The Terminator",
                    "E.T. the Extra-Terrestrial",
                    "Back to the Future",
                    "The Fifth Element",
                    // Animated Movies
                    "Toy Story",
                    "Frozen",
                    "Finding Nemo",
                    "Shrek",
                    "The Lion King",
                    "The Incredibles",
                    "Up",
                    "Cars",
                    "Moana",
                    "Coco"
                    // Add more movie names as needed...
             };

            MovieFactory filmFactory = new MovieFactory();
            for (int i = 0; i < insertables.Length; i++)
            {
                movies.Add(filmFactory.CreateMovie(insertables[i].Trim(), "Other", "General (G)", 120));
            }
            return movies;
        }

        public void TestLinkedList()
        {
            WriteLine($"Creating the linked list");
            LinkedList<int> linkedList = new LinkedList<int>(10);

            WriteLine($"This is the linked list after creation: {linkedList.ToString()}");

            WriteLine($"Now we attempt to insert 20");
            linkedList.Insert(20);
            WriteLine($"Insert Successful");

            WriteLine($"Now we attempt to insert 30");
            linkedList.Insert(30);
            WriteLine($"Insert Successful");

            WriteLine($"Now we attempt to insert 40");
            linkedList.Insert(40);
            WriteLine($"Insert Successful");

            foreach (int value in linkedList)
            {
                Console.WriteLine($"The value in the linked list is: {value}");
            }

            WriteLine($"Linked List: {linkedList.ToString()}"); // Expected output: 10 20 30 40

            linkedList.Delete(30);
            WriteLine("Linked List after deleting 30: " + linkedList.ToString()); // Expected output: 10 20 40

            linkedList.Delete(10);
            WriteLine("Linked List after deleting 10: " + linkedList.ToString()); // Expected output: 20 40

            linkedList.Delete(40);
            WriteLine("Linked List after deleting 40: " + linkedList.ToString()); // Expected output: 20

            ReadKey();
        }     
        public void TestHashFunction()
        {
            int tableSize = 7;
            HashFunctionFactory hashMaker = new HashFunctionFactory();

            // Division Hash Function
            string key = "2341";
            int solution = 3;
            HashFunction<string, int> f1 = hashMaker.CreateDivisionHash(tableSize);
            WriteLine($"The hash value of the key: {key} is {f1.Hash(key)} and it matches the solution of {solution}? {solution == f1.Hash(key)}");

            // Middle Square Hash Function
            key = "2134";
            int noDigits = 3;
            solution = 539;
            HashFunction<string, int> f2 = hashMaker.CreateMiddleSquareHash(noDigits);
            WriteLine($"The hash value of the key: {key} is {f2.Hash(key)} and it matches the solution of {solution}? {solution == f2.Hash(key)}");


            // Folding Hash Functions
            key = "189554134678";
            int digitGroupSize = 3;
            int tableLength = 1000;
            bool folding;

            // Folding Hash Function
            folding = false;
            
            solution = 555;
            HashFunction<string, int> f3 = hashMaker.CreateFoldingHash(digitGroupSize, folding,tableLength);
            WriteLine($"The hash value of the key: {key} is {f3.Hash(key)} and it matches the solution of {solution}? {solution == f3.Hash(key)}");

            // No Folding Hash Function
            folding = true;
            solution = 654;
            HashFunction<string, int> f4 = hashMaker.CreateFoldingHash(digitGroupSize, folding, tableLength);
            WriteLine($"The hash value of the key: {key} is {f4.Hash(key)} and it matches the solution of {solution}? {solution == f4.Hash(key)}");
            


            // Select Digits Hash Function
            key = "98214587412";
            solution = 519;
            int[] selections = new int[3] { 6, 4, 1 };
            HashFunction<string, int> f5 = hashMaker.CreateSelectDigitsHash(selections);
            WriteLine($"The hash value of the key: {key} is {f5.Hash(key)} and it matches the solution of {solution}? {solution == f5.Hash(key)}");
            ReadKey();

        }
        public void TestTransformString()
        {
            HashFunctionFactory hashMaker = new HashFunctionFactory();
            string input1 = "abc123";
            int transformed1 = (int)hashMaker.TransformString(input1);
            Console.WriteLine(transformed1);  // Output: 123123

            string input2 = "def456";
            int transformed2 = (int)hashMaker.TransformString(input2);
            Console.WriteLine(transformed2);  // Output: 456456

            string input3 = "xyz789";
            int transformed3 = (int)hashMaker.TransformString(input3);
            Console.WriteLine(transformed3);  // Output: 789789

            string input4 = "1a2b3c";
            int transformed4 = (int)hashMaker.TransformString(input4);
            Console.WriteLine(transformed4);  // Output: 123

            string input5 = "!@#$";
            int transformed5 = (int)hashMaker.TransformString(input5);
            Console.WriteLine(transformed5);  // Output: 
            ReadKey();
        }
        public void TestHashTable()
        {
            HashFunctionFactory factory = new HashFunctionFactory();
            int hashTableLength = 100;
            HashFunction<string, int> f1 = factory.CreateFoldingHash(1, false, hashTableLength);
            HashTable h = new HashTable(hashTableLength, f1);
            //h.SetLinearProbing();
            //h.SetQuadraticProbing();
            h.SetDoubleProbing(factory);

            string[] insertables = new string[] { "59", "39", "20", "33", "12", "29", "57" };
            Movie[] movies = new Movie[insertables.Length];

            MovieFactory filmFactory = new MovieFactory();
            for (int i = 0; i < insertables.Length;i++)
            {
                movies[i]=filmFactory.CreateMovie(insertables[i], "Other", "General (G)", 120);
            }

            KeyValuePair[] keyValuePairs = new KeyValuePair[insertables.Length];            
            for(int i = 0; i<insertables.Length;i++)
            {
                keyValuePairs[i] = new KeyValuePair(movies[i]);
            }
            foreach(var pair in keyValuePairs)
            {
                h.Insert(pair);
            }                
            h.Print();
            KeyValuePair delete1 = Array.Find(keyValuePairs, pair => pair.Key == "29");
            h.Delete(delete1);
            KeyValuePair delete2 = Array.Find(keyValuePairs, pair => pair.Key == "39");
            h.Delete(delete2);
            h.Print();
            ReadKey();
        }
        public void TestMovieCollection()
        {

            int hashTableLength = 1000;
            MovieCollection movies = new MovieCollection(hashTableLength);
            //string[] insertables = new string[] { "59", "39", "20", "33", "12", "29", "57" };

            string[] insertables = new string[]{
                    // Action Movies
                    "The Avengers",
                    "Die Hard",
                    "Mission: Impossible",
                    "Fast and Furious",
                    "The Dark Knight",
                    "Mad Max: Fury Road",
                    "John Wick",
                    "Terminator",
                    "Indiana Jones",
                    "The Matrix",
                    // Adventure Movies
                    "Jurassic Park",
                    "Pirates of the Caribbean",
                    "The Lord of the Rings",
                    "Harry Potter",
                    "Avatar",
                    "Star Wars",
                    "Jumanji",
                    "Indiana Jones",
                    "National Treasure",
                    "Back to the Future",
                    // Comedy Movies
                    "Anchorman",
                    "Superbad",
                    "Bridesmaids",
                    "Dumb and Dumber",
                    "The Hangover",
                    "Mean Girls",
                    "Pitch Perfect",
                    "Napoleon Dynamite",
                    "Step Brothers",
                    "Ted",
                    // Drama Movies
                    "The Shawshank Redemption",
                    "Fight Club",
                    "The Godfather",
                    "Schindler's List",
                    "Forrest Gump",
                    "The Social Network",
                    "Good Will Hunting",
                    "American Beauty",
                    "Pulp Fiction",
                    "A Beautiful Mind",
                    // Sci-Fi Movies
                    "Blade Runner",
                    "Inception",
                    "Interstellar",
                    "The Matrix",
                    "Star Wars",
                    "Star Wars",
                    "Star Wars",
                    "Avatar",
                    "The Terminator",
                    "E.T. the Extra-Terrestrial",
                    "Back to the Future",
                    "The Fifth Element",
                    // Animated Movies
                    "Toy Story",
                    "Frozen",
                    "Finding Nemo",
                    "Shrek",
                    "The Lion King",
                    "The Incredibles",
                    "Up",
                    "Cars",
                    "Moana",
                    "Coco"
                    // Add more movie names as needed...
             };



            MovieFactory filmFactory = new MovieFactory();
            for (int i = 0; i < insertables.Length; i++)
            {
                movies.Add(filmFactory.CreateMovie(insertables[i].Trim(), "Other", "General (G)", 120));
            }


            WriteLine("####################   View the whole movie collection   #######################\n");
            movies.Browse();
            WriteLine("\n######################################################\n");



            WriteLine("####################   View the hashtable   #######################\n");
            movies.Container.Print();
            WriteLine("\n######################################################\n");

            WriteLine("####################   View a specific Movie   #######################\n");
            movies.ViewMovieDetails();
            WriteLine("\n######################################################\n");

            // To Test
            // Borrow a movie
            // Return a movie

            SearchResult userInput = movies.UserInputMovie();
            Movie rentedMovie;
            if (userInput.Index != -1)
            {
                // Rent the movie four times.
                rentedMovie = movies.RentMovie(userInput.Input);
                WriteLine($"This movie shows that it is rented: {rentedMovie.Withdrawn} and has been rented {rentedMovie.RentedCount} times");
                movies.ReturnMovie(rentedMovie, false);
                //WriteLine($"This movie shows that it is rented: {rentedMovie.Withdrawn} and has been rented {rentedMovie.RentedCount} times");

                rentedMovie = movies.RentMovie(userInput.Input);
                //WriteLine($"This movie shows that it is rented: {rentedMovie.Withdrawn} and has been rented {rentedMovie.RentedCount} times");
                movies.ReturnMovie(rentedMovie, false);
                //WriteLine($"This movie shows that it is rented: {rentedMovie.Withdrawn} and has been rented {rentedMovie.RentedCount} times");

                rentedMovie = movies.RentMovie(userInput.Input);
                //WriteLine($"This movie shows that it is rented: {rentedMovie.Withdrawn} and has been rented {rentedMovie.RentedCount} times");
                movies.ReturnMovie(rentedMovie, false);
                //WriteLine($"This movie shows that it is rented: {rentedMovie.Withdrawn} and has been rented {rentedMovie.RentedCount} times");

                rentedMovie = movies.RentMovie(userInput.Input);
                //WriteLine($"This movie shows that it is rented: {rentedMovie.Withdrawn} and has been rented {rentedMovie.RentedCount} times");
                movies.ReturnMovie(rentedMovie, false);
                //WriteLine($"This movie shows that it is rented: {rentedMovie.Withdrawn} and has been rented {rentedMovie.RentedCount} times");
            }

            userInput = movies.UserInputMovie();
            if (userInput.Index != -1)
            {
                //Rent the Movie Once
                rentedMovie = movies.RentMovie(userInput.Input);
                movies.ReturnMovie(rentedMovie, false);
                ;
            }

            userInput = movies.UserInputMovie();
            if (userInput.Index != -1)
            {
                // Rent the Movie Twice
                rentedMovie = movies.RentMovie(userInput.Input);
                movies.ReturnMovie(rentedMovie, false);
                rentedMovie = movies.RentMovie(userInput.Input);
                movies.ReturnMovie(rentedMovie, false);

            }

            userInput = movies.UserInputMovie();
            if (userInput.Index != -1)
            {
                // Rent the Movie Three times
                rentedMovie = movies.RentMovie(userInput.Input);
                movies.ReturnMovie(rentedMovie, false);
                rentedMovie = movies.RentMovie(userInput.Input);
                movies.ReturnMovie(rentedMovie, false);
                rentedMovie = movies.RentMovie(userInput.Input);
                movies.ReturnMovie(rentedMovie, false);

            }

            WriteLine("####################   View the top 3 most rented movies  #######################\n");
            movies.Top3();
            WriteLine("\n######################################################\n");


            // Display a users currently withdrawn movies

            // Display the top 3 movies

            ReadKey();
        }
        public void TestWeek6()
        {
            WriteLine("\n\nThis is the hashtable workshop from week 6 - and generalized c# refresher");
            ReadKey();

            // Question 1
            int[] keys = new int[7] { 2341, 4234, 2839, 352, 22, 397, 3920 };
            int HASHTABLELENGTH = 7;
            WriteLine("\n\nThis is the solution to question 1, using the modulo hash function");
            HashQ1(keys, HASHTABLELENGTH);

            // Question 2
            WriteLine("\n\nThis is the solution to question 2, using the middle square method of hashing\nIn this example we are using the middle 3 bits as the index into the hash table for the values.\nAssume the hash table has an index ranging from 0 to 999");
            keys = new int[7] { 1221, 2134, 2254, 2452, 2941, 1000, 1874 };
            HASHTABLELENGTH = 999;
            HashQ2(keys, HASHTABLELENGTH);

            // Question 3 
            WriteLine("\n\nThis is the solution to question 3, this time using the folding method of hashing, again we have a hashtable of size 999");
            long[] longKeys = new long[6] { 189554134678, 98214587412, 567154662, 1546591, 65378456, 982154 };
            HashQ3(longKeys, HASHTABLELENGTH, 3, false);
            HashQ3(longKeys, HASHTABLELENGTH, 3, true);


            // Question 3d 
            WriteLine("\n\nThis is the solution to question 3d, this time using the selecting digits method, again we have a hashtable of size 999");
            int[] selections = new int[3] { 6, 4, 1 };
            HashQ3d(longKeys, HASHTABLELENGTH, selections);
        }
        public static void HashQ1(int[] keys, int tableLength)
        {
            int[] sortedValues = new int[7];
            int hashvalue;
            for (int i = 0; i < keys.Length; i++)
            {
                hashvalue = keys[i] % tableLength;
                sortedValues[hashvalue] = keys[i];
            }

            for (int i = 0; i < sortedValues.Length; i++)
            {
                WriteLine($"The hashtable index is {i} and the key {sortedValues[i]} has been mapped to it by our hash function");
            }
            ReadKey();

        }
        public static void HashQ2(int[] keys, int tableLength)
        {
            int[] hashTable = new int[tableLength];
            int j;
            int index;
            for (int i = 0; i < keys.Length; i++)
            {
                string result = Math.Pow(keys[i], 2).ToString();
                if (result.Length >= 3)
                {

                    j = (int)Math.Ceiling((double)(result.Length - 3) / 2);
                    index = int.Parse(result.Substring(j, 3)); ;
                    hashTable[index] = keys[i];
                }
            }
            for (int i = 0; i < hashTable.Length; i++)
            {
                if (hashTable[i] > 0)
                {
                    WriteLine($"The value of the key in position {i} = {hashTable[i]}. The squared value for reference is: {Math.Pow(hashTable[i], 2)}");
                }
            }
            ReadKey();
        }
        public static void HashQ3(long[] keys, int tableLength, int digitGroupSize, bool folding)
        {
            // Ideas. Parse key to string,
            // take substring, parse to int,
            // add to total loop counter will be i*digitGroupSize to (i-1)*digitGroupSize or something like that
            // and then the remainder of the string is parsed outside the loop
            WriteLine($"This attempt at hashing keys uses folding: {folding}");
            long[] hashTable = new long[tableLength];
            int i;
            int hashIndex;

            foreach (var key in keys)
            {
                i = 0;
                hashIndex = 0;
                string k = key.ToString();
                do
                {
                    if (i % 2 == 0 || !folding)
                    {
                        hashIndex += int.Parse(k.Substring(i * digitGroupSize, digitGroupSize));
                    }
                    else
                    {
                        char[] temp = k.Substring(i * digitGroupSize, digitGroupSize).ToCharArray();
                        Array.Reverse(temp);
                        hashIndex += int.Parse(string.Join("", temp));
                    }
                    i++;


                } while ((i + 1) * digitGroupSize < k.Length);

                if (i % 2 == 0 || !folding)
                {
                    hashIndex += int.Parse(k.Substring(i * digitGroupSize));
                }
                else
                {
                    char[] temp = k.Substring(i * digitGroupSize).ToCharArray();
                    Array.Reverse(temp);
                    hashIndex += int.Parse(string.Join("", temp));
                }
                WriteLine($"For key: {key} we have hash index: {hashIndex}");
            }
            ReadKey();
        }
        public static void HashQ3d(long[] keys, int tableLength, int[] selections)
        {
            long[] hashTable = new long[tableLength];
            int hashKey;
            string k, temp;

            foreach (var key in keys)
            {
                k = key.ToString();
                temp = "";
                foreach (var selection in selections)
                {
                    temp += k[selection - 1];

                }
                hashKey = int.Parse(temp);
                hashTable[hashKey] = key;
            }
            for (int i = 0; i < hashTable.Length; i++)
            {
                if (hashTable[i] > 0)
                {
                    WriteLine($"The value of the key in position {i} = {hashTable[i]}");
                }
            }
            ReadKey();

        }

    }

}
