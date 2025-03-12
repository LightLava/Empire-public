using Robust.Shared.Prototypes;
namespace Content.Server.Empire.OreTrasporter.Components;

[RegisterComponent]
public sealed partial class OreTransporterComponent : Component
{
    /// <summary>
    /// Баланс Очков
    /// </summary>
    [DataField]
    public float LogisticPoints = 0f;

    /// <summary>
    /// Модификатор получаемых очков
    /// </summary>
    [DataField]
    public float ValueModifier = 1f;

    /// <summary>
    /// Привязанный Энтити
    /// </summary>
    [DataField]
    public EntityUid Reciver;

}