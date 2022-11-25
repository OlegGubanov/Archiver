using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archiver.Algorithms
{
    public interface ICompressionAlgorithm
    {
        public void CompressFile(string filePath, string archivePath)
        {
            var data = File.ReadAllBytes(filePath);
            var compressedData = CompressData(data);
            File.WriteAllBytes(archivePath, compressedData);
        }

        public byte[] CompressData(byte[] data);
        public void DecompressFile(string archivePath, string filePath)
        {
            var data = File.ReadAllBytes(archivePath);
            var decompressedData = DecompressData(data);
            File.WriteAllBytes(filePath, decompressedData);
        }
        public byte[] DecompressData(byte[] data);
    }
}
