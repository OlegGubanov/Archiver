using Archiver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Archiver.Algorithms
{
    public class HuffmanAlgorithm : ICompressionAlgorithm
    {
        public byte[] CompressData(byte[] data)
        {
            var frequencies = CalculateFrequency(data);
            byte[] header = CreateHeader(data.Length, frequencies);
            var huffmanTree = CreateHuffmanTree(frequencies);
            var codes = CreateHuffmanCodes(huffmanTree);
            var compressedData = Compress(data, codes);
            return header.Concat(compressedData).ToArray();
        }

        private byte[] CreateHeader(int dataLength, int[] freqs)
        {
            List<byte> header = new List<byte>
            {
                (byte)(dataLength & 255),
                (byte)((dataLength >> 8) & 255),
                (byte)((dataLength >> 16) & 255),
                (byte)((dataLength >> 24) & 255)
            };
            for (int j = 0; j < 256; j++)
                header.Add((byte)freqs[j]);
            return header.ToArray();
        }

        private byte[] Compress(byte[] data, string[] codes)
        {
            List<byte> bits = new List<byte>();
            byte sum = 0;
            byte bit = 1;
            foreach (byte symbol in data)
                foreach (char c in codes[symbol])
                {
                    if (c == '1')
                        sum |= bit;
                    if (bit < 128)
                        bit <<= 1;
                    else
                    {
                        bits.Add(sum);
                        sum = 0;
                        bit = 1;
                    }
                }
            if (bit > 1)
                bits.Add(sum);
            return bits.ToArray();
        }

        public byte[] DecompressData(byte[] data)
        {
            ParseHeader(data, out int dataLength, out int startIndex, out int[] freqs);
            Node root = CreateHuffmanTree(freqs);
            byte[] decompressedData = Decompress(data, dataLength, startIndex, root);
            return decompressedData;
        }

        private void ParseHeader(byte[] data, out int dataLength, out int startIndex, out int[] freqs)
        {
            dataLength  = data[0] | (data[1] << 8) | (data[2] << 16) | (data[3] << 24);
            freqs = new int[256];
            for (int j = 0; j < 256; j++)
                freqs[j] = data[4 + j];

            startIndex = 4 + 256;
        }

        private byte[] Decompress(byte[] data, int dataLength, int startIndex, Node root)
        {
            int size = 0;
            Node current = root;
            List<byte> newData = new List<byte>();
            for (int j = startIndex; j < data.Length; j++)
                for (int bit = 1; bit <= 128; bit <<= 1)
                {
                    bool zero = (data[j] & bit) == 0;
                    if (zero)
                        current = current.bit0;
                    else
                        current = current.bit1;
                    if (current.bit0 != null)
                        continue;
                    if (size++ < dataLength)
                        newData.Add(current.Symbol);
                    current = root;
                }
            return newData.ToArray();
        }

        private int[] CalculateFrequency(byte[] data)
        {
            int[] freqs = new int[256];
            foreach (byte b in data)
                freqs[b]++;
            NormalizeFreqs();
            return freqs;

            void NormalizeFreqs()
            {
                int max = freqs.Max();
                if (max <= 255) return;
                for (int j = 0; j < 256; j++)
                    if (freqs[j] > 0)
                        freqs[j] = 1 + freqs[j] * 255 / (max + 1);
            }
        }

        private Node CreateHuffmanTree(int[] frequencies) 
        {
            var nodes = new List<Node>();
            for (int i = 0; i < 255; i++)
            {
                var node = new Node();
                node.Symbol = (byte)i;
                node.Frequency = frequencies[i];
                nodes.Add(node);
            }
            var root = CreateHuffmanTree(nodes).FirstOrDefault();
            return root;
        }

        private List<Node> CreateHuffmanTree(List<Node> nodes)
        {
            if (nodes.Count == 1)
                return nodes;
            else
            {
                nodes = nodes.OrderBy(n => n.Frequency).ToList();
                var bit0 = nodes[0];
                var bit1 = nodes[1];
                var newNode = new Node { Frequency = bit0.Frequency + bit1.Frequency, bit0 = bit0, bit1 = bit1 };
                nodes = nodes.Skip(2).ToList();
                nodes.Insert(0, newNode);
                return CreateHuffmanTree(nodes);
            }
        }

        private string[] CreateHuffmanCodes(Node root)
        {
            string[] codes = new string[256];
            Traverse(root, String.Empty);
            return codes;

            void Traverse(Node node, string code)
            {
                if (node.bit0 == null || node.bit1 == null)
                    codes[node.Symbol] = code;
                else
                {
                    Traverse(node.bit0, code + "0");
                    Traverse(node.bit1, code + "1");
                }
            }
        }  
    }
}
