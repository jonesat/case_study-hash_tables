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

namespace HashTableExploration
{
    public interface ICommand
    {
        void Execute();
    }

    // Example command implementation
    public class PromptCommand : ICommand
    {
        private Menu menu;
        public PromptCommand(Menu menu)
        {
            this.menu = menu;
        }
        public void Execute()
        {
            Console.WriteLine("Executing MyCommand...");
            Console.WriteLine("Your command is executed");
        }
    }


    public class BrowseCommand: ICommand
    {
        private MovieCollection collection;
        public BrowseCommand(MovieCollection movieCollection)
        {
            
            this.collection = movieCollection;
        }

        public void Execute()
        {
            collection.Browse();
        }

    }

    public class Top3Command: ICommand
    {
        private MovieCollection collection;
        public Top3Command(MovieCollection movieCollection)
        {

            this.collection = movieCollection;
        }

        public void Execute()
        {
            collection.Top3();
        }
    }

    public class NavigateMenuCommand: ICommand
    {
        private Menu menu;

        public NavigateMenuCommand(Menu menu)
        {
            this.menu = menu;
        }   
        public void Execute()
        {
            menu.DisplayMenu();
        }
    }

    public class LoginCommand : ICommand
    {
        private MemberCollection members;
        private Menu loginMenu;
        private ICommand navigateCommand;
        private Menu mainMenu;

        public LoginCommand(ref MemberCollection members,Menu loginMenu,ICommand navigateCommand,Menu mainMenu)
        {
            this.members = members;
            this.loginMenu = loginMenu;
            this.navigateCommand = navigateCommand;
            this.mainMenu = mainMenu;
        }
        public void Execute()
        {
            members.Authenticate();    

        }
    }

    public class EndProgramCommand : ICommand
    {
        public void Execute()
        {
            Environment.Exit(0);

        }
    }

    public class DisplayMovieCommand : ICommand
    {
        private MovieCollection collection;

        public DisplayMovieCommand(MovieCollection collection)
        {
            this.collection = collection;
        }
        public void Execute()
        {
            collection.ViewMovieDetails();
        }
    }

    public class RentMovieCommand : ICommand
    {
        private Member member;
        private MovieCollection movies;

        public RentMovieCommand(Member member, MovieCollection movies)
        {
            this.member = member;
            this.movies = movies;
        }

        public void Execute()
        {
            SearchResult search = movies.UserInputMovie();
            Movie movie = movies.RentMovie(search.Input);
            
            bool withdrawn = this.member.AddMovie(movie);
            if (!withdrawn)
            {
                bool duplicate = true;
                movies.ReturnMovie(movie, duplicate);
            }
        }
    }

    public class ReturnMovieCommand : ICommand
    {
        private Member member;
        private MovieCollection movies;

        public ReturnMovieCommand(Member member, MovieCollection movies)
        {
            this.member = member;
            this.movies = movies;
        }

        public void Execute()
        {
            Movie movie = member.RemoveMovie();
            bool duplicate = false;
            movies.ReturnMovie(movie, duplicate);           
        }
    }

    public class DisplayRentedMoviesCommand: ICommand
    {
        private MemberCollection members;
        public DisplayRentedMoviesCommand(ref MemberCollection members)
        {
            this.members = members;
        }
        public void Execute()
        {
            this.members.CurrentMember.PrintCollection();
        }
    }

    public class AddMovieCommand : ICommand
    {       
        private MovieCollection movieCollection;

        public AddMovieCommand(ref MovieCollection movieCollection)
        {
     
            this.movieCollection = movieCollection;
        }
        public void Execute()
        {
            Movie movie = this.movieCollection.MovieFactory.CreateMovieFromUserInput();
            this.movieCollection.Add(movie);
        }
    }
    public class DuplicateMovieCommand : ICommand
    {
        private MovieCollection movieCollection;

        public DuplicateMovieCommand(ref MovieCollection movieCollection)
        {

            this.movieCollection = movieCollection;
        }
        public void Execute()
        {
            this.movieCollection.DuplicateMovie();  
        }
    }

    public class AddMemberCommad:ICommand
    {
        private MemberCollection members;
        public AddMemberCommad(ref MemberCollection members)
        {
            this.members = members;
        }
        public void Execute() {
            members.AddMember();
        }
    }
    public class RemoveMemberCommand:ICommand
    {
        private MemberCollection members;
        public RemoveMemberCommand(ref MemberCollection members)
        {
            this.members = members;
        }
        public void Execute()
        {
            members.RemoveMemberByUsername();
        }
    }

    public class GetMemberCommand : ICommand
    {
        private MemberCollection members;

        public GetMemberCommand(ref MemberCollection members)
        {
            this.members = members;
        }   
        public void Execute()
        {
            this.members.GetMemberByName();
        }
    }

    public class LogOutCommand : ICommand
    {
        private MemberCollection members;
        private Menu loginMenu;        

        public LogOutCommand(ref MemberCollection members,Menu loginMenu)
        {
            this.members = members;
            this.loginMenu = loginMenu;
            
        }

        public void Execute()
        {
            if (loginMenu.OptionsCount() == 2)
            {
                loginMenu.RemoveOption($"Main Menu for {this.members.CurrentMember.FirstName} {this.members.CurrentMember.LastName}");
            }
            members.LogOut();
            //loginMenu.DisplayMenu();
            

        }
    }

    public class RemoveMovieCommand : ICommand
    {
        private MovieCollection movies;
        public RemoveMovieCommand(ref MovieCollection movies)
        {
            this.movies = movies;
        }
        public void Execute()
        {
            movies.RemoveMovie();
        }
    }

    public class DisplayMembersByMovieCommand : ICommand
    {
        private MemberCollection members;
        public DisplayMembersByMovieCommand(ref MemberCollection members)
        {
            this.members = members;
        }   
        public void Execute()
        {
            this.members.DisplayMembersByRentedMovie();
        }
    }
}
