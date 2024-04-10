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
using System.ComponentModel.Design.Serialization;

namespace HashTableExploration
{
    class Program
    {
        static void Main(string[] args)
        {
            Tester demo = new Tester("week6");

            //// Optional Test functions
            //demo.TestLinkedList();
            //demo.TestTransformString();
            //demo.TestHashFunction();
            //demo.TestHashTable();
            //demo.TestMovieCollection();
            //WriteLine("Begin Program? Press Any key\n\n\n\n\n");
            //ReadKey();


            //// Main Program Begins

            // Load some test data from the tester class.
            MovieCollection movies = demo.SeedCollection();

            MovieFactory movieFactory = new MovieFactory();
            movies.MovieFactory = movieFactory;
            MemberCollection members = new MemberCollection();
            string appTitle = "Community Library Movie Dvd Management System (CLMDMS)";
            
         
            Menu loginMenu = new HubMenu(appTitle, "Login Menu");
            Menu mainMenu = new HubMenu(appTitle, "Main Menu");

            Menu staffMenu;
            Menu memberMenu;


            ICommand navigateMemberMenuCommand;
            ICommand navigateStaffMenuCommand;
            ICommand navigateMainMenu = new NavigateMenuCommand(mainMenu);

            ICommand login;
            ICommand logout = null;
            
            // Main program loop
            while (true)
            {
                staffMenu = new Menu(appTitle, "Staff Menu");
                staffMenu = StaffMenuSetup(staffMenu, members, movies);
                navigateStaffMenuCommand = new NavigateMenuCommand(staffMenu);

                memberMenu = new Menu(appTitle, "Member Menu");
                memberMenu = MemberMenuSetup(memberMenu, members, movies);
                navigateMemberMenuCommand = new NavigateMenuCommand(memberMenu);                              

                login = new LoginCommand(ref members, loginMenu, navigateMainMenu,mainMenu);
                loginMenu.AddOption($"Log in to the {appTitle}", login);

                logout = new LogOutCommand(ref members, loginMenu);
                mainMenu.AddOption("Log out", logout);

                do
                {
                        loginMenu.DisplayMenu();
                } while (members.CurrentMember == null) ;
                if (members.CurrentMember != null)
                {
                    staffMenu = StaffMenuSetup(staffMenu, members, movies);
                    memberMenu = MemberMenuSetup(memberMenu, members, movies);
                    memberMenu.AddOption("Navigate to the main menu", navigateMainMenu);
                    mainMenu.AddOption("Navigate to member menu", navigateMemberMenuCommand);
                    memberMenu.RemoveOption("Navigate to the main menu");
                    memberMenu.AddOption("Navigate to the main menu", navigateMainMenu);
                }
                if (members.CurrentMember.MemberType == "Staff")
                {
                    
                    // Linking Main menu to Staff menu and updating links to member menu.
                    staffMenu.AddOption("Navigate to the main menu", navigateMainMenu);

                    //if (logout != null)
                    //{
                        mainMenu.RemoveOption("Log out");
                        logout = new LogOutCommand(ref members, loginMenu);
                        mainMenu.AddOption("Log out", logout);


                    //}
                    mainMenu.AddOption("Navigate to staff menu", navigateStaffMenuCommand);
                    mainMenu.AddOption("Log out", logout);

                    staffMenu.RemoveOption("Navigate to the main menu");
                    staffMenu.AddOption("Navigate to the main menu", navigateMainMenu);
                    memberMenu.RemoveOption("Navigate to the main menu");
                    memberMenu.AddOption("Navigate to the main menu", navigateMainMenu);

                    login = new LoginCommand(ref members, loginMenu, navigateMainMenu, mainMenu);
                    loginMenu.RemoveOption($"Log in to the {appTitle}");
                    loginMenu.AddOption($"Log in to the {appTitle}", login);

                }
                else if(members.CurrentMember.MemberType == "Member")
                {                    
                    mainMenu.AddOption("Navigate to staff menu", navigateStaffMenuCommand);
                    mainMenu.RemoveOption("Navigate to staff menu");                    
                }

                if (members.CurrentMember != null && loginMenu.OptionsCount() < 2)
                {
                    loginMenu.AddOption($"Main Menu for {members.CurrentMember.FirstName} {members.CurrentMember.LastName}", navigateMainMenu);
                    logout = new LogOutCommand(ref members, loginMenu);
                    mainMenu.AddOption("Log out", logout);


                }


            }            
        }
        public static Menu MemberMenuSetup(Menu memberMenu, MemberCollection members, MovieCollection movies)
        {
            if (members.CurrentMember != null)
            {
                //Command: Browse all the movies
                ICommand browse = new BrowseCommand(movies);
                memberMenu.AddOption("Browse all movies in library DVD collection", browse);
                //Command: Display a movie given the title
                ICommand displayMovie = new DisplayMovieCommand(movies);
                memberMenu.AddOption("Look up a Movie", displayMovie);
                //Command: Borrow a Movie DVD
                ICommand rentMovie = new RentMovieCommand(members.CurrentMember, movies);
                memberMenu.AddOption("Rent a movie from the library DVD collection", rentMovie);
                //Command: Return a Movie DVD
                ICommand returnMovie = new ReturnMovieCommand(members.CurrentMember, movies);
                memberMenu.AddOption("Return a movie to the library DVD collection", returnMovie);
                //Command: List current borrowing movies foreach(var movie in currentUser.Movies) movie.ToString()
                ICommand viewUsersMovies = new DisplayRentedMoviesCommand(ref members);
                memberMenu.AddOption("View your currently rented titles from the library DVD collection", viewUsersMovies);
                //Command: Display the top3 movies
                ICommand top3 = new Top3Command(movies);
                memberMenu.AddOption("Browse the top 3 most rented movies in the library DVD collection", top3);

            }
            return memberMenu;
        }

        public static Menu StaffMenuSetup(Menu staffMenu, MemberCollection members, MovieCollection movies)
        {
            if (members.CurrentMember != null) { 
            // Command: Add DVDs of a new movie to the system
            ICommand addMovie = new AddMovieCommand(ref movies);
            staffMenu.AddOption("Add new movie to the library DVD collection", addMovie);
            // Command: Add new DvDs of an existing movie to the system
            ICommand duplicateMovie = new DuplicateMovieCommand(ref movies);
            staffMenu.AddOption("Add another copy of an existing Movie to the library DVD collection.", duplicateMovie);
            // Command: Remove DVD of an existing
            ICommand removeMovie = new RemoveMovieCommand(ref movies);
            staffMenu.AddOption("Remove a DVD from the library DVD collection", removeMovie);
            // Command: Register a new member
            ICommand registerMember = new AddMemberCommad(ref members);
            staffMenu.AddOption("Add a new member to the library system.", registerMember);
            // Remove a registered member
            ICommand removeMember = new RemoveMemberCommand(ref members);
            staffMenu.AddOption("Remove a member from the library system,", removeMember);
            // Find a members deets given name
            ICommand findMember = new GetMemberCommand(ref members);
            staffMenu.AddOption("Find a member by name in the library system.", findMember);
            // Find members who are currently renting a particular Movie (Linq users.where(users.Movies.where(movie.Title=searchInput)))
            ICommand displayMembersbyMovie = new DisplayMembersByMovieCommand(ref members);
            staffMenu.AddOption("Find all users who have rented a particular movie.", displayMembersbyMovie);
            }
            return staffMenu;
        }
    }

}
            
    
    





