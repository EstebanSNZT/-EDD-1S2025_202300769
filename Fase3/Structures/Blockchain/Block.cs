using System.Security.Cryptography;
using System.Text;
using Classes;
using Newtonsoft.Json;

namespace Structures
{
    public class Block
    {
        public int Index { get; set; }
        public string Timestamp { get; set; }
        public string Data { get; set; }
        public int Nonce { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }
        public Block Next { get; set; }

        public Block(int index, User data, string previousHash)
        {
            Index = index;
            Timestamp = DateTime.Now.ToString("yyyy-MM-dd::HH:mm:ss.ff");
            Data = JsonConvert.SerializeObject(data);
            Nonce = 0;
            PreviousHash = previousHash;
            Hash = CalculateHash();
            Next = null;
        }

        private string CalculateHash()
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                string raw = Index + Timestamp + Data + Nonce + PreviousHash;
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(raw));
                StringBuilder hash = new StringBuilder();
                foreach (byte b in bytes)
                {
                    hash.Append(b.ToString("x2"));
                }
                return hash.ToString();
            }
        }

        public void MineBlock()
        {
            while (!Hash.StartsWith("00"))
            {
                Nonce++;
                Hash = CalculateHash();
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"-----------Block {Index}-----------");
            sb.AppendLine($"Index: {Index}");
            sb.AppendLine($"Timestamp: {Timestamp}");
            sb.AppendLine($"Data: {Data}");
            sb.AppendLine($"Nonce: {Nonce}");
            sb.AppendLine($"PreviousHash: {PreviousHash}");
            sb.AppendLine($"Hash: {Hash}");
            sb.AppendLine($"-----------------------------------");
            return sb.ToString();
        }
    }
}