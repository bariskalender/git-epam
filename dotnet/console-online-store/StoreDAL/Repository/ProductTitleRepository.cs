using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StoreDAL.Data;
using StoreDAL.Entities;
using StoreDAL.Interfaces;

namespace StoreDAL.Repository;

public class ProductTitleRepository : AbstractRepository, IProductTitleRepository
{
    private readonly DbSet<ProductTitle> dbSet;

    public ProductTitleRepository(StoreDbContext context)
        : base(context ?? throw new ArgumentNullException(nameof(context)))
    {
        this.dbSet = context.Set<ProductTitle>();
    }

    public void Add(ProductTitle entity)
    {
        this.dbSet.Add(entity);
        this.context.SaveChanges();
    }

    public void Delete(ProductTitle entity)
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

    public IEnumerable<ProductTitle> GetAll()
    {
        return this.dbSet
            .Include(x => x.Category)
            .ToList();
    }

    public IEnumerable<ProductTitle> GetAll(int pageNumber, int rowCount)
    {
        return this.dbSet
            .Skip((pageNumber - 1) * rowCount)
            .Take(rowCount)
            .ToList();
    }

    public ProductTitle GetById(int id)
    {
        var entity = this.dbSet
            .Include(x => x.Category)
            .FirstOrDefault(x => x.Id == id);

        if (entity == null)
        {
            throw new InvalidOperationException();
        }

        return entity;
    }

    public void Update(ProductTitle entity)
    {
        this.dbSet.Update(entity);
        this.context.SaveChanges();
    }
}