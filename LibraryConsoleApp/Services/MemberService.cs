using LibraryConsoleApp;
using LibraryConsoleApp.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;
using System;
using System.Linq;
using System.Threading.Channels;


namespace LibraryConsoleApp.Services
{
    internal class MemberService
    {
        private readonly LibraryDbContext _context;

        public MemberService(LibraryDbContext context)
        {
            _context = context;
        }

        //add

        public Member AddMember(string fullName)
        {

            var newMember = new Member { FullName = fullName };
            _context.Members.Add(newMember);
            int affect = _context.SaveChanges();
            Console.WriteLine($"Added member: {newMember.FullName}, Changes:{affect}");

            return newMember;
        }

        //update
        public bool UpdateMemberName(int memberId, string newFullName)
        {

            var memberToUpdate = _context.Members.Find(memberId);

            if (memberToUpdate == null)
            {
                Console.WriteLine("Member not found.");
                return false;
            }

            memberToUpdate.FullName = newFullName;
            int affect = _context.SaveChanges();
            Console.WriteLine($"Updated member ID {memberToUpdate} name to {newFullName}, Changes: {affect}");

            return true;
        }

        //read

        public void ShowMembers()
        {
            foreach (var member in _context.Members)
            {
                Console.WriteLine($"Member Id:{member.Id}, Member Name: {member.FullName}");
            }
        }

        //delete
        public void DeleteMember(int memberId)
        {
            var member = _context.Members.Find(memberId);
            if (member != null)
            {
                _context.Members.Remove(member);
                _context.SaveChanges();
            }

        }


    }
}


