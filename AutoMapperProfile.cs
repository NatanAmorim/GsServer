using AutoMapper;
using GsServer.Protobufs;
using GsServer.Models;

namespace GsServer.MapperProfile;

public class AutoMapperProfile : Profile
{
  public AutoMapperProfile()
  {
    CreateMap<Attendance, GetAttendanceByIdResponse>()
      .ForMember(dest => dest.AttendanceId, src => src.MapFrom(src => src.AttendanceId.ToString()))
      .ForMember(dest => dest.AttendeesStatuses, src => src.MapFrom(src => src.AttendeesStatuses));
    CreateMap<CreateAttendanceRequest, Attendance>()
      .ForMember(dest => dest.DisciplineId, src => src.MapFrom(src => Ulid.Parse(src.DisciplineId)))
      .ForMember(dest => dest.AttendeesStatuses, src => src.MapFrom(src => src.AttendeesStatuses));
    CreateMap<Models.AttendanceAttendeeStatus, Protobufs.AttendanceAttendeeStatus>()
      .ForMember(dest => dest.PersonId, src => src.MapFrom(src => src.PersonId.ToString()));

    CreateMap<Customer, GetCustomerByIdResponse>()
      .ForMember(dest => dest.CustomerId, src => src.MapFrom(src => src.CustomerId.ToString()))
      .ForMember(dest => dest.Dependents, src => src.MapFrom(src => src.Dependents));
    CreateMap<Customer, CustomerOption>()
      .ForMember(dest => dest.CustomerId, src => src.MapFrom(src => src.CustomerId.ToString()))
      .ForMember(dest => dest.Dependents, src => src.MapFrom(src => src.Dependents));
    CreateMap<CreateCustomerRequest, Customer>()
      .ForMember(dest => dest.Dependents, src => src.MapFrom(src => src.Dependents));

    CreateMap<Discipline, GetDisciplineByIdResponse>()
      .ForMember(dest => dest.DisciplineId, src => src.MapFrom(src => src.DisciplineId.ToString()))
      .ForMember(dest => dest.ClassDays, src => src.MapFrom(src => src.ClassDays));
    CreateMap<CreateDisciplineRequest, Discipline>()
      .ForMember(dest => dest.InstructorId, src => src.MapFrom(src => Ulid.Parse(src.InstructorId)))
      .ForMember(dest => dest.ClassDays, src => src.MapFrom(src => src.ClassDays));

    CreateMap<Instructor, GetInstructorByIdResponse>();
    CreateMap<Instructor, InstructorOption>();
    CreateMap<CreateInstructorRequest, Instructor>();

    CreateMap<Notification, GetNotificationByIdResponse>()
      .ForMember(dest => dest.NotificationId, src => src.MapFrom(src => src.NotificationId.ToString()))
      .ForMember(dest => dest.UserId, src => src.MapFrom(src => src.UserId.ToString()));
    CreateMap<CreateNotificationRequest, Notification>()
      .ForMember(dest => dest.UserId, src => src.MapFrom(src => Ulid.Parse(src.UserId)));

    // TODO
    // CreateMap<Order, GetOrderByIdResponse>();
    // CreateMap<CreateOrderRequest, Order>();
    // CreateMap<Models.OrderStatus, Protobufs.OrderStatus>();

    CreateMap<Payment, GetPaymentByIdResponse>()
      .ForMember(dest => dest.PaymentId, src => src.MapFrom(src => src.PaymentId.ToString()))
      .ForMember(dest => dest.Installments, src => src.MapFrom(src => src.Installments));
    CreateMap<CreatePaymentRequest, Payment>()
      .ForMember(dest => dest.Installments, src => src.MapFrom(src => src.Installments));
    CreateMap<Models.PaymentInstallment, Protobufs.PaymentInstallment>()
      .ForMember(dest => dest.PaymentInstallmentId, src => src.MapFrom(src => src.PaymentInstallmentId.ToString()));

    CreateMap<Models.Person, Protobufs.Person>();

    CreateMap<Product, GetProductByIdResponse>()
      .ForMember(dest => dest.ProductId, src => src.MapFrom(src => src.ProductId.ToString()))
      .ForMember(dest => dest.Variants, src => src.MapFrom(src => src.Variants));
    CreateMap<CreateProductRequest, Product>()
      .ForMember(dest => dest.Variants, src => src.MapFrom(src => src.Variants));
    CreateMap<Models.ProductBrand, Protobufs.ProductBrand>();
    CreateMap<Models.ProductCategory, Protobufs.ProductCategory>();
    // TODO
    // CreateMap<Models.ProductStockHistory, Protobufs.ProductStockHistory>();
    CreateMap<Models.ProductVariant, Protobufs.ProductVariant>()
      .ForMember(dest => dest.ProductVariantId, src => src.MapFrom(src => src.ProductVariantId.ToString()));
    CreateMap<Models.ProductVariantInventory, Protobufs.ProductVariantInventory>()
      .ForMember(dest => dest.ProductVariantInventoryId, src => src.MapFrom(src => src.ProductVariantInventoryId.ToString()))
      .ForMember(dest => dest.ProductVariantId, src => src.MapFrom(src => src.ProductVariantId.ToString()));


    CreateMap<Promotion, GetPromotionByIdResponse>()
      .ForMember(dest => dest.PromotionId, src => src.MapFrom(src => src.PromotionId.ToString()));
    CreateMap<CreatePromotionRequest, Promotion>()
      .ForMember(dest => dest.CustomerId, src => src.MapFrom(src => Ulid.Parse(src.CustomerId)));
    ;

    CreateMap<Return, GetReturnByIdResponse>()
      .ForMember(dest => dest.ReturnId, src => src.MapFrom(src => src.ReturnId.ToString()))
      .ForMember(dest => dest.ItemsReturned, src => src.MapFrom(src => src.ItemsReturned));
    CreateMap<CreateReturnRequest, Return>()
      .ForMember(dest => dest.ItemsReturned, src => src.MapFrom(src => src.ItemsReturned));
    CreateMap<Models.ReturnItem, Protobufs.ReturnItem>()
      .ForMember(dest => dest.ReturnItemId, src => src.MapFrom(src => src.ReturnItemId.ToString()))
      .ForMember(dest => dest.ProductVariantId, src => src.MapFrom(src => src.ProductVariantId.ToString()));

    CreateMap<Sale, GetSaleByIdResponse>()
      .ForMember(dest => dest.SaleId, src => src.MapFrom(src => src.SaleId.ToString()))
      .ForMember(dest => dest.ItemsSold, src => src.MapFrom(src => src.ItemsSold));
    CreateMap<CreateSaleRequest, Sale>()
      .ForMember(dest => dest.CustomerId, src => src.MapFrom(src => Ulid.Parse(src.CustomerId)))
      .ForMember(dest => dest.ItemsSold, src => src.MapFrom(src => src.ItemsSold));
    CreateMap<SaleBilling, GetSaleBillingByIdResponse>()
      .ForMember(dest => dest.SaleBillingId, src => src.MapFrom(src => src.SaleBillingId.ToString()));
    CreateMap<CreateSaleBillingRequest, SaleBilling>()
      .ForMember(dest => dest.SaleId, src => src.MapFrom(src => Ulid.Parse(src.SaleId)));
    CreateMap<Models.SaleItem, Protobufs.SaleItem>()
      .ForMember(dest => dest.ProductVariantId, src => src.MapFrom(src => src.ProductVariantId.ToString()));

    CreateMap<Subscription, GetSubscriptionByIdResponse>()
      .ForMember(dest => dest.SubscriptionId, src => src.MapFrom(src => src.SubscriptionId.ToString()));
    CreateMap<CreateSubscriptionRequest, Subscription>()
      .ForMember(dest => dest.DisciplineId, src => src.MapFrom(src => Ulid.Parse(src.DisciplineId)))
      .ForMember(dest => dest.CustomerId, src => src.MapFrom(src => Ulid.Parse(src.CustomerId)));
    CreateMap<SubscriptionBilling, GetSubscriptionBillingByIdResponse>()
      .ForMember(dest => dest.SubscriptionBillingId, src => src.MapFrom(src => src.SubscriptionBillingId.ToString()));
    CreateMap<CreateSubscriptionBillingRequest, SubscriptionBilling>()
      .ForMember(dest => dest.SubscriptionId, src => src.MapFrom(src => Ulid.Parse(src.SubscriptionId)));

    CreateMap<User, GetUserByIdResponse>();
    CreateMap<RegisterRequest, User>();

    CreateMap<System.DayOfWeek, Protobufs.DayOfWeek>();
  }
}