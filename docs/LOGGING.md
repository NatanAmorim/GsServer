# Logging

Source <https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-8.0>
Source <https://docs.aws.amazon.com/lambda/latest/operatorguide/parse-logs.html>
Source <https://docs.aws.amazon.com/wellarchitected/latest/serverless-applications-lens/opex-logging.html>
Source <https://www.loggly.com/use-cases/what-is-structured-logging-and-how-to-use-it/>
Source <https://cheatsheetseries.owasp.org/cheatsheets/Logging_Cheat_Sheet.html#event-attributes>
Source <https://stripe.com/blog/canonical-log-lines>
Source <https://betterstack.com/community/guides/logging/how-to-view-and-configure-nginx-access-and-error-logs/>
Source <https://betterstack.com/community/guides/logging/how-to-view-and-configure-linux-logs-on-ubuntu-20-04/>
Source <https://betterstack.com/community/guides/logging/monitoring-linux-auth-logs/>
Source <https://betterstack.com/community/guides/logging/how-to-control-journald-with-journalctl/>
Source <https://betterstack.com/community/guides/logging/how-to-start-logging-with-postgresql/>
Source <https://betterstack.com/community/guides/logging/how-to-start-logging-with-docker/>
Source <https://betterstack.com/community/guides/logging/aws-logging/>
Source <https://betterstack.com/community/guides/logging/logging-best-practices/>
Source <https://betterstack.com/community/guides/logging/how-to-start-logging-with-net/>

## Serilog

[Serilog](https://github.com/serilog/serilog) is a diagnostic logging library for .NET applications, Serilog's support for structured logging shines when instrumenting complex, distributed, and asynchronous applications and systems.

To learn more about Serilog, check out the [documentation](https://github.com/serilog/serilog/wiki) - you'll find information there on the most common scenarios. If Serilog isn't working the way you expect, you may find the [troubleshooting guide](https://github.com/serilog/serilog/wiki/Debugging-and-Diagnostics) useful.

Serilog [provides sinks](https://github.com/serilog/serilog/wiki/Provided-Sinks) for writing log events to storage in various formats. Many of the sinks listed below are developed and supported by the wider Serilog community; please direct questions and issues to the relevant repository.

## Log levels

`Microsoft.Extensions.Logging` namespace and consists of the 7 levels:

- `Critical` — used for reporting about errors that are forcing shutdown of the application.
- `Error` — used for logging serious problems occurring during execution of the program.
- `Warning`  — used for reporting non-critical unusual behavior.
- `Info` — used for informative messages highlighting the progress of the application for sysadmins and end users.
- `Debug` — used for debugging messages with extended information about application processing.
- `Trace` — used for tracing the code.
- `None` — not used for writing log messages. Specifies that a logging category should not write any messages.

However in this application, the Serilog's levels system was used. The system consists of the 6 levels:

- `Fatal` — used for reporting about errors that are forcing shutdown of the application.
- `Error` — used for logging serious problems occurred during execution of the program.
- `Warning`  — used for reporting non-critical unusual behavior.
- `Information` — used for informative messages highlighting the progress of the application for sysadmins and end users.
- `Debug` — used for debugging messages with extended information about application processing.
- `Verbose` — the noisiest level, used for tracing the code.

## Data to exclude

> [!IMPORTANT]\
> *DON'T* log any sensitive data like name, personal documents, Gov issued ID, phone number, GPS location, email address, username, password, etc..\
> Protect user privacy, especially after multiple failed attempts.

## What to include in a log

While the message often stands in the center of our attention, in reality there's often additional info that we might need. Information such as:

- `Timestamp` - Indicates when an event has occurred.
- `Computer/Server Name` - Useful in distributed systems to identify the source.
- `Process` ID - in case you are running multiple processes
- `Message` - an actual message with some content
- `Stack Trace` - in case we are logging an error
- `Maybe` some additional variables/info

- `Timestamp`: Indicates when an event occurred.
- `Message`: The actual content or description of the event.
- `Stack Trace`: Relevant when logging errors.
- `Correlation ID`: A unique identifier to track related events across services, it helps trace the flow of an operation across multiple systems or services..
- `Thread ID`: Identifies the executing thread within a process.
- `Process ID (PID)`: Helps differentiate logs from multiple processes.
- `IP Address`: Records the originating IP for network-related events.
- `Computer/Server Name`: Useful in distributed systems to identify the source.
- `Additional Variables/Info`: Any other context-specific data.

## Which events to log

Where possible, always log:

The level and content of security monitoring, alerting, and reporting needs to be set during the requirements and design stage of projects, and should be proportionate to the information security risks. This can then be used to define what should be logged.

There is no one size fits all solution, and a blind checklist approach can lead to unnecessary "alarm fog" that means real problems go undetected.

- Input validation failures e.g. protocol violations, unacceptable encodings, invalid parameter names and values
- Output validation failures e.g. database record set mismatch, invalid data encoding
- Authentication successes and failures
- Authorization (access control) failures
- Session management failures e.g. cookie session identification value modification
- Application errors and system events e.g. syntax and runtime errors, connectivity problems, performance issues, third party service error messages, file system errors, file upload virus detection, configuration changes
- Application and related systems start-ups and shut-downs, and logging initialization (starting, stopping or pausing)
- Use of higher-risk functionality e.g. network connections, addition or deletion of users, changes to privileges, assigning users to tokens, adding or deleting tokens, use of systems administrative privileges, access by application administrators, all actions by users with administrative privileges, access to payment cardholder data, use of data encrypting keys, key changes, creation and deletion of system-level objects, data import and export including screen-based reports, submission of user-generated content - especially file uploads
- Legal and other opt-ins e.g. permissions for mobile phone capabilities, terms of use, terms & conditions, personal data usage consent, permission to receive marketing communications
