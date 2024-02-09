namespace backend.Interfaces
{
    public interface IBMP_Repository
    {
        public Task UploadBMP(byte[] BMPData);

    }
}
