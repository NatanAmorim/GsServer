using AutoMapper;
using GsServer.Protobufs;
using GsServer.Models;

namespace GsServer.MapperProfile;

public class AutoMapperProfile : Profile
{
  public AutoMapperProfile()
  {
    CreateMap<Attendance, GetAttendanceByIdResponse>()
      .ForMember(dest => dest.AttendeesStatuses, src => src.MapFrom(src => src.AttendeesStatuses));
    CreateMap<CreateAttendanceRequest, Attendance>()
      .ForMember(dest => dest.AttendeesStatuses, src => src.MapFrom(src => src.AttendeesStatuses));
    CreateMap<Models.AttendanceAttendeeStatus, Protobufs.AttendanceAttendeeStatus>();

    CreateMap<Customer, GetCustomerByIdResponse>()
      .ForMember(dest => dest.Dependents, src => src.MapFrom(src => src.Dependents));
    CreateMap<Customer, CustomerOption>()
      .ForMember(dest => dest.Dependents, src => src.MapFrom(src => src.Dependents));
    CreateMap<CreateCustomerRequest, Customer>()
      .ForMember(dest => dest.Dependents, src => src.MapFrom(src => src.Dependents));

    CreateMap<Discipline, GetDisciplineByIdResponse>()
      .ForMember(dest => dest.ClassDays, src => src.MapFrom(src => src.ClassDays));
    CreateMap<CreateDisciplineRequest, Discipline>()
      .ForMember(dest => dest.ClassDays, src => src.MapFrom(src => src.ClassDays));

    CreateMap<Instructor, GetInstructorByIdResponse>();
    CreateMap<Instructor, InstructorOption>();
    CreateMap<CreateInstructorRequest, Instructor>();

    CreateMap<Notification, GetNotificationByIdResponse>();
    CreateMap<CreateNotificationRequest, Notification>();

    // TODO
    // CreateMap<Order, GetOrderByIdResponse>();
    // CreateMap<CreateOrderRequest, Order>();
    // CreateMap<Models.OrderStatus, Protobufs.OrderStatus>();

    CreateMap<Payment, GetPaymentByIdResponse>()
      .ForMember(dest => dest.Installments, src => src.MapFrom(src => src.Installments));
    CreateMap<CreatePaymentRequest, Payment>()
      .ForMember(dest => dest.Installments, src => src.MapFrom(src => src.Installments));
    CreateMap<Models.PaymentInstallment, Protobufs.PaymentInstallment>();

    CreateMap<Models.Person, Protobufs.Person>();

    CreateMap<Product, GetProductByIdResponse>()
      .ForMember(dest => dest.Variants, src => src.MapFrom(src => src.Variants));
    CreateMap<CreateProductRequest, Product>()
      .ForMember(dest => dest.Variants, src => src.MapFrom(src => src.Variants));
    CreateMap<Models.ProductBrand, Protobufs.ProductBrand>();
    CreateMap<Models.ProductCategory, Protobufs.ProductCategory>();
    // TODO
    // CreateMap<Models.ProductStockHistory, Protobufs.ProductStockHistory>();
    CreateMap<Models.ProductVariant, Protobufs.ProductVariant>();
    CreateMap<Models.ProductVariantInventory, Protobufs.ProductVariantInventory>();

    CreateMap<Promotion, GetPromotionByIdResponse>();
    CreateMap<CreatePromotionRequest, Promotion>();

    CreateMap<Return, GetReturnByIdResponse>()
      .ForMember(dest => dest.ItemsReturned, src => src.MapFrom(src => src.ItemsReturned));
    CreateMap<CreateReturnRequest, Return>()
      .ForMember(dest => dest.ItemsReturned, src => src.MapFrom(src => src.ItemsReturned));
    CreateMap<Models.ReturnItem, Protobufs.ReturnItem>();

    CreateMap<Sale, GetSaleByIdResponse>()
      .ForMember(dest => dest.ItemsSold, src => src.MapFrom(src => src.ItemsSold));
    CreateMap<CreateSaleRequest, Sale>()
      .ForMember(dest => dest.ItemsSold, src => src.MapFrom(src => src.ItemsSold));
    CreateMap<SaleBilling, GetSaleBillingByIdResponse>();
    CreateMap<CreateSaleBillingRequest, SaleBilling>();
    CreateMap<Models.SaleItem, Protobufs.SaleItem>();

    CreateMap<Subscription, GetSubscriptionByIdResponse>();
    CreateMap<CreateSubscriptionRequest, Subscription>();
    CreateMap<SubscriptionBilling, GetSubscriptionBillingByIdResponse>();
    CreateMap<CreateSubscriptionBillingRequest, SubscriptionBilling>();

    CreateMap<User, GetUserByIdResponse>();
    CreateMap<RegisterRequest, User>();

    CreateMap<System.DayOfWeek, Protobufs.DayOfWeek>();
  }
}