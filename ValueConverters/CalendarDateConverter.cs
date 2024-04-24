namespace GsServer.Protobufs;

public partial class CalendarDate
{
  public CalendarDate(int year, int month, int day)
  {
    Year = year;
    Month = month;
    Day = day;
  }

  public static implicit operator DateOnly(CalendarDate date)
  {
    return new DateOnly(
      date.Year,
      date.Month,
      date.Day
    );
  }

  public static implicit operator CalendarDate(DateOnly date)
  {
    return new CalendarDate()
    {
      Day = date.Day,
      Month = date.Month,
      Year = date.Year
    };
  }
}
