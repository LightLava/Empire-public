using Robust.Shared.Prototypes;
using Robust.Shared.Audio;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Empire.PointsWithdraw.Components;

[RegisterComponent]
public sealed partial class PointsWithdrawComponent : Component
{
    /// <summary>
    /// Баланс
    /// </summary>
    [DataField]
    public float Balance = 0f;

    [ViewVariables(VVAccess.ReadWrite), DataField("soundWithdraw")]
    public SoundSpecifier? SoundWithdraw = new SoundPathSpecifier("/Audio/Effects/Cargo/ping.ogg")
    {
        Params = AudioParams.Default.WithVolume(-5f),
    };
}