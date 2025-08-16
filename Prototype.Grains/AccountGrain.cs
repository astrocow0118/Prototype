using Prototype.Contracts;

namespace Prototype.Grains;

[GenerateSerializer]
public sealed class AccountState
{
    [Id(0)] public string AccountKey { get; set; } = "";
    [Id(1)] public Guid PublicId { get; set; }
    [Id(2)] public DateTime CreateAt { get; set; }
}

public class AccountGrain(
    [PersistentState("account")] IPersistentState<AccountState> state
) : Grain, IAccountGrain
{
    private readonly IPersistentState<AccountState> _state = state;

    public async Task<Account> Authenticate()
    {
        if (!_state.RecordExists)
        {
            _state.State.AccountKey = this.GetPrimaryKeyString();
            _state.State.PublicId = Guid.NewGuid();
            _state.State.CreateAt = DateTime.UtcNow;
            await _state.WriteStateAsync();
        }

        return new Account(
            _state.State.AccountKey,
            _state.State.PublicId,
            _state.State.CreateAt
        );
    }
}
