using CounterStrikeSharp.API.Core;

namespace CSSharpFixes.Extensions;

public static class CGameSceneNodeExtensions
{
    public static float[,]? EntityToWorldTransform(this CGameSceneNode? sceneNode)
    {
        if (sceneNode is null) return null;
        
        // matrix3x4_t
        float[,] mat = new float[3, 4];

		System.Numerics.Vector3 angles = (System.Numerics.Vector3)sceneNode.AbsRotation;

		Utils.SinCos(Utils.DegToRad(angles.Y), out float sy, out float cy); // YAW
		Utils.SinCos(Utils.DegToRad(angles.X), out float sp, out float cp); // PITCH
        Utils.SinCos(Utils.DegToRad(angles.Z), out float sr, out float cr); // ROLL

        mat[0, 0] = cp * cy;
        mat[1, 0] = cp * sy;
        mat[2, 0] = -sp;

        float crcy = cr * cy;
        float crsy = cr * sy;
        float srcy = sr * cy;
        float srsy = sr * sy;
        
        mat[0, 1] = sp * srcy - crsy;
        mat[1, 1] = sp * srsy + crcy;
        mat[2, 1] = sr * cp;
        
        mat[0, 2] = sp * crcy + srsy;
        mat[1, 2] = sp * crsy - srcy;
        mat[2, 2] = cr * cp;

		System.Numerics.Vector3 pos = (System.Numerics.Vector3)sceneNode.AbsOrigin;
        mat[0, 3] = pos.X;
        mat[1, 3] = pos.Y;
        mat[2, 3] = pos.Z;

        return mat;
    }
}