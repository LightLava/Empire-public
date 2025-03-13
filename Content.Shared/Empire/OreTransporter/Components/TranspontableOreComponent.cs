using Robust.Shared.Prototypes;
namespace Content.Server.Empire.OreTrasporter.Components;

[RegisterComponent]
public sealed partial class TransportableOreComponent : Component
{
    /// <summary>
    /// Ценность руды за еденицу
    /// </summary>
    [DataField]
    public float OreValue = 1f;

}