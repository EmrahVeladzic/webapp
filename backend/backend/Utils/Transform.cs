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
            Matrix4x4 translationMatrix = Matrix4x4.CreateTranslation(Translation);
            Matrix4x4 rotationMatrix = Matrix4x4.CreateFromQuaternion(Rotation);
            Matrix4x4 scaleMatrix = Matrix4x4.CreateScale(Scale);

            return scaleMatrix * rotationMatrix * translationMatrix;
        }


        public Transform(Matrix4x4 matrix)
        {
                       
            this.Translation = matrix.Translation;


            this.Scale = new Vector3(
                new Vector3(matrix.M11, matrix.M12, matrix.M13).Length(),
                new Vector3(matrix.M21, matrix.M22, matrix.M23).Length(),
                new Vector3(matrix.M31, matrix.M32, matrix.M33).Length()
            );

            Matrix4x4 rotationMatrix = new Matrix4x4(
                matrix.M11 / Scale.X, matrix.M12 / Scale.X, matrix.M13 / Scale.X, 0,
                matrix.M21 / Scale.Y, matrix.M22 / Scale.Y, matrix.M23 / Scale.Y, 0,
                matrix.M31 / Scale.Z, matrix.M32 / Scale.Z, matrix.M33 / Scale.Z, 0,
                0, 0, 0, 1
            );

            this.Rotation = Quaternion.CreateFromRotationMatrix(rotationMatrix);

         
        }

    }
}
