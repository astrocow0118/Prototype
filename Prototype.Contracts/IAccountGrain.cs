namespace Prototype.Contracts;

public interface IAccountGrain : IGrainWithStringKey
{
    Task<Account> Authenticate();
}

public sealed record class Account(
    string AccountKey,
    Guid PublicId,
    DateTime CreateDate
);