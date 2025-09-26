using System.Runtime.InteropServices;
using System.Text;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;

namespace CSSharpFixes;

public class Utils
{
    public static T BuildMemoryFunction<T>(string signatureName) where T : BaseMemoryFunction
    {
        if (string.IsNullOrEmpty(signatureName))
            throw new ArgumentException("Signature name must not be null or empty", nameof(signatureName));

		string? signature;
		try
        {
            signature = GameData.GetSignature(signatureName);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to build memory function of type {typeof(T).FullName}. There was an error attempting to get the signature '{signatureName}'.", ex);
        }

        if (string.IsNullOrEmpty(signature))
            throw new InvalidOperationException($"Failed to build memory function of type {typeof(T).FullName}. The signature '{signatureName}' was found but is empty.");

        object? instance = Activator.CreateInstance(typeof(T), signature);
        
        if (instance == null)
            throw new InvalidOperationException($"Failed to build memory function of type {typeof(T).FullName}. The instance of type {typeof(T).FullName} was null.");
        
        return (T)instance;
    }
    
    // Mimic the behavior of the C++ function std::vector<int16_t> CGameConfig::HexToByte(std::string_view src)
    public static List<byte> HexToByte(string src)
    {
        if (string.IsNullOrEmpty(src))
        {
            return [];
        }

        Func<char, byte> hexCharToByte = c =>
        {
            if (c >= '0' && c <= '9') return (byte)(c - '0');
            if (c >= 'A' && c <= 'F') return (byte)(c - 'A' + 10);
            if (c >= 'a' && c <= 'f') return (byte)(c - 'a' + 10);
            return 0xFF; // Invalid hex character
        };

        List<byte> result = [];
        bool isCodeStyle = src[0] == '\\';
        string pattern = isCodeStyle ? "\\x" : " ";
        string wildcard = isCodeStyle ? "2A" : "?";
        int pos = 0;

        while (pos < src.Length)
        {
            int found = src.IndexOf(pattern, pos);
            if (found == -1)
            {
                found = src.Length;
            }

            string str = src.Substring(pos, found - pos);
            pos = found + pattern.Length;

            if (string.IsNullOrEmpty(str)) continue;

            string byteStr = str;

            if (byteStr[..wildcard.Length] == wildcard)
            {
                result.Add(0xFF); // Representing wildcard as 0xFF
                continue;
            }

            if (byteStr.Length < 2)
            {
                return []; // Invalid byte length
            }

            byte high = hexCharToByte(byteStr[0]);
            byte low = hexCharToByte(byteStr[1]);

            if (high == 0xFF || low == 0xFF)
            {
                return []; // Invalid hex character
            }

            result.Add((byte)((high << 4) | low));
        }

        return result;
    }
    
    public static string ByteToHex(List<byte> bytes, bool useCodeStyle = false)
    {
        if (bytes == null || bytes.Count == 0)
        {
            return string.Empty;
        }

        Func<byte, char> byteToHexChar = b =>
        {
            if (b < 10) return (char)(b + '0');
            return (char)(b - 10 + 'A');
        };

        StringBuilder result = new();
        string pattern = useCodeStyle ? "\\x" : " ";

        foreach (byte b in bytes)
        {
            if (b == 0xFF) // Wildcard representation
            {
                result.Append(useCodeStyle ? "\\x2A" : "??");
            }
            else
            {
                result.Append(byteToHexChar((byte)(b >> 4))); // High nibble
                result.Append(byteToHexChar((byte)(b & 0x0F))); // Low nibble
                if (useCodeStyle)
                {
                    result.Append(pattern); // Separator
                }
            }
        }

        // Remove the trailing separator if using code style
        if (useCodeStyle && result.Length > 0)
        {
            result.Remove(result.Length - pattern.Length, pattern.Length);
        }

        return result.ToString();
    }
    
    public static List<byte> ReadBytesFromAddress(IntPtr address, int length)
    {
        List<byte> result = [];
        for (int i = 0; i < length; i++)
        {
            result.Add(ReadByteFromAddress(address + i));
        }

        return result;
    }

    public static unsafe byte ReadByteFromAddress(IntPtr address)
    {
        return *(byte*)address.ToPointer();
    }
    
    public static void WriteBytesToAddress(IntPtr address, List<byte> bytes)
    {
        int patchSize = bytes.Count;
        if(patchSize == 0) throw new ArgumentException("Patch bytes list cannot be empty.");
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Memory.UnixMemoryUtils.PatchBytesAtAddress(address, [.. bytes], patchSize);
        }
        else
        {
            Memory.WinMemoryUtils.PatchBytesAtAddress(address, [.. bytes], patchSize);
        }
    }
    
    public static List<byte> FloatToByteArray(float value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        return [.. bytes];
    }
    
    public static byte[] ReadMemory(IntPtr address, int size)
    {
        byte[] buffer = new byte[size];
        Marshal.Copy(address, buffer, 0, size);
        return buffer;
    }

    public static bool IsSolid(CCollisionProperty collision)
    {
        return (collision.SolidType != SolidType_t.SOLID_NONE
                && !SolidFlagIsSet(collision, 0x4)); // FSOLID_NOT_SOLID = 0x4
    }
    
    public static bool SolidFlagIsSet(CCollisionProperty collision, byte flag)
    {
        return (collision.SolidFlags & flag) > 0;
    }
    
    public static void SinCos(float radians, out float sine, out float cosine)
    {
        sine = (float)Math.Sin(radians);
        cosine = (float)Math.Cos(radians);
    }

    public static float DegToRad(float degrees)
    {
        return (float)(Math.PI / 180) * degrees;
    }
    
    public static void VectorRotate(System.Numerics.Vector3 vecIn, float[,] matIn, ref System.Numerics.Vector3 vecOut)
    {
        vecOut.X = vecIn.X * matIn[0, 0] + vecIn.Y * matIn[0, 1] + vecIn.Z * matIn[0, 2];
        vecOut.Y = vecIn.X * matIn[1, 0] + vecIn.Y * matIn[1, 1] + vecIn.Z * matIn[1, 2];
        vecOut.Z = vecIn.X * matIn[2, 0] + vecIn.Y * matIn[2, 1] + vecIn.Z * matIn[2, 2];
    }
}