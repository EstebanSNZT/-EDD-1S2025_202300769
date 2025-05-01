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

        private void AddExistingBlock(Block block)
        {
            if (head == null)
            {
                head = block;
                return;
            }

            Block current = head;
            while (current.Next != null)
            {
                current = current.Next;
            }
            current.Next = block;
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

        public bool ContainsByEmail(string email)
        {
            return GetByEmail(email) != null;
        }

        public bool IsEmpty()
        {
            return head == null;
        }

        public string GenerateJson()
        {
            if (head == null) return "[]";

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

        public void LoadJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                Console.WriteLine("El JSON está vacío o es nulo.");
                return;
            }

            var settings = new JsonSerializerSettings
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            };


            var blocks = JsonConvert.DeserializeObject<List<Block>>(json, settings);

            if (blocks == null || blocks.Count == 0)
            {
                Console.WriteLine("JSON no contiene bloques válidos.");
                return;
            }

            foreach (var block in blocks)
            {
                AddExistingBlock(block);
            }
        }

        public bool AnalyzeBlockchain()
        {
            if (head == null)
            {
                Console.WriteLine("El blockchain está vacío.");
                return false;
            }

            Block current = head;
            Block previous = null;

            while (current != null)
            {
                if (!current.Hash.StartsWith("0000"))
                {
                    Console.WriteLine($"El bloque {current.Index} no cumple con la prueba de trabajo.");
                    return false;
                }

                if (previous != null && current.PreviousHash != previous.Hash)
                {
                    Console.WriteLine($"El PreviousHash del bloque {current.Index} no coincide con el hash del bloque {previous.Index}. Ha sido alterado.");
                    return false;
                }

                if (current.Hash != current.CalculateHash())
                {
                    Console.WriteLine($"El hash del bloque {current.Index} es inválido. Ha sido alterado.");
                    return false;
                }

                previous = current;
                current = current.Next;
            }

            Console.WriteLine("El blockchain es válido y no ha sido alterado.");
            return true;
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

        public string GenerateDot()
        {
            var graph = "digraph Blockchain {\n";
            graph += "    node [shape=record];\n";
            graph += "    rankdir=LR;\n";
            graph += "    subgraph cluster_0 {\n";
            graph += "        label = \"Usuarios\";\n";
            if (head == null) graph += "        empty [label=\"Cadena vacía\"];\n";
            else
            {
                Block current = head;
                while (current != null)
                {
                    graph += $"        {current.ToDotNode()}\n";
                    current = current.Next;
                }

                current = head;

                while (current != null)
                {
                    if (current.Next != null)
                    {
                        graph += $"        block{current.Index} -> block{current.Next.Index};\n";
                    }
                    current = current.Next;
                }

            }
            graph += "    }\n";
            graph += "}\n";
            return graph;
        }
    }
}
