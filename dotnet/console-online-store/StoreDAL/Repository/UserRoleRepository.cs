namespace StoreDAL.Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StoreDAL.Data;
using StoreDAL.Entities;
using StoreDAL.Interfaces;

public class UserRoleRepository : AbstractRepository, IUserRoleRepository
{
    private readonly DbSet<UserRole> dbSet;

    public UserRoleRepository(StoreDbContext context)
        : base(context)
    {
        ArgumentNullException.ThrowIfNull(context);
        this.dbSet = context.Set<UserRole>();
    }

    public void Add(UserRole entity)
    {
        this.dbSet.Add(entity);
        this.context.SaveChanges();
    }

    public void Delete(UserRole entity)
    {
        this.dbSet.Remove(entity);
        this.context.SaveChanges();
    }

    public void DeleteById(int id)
    {
        var entity = this.dbSet.Find(id);
        if (entity != null)
        {
            this.dbSet.Remove(entity);
            this.context.SaveChanges();
        }
    }

    public IEnumerable<UserRole> GetAll()
    {
        return this.dbSet.ToList();
    }

    public IEnumerable<UserRole> GetAll(int pageNumber, int rowCount)
    {
        return this.dbSet
            .Skip((pageNumber - 1) * rowCount)
            .Take(rowCount)
            .ToList();
    }

    public UserRole GetById(int id)
    {
        return this.dbSet.Find(id);
    }

    public void Update(UserRole entity)
    {
        this.dbSet.Update(entity);
        this.context.SaveChanges();
    }
}