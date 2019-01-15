using PgpDemo.Core.Model;

namespace PgpDemo.Core.Interfaces
{
    public interface IPgpService
    {
        void DecryptFile(string inputFilePath, string outputFilePath, string privateKey, string password);
        void EncryptFile(string inputFilePath, string outputFilePath, string publicKey);
        PgpDto GenerateKeys(string userName, string password, int length = 2048);
    }
}