namespace gs_server.ControlFlow.Mensalidades;

// MissingRequiredField: When a required field is not provided during creation or editing.
// InvalidFormat: When the format of a field is not valid(e.g., email, phone number, etc.).
// DuplicateEntry: When a unique field(e.g., email) is duplicated during creation or editing.
// RecordNotFound: When trying to access or manipulate a non-existent record.
public enum MensalidadeErrors
{
  MissingRequiredField,
  InvalidFormat,
  DuplicateEntry,
  RecordNotFound,
}

