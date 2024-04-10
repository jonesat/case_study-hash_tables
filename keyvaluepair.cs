using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTableExploration
{
    public class KeyValuePair
    {
        private string key;
        private Movie movie;
        public KeyValuePair(Movie film)
        {
            this.key = film.Title;
            this.movie = film;
        }
        public KeyValuePair(string key)
        {
            this.key = key;
        }

        public string Key
        {
            get { return key; }
            set { key = value; }
        }

        public Movie Movie
        {
            get { return movie; }
            set { movie = value; }
        }

       
    }
}
