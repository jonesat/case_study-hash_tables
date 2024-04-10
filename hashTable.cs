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
using System.Numerics;
using System.CodeDom.Compiler;
using System.Diagnostics;

namespace HashTableExploration
{
    public class HashTable
    {
        private int noOccupied;
        private int noSlots;
        private KeyValuePair[] hashTable;
        private HashFunction<string, int> hashFunction;

        private Func<int, string, int> ProbingFunction;

        private const string EMPTY = "EMPTY";
        private const string DELETED = "DELETED";

        public HashTable()
        {

        }
        public HashTable(int noSlots)
        {
            if (noSlots > 0)
            {
                this.noSlots = this.NearestPrime(noSlots);
            }
            noOccupied = 0;
            this.hashTable = new KeyValuePair[noSlots];
            for (int i = 0; i < this.hashTable.Length; i++)
            {
                this.hashTable[i] = new KeyValuePair(EMPTY);
            }
        }
        public HashTable(int noSlots, HashFunction<string, int> hashFunction)
        {
            if (noSlots > 0)
            {
                this.noSlots = this.NearestPrime(noSlots);
            }
            noOccupied = 0;
            this.hashTable = new KeyValuePair[noSlots];
            for (int i = 0; i < this.hashTable.Length; i++)
            {
                this.hashTable[i] = new KeyValuePair(EMPTY);
            }

            this.hashFunction = hashFunction;
        }
        public int NoOccupied
        {
            get { return noOccupied; }
        }
        public int NoSlots
        {
            get { return noSlots; }
            set { noSlots = NoSlots; }
        }
        public int FindNextEmptyIndex(string key)
        {
            // Returns the position of the key in the Hash Table or -1 if not present.
            int index = Hash(key);
            int i = 0;
            int offset = 0;
            while ((i < this.noSlots) && (hashTable[(index + offset) % this.noSlots].Key != key) && (hashTable[(index + offset) % this.noSlots].Key != EMPTY))
            {
                offset = this.ProbingFunction(i, key);
                i++;
            }
            return (index + offset) % this.noSlots;
        }
        public int Search(string key)
        {
            // Returns the position of the key in the Hash Table or -1 if not present.
            int index = Hash(key);
            int i = 0;
            int offset = 0;

            bool capacityFlag = (i < this.noSlots);
            bool uniquenessFlag = (hashTable[(index + offset) % this.noSlots].Key != key);
            bool nonEmptyFlag = (hashTable[(index + offset) % this.noSlots].Key != EMPTY);
            while (capacityFlag && uniquenessFlag && nonEmptyFlag)
            {
                offset += this.ProbingFunction(i, key);
                i++;
                capacityFlag = (i < this.noSlots);
                uniquenessFlag = (hashTable[(index + offset) % this.noSlots].Key != key);
                nonEmptyFlag = (hashTable[(index + offset) % this.noSlots].Key != EMPTY);


            }
            if (hashTable[(index + offset) % this.noSlots].Key == key)
            {
                return (index + offset) % this.noSlots;
            }
            else
            {
                
                return -1;
            }

        }
        public void Delete(KeyValuePair film)
        {
            WriteLine($"Deleting Key: {film.Key}");
            int index = Search(film.Key);
            if (index != -1)
            {
                this.hashTable[index] = new KeyValuePair(DELETED);
                this.noOccupied--;
            }
            else
            {
                Write("");
                //WriteLine($"The key {film.Key} can not be found in the hash table to delete it");
            }
        }
        public void Insert(KeyValuePair film)
        {
            

            int index = Hash(film.Key);
            int i = 0;
            int offset = 0;

            bool capacityFlag = (i < this.noSlots);
            bool nonDeletedFlag = (hashTable[(index + offset) % this.noSlots].Key != DELETED);
            bool nonEmptyFlag = (hashTable[(index + offset) % this.noSlots].Key != EMPTY);
            
            // This loop looks for the next empty spot in the probing sequence to insert a key.
            while (capacityFlag && nonDeletedFlag && nonEmptyFlag)
            {
                offset += this.ProbingFunction(i, film.Key);
                i++;
                capacityFlag = (i < this.noSlots);
                nonDeletedFlag = (hashTable[(index + offset) % this.noSlots].Key != DELETED);
                nonEmptyFlag = (hashTable[(index + offset) % this.noSlots].Key != EMPTY);               

            }            
            if (hashTable[(index + offset) % this.noSlots].Key == EMPTY || hashTable[(index + offset) % this.noSlots].Key == DELETED)
            {
                hashTable[(index + offset) % this.noSlots] = film;
            }
            else
            {
                WriteLine($"Did not insert {film.Key} in slot {(index + offset) % this.noSlots}");
            }

        }
        public void Clear()
        {
            this.noOccupied = 0;
            for (int i = 0; i < this.noSlots; i++)
            {
                this.hashTable[i].Key = EMPTY;
            }
            this.hashFunction = null;
        }
        public void Print()
        {
            foreach (var item in hashTable)
            {
                if ((item.Key == EMPTY) || (item.Key == DELETED))
                {
                    Write(" _ ");
                }
                else
                {
                    Write($" {item.Key.ToString()} ");
                }

            }
            WriteLine();
        }
        public void SetHashFunction(HashFunction<string, int> function)
        {
            this.hashFunction = function;
        }
        public void SetLinearProbing()
        {
            Func<int, string, int> function = (i, key) =>
            {
                return i + 1;
            };
            this.ProbingFunction = function;
        }
        public void SetQuadraticProbing()
        {
            Func<int, string, int> function = (i, key) =>
            {
                i++;
                return i * i;
            };
            this.ProbingFunction = function;
        }
        public void SetDoubleProbing(HashFunctionFactory factory)
        {
            HashFunction<string, int> hasher = factory.CreateDivisionHash(this.noSlots);
            Func<int, string, int> function = (i, key) =>
            {
                return this.NoSlots - hasher.Hash(key);
            };
            this.ProbingFunction = function;
        }
        public int NearestPrime(int n)
        {
            // This method has O(n log n) time complexity as opposed to a bruteforce method that takes O(n^2).
            bool[] isPrime = new bool[n + 1];
            int largestPrime = 0;

            // Initialize all elements as true
            for (int i = 2; i <= n; i++)
            {
                isPrime[i] = true;
            }

            // Sieve of Eratosthenes
            for (int p = 2; p * p <= n; p++)
            {
                if (isPrime[p])
                {
                    largestPrime = p;  // Update the largest prime found so far

                    // Mark all multiples of p as false
                    for (int i = p * p; i <= n; i += p)
                    {
                        isPrime[i] = false;
                    }
                }
            }

            // Check the remaining numbers after the loop
            for (int i = Math.Max(2, largestPrime + 1); i <= n; i++)
            {
                if (isPrime[i])
                {
                    largestPrime = i;
                }
            }

            return largestPrime;
        }
        public void SetSeparateChaining(int combineChaining)
        {
            // Not implemented.
        }
        private int Hash(string key)
        {

            int output = this.hashFunction.Hash(key);
            //WriteLine($"Key {key} hashes to: {output}");
            return output;
        }
        public Movie Withdraw(string key)
        {
            int index = Hash(key);
            int i = 0;
            int offset = 0;

            bool capacityFlag = (i < this.noSlots);
            bool notMatchingFlag = (hashTable[(index + offset) % this.noSlots].Key != key);
            bool nonEmptyFlag = (hashTable[(index + offset) % this.noSlots].Key != EMPTY);
            bool deletedFlag = (hashTable[(index + offset) % this.noSlots].Key != DELETED);                        
            bool withdrawnFlag = (hashTable[(index + offset) % this.noSlots].Movie.Withdrawn);

            while (capacityFlag && ((notMatchingFlag && (nonEmptyFlag|| deletedFlag))|| (!notMatchingFlag && withdrawnFlag)))
            {
                offset += this.ProbingFunction(i, key);
                i++;
                capacityFlag = (i < this.noSlots);
                notMatchingFlag = (hashTable[(index + offset) % this.noSlots].Key != key);
                nonEmptyFlag = (hashTable[(index + offset) % this.noSlots].Key != EMPTY);
                deletedFlag = (hashTable[(index + offset) % this.noSlots].Key != DELETED);
                withdrawnFlag = (hashTable[(index + offset) % this.noSlots].Movie.Withdrawn);


            }
            if (hashTable[(index + offset) % this.noSlots].Key == key & !withdrawnFlag)
            {
                hashTable[(index + offset) % this.noSlots].Movie.RentMovie();
                KeyValuePair item = hashTable[(index + offset) % this.noSlots];
                //WriteLine($"The movie {hashTable[(index + offset) % this.noSlots].Movie.Title} was withdrawn");
                return item.Movie;                
            }
            else
            {
                WriteLine($"For input string {key} we found hashTable value: {hashTable[(index + offset) % this.noSlots].Key}");
                return null;
            }            
        }
        public void Return(Movie movie, bool duplicate)
        {
            string key = movie.Title;
            int index = Hash(key);
            int i = 0;
            int offset = 0;

            bool capacityFlag = (i < this.noSlots);
            bool uniquenessFlag = (hashTable[(index + offset) % this.noSlots].Movie.Id != movie.Id);
            bool nonEmptyFlag = (hashTable[(index + offset) % this.noSlots].Key != EMPTY);            
            
            while (capacityFlag && uniquenessFlag && nonEmptyFlag)
            {
                offset += this.ProbingFunction(i, key);
                i++;
                capacityFlag = (i < this.noSlots);
                uniquenessFlag = (hashTable[(index + offset) % this.noSlots].Key != key);
                nonEmptyFlag = (hashTable[(index + offset) % this.noSlots].Key != EMPTY);
            }
            if (hashTable[(index + offset) % this.noSlots].Key == key && !uniquenessFlag)
            {
                hashTable[(index + offset) % this.noSlots].Movie.ReturnMovie();
                //WriteLine(hashTable[(index + offset) % this.noSlots].Movie.ToString());
                //WriteLine($"The movie {hashTable[(index + offset) % this.noSlots].Movie.Title} was returned");

                if (duplicate)
                {
                    hashTable[(index + offset) % this.noSlots].Movie.DecreaseRented(); ;
                }
            }

        }
        public void ViewMovie(int index)
        {
            if (index >= 0 && index < noSlots)
            {
                WriteLine(hashTable[index].Movie.ToString());
            }
        }
        public static KeyValuePair[] Merge(KeyValuePair[] left, KeyValuePair[] right,string mode)
        {
            int leftIndex = 0; int rightIndex =0; int mergedIndex = 0; 
            int mergedLength = left.Length+right.Length;
            KeyValuePair[] temp = new KeyValuePair[mergedLength];

            Func<int, int, bool> condition;
            if (mode == "Count")
            {
                condition = (x, y) =>left[x].Movie.RentedCount <= right[y].Movie.RentedCount;
            }
            else
            {
                condition = (x, y) =>
                {
                    //WriteLine($"We are comparing {left[x].Movie.Title} to {right[y].Movie.Title}");
                    //WriteLine($"Movie[x] <= Movie[y]: {left[x].Movie<=right[y].Movie}");
                    return left[x].Movie <= right[y].Movie;
                };
            }

            while (leftIndex < left.Length && rightIndex < right.Length)
            {
                //if (pairs[p].Movie <= pairs[q].Movie)
                
                if (condition(leftIndex,rightIndex))
                {
                    temp[mergedIndex] = left[leftIndex];
                    leftIndex++;
                }
                else
                {
                    temp[mergedIndex] = right[rightIndex];
                    rightIndex++;
                }
                mergedIndex++;
            }
            while(leftIndex < left.Length)
            {
                temp[mergedIndex] = left[leftIndex];
                leftIndex++;
                mergedIndex++;
            }
            while(rightIndex < right.Length)
            {
                //WriteLine($"At remainder of mid:end left Index = {leftIndex}, right Index = {rightIndex} mergedIndex = {mergedIndex} the length of the temporary array is {temp.Length}");
                temp[mergedIndex] = right[rightIndex];
                rightIndex++;
                mergedIndex++;
            }
            return temp;
        }
        public KeyValuePair[] MergeSort(KeyValuePair[] pairs,string mode)
        {
            if (pairs.Length <= 1)
            {
                return pairs;
            }

            int middle = (pairs.Length) / 2;
            KeyValuePair[] left = new KeyValuePair[middle];
            KeyValuePair[] right = new KeyValuePair[pairs.Length-middle];

            Array.Copy(pairs, 0, left, 0, left.Length);
            Array.Copy(pairs, middle, right, 0, right.Length);

            left = MergeSort(left,mode);
            right = MergeSort(right,mode);
            return Merge(left,right,mode);
        }
        private KeyValuePair[] AlphabeticalSort()
        {
            KeyValuePair[] deepCopy = hashTable.Where(pair => pair.Movie != null).ToArray();
            string mode = "Title";
            deepCopy = MergeSort(deepCopy,mode);
            return deepCopy;
        }
        public IEnumerable<String> IterateHashTable()
        {
            int count = 0;
            KeyValuePair[] sortedTable = AlphabeticalSort();

            foreach (var item in sortedTable)
            {
                if (count < 3)
                {
                    if (item.Movie != null && ((item.Key != EMPTY) || (item.Key != DELETED)))
                    {
                        yield return item.Movie.ToString();
                        count++;
                    }
                }
                else
                {
                    WriteLine("Press any key to continue");
                    ReadKey();
                    count = 0;
                }
            }
        }
        private KeyValuePair[] CountSort()
        {
            MovieFactory factory = new MovieFactory();
            KeyValuePair[] deepCopy = hashTable
                .Where(pair => pair.Movie != null)
                .GroupBy(pair => pair.Key)
                .Select(group =>
                {
                    Movie movie = factory.CreateMovie(group.Key, group.First().Movie.Genre, group.First().Movie.Classification, group.First().Movie.Duration);
                    movie.RentedCount = group.Sum(pair => pair.Movie.RentedCount);
                    return new KeyValuePair(movie);

                }).ToArray();
            string mode = "Count";
            deepCopy = MergeSort(deepCopy,mode);
            return deepCopy;
                    
        }
        public IEnumerable<String> IterateRentedMovies()
        {
            int count = 0;
            KeyValuePair[] sortedTable = CountSort();

            for(int i = sortedTable.Length-1;i>=sortedTable.Length-3;i--)
            {
                KeyValuePair item = sortedTable[i];
                if (item.Movie != null && ((item.Key != EMPTY) || (item.Key != DELETED)))
                {
                    yield return item.Movie.ToString();
                    count++;
                }
                else
                {
                    WriteLine("Press any key to continue");
                    ReadKey();
                }
            }
            
        }
    }
}
