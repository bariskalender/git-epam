using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StoreDAL.Data;
using StoreDAL.Entities;
using StoreDAL.Interfaces;

namespace StoreDAL.Repository
{
    public class UserRepository : AbstractRepository, IUserRepository
    {
        public UserRepository(StoreDbContext context)
            : base(context ?? throw new ArgumentNullException(nameof(context)))
        {
        }

        public void Add(User entity)
        {
            this.context.Users.Add(entity);
            this.context.SaveChanges();
        }

        public void Delete(User entity)
        {
            this.context.Users.Remove(entity);
            this.context.SaveChanges();
        }

        public void DeleteById(int id)
        {
            var entity = this.context.Users.FirstOrDefault(x => x.Id == id);

            if (entity != null)
            {
                this.context.Users.Remove(entity);
                this.context.SaveChanges();
            }
        }

        public IEnumerable<User> GetAll()
        {
            return this.context.Users
                .Include(x => x.Role)
                .ToList();
        }

        public IEnumerable<User> GetAll(int pageNumber, int rowCount)
        {
            return this.context.Users
                .Include(x => x.Role)
                .Skip((pageNumber - 1) * rowCount)
                .Take(rowCount)
                .ToList();
        }

        public User GetById(int id)
        {
            var entity = this.context.Users
                .Include(x => x.Role)
                .FirstOrDefault(x => x.Id == id);

            if (entity == null)
            {
                throw new InvalidOperationException();
            }

            return entity;
        }

        public void Update(User entity)
        {
            this.context.Users.Update(entity);
            this.context.SaveChanges();
        }
    }
}