using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StoreDAL.Data;
using StoreDAL.Entities;
using StoreDAL.Interfaces;

namespace StoreDAL.Repository;

public class OrderDetailRepository : AbstractRepository, IOrderDetailRepository
{
    private readonly DbSet<OrderDetail> dbSet;

    public OrderDetailRepository(StoreDbContext context)
        : base(context ?? throw new ArgumentNullException(nameof(context)))
    {
        this.dbSet = context.Set<OrderDetail>();
    }

    public void Add(OrderDetail entity)
    {
        this.dbSet.Add(entity);
        this.context.SaveChanges();
    }

    public void Delete(OrderDetail entity)
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

    public IEnumerable<OrderDetail> GetAll()
    {
        return this.dbSet
            .Include(x => x.Product)
            .Include(x => x.Order)
            .ToList();
    }

    public IEnumerable<OrderDetail> GetAll(int pageNumber, int rowCount)
    {
        return this.dbSet
            .Skip((pageNumber - 1) * rowCount)
            .Take(rowCount)
            .ToList();
    }

    public OrderDetail GetById(int id)
    {
        var entity = this.dbSet
            .Include(x => x.Product)
            .Include(x => x.Order)
            .FirstOrDefault(x => x.Id == id);

        if (entity == null)
        {
            throw new InvalidOperationException();
        }

        return entity;
    }

    public void Update(OrderDetail entity)
    {
        this.dbSet.Update(entity);
        this.context.SaveChanges();
    }
}