using Content.Server.Storage.Components;
using Content.Shared.Whitelist;
using Robust.Shared.Map;
using Robust.Shared.Physics.Components;
using Robust.Shared.Timing;
using Robust.Shared.Prototypes;
using Content.Server.Empire.OreTrasporter.Components;
using Content.Shared.Storage.EntitySystems;
using Content.Shared.Storage;
using Content.Shared.Interaction;
using Content.Shared.Stacks;
using Content.Shared.Examine;
using Content.Shared.Popups;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Content.Shared.Store.Components;
using Content.Shared.Store;
using Content.Shared.Interaction.Events;
using Robust.Shared.Configuration;
using Content.Server.Empire.PointsWithdraw.Components;

public sealed class OreTransporterSystem : EntitySystem
{
    [Dependency] private readonly EntityWhitelistSystem _whitelistSystem = default!;
    [Dependency] private readonly SharedStorageSystem _storage = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] protected readonly SharedAudioSystem _audioSystem = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<OreTransporterComponent, MapInitEvent>(OnTransporterInit);
        SubscribeLocalEvent<OreTransporterComponent, InteractUsingEvent>(OnInteractUsing);

        SubscribeLocalEvent<OreTransporterComponent, ExaminedEvent>(OnExamineTransporter);
        SubscribeLocalEvent<PointsWithdrawComponent, ExaminedEvent>(OnExamineWithdraw);
    }

    private void OnTransporterInit(EntityUid uid, OreTransporterComponent comp, MapInitEvent args)
    {

        var XForm = Transform(uid);
        var query = EntityQueryEnumerator<OreReciverComponent>();

        while(query.MoveNext(out var recUid, out var recComp))
        {
            var recXForm = Transform(recUid);
            comp.Reciver = recUid;
        }
    }

    private void OnInteractUsing(Entity<OreTransporterComponent> ent, ref InteractUsingEvent args)
    {
        if (args.Handled)
            return;
        
        if(HasComp<TransportableOreComponent>(args.Used))
        {
            if (!TryComp<TransportableOreComponent>(args.Used, out var ore))
                return;
            if (!TryComp<StorageComponent>(ent.Comp.Reciver, out var storage))
                return;
            if (!TryComp<StackComponent>(args.Used, out var num))
                return;

            if (!_storage.HasSpace((ent.Comp.Reciver, storage)))
                return;

            _storage.Insert(ent.Comp.Reciver, args.Used, out var stacked, storageComp: storage, playSound: true);

            float value = (ent.Comp.ValueModifier * ore.OreValue) * num.Count;
            ent.Comp.LogisticPoints += value;

        }
        if(HasComp<PointsWithdrawComponent>(args.Used))
        {
            if (!TryComp<PointsWithdrawComponent>(args.Used, out var num2))
                return;
            
            _popup.PopupClient(Loc.GetString("points-withdraw-value-insert"), args.User, args.User);

            num2.Balance += ent.Comp.LogisticPoints;
            ent.Comp.LogisticPoints = 0f;

            if (num2.SoundWithdraw != null)
                _audioSystem.PlayPredicted(num2.SoundWithdraw, args.Used, args.Used, num2.SoundWithdraw.Params);
        }
        
    }

    private void OnExamineTransporter(EntityUid uid, OreTransporterComponent comp, ExaminedEvent args)
    {
        args.PushMarkup(Loc.GetString("ore-transpotrer-value") + " " + comp.LogisticPoints.ToString());
    }

    private void OnExamineWithdraw(EntityUid uid, PointsWithdrawComponent comp, ExaminedEvent args)
    {
        args.PushMarkup(Loc.GetString("points-withdraw-value") + " " + comp.Balance.ToString());
    }

    

}