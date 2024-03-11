namespace gs_server.BackgroundServices;

/// <summary>
/// Invoice: Lists the products or services delivered or provided, along with
/// their prices, it specifies the payment terms, including the due date by which
/// the customer should pay  and may  include detailed client information.
/// An invoice may indicate the existence of credit, as payment is not immediate.
/// </summary>
public class SubscriptionInvoiceBackgroundJob : BackgroundService
{
    private readonly ILogger<SubscriptionInvoiceBackgroundJob> _logger;
    public SubscriptionInvoiceBackgroundJob(ILogger<SubscriptionInvoiceBackgroundJob> logger)
    {
        _logger = logger;
    }

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
                _logger.LogInformation("Background subscription invoice service started.");

                // Delay for Until 12 AM UTC (9 AM Brasilia time) time asynchronously.
                await Task.Delay((int)Math.Ceiling(MillisecondsUntilTwelveAmUtc()), stoppingToken);

                _logger.LogInformation("Background subscription invoice service, generating invoices.");
                // TODO

                _logger.LogInformation("Background subscription invoice service, sending emails.");
                // TODO

                _logger.LogInformation("Background subscription invoice service work completed.");
            }
            catch (Exception Exception)
            {
                _logger.LogError(
                    "Background invoice service stopped because of an error {Exception}",
                    Exception
                );
                // break; // TODO do i need to break?
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