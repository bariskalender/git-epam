namespace StoreBLL.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using StoreBLL.Interfaces;
    using StoreBLL.Models;
    using StoreDAL.Entities;
    using StoreDAL.Interfaces;

    public class ProductTitleService : IProductTitleService
    {
        private readonly IProductTitleRepository repository;

        public ProductTitleService(IProductTitleRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public IEnumerable<AbstractModel> GetAll()
        {
            return this.repository.GetAll()
                .Select(x => new ProductTitleModel(x.Id, x.Title ?? string.Empty, x.CategoryId))
                .ToList();
        }

        public AbstractModel GetById(int id)
        {
            var entity = this.repository.GetById(id);

            if (entity == null)
            {
                throw new InvalidOperationException();
            }

            return new ProductTitleModel(entity.Id, entity.Title ?? string.Empty, entity.CategoryId);
        }

        public void Add(AbstractModel model)
        {
            var m = model as ProductTitleModel;

            if (m == null)
            {
                throw new InvalidOperationException();
            }

            var entity = new ProductTitle(m.Id, m.Name, m.CategoryId);
            this.repository.Add(entity);
        }

        public void Update(AbstractModel model)
        {
            var m = model as ProductTitleModel;

            if (m == null)
            {
                throw new InvalidOperationException();
            }

            var entity = new ProductTitle(m.Id, m.Name, m.CategoryId);
            this.repository.Update(entity);
        }

        public void Delete(int modelId)
        {
            this.repository.DeleteById(modelId);
        }
    }
}