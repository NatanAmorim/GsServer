# TODO List

## High Priority

- [x] Create a `TODO.md`
- [ ] Implement gRPC services
- [ ] Implement InvoiceBackgroundService

## Medium Priority

- [ ] Finish implementing Order Architecture
- [ ] Finish implementing Attendance Architecture
- [ ] Implement RpcService Put Methods
- [ ] Implement [ULID](https://github.com/Cysharp/Ulid) with EntityFramework
- [ ] Add Something like RequestId to every log (Maybe `RequestId = Guid.newGuid().ToString()`)
- [ ] Track ProductStockAmount changes in a ephemeral table
- [ ] Add something like `app.UseSerilogRequestLogging();` but for gRPC

## Low Priority

- [ ] Implement something like How to use CancellationToken to avoid wasting server resources
- [ ] Identify Race Condition Vulnerabilities with a good concurrency
- [ ] Add cursor pagination while listing data from DB
- [ ] Add a method to change Roles on AuthServices
- [ ] Add a RateLimiter
- [ ] Add a GlobalExceptionHandlingMiddleware
- [ ] Add a Payment Gateway like Stripe, Paypal, MercadoPago, Nubank for invoice, billing and automatic debit payment.
- [ ] Create Better documentation in a folder called `docs/`

<!-- ## Notes -->
