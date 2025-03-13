using Content.Shared.Store.Components;
using Content.Shared.Store;
using Content.Server.Empire.OreTrasporter.Components;
using Content.Server.Empire.PointsWithdraw.Components;
using Robust.Shared.Timing;
using Content.Shared.Interaction;
using Content.Server.Store.Systems;
using Content.Shared.Access.Systems;
using Content.Shared.FixedPoint;
using Robust.Shared.Prototypes;

public sealed class PointWithdrawnSystem : EntitySystem
{
    public const string CurrencyPrototype = "LogisticsPoints";

    [Dependency] private readonly StoreSystem _store = default!;

    public FixedPoint2 balance;

    public override void Initialize()
    {
        SubscribeLocalEvent<PointsWithdrawComponent, BeforeRangedInteractEvent>(OnInteractWithdraw);
    }

    private void OnInteractWithdraw(Entity<PointsWithdrawComponent> ent, ref BeforeRangedInteractEvent args)
    {   
        if (!args.CanReach)
            return;
        if(args.Target != null)
        {
            if(HasComp<PointAddOnUseComponent>(args.Target.Value))
            {
                var store = EnsureComp<StoreComponent>(args.Target.Value);
                balance = ent.Comp.Balance;
                _store.TryAddCurrency(new Dictionary<string, FixedPoint2> { { CurrencyPrototype, balance } }, args.Target.Value, store);
                ent.Comp.Balance = 0f;
            }
            
        }
    }
}