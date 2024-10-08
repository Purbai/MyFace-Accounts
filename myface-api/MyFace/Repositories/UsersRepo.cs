﻿using System.Collections.Generic;
using System.Linq;
using MyFace.Models.Database;
using MyFace.Models.Request;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace MyFace.Repositories
{
    public interface IUsersRepo
    {
        IEnumerable<User> Search(UserSearchRequest search);
        int Count(UserSearchRequest search);
        User GetById(int id);
        User Create(CreateUserRequest newUser);
        User Update(int id, UpdateUserRequest update);
        void Delete(int id);
        //Task<User> Authenticate(string username, string Salt);
        User Authenticate(string username, string Salt);
    }

    public class UsersRepo : IUsersRepo
    {
        private readonly MyFaceDbContext _context;

        public UsersRepo(MyFaceDbContext context)
        {
            _context = context;
        }

        public IEnumerable<User> Search(UserSearchRequest search)
        {
            return _context.Users
                .Where(p => search.Search == null ||
                            (
                                p.FirstName.ToLower().Contains(search.Search) ||
                                p.LastName.ToLower().Contains(search.Search) ||
                                p.Email.ToLower().Contains(search.Search) ||
                                p.Username.ToLower().Contains(search.Search)
                            ))
                .OrderBy(u => u.Username)
                .Skip((search.Page - 1) * search.PageSize)
                .Take(search.PageSize);
        }
                public IEnumerable<User> SearchExact(UserSearchRequest search)
        {
            return _context.Users
                .Where(p => search.Search == null ||
                            (   
                                p.Username.ToLower().Equals(search.Search)
                            ))
                .OrderBy(u => u.Username)
                .Skip((search.Page - 1) * search.PageSize)
                .Take(search.PageSize);
        }

        public int Count(UserSearchRequest search)
        {
            return _context.Users
                .Count(p => search.Search == null ||
                            (
                                p.FirstName.ToLower().Contains(search.Search) ||
                                p.LastName.ToLower().Contains(search.Search) ||
                                p.Email.ToLower().Contains(search.Search) ||
                                p.Username.ToLower().Contains(search.Search)
                            ));
        }

        public User GetById(int id)
        {
            return _context.Users
                .Single(user => user.Id == id);
        }

        public User Create(CreateUserRequest newUser)
        {
            // ---- Need to refactor this since we are doing this in the sampleuser generator as well -----
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }
            var insertResponse = _context.Users.Add(new User
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                Username = newUser.Username,
                Salt = Convert.ToBase64String(salt),
                Hashed_Password = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: newUser.User_Password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8)),
                ProfileImageUrl = newUser.ProfileImageUrl,
                CoverImageUrl = newUser.CoverImageUrl,
            });
            _context.SaveChanges();

            return insertResponse.Entity;
        }

        public User Update(int id, UpdateUserRequest update)
        {
            var user = GetById(id);

            user.FirstName = update.FirstName;
            user.LastName = update.LastName;
            user.Username = update.Username;
            user.Email = update.Email;
            user.ProfileImageUrl = update.ProfileImageUrl;
            user.CoverImageUrl = update.CoverImageUrl;

            _context.Users.Update(user);
            _context.SaveChanges();

            return user;
        }

        public void Delete(int id)
        {
            var user = GetById(id);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        //public Task<User> Authenticate(string username, string password)
        public User Authenticate(string username, string passwd)
        {
            // search for user - check if the user is in the userlist
            UserSearchRequest searchUser = new UserSearchRequest();
            searchUser.Search = username;
            // if user is return then check the password else not ok
            List<User> searchResult = SearchExact(searchUser).ToList();
            string checkPWD = "";
            //Console.WriteLine($"Authencating - checking password ....user = {username}, password = {passwd}");
            foreach (var user in searchResult)
            {
                checkPWD = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: passwd,
                    //salt: System.Text.Encoding.UTF8.GetBytes(user.Salt),
                    salt: Convert.FromBase64String(user.Salt),
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8));
                //Console.WriteLine($"looping and checking searchResults - {user.Username}, {user.Salt}, {user.Hashed_Password}, {checkPWD}");
                if (checkPWD == user.Hashed_Password)
                {
                    //Console.WriteLine("hash pwd and pwd matches ....");
                    return user;
                }
            }

            return null;

            throw new NotImplementedException();

        }
    }
}