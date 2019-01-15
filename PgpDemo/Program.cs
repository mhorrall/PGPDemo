using System;
using PgpDemo.Core.Interfaces;
using PgpDemo.Core.Model;
using PgpDemo.Infrastructure.Services;

namespace PgpDemo
{
    class Program
    {
        // Instantiate service, this could be done with an IOC container
        private static readonly IPgpService PgpService = new PgpService();
        private static PgpDto _pgpDto;

        private static string _username = "testUser";
        private static string _password = "password";

        static void Main(string[] args)
        {
            GenerateKeys();

            EncryptFile("TestEncrypt.txt");

            DecryptFile("EncryptedFile.pgp");
        }

        private static void GenerateKeys()
        {
            // Input username, password, strength if desired default is 2048
            _pgpDto = PgpService.GenerateKeys(_username, _password);

            Console.WriteLine($"Public Key: \r\n {_pgpDto.PublicKey}");
            Console.WriteLine($"Private Key: \r\n {_pgpDto.PrivateKey}");
            Console.ReadLine();
        }

        private static void EncryptFile(string filePath)
        {
            PgpService.EncryptFile(filePath, "EncryptedFile.pgp", _pgpDto.PublicKey);
            Console.WriteLine("File has been encrypted");
            Console.ReadLine();
        }

        private static void DecryptFile(string filePath)
        {
            PgpService.DecryptFile(filePath, "DecryptedFile.txt", _pgpDto.PrivateKey, _password);
            Console.WriteLine("File has been decrypted");
            Console.ReadLine();
        }
    }
}
