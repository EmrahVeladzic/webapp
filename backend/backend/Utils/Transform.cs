using System.Numerics;

namespace backend.Utils
{
    public class Transform
    {

        public Vector3 Translation { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Scale { get; set; }

        public Transform(Vector3 t = default, Quaternion r = default, Vector3 s = default)
        {
            Translation = t == default ? Vector3.Zero : t;
            Rotation = r == default ? Quaternion.Identity : r;
            Scale = s == default ? Vector3.One : s;
        }

        public Matrix4x4 ToMatrix()
        {
            Matrix4x4 mat = Matrix4x4.Identity;




            return mat;
        }

    }
}
