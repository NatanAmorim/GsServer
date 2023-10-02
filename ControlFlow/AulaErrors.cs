namespace gs_server.ControlFlow.Aulas;

// MissingRequiredField: When a required field(e.g., sale date, products) is not provided during sale creation.
// InvalidFormat: When the format of a field is not valid (e.g., invalid sale ID format).
// OutOfRange: When a value is out of the acceptable range(e.g., age, date).
// OverlappingSchedule: When the lecture schedule overlaps with another scheduled event.
// CapacityExceeded: When the number of participants registered for the lecture exceeds the capacity.
public enum AulaErros
{
  MissingRequiredField,
  InvalidFormat,
  OutOfRange,
  OverlappingSchedule,
  CapacityExceeded,
}