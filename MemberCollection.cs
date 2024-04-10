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
    public class MemberCollection
    {
        private Member[] members;
        private Member currentMember;
        private readonly string[] memberTypes = { "Staff", "Member" };


        public MemberCollection()
        {
            members = new Member[0];
            Member adminStaff = new Member("Staff", "Member", "123465789");
            adminStaff.SetCredentials("Staff", "today123","Staff");
            UpdateCollection(adminStaff);
            Member adminMember = new Member("Egg", "Jones", "04123456789");
            adminMember.SetCredentials("egg@uni", "password123", "Member");
            UpdateCollection(adminMember);
            currentMember = null;
        }
        public Member CurrentMember
        {
            get { return currentMember; }
            set {
                    Remove(currentMember);
                    UpdateCollection(currentMember);
                    currentMember = value; 
            }
        }
        public void UpdateCollection(Member member)
        {
            Array.Resize(ref members, members.Length + 1);
            members[members.Length - 1] = member;
        }
        public void RemoveMemberByUsername()
        {
            string username = RequestUserInput("username");
            const string SENTINEL = "0";
            while (username != SENTINEL)
            {
                Member memberToRemove = members.FirstOrDefault(m => m.UserName == username);

                if (memberToRemove != null)
                {
                    Remove(memberToRemove);
                    WriteLine(memberToRemove);
                    WriteLine($"Member '{username}' removed successfully!");                    
                    return;
                }

                WriteLine($"Member {username} not found.");
                WriteLine($"Press {SENTINEL} to exit or enter another members User Name:");
                username = RequestUserInput("Username");
            }            
        }
        public void GetMemberByName()
        {
            string firstName = RequestUserInput("First Name");
            string lastName = RequestUserInput("Last Name");
            const string SENTINEL = "0";

            while (firstName != SENTINEL && lastName != SENTINEL)
            {
                Member member = members.FirstOrDefault(m => m.FirstName == firstName && m.LastName == lastName);

                if (member != null)
                {
                    WriteLine($"Member found: {member}");
                    return;
                }

                WriteLine($"Member '{firstName} {lastName}' not found.");
                WriteLine($"Press {SENTINEL} to exit or enter another member name:");
                firstName = RequestUserInput("First Name");
                lastName = RequestUserInput("Last Name");
            }           
        }
        public void Remove(Member member)
        {
            int index = Array.IndexOf(members, member);

            if (index >= 0)
            {
                for (int i = index; i < members.Length - 1; i++)
                {
                    members[i] = members[i + 1];
                }

                Array.Resize(ref members, members.Length - 1);                
            }
        }
        private string GetValidOption(string[] options, string inputName)
        {
            int optionIndex;
            bool validOption = false;
            do
            {
                WriteLine($"Select the {inputName} by entering the corresponding number:");
                for (int i = 0; i < options.Length; i++)
                {
                    WriteLine($"{i + 1}. {options[i]}");
                }

                string userInput = RequestUserInput(inputName);

                if (int.TryParse(userInput, out optionIndex) && optionIndex >= 1 && optionIndex <= options.Length)
                {
                    validOption = true;
                }
                else
                {
                    WriteLine($"Invalid {inputName}. Please select a valid option by entering the corresponding number.");
                }

            } while (!validOption);

            return options[optionIndex - 1];
        }
        public void AddMember()
        {
            WriteLine("Enter the following details to register the new member:");

            string firstName = RequestUserInput("First Name");
            string lastName = RequestUserInput("Last Name");
            string phoneNumber = RequestUserInput("Phone Number");
            string userName = RequestUserInput("Username");
            string password = RequestUserInput("Password");
            string memberType = GetValidOption(memberTypes, "memberType");

            Member existingMember = members.FirstOrDefault(m => m.FirstName == firstName && m.LastName == lastName && m.PhoneNumber == phoneNumber);
            if (existingMember != null)
            {
                WriteLine("A member with the same first name, last name, and phone number already exists.");
                WriteLine("Press any key to continue");
                ReadKey();
                return;
            }

            Member existingUser = members.FirstOrDefault(m => m.UserName == userName);
            if (existingUser != null)
            {
                WriteLine("A member with the same username already exists.");
                WriteLine("Press any key to continue");
                ReadKey();
                return;
            }

            Member member = new Member(firstName, lastName, phoneNumber);
            member.SetCredentials(userName, password,memberType);

            UpdateCollection(member);
            

            WriteLine($"Member {userName} added successfully!");
            WriteLine("Press any key to continue");
            ReadKey();
        }
        public string RequestUserInput(string inputName)
        {
            string userInput;
            bool confirmed = false;

            do
            {
                Write($"{inputName}: ");
                userInput = ReadLine();

                Console.Write("Are you sure (y/n)? ");
                string confirmation = Console.ReadLine();

                if (confirmation.Equals("y", StringComparison.OrdinalIgnoreCase))
                {
                    confirmed = true;
                }

            } while (!confirmed);

            return userInput;
        }
        public void DisplayMembersByRentedMovie()
        {
            string movieTitle = RequestUserInput("Movie Title");

            List<Member> rentedMembers = members
                   .Where(member => member.RentedMovies != null && member.RentedMovies.Any(movie => movie != null && movie.Title == movieTitle))
                   .ToList();

            WriteLine($"Members renting the movie '{movieTitle}':");
            foreach (Member member in rentedMembers)
            {
                WriteLine(member.ToString());
            }
        }
        public void Authenticate()
        {
            string usernameInput, passwordInput;            
            bool isAuthenticated = false;
            const string SENTINEL = "0";
            while (!isAuthenticated)
            {
                usernameInput = RequestUserInput("Enter your username (or 0 to exit): ").Trim();                

                if (usernameInput.ToLower() == SENTINEL)
                {
                    WriteLine("Exiting authentication.");
                    return;
                    
                }

                passwordInput = RequestUserInput("Enter your password: ").Trim();                                
                foreach (Member member in members)
                {   
                    if (member.UserName == usernameInput && member.Password == passwordInput)
                    {
                        currentMember = member;
                        isAuthenticated = true;
                        
                    }
                }

                if (!isAuthenticated)
                {
                    WriteLine("Authentication failed. Invalid username or password.");
                }
            }

            
        }
        public void LogOut()
        {
            this.currentMember = null;
            return;
        }
    }
}

