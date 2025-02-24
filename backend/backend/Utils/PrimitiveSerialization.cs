using System.Numerics;

namespace backend.Utils
{
    public static class PrimitiveSerialization
    {
        public static void SerializePrimitive(UInt16 data, List<byte>buffer)
        {                     
            
            buffer.AddRange(BitConverter.GetBytes(data));
                        
        }

        public static void SerializePrimitive(Int16 data, List<byte> buffer)
        {

            buffer.AddRange(BitConverter.GetBytes(data));

        }

        public static void SerializePrimitive(UInt32 data, List<byte> buffer)
        {

            buffer.AddRange(BitConverter.GetBytes(data));

        }

        public static void SerializePrimitive(Int32 data, List<byte> buffer)
        {

            buffer.AddRange(BitConverter.GetBytes(data));

        }

        public static void SerializePrimitive(UInt64 data, List<byte> buffer)
        {

            buffer.AddRange(BitConverter.GetBytes(data));

        }

        public static void SerializePrimitive(Int64 data, List<byte> buffer)
        {

            buffer.AddRange(BitConverter.GetBytes(data));

        }

        public static void SerializePrimitive(float data, List<byte> buffer)
        {

            buffer.AddRange(BitConverter.GetBytes(data));

        }

        public static void SerializePrimitive(double data, List<byte> buffer)
        {

            buffer.AddRange(BitConverter.GetBytes(data));

        }

    }
}
