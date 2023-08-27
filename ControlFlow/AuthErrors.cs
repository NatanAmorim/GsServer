namespace sgd_cms.ControlFlow.Auth;

// MissingCredentials: This error occurs when the user doesn't provide both the username and password required for authentication. The user is prompted to enter both the username and password to proceed.
// InvalidCredentials: This error arises when the provided username and password combination is not valid. It indicates that the user's authentication credentials are incorrect and they need to provide accurate credentials to log in.
// AccountLocked: This error indicates that the user's account has been temporarily locked due to a certain number of unsuccessful login attempts. The user is advised to contact support for assistance in unlocking the account.
// InactiveAccount: This error occurs when the user's account is marked as inactive, often due to certain conditions such as non-payment or other administrative reasons. The user is directed to contact support to reactivate their account.
// RolePermissionDenied: This error occurs when a user attempts to access a resource or perform an action for which they don't have the necessary role-based permissions. The user is informed that they don't have the required privileges to perform the action.
public enum AuthErrors
{
  MissingCredentials,
  InvalidCredentials,
  AccountLocked,
  InactiveAccount,
  RolePermissionDenied
}