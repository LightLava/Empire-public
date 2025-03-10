using Content.Shared.Item.ItemToggle.Components;

namespace Content.Shared.Item;


public abstract partial class SharedItemSystem
{
    private void InitializeSupplementation()
    {
        SubscribeLocalEvent<ItemToggleSizeComponent, ItemToggleActivateAttemptEvent>(OnToggleAttempt);
    }

    private void OnToggleAttempt(EntityUid uid, ItemToggleSizeComponent itemToggleSize, ref ItemToggleActivateAttemptEvent args)
    {
        if (args.User == null) return;
        if (_handsSystem.IsHolding(args.User.Value, uid)) return;

        args.Cancelled = true;
    }
}
