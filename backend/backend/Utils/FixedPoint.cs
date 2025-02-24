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

    public struct FVector2
    {
        public Int32 X { get; set; }

        public Int32 Y { get; set; }

        public FVector2(Vector2 v, byte prec = 12)
        {
            this.X = Utils.FixedPoint.GetFixed<Int32>(v.X, prec);

            this.Y = Utils.FixedPoint.GetFixed<Int32>(v.Y, prec);

        }

        public void Serialize(List<byte> data)
        {
            PrimitiveSerialization.SerializePrimitive(X, data);
            PrimitiveSerialization.SerializePrimitive(Y, data);
        }

    }

    public struct FVector3 
    {

        public Int32 X { get; set; }

        public Int32 Y { get; set; }
        
        public Int32 Z { get; set; }

        public FVector3(Vector3 v, byte prec = 12)
        {
            this.X = Utils.FixedPoint.GetFixed<Int32>(v.X, prec);

            this.Y = Utils.FixedPoint.GetFixed<Int32>(v.Y, prec);

            this.Z = Utils.FixedPoint.GetFixed<Int32>(v.Z, prec);

        }

        public void Serialize(List<byte> data)
        {
            PrimitiveSerialization.SerializePrimitive(X, data);
            PrimitiveSerialization.SerializePrimitive(Y, data);
            PrimitiveSerialization.SerializePrimitive(Z, data);
        }
    }

    public struct FQuaternion
    {

        public Int32 X { get; set; }

        public Int32 Y { get; set; }

        public Int32 Z { get; set; }

        public Int32 W { get; set; }

        public FQuaternion(Quaternion q,  byte prec = 12)
        {
            Quaternion temp = Quaternion.Normalize(q);

            this.X = Utils.FixedPoint.GetFixed<Int32>(temp.X, prec);
                                                      
            this.Y = Utils.FixedPoint.GetFixed<Int32>(temp.Y, prec);
                                                      
            this.Z = Utils.FixedPoint.GetFixed<Int32>(temp.Z, prec);
                                                      
            this.W = Utils.FixedPoint.GetFixed<Int32>(temp.W, prec);

        }

        public void Serialize(List<byte> data)
        {
          
            PrimitiveSerialization.SerializePrimitive(X, data);
            PrimitiveSerialization.SerializePrimitive(Y, data);
            PrimitiveSerialization.SerializePrimitive(Z, data);
            PrimitiveSerialization.SerializePrimitive(W, data);
        }

    }

    public struct FTransform
    {

        public FVector3 Translation { get; set; }

        public FQuaternion Rotation { get; set; }

        public FVector3 Scale { get; set; }


        public FTransform(Transform t, byte prec = 12)
        {
            this.Translation = new FVector3(t.Translation, prec);
            this.Rotation = new FQuaternion(t.Rotation, prec);
            this.Scale = new FVector3(t.Scale, prec);
        }

        public void Serialize(List<byte> data)
        {
            this.Translation.Serialize(data);
            this.Rotation.Serialize(data);
            this.Scale.Serialize(data);
        }

    }


}
