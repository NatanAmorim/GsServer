namespace GsServer.BackgroundServices;

/// <summary>
/// Invoice: Lists the products or services delivered or provided, along with
/// their prices, it specifies the payment terms, including the due date by which
/// the customer should pay  and may  include detailed client information.
/// An invoice may indicate the existence of credit, as payment is not immediate.
/// </summary>
public class SubscriptionInvoiceBackgroundJob
(
    ILogger<SubscriptionInvoiceBackgroundJob> logger
) : BackgroundService
{
    private readonly ILogger<SubscriptionInvoiceBackgroundJob> _logger = logger;

    /// <summary>
    /// Generates invoices in the background, runs daily at 12 AM UTC time.
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns>void</returns>
    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (stoppingToken.IsCancellationRequested == false)
        {
            try
            {
                _logger.LogInformation(
                    "Executing Background Service: {BackgroundServiceName} started",
                    typeof(SubscriptionInvoiceBackgroundJob).Name
                );

                // Delay for Until 12 AM UTC time asynchronously.
                // 9 AM BRT (Brasília Time), UTC/GMT -3 hours.
                await Task.Delay((int)Math.Ceiling(MillisecondsUntilTwelveAmUtc()), stoppingToken);

                // TODO store job in DB
                _logger.LogInformation(
                    "Executing Background Service: {BackgroundServiceName}, generating invoices",
                    typeof(SubscriptionInvoiceBackgroundJob).Name
                );
                // TODO generate invoices

                _logger.LogInformation(
                    "Executing Background Service: {BackgroundServiceName}, sending notifications",
                    typeof(SubscriptionInvoiceBackgroundJob).Name
                );
                // TODO alert send emails or push notifications

                _logger.LogInformation(
                    "Executing Background Service: {BackgroundServiceName} work completed",
                    typeof(SubscriptionInvoiceBackgroundJob).Name
                );
                // TODO Update `isCompleted = true;` in DB
            }
            catch (Exception Exception)
            {
                _logger.LogError(
                    "Executing Background Service: {BackgroundServiceName} stopped because of an error {Exception}",
                    typeof(SubscriptionInvoiceBackgroundJob).Name,
                    Exception
                );
                break; // TODO do I need to break?
            }
        }
    }

    public static double MillisecondsUntilTwelveAmUtc()
    {
        // Get current UTC time
        DateTime utcNow = DateTime.UtcNow;

        // Create a DateTime for 12 AM UTC today
        DateTime targetTimeUtc =
            new(utcNow.Year, utcNow.Month, utcNow.Day, 12, 0, 0);

        // If the target time has already passed today, add 24 hours to get
        // tomorrow's 12 AM UTC
        if (utcNow > targetTimeUtc)
        {
            targetTimeUtc = targetTimeUtc.AddDays(1);
        }

        // Calculate the time difference
        TimeSpan timeLeft = targetTimeUtc - utcNow;

        // Return the total milliseconds
        return timeLeft.TotalMilliseconds;
    }
}