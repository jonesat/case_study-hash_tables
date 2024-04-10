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

namespace HashTableExploration
{
    interface iHashtable
    {
        int NoOccupied //get the number of elements in the hash table
        {
            get;
        }
        int NoSlots //get and set of the capacity of the hash table
        {
            get;
            set;
        }
        /* pre: true
        * post: return the bucket where the key is stored
        * if the given key in the hash table;
        * otherwise, return -1.
        */
        int Search(string key);
        /* pre: true
        * post: all the elements in the hash table have been removed
        */
        void Clear();
        /* pre: true
        * post: the given key has been inserted into the hash table
        */
        void Insert(string key);
        /* pre: nil
        * post: the given key has been removed from the hash table
        *if the given key is in the hash table
        */
        void Delete(string key); //delete the key from the hash table
        /* pre: nil
        * post: print all the elements in the hash table
        */
        void Print();
    }
}
