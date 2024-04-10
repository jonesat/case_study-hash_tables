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
    class LinkedList<T> : IEnumerable<T>
    {
        // This is an implementation of linked lists for integers
        private Node<T> head;

        public LinkedList(T value)
        {

            this.head = new Node<T>(value);
        }

        public void Insert(T value)
        {
            Node<T> node = new Node<T>(value);
            if (head == null)
            {
                this.head = node;
            }
            else
            {
                Node<T> current = this.head;
                while (current.Next != null)
                {
                    current = current.Next;
                }

                current.Next = node;
            }
        }

        public void Delete(T value)
        {
            if (head == null)
            {
                return;
            }

            if (EqualityComparer<T>.Default.Equals(this.head.Data, value))
            {
                this.head = this.head.Next;
                return;
            }

            Node<T> current = this.head;
            while (current.Next != null)
            {
                if (EqualityComparer<T>.Default.Equals(current.Next.Data, value))
                {
                    current.Next = current.Next.Next;
                    return;
                }
                current = current.Next;
            }
        }

        public override string ToString()
        {
            string message = "";
            Node<T> current = this.head;
            do
            {
                message += current.ToString() + " - ";
                current = current.Next;
            } while (current != null);
            return message;
        }

        public IEnumerator<T> GetEnumerator()
        {
            Node<T> current = this.head;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


    }
}
