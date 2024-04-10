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
    class Node<T>
    {
        private T data;
        private Node<T> next;

        public Node(T data)
        {
            this.data = data;
        }
        public Node(T data, Node<T> next)
        {
            this.data = data;
            this.next = next;
        }

        public T Data
        {
            get { return this.data; }
        }
        public Node<T> Next { get { return this.next; } set { this.next = value; } }

        public override string ToString()
        {
            return this.data.ToString();
        }

    }
}
