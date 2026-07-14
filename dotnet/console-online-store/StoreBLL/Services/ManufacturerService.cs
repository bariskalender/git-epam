namespace StoreBLL.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using StoreBLL.Interfaces;
    using StoreBLL.Models;
    using StoreDAL.Entities;
    using StoreDAL.Interfaces;

    public class ManufacturerService : IManufacturerService
    {
        private readonly IManufacturerRepository repository;

        public ManufacturerService(IManufacturerRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public IEnumerable<AbstractModel> GetAll()
        {
            return this.repository.GetAll()
                .Select(x => new ManufacturerModel(x.Id, x.Name ?? string.Empty))
                .ToList();
        }

        public AbstractModel GetById(int id)
        {
            var entity = this.repository.GetById(id);

            if (entity == null)
            {
                throw new InvalidOperationException();
            }

            return new ManufacturerModel(entity.Id, entity.Name ?? string.Empty);
        }

        public void Add(AbstractModel model)
        {
            var m = model as ManufacturerModel;

            if (m == null)
            {
                throw new InvalidOperationException();
            }

            var entity = new Manufacturer(m.Id, m.Name);
            this.repository.Add(entity);
        }

        public void Update(AbstractModel model)
        {
            var m = model as ManufacturerModel;

            if (m == null)
            {
                throw new InvalidOperationException();
            }

            var entity = new Manufacturer(m.Id, m.Name);
            this.repository.Update(entity);
        }

        public void Delete(int modelId)
        {
            this.repository.DeleteById(modelId);
        }
    }
}