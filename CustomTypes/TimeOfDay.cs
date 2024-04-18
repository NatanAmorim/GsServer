namespace GsServer.Protobufs;

public partial class TimeOfDay
{
  public TimeOfDay(int hour, int minute)
  {
    Hour = hour;
    Minute = minute;
  }

  public static implicit operator TimeOnly(TimeOfDay time)
  {
    return new TimeOnly(
      time.Hour,
      time.Minute
    );
  }

  public static implicit operator TimeOfDay(TimeOnly time)
  {
    return new TimeOfDay()
    {
      Hour = time.Hour,
      Minute = time.Minute
    };
  }
}
