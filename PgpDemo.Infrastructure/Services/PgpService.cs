using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PgpCore;
using PgpDemo.Core.Interfaces;
using PgpDemo.Core.Model;

namespace PgpDemo.Infrastructure.Services
{
    public class PgpService : IPgpService
    {
        public PgpDto GenerateKeys(string userName, string password, int length = 2048)
        {
            var pgpDto = new PgpDto();
            using (var pgp = new PGP())
            {
                var publicKeyStream = new MemoryStream();
                var privateKeyStream = new MemoryStream();

                pgp.GenerateKey(publicKeyStream, privateKeyStream, userName, password, length);

                pgpDto.PrivateKey = GetStringFromStream(privateKeyStream);
                pgpDto.PublicKey = GetStringFromStream(publicKeyStream);
            }

            return pgpDto;
        }

        private string GetStringFromStream(MemoryStream ms)
        {
            ms.Position = 0;
            var sr = new StreamReader(ms);
            return sr.ReadToEnd();
        }

        private static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public void EncryptFile(string inputFilePath, string outputFilePath, string publicKey)
        {
            using (var pgp = new PGP())
            {
                using (FileStream inputFileStream = new FileStream(inputFilePath, FileMode.Open))
                using (Stream outputFileStream = File.Create(outputFilePath))
                using (Stream publicKeyStream = GenerateStreamFromString(publicKey))
                    pgp.EncryptStream(inputFileStream, outputFileStream, publicKeyStream, true, true);
            }
        }

        public void DecryptFile(string inputFilePath, string outputFilePath, string privateKey, string password)
        {
            using (var pgp = new PGP())
            {
                using (FileStream inputFileStream =
                    new FileStream(inputFilePath, FileMode.Open))
                using (Stream outputFileStream = File.Create(outputFilePath))
                using (Stream privateKeyStream = GenerateStreamFromString(privateKey))
                    pgp.DecryptStream(inputFileStream, outputFileStream, privateKeyStream, password);
            }
        }
    }
}
