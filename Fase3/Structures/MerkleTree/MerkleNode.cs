using System.Security.Cryptography;
using System.Text;
using Classes;

namespace Structures
{
    public class MerkleNode
    {
        public Invoice Data { get; set; }
        public MerkleNode Left { get; set; }
        public MerkleNode Right { get; set; }
        public string Hash { get; set; }

        public MerkleNode(Invoice data)
        {
            Data = data;
            Left = null;
            Right = null;
            Hash = Data.GetHash();
        }

        public MerkleNode(MerkleNode left, MerkleNode right)
        {
            Data = null;
            Left = left;
            Right = right;
            Hash = CalculateHash(left.Hash, right?.Hash);
        }

        private string CalculateHash(string leftHash, string rightHash)
        {
            string combinedHash = leftHash + (rightHash ?? leftHash);
            using (SHA256 hash = SHA256.Create())
            {
                byte[] bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(combinedHash));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}