using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StoreDAL.Data;
using StoreDAL.Entities;
using StoreDAL.Interfaces;

namespace StoreDAL.Repository;

public class CustomerOrderRepository : AbstractRepository, ICustomerOrderRepository
{
    private readonly DbSet<CustomerOrder> dbSet;

    public CustomerOrderRepository(StoreDbContext context)
        : base(context ?? throw new ArgumentNullException(nameof(context)))
    {
        this.dbSet = context.Set<CustomerOrder>();
    }

    public void Add(CustomerOrder entity)
    {
        this.dbSet.Add(entity);
        this.context.SaveChanges();
    }

    public void Delete(CustomerOrder entity)
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

    public IEnumerable<CustomerOrder> GetAll()
    {
        return this.dbSet
            .Include(x => x.User)
            .Include(x => x.State)
            .Include(x => x.Details)
            .ToList();
    }

    public IEnumerable<CustomerOrder> GetAll(int pageNumber, int rowCount)
    {
        return this.dbSet
            .Skip((pageNumber - 1) * rowCount)
            .Take(rowCount)
            .ToList();
    }

    public CustomerOrder GetById(int id)
    {
        var entity = this.dbSet
            .Include(x => x.User)
            .Include(x => x.State)
            .Include(x => x.Details)
            .FirstOrDefault(x => x.Id == id);

        if (entity == null)
        {
            throw new InvalidOperationException();
        }

        return entity;
    }

    public void Update(CustomerOrder entity)
    {
        this.dbSet.Update(entity);
        this.context.SaveChanges();
    }
}