using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StoreDAL.Data;
using StoreDAL.Entities;
using StoreDAL.Interfaces;

namespace StoreDAL.Repository;

public class ManufacturerRepository : AbstractRepository, IManufacturerRepository
{
    private readonly DbSet<Manufacturer> dbSet;

    public ManufacturerRepository(StoreDbContext context)
        : base(context ?? throw new ArgumentNullException(nameof(context)))
    {
        this.dbSet = context.Set<Manufacturer>();
    }

    public void Add(Manufacturer entity)
    {
        this.dbSet.Add(entity);
        this.context.SaveChanges();
    }

    public void Delete(Manufacturer entity)
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

    public IEnumerable<Manufacturer> GetAll()
    {
        return this.dbSet.ToList();
    }

    public IEnumerable<Manufacturer> GetAll(int pageNumber, int rowCount)
    {
        return this.dbSet
            .Skip((pageNumber - 1) * rowCount)
            .Take(rowCount)
            .ToList();
    }

    public Manufacturer GetById(int id)
    {
        var entity = this.dbSet.FirstOrDefault(x => x.Id == id);

        if (entity == null)
        {
            throw new InvalidOperationException();
        }

        return entity;
    }

    public void Update(Manufacturer entity)
    {
        this.dbSet.Update(entity);
        this.context.SaveChanges();
    }
}