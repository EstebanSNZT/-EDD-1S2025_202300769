using Classes;
using Newtonsoft.Json;

namespace Structures
{
    public class Blockchain
    {
        private Block head;

        public Blockchain()
        {
            head = null;
        }

        public void AddBlock(User user)
        {
            if (head == null)
            {
                Block firstBlock = new Block(0, user, "0000");
                firstBlock.MineBlock();
                head = firstBlock;
                return;
            }

            Block current = head;
            while (current.Next != null)
            {
                current = current.Next;
            }
            Block newBlock = new Block(current.Index + 1, user, current.Hash);
            newBlock.MineBlock();
            current.Next = newBlock;
        }

        public User Get(int id)
        {
            Block current = head;
            while (current != null)
            {
                User user = JsonConvert.DeserializeObject<User>(current.Data);
                if (user.Id == id)
                {
                    return user;
                }
                current = current.Next;
            }
            return null;
        }

        public User GetByEmail(string email)
        {
            Block current = head;
            while (current != null)
            {
                User user = JsonConvert.DeserializeObject<User>(current.Data);
                if (user.Email.Equals(email))
                {
                    return user;
                }
                current = current.Next;
            }
            return null;
        }

        public bool Contains(int id)
        {
            return Get(id) != null;
        }

        public bool IsEmpty()
        {
            return head == null;
        }

        public string GenerateJson()
        {
            if (head == null) return null;

            List<object> blocks = new List<object>();
            Block current = head;

            while (current != null)
            {
                blocks.Add(new
                {
                    Index = current.Index,
                    Timestamp = current.Timestamp,
                    Data = current.Data,
                    Nonce = current.Nonce,
                    PreviousHash = current.PreviousHash,
                    Hash = current.Hash
                });
                current = current.Next;
            }

            return JsonConvert.SerializeObject(blocks, Formatting.Indented);
        }

        public void ViewAllBlocks()
        {
            Block current = head;
            while (current != null)
            {
                Console.WriteLine(current.ToString());
                current = current.Next;
            }
        }

        public void ViewBlock(int index)
        {
            Block current = head;
            while (current != null)
            {
                if (current.Index == index)
                {
                    Console.WriteLine(current.ToString());
                    return;
                }
                current = current.Next;
            }
            Console.WriteLine($"Bloque con índice {index} no encontrado.");
        }
    }
}
