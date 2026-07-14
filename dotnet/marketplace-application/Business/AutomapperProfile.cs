using AutoMapper;
using Business.Models;
using Data.Entities;

namespace Business;

public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        this.CreateMap<ProductCategory, CategoryModel>()
            .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.CategoryName))
            .ForMember(
                dest => dest.ProductIds,
                opt => opt.MapFrom(src =>
                    src.Products != null
                        ? src.Products.Select(x => x.Id)
                        : null));

        this.CreateMap<CategoryModel, ProductCategory>()
            .ForMember(
                dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Name))
            .ForMember(
                dest => dest.Products,
                opt => opt.MapFrom(src =>
                    (ICollection<Product>?)null));

        this.CreateMap<Product, ProductModel>()
            .ForMember(
                dest => dest.CategoryName,
                opt => opt.MapFrom(src =>
                    src.Category != null
                        ? src.Category.CategoryName
                        : string.Empty))
            .ForMember(
                dest => dest.ReceiptDetailIds,
                opt => opt.MapFrom(src =>
                    src.ReceiptDetails != null
                        ? src.ReceiptDetails.Select(x => x.Id)
                        : null));

        this.CreateMap<ProductModel, Product>()
            .ForMember(
                dest => dest.Category,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.ReceiptDetails,
                opt => opt.MapFrom(src =>
                    (ICollection<ReceiptDetail>?)null));

        this.CreateMap<ReceiptDetail, ReceiptDetailModel>();

        this.CreateMap<ReceiptDetailModel, ReceiptDetail>()
            .ForMember(
                dest => dest.Product,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.Receipt,
                opt => opt.Ignore());

        this.CreateMap<Receipt, ReceiptModel>()
            .ForMember(
                dest => dest.ReceiptDetailsIds,
                opt => opt.MapFrom(src =>
                    src.ReceiptDetails != null
                        ? src.ReceiptDetails.Select(x => x.Id)
                        : null));

        this.CreateMap<ReceiptModel, Receipt>()
            .ForMember(
                dest => dest.Customer,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.ReceiptDetails,
                opt => opt.MapFrom(src =>
                    (ICollection<ReceiptDetail>?)null));

        this.CreateMap<Customer, CustomerModel>()
            .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src =>
                    src.Person != null
                        ? src.Person.Name
                        : string.Empty))
            .ForMember(
                dest => dest.Surname,
                opt => opt.MapFrom(src =>
                    src.Person != null
                        ? src.Person.Surname
                        : string.Empty))
            .ForMember(
                dest => dest.BirthDate,
                opt => opt.MapFrom(src =>
                    src.Person != null
                        ? src.Person.BirthDate
                        : DateTime.MinValue))
            .ForMember(
                dest => dest.ReceiptsIds,
                opt => opt.MapFrom(src =>
                    src.Receipts != null
                        ? src.Receipts.Select(x => x.Id)
                        : null));

        this.CreateMap<CustomerModel, Customer>()
            .ForMember(
                dest => dest.PersonId,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(
                dest => dest.Person,
                opt => opt.MapFrom(src => new Person
                {
                    Id = src.Id,
                    Name = src.Name,
                    Surname = src.Surname,
                    BirthDate = src.BirthDate,
                }))
            .ForMember(
                dest => dest.Receipts,
                opt => opt.MapFrom(src =>
                    (ICollection<Receipt>?)null));

        this.CreateMap<Customer, CustomerActivityModel>()
            .ForMember(
                dest => dest.CustomerId,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(
                dest => dest.CustomerName,
                opt => opt.MapFrom(src =>
                    src.Person != null
                        ? $"{src.Person.Name} {src.Person.Surname}"
                        : string.Empty))
            .ForMember(
                dest => dest.ReceiptSum,
                opt => opt.Ignore());
    }
}
