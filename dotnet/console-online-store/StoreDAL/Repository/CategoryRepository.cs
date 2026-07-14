using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StoreDAL.Data;
using StoreDAL.Entities;
using StoreDAL.Interfaces;

namespace StoreDAL.Repository;

public class CategoryRepository : AbstractRepository, ICategoryRepository
{
    private readonly DbSet<Category> dbSet;

    public CategoryRepository(StoreDbContext context)
        : base(context ?? throw new ArgumentNullException(nameof(context)))
    {
        this.dbSet = context.Set<Category>();
    }

    public void Add(Category entity)
    {
        this.dbSet.Add(entity);
        this.context.SaveChanges();
    }

    public void Delete(Category entity)
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

    public IEnumerable<Category> GetAll()
    {
        return this.dbSet.ToList();
    }

    public IEnumerable<Category> GetAll(int pageNumber, int rowCount)
    {
        return this.dbSet
            .Skip((pageNumber - 1) * rowCount)
            .Take(rowCount)
            .ToList();
    }

    public Category GetById(int id)
    {
        var entity = this.dbSet.FirstOrDefault(x => x.Id == id);

        if (entity == null)
        {
            throw new InvalidOperationException();
        }

        return entity;
    }

    public void Update(Category entity)
    {
        this.dbSet.Update(entity);
        this.context.SaveChanges();
    }
}