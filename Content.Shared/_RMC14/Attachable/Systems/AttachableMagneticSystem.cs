using Content.Shared._RMC14.Armor.Magnetic;
using Content.Shared._RMC14.Attachable.Components;
using Content.Shared._RMC14.Attachable.Events;
using Robust.Shared.Timing;

namespace Content.Shared._RMC14.Attachable.Systems;

public sealed class AttachableMagneticSystem : EntitySystem
{
    [Dependency] private readonly RMCMagneticSystem _magneticSystem = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<AttachableMagneticComponent, AttachableAlteredEvent>(OnAttachableAltered);
    }

    private void OnAttachableAltered(Entity<AttachableMagneticComponent> attachable, ref AttachableAlteredEvent args)
    {
        if (_timing.ApplyingState)
            return;

        switch (args.Alteration)
        {
            case AttachableAlteredType.Attached:
                var comp = EnsureComp<RMCMagneticItemComponent>(args.Holder);
                _magneticSystem.SetMagnetizeToSlots((args.Holder, comp), attachable.Comp.MagnetizeToSlots);
                break;

            case AttachableAlteredType.Detached:
                RemCompDeferred<RMCMagneticItemComponent>(args.Holder);
                break;
        }
    }
}
