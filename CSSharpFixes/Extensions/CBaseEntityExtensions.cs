using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;

namespace CSSharpFixes.Extensions;

public static class CBaseEntityExtensions
{
    public static void SetGroundEntity(this CBaseEntity? baseEntity, IntPtr groundEntityHandle)
    {
        if (baseEntity is null) return;
        
        MemoryFunctionVoid<IntPtr, IntPtr, IntPtr> setGroundEntityFunc = new(GameData.GetSignature("SetGroundEntity"));
        Action<IntPtr, IntPtr, IntPtr> setGroundEntity = setGroundEntityFunc.Invoke;
        setGroundEntity(baseEntity.Handle, groundEntityHandle, IntPtr.Zero);
    }
    
    // Add a setter for BaseVelocity to CBaseEntity so it can be set with a Vector in 1 line
    public static void SetBaseVelocity(this CBaseEntity? baseEntity, System.Numerics.Vector3 baseVelocity)
    {
        if(baseEntity is null) return;
        if(!baseEntity.IsValid) return;
        baseEntity.BaseVelocity.X = baseVelocity.X;
        baseEntity.BaseVelocity.Y = baseVelocity.Y;
        baseEntity.BaseVelocity.Z = baseVelocity.Z;
    }
    
    public static void TeleportPositionOnly(this CBaseEntity? baseEntity, System.Numerics.Vector3 position)
    {
        if(baseEntity is null) return;
        if(!baseEntity.IsValid) return;
        baseEntity.Teleport(position);
        /*VirtualFunction.CreateVoid<IntPtr, IntPtr, IntPtr, IntPtr>(baseEntity.Handle, GameData.GetOffset("CBaseEntity_Teleport"))(
            baseEntity.Handle, position.Handle, IntPtr.Zero, IntPtr.Zero);*/
    }
}