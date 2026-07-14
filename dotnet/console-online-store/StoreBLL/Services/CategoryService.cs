namespace StoreBLL.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using StoreBLL.Interfaces;
    using StoreBLL.Models;
    using StoreDAL.Entities;
    using StoreDAL.Interfaces;

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository repository;

        public CategoryService(ICategoryRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public IEnumerable<AbstractModel> GetAll()
        {
            return this.repository.GetAll()
                .Select(x => new CategoryModel(x.Id, x.Name))
                .ToList();
        }

        public AbstractModel GetById(int id)
        {
            var entity = this.repository.GetById(id);

            if (entity == null)
            {
                throw new InvalidOperationException();
            }

            return new CategoryModel(entity.Id, entity.Name);
        }

        public void Add(AbstractModel model)
        {
            var m = model as CategoryModel;

            if (m == null)
            {
                throw new InvalidOperationException();
            }

            var entity = new Category(m.Id, m.Name);
            this.repository.Add(entity);
        }

        public void Update(AbstractModel model)
        {
            var m = model as CategoryModel;

            if (m == null)
            {
                throw new InvalidOperationException();
            }

            var entity = new Category(m.Id, m.Name);
            this.repository.Update(entity);
        }

        public void Delete(int modelId)
        {
            this.repository.DeleteById(modelId);
        }
    }
}