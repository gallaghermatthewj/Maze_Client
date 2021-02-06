using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ConversionUtility
{
    public static byte[] ToBytes<T>(this T[,,] array) where T : struct
    {

        var buffer = new byte[array.GetLength(0) * array.GetLength(1) * System.Runtime.InteropServices.Marshal.SizeOf(typeof(T))];
        Buffer.BlockCopy(array, 0, buffer, 0, buffer.Length);
        return buffer;
    }
    public static void FromBytes<T>(this T[,,] _destinationArray, byte[] _sourceBufferByteArray) where T : struct
    {
        var len = Math.Min(_destinationArray.GetLength(0) * _destinationArray.GetLength(1) * System.Runtime.InteropServices.Marshal.SizeOf(typeof(T)), _sourceBufferByteArray.Length);
        Buffer.BlockCopy(_sourceBufferByteArray, 0, _destinationArray, 0, len);
    }
}
