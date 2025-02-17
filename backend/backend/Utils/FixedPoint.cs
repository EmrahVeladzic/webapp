using System.Numerics;

namespace backend.Utils
{
    public static class FixedPoint
    {
        private static readonly Type[] AllowedTypes = { typeof(UInt16), typeof(Int16), typeof(UInt32), typeof(Int32) };
        public static T GetFixed<T>(float Real, byte PrecisionBits) where T : INumber<T>
        {
            if (!AllowedTypes.Contains(typeof(T)))
            {
                throw new NotSupportedException($"Type {typeof(T)} is not supported.");
            }
            else
            {
                Int32 Data = (Int32)(Real * (float)(1 << PrecisionBits));
                return T.CreateTruncating(Data);
            }
        }
        public static float GetFloat<T>(T Approx, byte PrecisionBits) where T : INumber<T>
        {
            {
                if (!AllowedTypes.Contains(typeof(T)))
                {
                    throw new NotSupportedException($"Type {typeof(T)} is not supported.");
                }
                else
                {
                    return float.CreateTruncating(Approx) / (float)(1 << PrecisionBits);
                }
            }
        }
    }
}
