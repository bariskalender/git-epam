using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StoreDAL.Data;
using StoreDAL.Entities;
using StoreDAL.Interfaces;

namespace StoreDAL.Repository
{
    public class ProductRepository : AbstractRepository, IProductRepository
    {
        public ProductRepository(StoreDbContext context)
            : base(context ?? throw new ArgumentNullException(nameof(context)))
        {
        }

        public void Add(Product entity)
        {
            this.context.Products.Add(entity);
            this.context.SaveChanges();
        }

        public void Delete(Product entity)
        {
            this.context.Products.Remove(entity);
            this.context.SaveChanges();
        }

        public void DeleteById(int id)
        {
            var entity = this.context.Products.FirstOrDefault(x => x.Id == id);

            if (entity != null)
            {
                this.context.Products.Remove(entity);
                this.context.SaveChanges();
            }
        }

        public IEnumerable<Product> GetAll()
        {
            return this.context.Products
                .Include(x => x.ProductTitle)
                .Include(x => x.Manufacturer)
                .ToList();
        }

        public IEnumerable<Product> GetAll(int pageNumber, int rowCount)
        {
            return this.context.Products
                .Include(x => x.ProductTitle)
                .Include(x => x.Manufacturer)
                .Skip((pageNumber - 1) * rowCount)
                .Take(rowCount)
                .ToList();
        }

        public Product GetById(int id)
        {
            var entity = this.context.Products
                .Include(x => x.ProductTitle)
                .Include(x => x.Manufacturer)
                .FirstOrDefault(x => x.Id == id);

            if (entity == null)
            {
                throw new InvalidOperationException();
            }

            return entity;
        }

        public void Update(Product entity)
        {
            this.context.Products.Update(entity);
            this.context.SaveChanges();
        }
    }
}