namespace Prototype.Contracts;

[Alias("Prototype.Contracts.IPlayerGain")]
public interface IPlayerGain : IGrainWithGuidKey
{
    [Alias("Get")]
    Task<PlayerSnapshot> Get();
}

public sealed record class PlayerSnapshot(Guid Id, string Name, int Level);