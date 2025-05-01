using System.Text;
using Newtonsoft.Json;

namespace Structures
{
    public class HuffmanCompressor
    {
        private readonly string BackupPath;

        public HuffmanCompressor()
        {
            BackupPath = Path.Combine(Directory.GetCurrentDirectory(), "Backup");

            if (!Directory.Exists(BackupPath))
            {
                Directory.CreateDirectory(BackupPath);
            }
        }

        public void Compress(string text, string EntityName)
        {
            Dictionary<char, int> frequencies = CalculateFrequency(text);
            HuffmanNode root = BuildHuffmanTree(frequencies);
            Dictionary<char, string> huffmanCodes = GenerateHuffmanCodes(root);

            string compressedFilePath = Path.Combine(BackupPath, $"{EntityName}.edd");
            string huffmanTreeFilePath = Path.Combine(BackupPath, $"{EntityName}HuffmanTree.json");

            SaveCompressedFile(text, huffmanCodes, compressedFilePath);
            SaveHuffmanTree(root, huffmanTreeFilePath);

            Console.WriteLine($"Archivo comprimido guardado en: {compressedFilePath}\n");
        }

        public string Decompress(string entityName)
        {
            string compressedFilePath = Path.Combine(BackupPath, $"{entityName}.edd");
            string huffmanTreeFilePath = Path.Combine(BackupPath, $"{entityName}HuffmanTree.json");

            if (!File.Exists(compressedFilePath))
            {
                Console.WriteLine($"El archivo comprimido {entityName}.edd no existe.");
                return string.Empty;
            }

            if (!File.Exists(huffmanTreeFilePath))
            {
                Console.WriteLine($"El archivo del árbol de Huffman {entityName}HuffmanTree.json no existe.");
                return string.Empty;
            }

            byte[] compressedBytes = File.ReadAllBytes(compressedFilePath);
            HuffmanNode root = LoadHuffmanTree(huffmanTreeFilePath);

            string decompressedText = DecompressText(compressedBytes, root);

            Console.WriteLine($"Archivo {entityName}.edd descomprimido con éxito.");
            
            return decompressedText;
        }

        private Dictionary<char, int> CalculateFrequency(string text)
        {
            Dictionary<char, int> frequencies = new Dictionary<char, int>();

            foreach (char c in text)
            {
                if (!frequencies.ContainsKey(c))
                {
                    frequencies[c] = 0;
                }

                frequencies[c]++;
            }

            return frequencies;
        }

        private HuffmanNode BuildHuffmanTree(Dictionary<char, int> frequencies)
        {
            PriorityQueue<HuffmanNode> priorityQueue = new PriorityQueue<HuffmanNode>();

            foreach (var symbol in frequencies)
            {
                priorityQueue.Enqueue(new HuffmanNode()
                {
                    Character = symbol.Key,
                    Frequency = symbol.Value,
                    Left = null,
                    Right = null
                });
            }

            while (priorityQueue.Count > 1)
            {
                HuffmanNode left = priorityQueue.Dequeue();
                HuffmanNode right = priorityQueue.Dequeue();

                HuffmanNode parent = new HuffmanNode()
                {
                    Frequency = left.Frequency + right.Frequency,
                    Left = left,
                    Right = right
                };

                priorityQueue.Enqueue(parent);
            }

            return priorityQueue.Dequeue();
        }

        private Dictionary<char, string> GenerateHuffmanCodes(HuffmanNode root)
        {
            Dictionary<char, string> huffmanCodes = new Dictionary<char, string>();
            GenerateHuffmanCodesRecursive(root, "", huffmanCodes);
            return huffmanCodes;
        }

        private void GenerateHuffmanCodesRecursive(HuffmanNode node, string code, Dictionary<char, string> huffmanCodes)
        {
            if (node.IsLeaf())
            {
                huffmanCodes[node.Character] = code.Length > 0 ? code : "0";
                return;
            }

            if (node.Left != null)
                GenerateHuffmanCodesRecursive(node.Left, code + "0", huffmanCodes);

            if (node.Right != null)
                GenerateHuffmanCodesRecursive(node.Right, code + "1", huffmanCodes);
        }

        private void SaveCompressedFile(string text, Dictionary<char, string> huffmanCodes, string filePath)
        {
            StringBuilder encodedText = new StringBuilder();

            foreach (char c in text)
            {
                encodedText.Append(huffmanCodes[c]);
            }

            int numOfBytes = (encodedText.Length + 7) / 8;
            byte[] bytes = new byte[numOfBytes];
            int byteIndex = 0, bitIndex = 0;

            for (int i = 0; i < encodedText.Length; i++)
            {
                if (encodedText[i] == '1')
                {
                    bytes[byteIndex] |= (byte)(1 << (7 - bitIndex));
                }

                bitIndex++;
                if (bitIndex == 8)
                {
                    byteIndex++;
                    bitIndex = 0;
                }
            }

            File.WriteAllBytes(filePath, bytes);
        }

        private void SaveHuffmanTree(HuffmanNode root, string filePath)
        {
            string json = JsonConvert.SerializeObject(root, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public HuffmanNode LoadHuffmanTree(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<HuffmanNode>(json);
        }

        private string DecompressText(byte[] compressedBytes, HuffmanNode root)
        {
            StringBuilder bits = ConvertBytesToBits(compressedBytes);
            StringBuilder decompressedText = new StringBuilder();
            HuffmanNode currentNode = root;

            foreach (char bit in bits.ToString())
            {
                currentNode = bit == '0' ? currentNode.Left : currentNode.Right;

                if (currentNode.IsLeaf())
                {
                    decompressedText.Append(currentNode.Character);
                    currentNode = root;
                }
            }

            return decompressedText.ToString();
        }

        private StringBuilder ConvertBytesToBits(byte[] bytes)
        {
            StringBuilder bits = new StringBuilder();

            foreach (byte b in bytes)
            {
                for (int i = 7; i >= 0; i--)
                {
                    bits.Append((b & (1 << i)) != 0 ? "1" : "0");
                }
            }

            return bits;
        }
    }
}