using NitroxClient.Communication.Packets.Processors.Abstract;
using NitroxClient.MonoBehaviours;
using NitroxClient.Unity.Helper;
using NitroxModel.Packets;
using NitroxModel_Subnautica.DataStructures;
using UnityEngine;

namespace NitroxClient.Communication.Packets.Processors;

public class GrapplingHookMovementProcessor : ClientPacketProcessor<GrapplingHookMovement>
{
    public override void Process(GrapplingHookMovement packet)
    {
        Exosuit exosuit = NitroxEntity.RequireObjectFrom(packet.ExosuitId).RequireComponent<Exosuit>();

        IExosuitArm arm = null;

        switch (packet.ArmSide)
        {
            case Exosuit.Arm.Left:
                arm = exosuit.leftArm;
                break;
            case Exosuit.Arm.Right:
                arm = exosuit.rightArm;
                break;
        }

        if (arm is not ExosuitGrapplingArm grapplingArm)
        {
            Log.Error($"{packet.ArmSide} arm of exosuit {packet.ExosuitId} is not a grappling arm");
            return;
        }

        if (grapplingArm.hook.resting)
        {
            grapplingArm.rope.LaunchHook(35);
        }

        Rigidbody rb = grapplingArm.hook.RequireComponent<Rigidbody>();

        rb.position = packet.Position.ToUnity();
        rb.velocity = packet.Velocity.ToUnity();
        rb.rotation = packet.Rotation.ToUnity();
    }
}
