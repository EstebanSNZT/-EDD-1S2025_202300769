# Manual Técnico

## Introducción
Este manual tiene el propósito de orientar a quienes estén interesados en comprender el funcionamiento interno del programa para la gestión de un taller AutoGest Pro Fase 3, así como proporcionar las clase y estructuras abstractas utilizadas en su desarrollo. Con esto, buscamos proporcionar orientación a la gente en el desarrollo de sus propias aplicaciones o simplemente acercar a nuevas personas al mundo de la programación con C# y creación de interfaces gráficas con la libreria para interfaces en linux GTK. Comprender el funcionamiento interno del programa AutoGest Pro permitirá a los desarrolladores aprender buenas prácticas de programación y mejorar su capacidad para crear software eficiente y confiable en C#.

## Objetivos

### General
Proporcionar orientación a aquellos interesados en el funcionamiento interno del programa AutoGest Pro Fase 3, ofreciendo una descripción detallada de su estructura y su elaboración.

### Específicos
- Proporcionar una detallada inspección del código del programa AutoGest Pro, destacando las clases, funciones y procedimientos clave utilizados en su desarrollo.

- Facilitar la comprensión y aplicación del código para los desarrolladores en sus futuros proyectos.

### Alcances del sistema
El propósito de este manual es las principales estructuras abstractas utilizadas en el programa AutoGest Pro Fase3. Su objetivo es orientar a aquellos interesados en el desarrollo de este programa, brindándoles una comprensión completa de su estructura y funcionamiento interno, para que puedan aplicarlos en sus futuros proyectos. Este manual es una herramienta esencial tanto para desarrolladores novatos como para aquellos con experiencia que deseen profundizar en el funcionamiento del programa AutoGest Pro Fase 3.

### Especificación técnica

### Requisitos de hardware

- Procesador de al menos 2 GHz de velocidad.

- Memoria RAM de al menos 2 GB.

- Espacio de almacenamiento disponible de al menos 500 MB.

### Requisitos de software

- Sistema operativo compatible: Una distribución de Linux compatible.

- .NET SDK 6.0 o superior.

- GTK 4 y sus dependencias instaladas.

- Compilador de C# como dotnet o mono.

- Editor de código como Visual Studio Code o JetBrains Rider.

## Descripción de la solución
AutoGest pro es un programa de con interfaz grafica diseñado con el lenguaje de programación C#.

Para la interfaz gráfica se utilizo la libreria para interfaces en linux GTK.

En esta fase se tuvo un enfoque mayor el la seguridad de nuestro usuarios

Para almacenar los datos en memoria se utilizaron estructuras de datos abstractas tanto las comunes como estructuras más seguras. Se utilizo blockchain para almacenar los usuarios, la lista doblemente enlazada para los vehículos, el árbol AVL para los repuestos, el árbol binario de búsqueda para los servicios y el árbol de merkle para las facturas. Además se utilizo un grafo no dirigido para mapear las relaciones entre vehiculos y repuestos.

El software fue diseñado para simplificar y optimizar las tareas de gestión para un taller de vehiculos. Siendo administrador en el programa se permite realizar cargas masivas, de usuarios, vehiculos, repuestos y para esta tercera fase se permitio la carga masiva de servicios. Puede actualizar los repuestos, ver los repuesto en distinto recorridos como tambien puede generar servicios, generar reporte del login y reportes graficos de las estructuras. Como usuario de la aplicación puedes insertar vehiculos, ver tus servicios en diferentes recorrido y ver tus facturas por cancelar. En esta fase se volvió a agregar la opción de inserción y visualización de usuarios.


## Lógica del programa

## BlockChain

El blockchain fue la estructura central de esta fase. Es una estructura realmente segura. Pero sacrifica seguridad por rendimiento. El calculo del nonce correcto para insertar un bloque, a pesar de que en este proyecto fueron solo 4 ceros de prueba de trabajo, requeria de muchas iteraciones. Es una estructura utilizada para cosas que requieran mucha proteccion como cosas relacionadas con dinero o documentos importantes.

### Estructura

```csharp
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

```

### Bloque (Block)
```csharp
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

        [JsonConstructor]
        public Block(int index, string timestamp, string data, int nonce, string previousHash, string hash)
        {
            Index = index;
            Timestamp = timestamp;
            Data = data;
            Nonce = nonce;
            PreviousHash = previousHash;
            Hash = hash;
            Next = null;
        }

        public string CalculateHash()
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
            while (!Hash.StartsWith("0000"))
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

        public string ToDotNode()
        {
            User userData = JsonConvert.DeserializeObject<User>(Data);

            var dataFormatted = new
            {
                ID = userData.Id,
                Nombres = userData.Names,
                Apellidos = userData.LastNames,
                Correo = userData.Email,
                Edad = userData.Age,
                Contraseña = userData.Password.Length > 16 ? 
                            userData.Password.Substring(0, 16) + "..." : 
                            userData.Password
            };

            string jsonFormatted = JsonConvert.SerializeObject(dataFormatted, Formatting.Indented);

            string escapedData = jsonFormatted
                .Replace("\"", "")
                .Replace("{", "\\{")
                .Replace("}", "\\}")
                .Replace(":", ": ")
                .Replace(",", ", ");

            string hashDisplay = Hash.Length > 16 ? Hash.Substring(0, 16) + "..." : Hash;
            string prevHashDisplay = PreviousHash.Length > 16 ? PreviousHash.Substring(0, 16) + "..." : PreviousHash;
            
            string nodeLabel = $"\"{{<data> INDEX: {Index} \\n TIMESTAMP: {Timestamp} \\n DATA: {escapedData} \\n NONCE: {Nonce} \\n PREVIOUS HASH: {prevHashDisplay} \\n HASH: {hashDisplay}}}\"".Trim();
            return $"block{Index} [label = {nodeLabel}];".Trim();
        }
    }
}

```

# Lista Doblemente Enlazada (DoublyLinkedList)

La lista doblemente enlazada es basica pero muy útil, por algo se mantuvo en las tre fases de este proyecto. En esta ocasion se requirió un poco mas de se uso ya que debia de generar su contenido en texto plano y de esta modo cargarlo. También era necesario obtener los vehículos para cada usuario. Es una muy buena estructura.

### Lista

```csharp
using System.Text;
using Classes;
using Gtk;
using Global;

namespace Structures
{
    public class DoublyLinkedList
    {
        public DoublyLinkedNode head;
        public DoublyLinkedNode tail;

        public DoublyLinkedList()
        {
            head = null;
            tail = null;
        }

        public void Add(Vehicle data)
        {
            DoublyLinkedNode newNode = new DoublyLinkedNode(data);

            if (head == null)
            {
                head = tail = newNode;
            }
            else
            {
                tail.Next = newNode;
                newNode.Prev = tail;
                tail = newNode;
            }
        }

        public bool Delete(int id)
        {
            if (head == null) return false;

            DoublyLinkedNode current = head;
            while (current != null)
            {
                if (current.Data.Id == id)
                {
                    if (current.Prev != null)
                        current.Prev.Next = current.Next;
                    else
                        head = current.Next;

                    if (current.Next != null)
                        current.Next.Prev = current.Prev;
                    else
                        tail = current.Prev;

                    return true;
                }
                current = current.Next;
            }
            return false;
        }

        public void DeleteByUserId(int userId)
        {
            if (head == null) return;

            DoublyLinkedNode current = head;
            while (current != null)
            {
                if (current.Data.UserId == userId)
                {
                    if (current.Prev != null)
                        current.Prev.Next = current.Next;
                    else
                        head = current.Next;
                    if (current.Next != null)
                        current.Next.Prev = current.Prev;
                    else
                        tail = current.Prev;
                }
                current = current.Next;
            }
        }

        public Vehicle Get(int id)
        {
            DoublyLinkedNode current = head;
            while (current != null)
            {
                if (current.Data.Id == id) return current.Data;
                current = current.Next;
            }
            return null;
        }

        public ListStore GetUserVehicles(int userId)
        {
            ListStore result = new ListStore(typeof(int), typeof(int), typeof(string), typeof(int), typeof(string));
            DoublyLinkedNode current = head;

            while (current != null)
            {
                Vehicle vehicle = current.Data;
                if (vehicle.UserId == userId)
                {
                    result.AppendValues(vehicle.Id, vehicle.UserId, vehicle.Brand, vehicle.Model, vehicle.Plate);
                }
                current = current.Next;
            }

            return result;
        }

        public bool IsEmpty()
        {
            return head == null;
        }

        public bool Contains(int id)
        {
            return Get(id) != null;
        }

        public void Print()
        {
            Console.WriteLine("-------------------------------------------------------------------------------");
            DoublyLinkedNode current = head;
            while (current != null)
            {
                Console.WriteLine(current.Data.ToString());
                current = current.Next;
            }
            Console.WriteLine();
        }

        public string PlainText()
        {
            StringBuilder text = new StringBuilder();

            DoublyLinkedNode current = head;

            while (current != null)
            {
                text.AppendLine(current.Data.ToString());
                current = current.Next;
            }

            Console.WriteLine($"Lista Doblemente Enlazada en Texto Plano:\n{text}");

            return text.ToString();
        }

        public void LoadPlainText(string text)
        {

            if (string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine("El texto proporcionado está vacío.");
                return;
            }

            string[] lines = text.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] parts = line.Split(',');

                if (parts.Length != 5)
                {
                    Console.WriteLine($"Línea ignorada (formato incorrecto): {line}");
                    continue;
                }

                if (!int.TryParse(parts[0], out int id) ||
                    !int.TryParse(parts[1], out int userId) ||
                    !int.TryParse(parts[3], out int model))
                {
                    Console.WriteLine($"Línea ignorada (datos numéricos inválidos): {line}");
                    continue;
                }

                string brand = parts[2];
                string plate = parts[4];

                if (Contains(id))
                {
                    Console.WriteLine($"Línea ignorada (ID ya existe): {line}");
                }
                else
                {
                    if (GlobalStructures.UsersBlockchain.Contains(userId))
                    {
                        Vehicle vehicle = new Vehicle(id, userId, brand, model, plate);
                        Add(vehicle);
                    }
                    else
                    {
                        Console.WriteLine($"Línea ignorada (ID de usuario no encontrado): {line}");
                    }
                }
            }
        }

        public string GenerateDot()
        {
            StringBuilder graph = new StringBuilder();
            graph.AppendLine("digraph DoublyLinkedList {");
            graph.AppendLine("    node [shape=record];");
            graph.AppendLine("    rankdir=LR;");
            graph.AppendLine("    subgraph cluster_0 {");
            graph.AppendLine("        label = \"Lista Doblemente Enlazada\";");
            DoublyLinkedNode current = head;
            int index = 0;

            while (current != null)
            {
                graph.AppendLine($"        n{index} {current.ToDotNode()}");
                current = current.Next;
                index++;
            }

            for (int i = 0; i < index; i++)
            {
                if (i < index - 1)
                {
                    graph.AppendLine($"        n{i} -> n{i + 1};");
                }
                if (i > 0)
                {
                    graph.AppendLine($"        n{i} -> n{i - 1};");
                }
            }

            graph.AppendLine("    }");
            graph.AppendLine("}");
            return graph.ToString();
        }
    }
}

```

### Nodo (DoublyLinkedNode)
```csharp
using Classes;
namespace Structures
{
    public class DoublyLinkedNode
    {
        public Vehicle Data { get; set; }
        public DoublyLinkedNode Next { get; set; }
        public DoublyLinkedNode Prev { get; set; }

        public DoublyLinkedNode(Vehicle data)
        {
            Data = data;
            Next = null;
        }

        public string ToDotNode()
        {
            return $"[label = \"{{<data> ID: {Data.Id} \\n ID_Usuario: {Data.UserId} \\n Marca: {Data.Brand} \\n Modelo: {Data.Model} \\n Placa: {Data.Plate}}}\"];";
        }
    }
}
```

## Árbol AVL (AVLTree)

El arból AVL en esta ocasión se agrego el devolver sus datos en texto plano y cargar esos datos en texto plano. Muy útil, creo que el balanceo automatico fue lo que más me gusto de esta estructura. La búsqueda es muy eficiente por lo que lo hace una estructura que pienso que podria seguir viendo o utilizando a futuro.

### Árbol
```csharp
using System.Text;
using Classes;
using Gtk;

namespace Structures
{
    public class AVLTree
    {
        private AVLNode root { get; set; }

        public AVLTree()
        {
            root = null;
        }

        private int GetHeight(AVLNode node)
        {
            return node == null ? 0 : node.Height;
        }

        private int GetBalance(AVLNode node)
        {
            return node == null ? 0 : GetHeight(node.Left) - GetHeight(node.Right);
        }

        private AVLNode RotateRight(AVLNode y)
        {
            AVLNode x = y.Left;

            y.Left = x.Right;
            x.Right = y;

            y.Height = 1 + Math.Max(GetHeight(y.Left), GetHeight(y.Right));
            x.Height = 1 + Math.Max(GetHeight(x.Left), GetHeight(x.Right));

            return x;
        }

        private AVLNode RotateLeft(AVLNode x)
        {
            AVLNode y = x.Right;

            x.Right = y.Left;
            y.Left = x;

            x.Height = 1 + Math.Max(GetHeight(x.Left), GetHeight(x.Right));
            y.Height = 1 + Math.Max(GetHeight(y.Left), GetHeight(y.Right));

            return y;
        }

        public void Add(SparePart data)
        {
            root = AddRecursive(root, data);
        }

        private AVLNode AddRecursive(AVLNode node, SparePart data)
        {
            if (node == null) return new AVLNode(data);

            if (data.Id < node.Data.Id)
                node.Left = AddRecursive(node.Left, data);
            else if (data.Id > node.Data.Id)
                node.Right = AddRecursive(node.Right, data);
            else
                return node;

            node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));

            int balance = GetBalance(node);

            // Left-Left
            if (balance > 1 && data.Id < node.Left.Data.Id)
                return RotateRight(node);

            // Right-Right
            if (balance < -1 && data.Id > node.Right.Data.Id)
                return RotateLeft(node);

            // Left-Right
            if (balance > 1 && data.Id > node.Left.Data.Id)
            {
                node.Left = RotateLeft(node.Left);
                return RotateRight(node);
            }

            // Right-Left
            if (balance < -1 && data.Id < node.Right.Data.Id)
            {
                node.Right = RotateRight(node.Right);
                return RotateLeft(node);
            }

            return node;
        }

        public SparePart Get(int id)
        {
            return GetRecursive(root, id);
        }

        private SparePart GetRecursive(AVLNode node, int id)
        {
            if (node == null) return null;

            if (id < node.Data.Id)
                return GetRecursive(node.Left, id);
            else if (id > node.Data.Id)
                return GetRecursive(node.Right, id);
            else
                return node.Data;
        }

        public bool Contains(int id)
        {
            return Get(id) != null;
        }

        public bool IsEmpty()
        {
            return root == null;
        }

        public ListStore PreOrder()
        {
            ListStore result = new ListStore(typeof(int), typeof(string), typeof(string), typeof(string));
            PreOrderRecursive(root, result);
            return result;
        }

        private void PreOrderRecursive(AVLNode node, ListStore result)
        {
            if (node == null) return;

            SparePart sparePart = node.Data;
            result.AppendValues(sparePart.Id, sparePart.Spare, sparePart.Details, sparePart.Cost.ToString("F2"));
            PreOrderRecursive(node.Left, result);
            PreOrderRecursive(node.Right, result);
        }

        public ListStore InOrder()
        {
            ListStore result = new ListStore(typeof(int), typeof(string), typeof(string), typeof(string));
            InOrderRecursive(root, result);
            return result;
        }

        private void InOrderRecursive(AVLNode node, ListStore result)
        {
            if (node == null) return;

            SparePart sparePart = node.Data;
            InOrderRecursive(node.Left, result);
            result.AppendValues(sparePart.Id, sparePart.Spare, sparePart.Details, sparePart.Cost.ToString("F2"));
            InOrderRecursive(node.Right, result);
        }

        public ListStore PostOrder()
        {
            ListStore result = new ListStore(typeof(int), typeof(string), typeof(string), typeof(string));
            PostOrderRecursive(root, result);
            return result;
        }

        private void PostOrderRecursive(AVLNode node, ListStore result)
        {
            if (node == null) return;

            SparePart sparePart = node.Data;
            PostOrderRecursive(node.Left, result);
            PostOrderRecursive(node.Right, result);
            result.AppendValues(sparePart.Id, sparePart.Spare, sparePart.Details, sparePart.Cost.ToString("F2"));
        }

        public string PlainText()
        {
            StringBuilder text = new StringBuilder();
            PlainTextRecursive(root, text);
            Console.WriteLine($"Árbol AVL en Texto Plano:\n{text}");
            return text.ToString();
        }

        public void PlainTextRecursive(AVLNode node, StringBuilder text)
        {
            if (node == null) return;

            PlainTextRecursive(node.Left, text);
            text.AppendLine(node.Data.ToString());
            PlainTextRecursive(node.Right, text);
        }

        public void LoadPlainText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine("El texto proporcionado está vacío.");
                return;
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine("El texto proporcionado está vacío.");
                return;
            }

            string[] lines = text.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] parts = line.Split(',');

                if (parts.Length != 4)
                {
                    Console.WriteLine($"Línea ignorada (formato incorrecto): {line}");
                    continue;
                }

                if (!int.TryParse(parts[0], out int id) || !double.TryParse(parts[3], out double cost))
                {
                    Console.WriteLine($"Línea ignorada (datos numéricos inválidos): {line}");
                    continue;
                }

                string spare = parts[1];
                string details = parts[2];

                if (Contains(id))
                {
                    Console.WriteLine($"Línea ignorada (ID ya existe): {line}");
                    continue;
                }
                else
                {
                    SparePart sparePart = new SparePart(id, spare, details, cost);
                    Add(sparePart);
                }
            }
        }

        public string GenerateDot()
        {
            StringBuilder graph = new StringBuilder();
            graph.AppendLine("digraph ALVTree {");
            graph.AppendLine("    node [shape=box];");
            graph.AppendLine("    rankdir=TB;");
            graph.AppendLine("    subgraph cluster_0 {");
            graph.AppendLine("        label = \"Árbol AVL\";");
            GenerateDotNodes(root, graph);
            GenerateDotConnections(root, graph);
            graph.AppendLine("    }");
            graph.AppendLine("}");
            return graph.ToString();
        }

        public void GenerateDotNodes(AVLNode node, StringBuilder graph)
        {
            if (node == null) return;
            graph.Append("        ").Append(node.ToDotNode()).AppendLine();
            GenerateDotNodes(node.Left, graph);
            GenerateDotNodes(node.Right, graph);
        }

        public void GenerateDotConnections(AVLNode node, StringBuilder graph)
        {
            if (node == null) return;
            if (node.Left != null)
                graph.AppendLine($"        \"{node.Data.Id}\" -> \"{node.Left.Data.Id}\";");
            if (node.Right != null)
                graph.AppendLine($"        \"{node.Data.Id}\" -> \"{node.Right.Data.Id}\";");
            GenerateDotConnections(node.Left, graph);
            GenerateDotConnections(node.Right, graph);
        }
    }
}
```

### Nodo (AVLNode)
```csharp
using Classes;

namespace Structures
{
    public class AVLNode
    {
        public SparePart Data { get; set; }
        public AVLNode Left { get; set; }
        public AVLNode Right { get; set; }
        public int Height { get; set; }

        public AVLNode(SparePart data)
        {
            Data = data;
            Left = null;
            Right = null;
            Height = 1;
        }

        public string ToDotNode()
        {
            return $"\"{Data.Id}\" [label=\"ID: {Data.Id}\\nRepuesto: {Data.Spare}\\nDetalles: {Data.Details}\\nCosto: {Data.Cost}\"];";
        }
    }
}
```

## Árbol Binario (BinaryTree)

El arból binario es una estructara buena. Puede que no tenga el mejor acceso pero tiene una mejor inserción que el árbol AVL. Creo que es bastante básico pero muy útil en este proyecto. En esta fase no se agregó nada extra relevante a su clase.

### Árbol
```csharp
using System.Text;
using Classes;
using Gtk;
using Global;

namespace Structures
{
    public class BinaryTree
    {
        private BinaryNode root { get; set; }

        public BinaryTree()
        {
            root = null;
        }

        public void Add(Service data)
        {
            if (root == null)
                root = new BinaryNode(data);
            else
                AddRecursive(root, data);
        }

        private void AddRecursive(BinaryNode node, Service data)
        {
            if (data.Id < node.Data.Id)
            {
                if (node.Left == null)
                    node.Left = new BinaryNode(data);
                else
                    AddRecursive(node.Left, data);
            }
            else if (data.Id > node.Data.Id)
            {
                if (node.Right == null)
                    node.Right = new BinaryNode(data);
                else
                    AddRecursive(node.Right, data);
            }
            else
                return;
        }

        public Service Get(int id)
        {
            return GetRecursive(root, id);
        }

        private Service GetRecursive(BinaryNode node, int id)
        {
            if (node == null) return null;

            if (id < node.Data.Id)
                return GetRecursive(node.Left, id);
            else if (id > node.Data.Id)
                return GetRecursive(node.Right, id);
            else
                return node.Data;
        }

        public bool Contains(int id)
        {
            return Get(id) != null;
        }

        public bool IsEmpty()
        {
            return root == null;
        }

        public ListStore PreOrder(int userId)
        {
            ListStore result = new ListStore(typeof(int), typeof(int), typeof(int), typeof(string), typeof(string));
            PreOrderRecursive(root, result, userId);
            return result;
        }

        private void PreOrderRecursive(BinaryNode node, ListStore result, int userId)
        {
            if (node == null) return;

            Service service = node.Data;
            Vehicle vehicle = GlobalStructures.VehiclesList.Get(service.VehicleId);
            if (vehicle != null && vehicle.UserId == userId)
                result.AppendValues(service.Id, service.SparePartId, service.VehicleId, service.Details, service.Cost.ToString("F2"));

            PreOrderRecursive(node.Left, result, userId);
            PreOrderRecursive(node.Right, result, userId);
        }

        public ListStore InOrder(int userId)
        {
            ListStore result = new ListStore(typeof(int), typeof(int), typeof(int), typeof(string), typeof(string));
            InOrderRecursive(root, result, userId);
            return result;
        }

        private void InOrderRecursive(BinaryNode node, ListStore result, int userId)
        {
            if (node == null) return;

            InOrderRecursive(node.Left, result, userId);

            Service service = node.Data;
            Vehicle vehicle = GlobalStructures.VehiclesList.Get(service.VehicleId);
            if (vehicle != null && vehicle.UserId == userId)
                result.AppendValues(service.Id, service.SparePartId, service.VehicleId, service.Details, service.Cost.ToString("F2"));

            InOrderRecursive(node.Right, result, userId);
        }

        public ListStore PostOrder(int userId)
        {
            ListStore result = new ListStore(typeof(int), typeof(int), typeof(int), typeof(string), typeof(string));
            PostOrderRecursive(root, result, userId);
            return result;
        }

        private void PostOrderRecursive(BinaryNode node, ListStore result, int userId)
        {
            if (node == null) return;

            PostOrderRecursive(node.Left, result, userId);
            PostOrderRecursive(node.Right, result, userId);

            Service service = node.Data;
            Vehicle vehicle = GlobalStructures.VehiclesList.Get(service.VehicleId);
            if (vehicle != null && vehicle.UserId == userId)
                result.AppendValues(service.Id, service.SparePartId, service.VehicleId, service.Details, service.Cost.ToString("F2"));
        }

        public string GenerateDot()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("digraph BinaryTree {");
            sb.AppendLine("    node [shape=box];");
            sb.AppendLine("    rankdir=TB;");
            sb.AppendLine("    subgraph cluster_0 {");
            sb.AppendLine("        label = \"Árbol Binario\";\n");
            GenerateDotNodes(root, sb);
            GenerateDotConnections(root, sb);
            sb.AppendLine("    }"); 
            sb.AppendLine("}");
            return sb.ToString();
        }

        public void GenerateDotNodes(BinaryNode node, StringBuilder sb)
        {
            if (node == null) return;
            sb.Append("        ").Append(node.ToDotNode()).AppendLine();
            GenerateDotNodes(node.Left, sb);
            GenerateDotNodes(node.Right, sb);
        }

        public void GenerateDotConnections(BinaryNode node, StringBuilder sb)
        {
            if (node == null) return;
            if (node.Left != null)
                sb.AppendLine($"        \"{node.Data.Id}\" -> \"{node.Left.Data.Id}\";");
            if (node.Right != null)
                sb.AppendLine($"        \"{node.Data.Id}\" -> \"{node.Right.Data.Id}\";");
            GenerateDotConnections(node.Left, sb);
            GenerateDotConnections(node.Right, sb);
        }
    }
}
```

### Nodo (BinaryNode)
```csharp
using Classes;

namespace Structures
{
    public class BinaryNode
    {
        public Service Data { get; set; }
        public BinaryNode Left { get; set; }
        public BinaryNode Right { get; set; }

        public BinaryNode(Service data)
        {
            Data = data;
            Left = null;
            Right = null;
        }

        public string ToDotNode()
        {
            return $"\"{Data.Id}\" [label=\"ID: {Data.Id}\\nId_Repuesto: {Data.SparePartId}\\nId_Vehículo: {Data.VehicleId}\\nDetalles: {Data.Details}\\nCosto: {Data.Cost}\"];";
        }
    }
}

```

## Árbol de Merkle (MerkleTree)

El árbol de merkle es una estructura que en este caso no sacrifica rendemiento por seguridad porque el arból de merkle es más para verificar la integridad de grandes cantidades de datos de forma eficiente, sin necesidad de revisar cada elemento. Realmente es más para verificaciones que para accesos de datos y sus verificaciones al estar en forma de arbol son más rápidas.

### Árbol
```csharp
using System.Text;
using Classes;
using Global;
using Gtk;

namespace Structures
{
    public class MerkleTree
    {
        private MerkleNode root;
        private List<MerkleNode> leaves;

        public MerkleTree()
        {
            root = null;
            leaves = new List<MerkleNode>();
        }

        public void Add(Invoice data)
        {
            MerkleNode newLeaf = new MerkleNode(data);
            leaves.Add(newLeaf);
            BuildTree();
        }

        private void BuildTree()
        {
            if (IsEmpty())
            {
                root = null;
                return;
            }

            List<MerkleNode> currentLevel = new List<MerkleNode>(leaves);

            while (currentLevel.Count > 1)
            {
                List<MerkleNode> nextLevel = new List<MerkleNode>();

                for (int i = 0; i < currentLevel.Count; i += 2)
                {
                    MerkleNode left = currentLevel[i];
                    MerkleNode right = (i + 1 < currentLevel.Count) ? currentLevel[i + 1] : null;
                    MerkleNode parent = new MerkleNode(left, right);
                    nextLevel.Add(parent);
                }

                currentLevel = nextLevel;
            }

            root = currentLevel[0];
        }

        public bool IsEmpty()
        {
            return leaves.Count == 0;
        }

        public ListStore GetUserInvoices(int userId)
        {
            ListStore result = new ListStore(typeof(int), typeof(int), typeof(string), typeof(string), typeof(string));

            foreach (var leaf in leaves)
            {
                Invoice invoice = leaf.Data;

                Service service = GlobalStructures.ServicesTree.Get(invoice.ServiceId);

                if (service != null)
                {
                    Vehicle vehicle = GlobalStructures.VehiclesList.Get(service.VehicleId);
                    if (vehicle != null && vehicle.UserId == userId)
                        result.AppendValues(invoice.Id, service.Id, invoice.Total.ToString("F2"), invoice.PaymentMethod, invoice.Date);
                }
            }

            return result;
        }

        public string GenerateDot()
        {
            StringBuilder dot = new StringBuilder();
            dot.AppendLine("digraph MerkleTree {");
            dot.AppendLine("    node [shape=record];");
            dot.AppendLine("    rankdir=BT;");
            dot.AppendLine("    subgraph cluster_0 {");
            dot.AppendLine("        label = \"Facturas\";");
            if (IsEmpty()) dot.AppendLine("        empty [label=\"Arbol vacío\"];");
            else
            {
                Dictionary<string, int> nodeIds = new Dictionary<string, int>();
                int idCounter = 0;
                GenerateDotNodes(root, dot, nodeIds, ref idCounter);
                GenerateDotConnections(root, dot, nodeIds);
            }
            dot.AppendLine("    }");
            dot.AppendLine("}");
            return dot.ToString();
        }

        private void GenerateDotNodes(MerkleNode node, StringBuilder dot, Dictionary<string, int> nodeIds, ref int idCounter)
        {
            if (node == null) return;

            if (!nodeIds.ContainsKey(node.Hash)) nodeIds[node.Hash] = idCounter++;
            int nodeId = nodeIds[node.Hash];

            dot.AppendLine($"        node{nodeId} {node.ToDotNode()};");

            GenerateDotNodes(node.Left, dot, nodeIds, ref idCounter);
            GenerateDotNodes(node.Right, dot, nodeIds, ref idCounter);
        }

        private void GenerateDotConnections(MerkleNode node, StringBuilder dot, Dictionary<string, int> nodeIds)
        {
            if (node == null) return;

            int nodeId = nodeIds[node.Hash];

            if (node.Left != null)
            {
                int leftId = nodeIds[node.Left.Hash];
                dot.AppendLine($"        node{leftId} -> node{nodeId};");
            }

            if (node.Right != null)
            {
                int rightId = nodeIds[node.Right.Hash];
                dot.AppendLine($"        node{rightId} -> node{nodeId};");
            }

            GenerateDotConnections(node.Left, dot, nodeIds);
            GenerateDotConnections(node.Right, dot, nodeIds);
        }
    }
}

```

### Nodo (MerkleNode)
```csharp
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

        public string ToDotNode()
        {
            string hashDisplay = Hash.Length > 16 ? Hash.Substring(0, 16) + "..." : Hash;

            if (Data != null) return $"[label=\"ID: {Data.Id}\\nID_Servicio: {Data.ServiceId}\\nTotal: {Data.Total}\\nMétodo de pago: {Data.PaymentMethod}\\nFecha: {Data.Date}\\nHash: {hashDisplay}\"]";
            else return $"[label=\"Hash: {hashDisplay}\"]";
        }
    }
}
```

## Grafo no dirigido

El grafo no dirigido es una estructura que sirve para modelar relaciones bidireccionales entre elementos, donde las conexiones no tienen una dirección específica. Esta estructura a mi parecer quedo desaprovechada en este proyecto, dado que tiene aplicaciones más relevantes como redes de transporte, asi como algoritmos utiles como dijkstra para encontrar caminos más cortos y eficientes.

### Estructura
```csharp
using System.Text;

namespace Structures
{
    public class UndirectedGraph
    {
        private readonly Dictionary<string, List<string>> adjacencyList;

        public UndirectedGraph()
        {
            adjacencyList = new Dictionary<string, List<string>>();
        }

        public void AddEdge(string nodeVehicle, string nodeSparePart)
        {
            if (string.IsNullOrEmpty(nodeVehicle) || string.IsNullOrEmpty(nodeSparePart))
            {
                throw new ArgumentException("Los nombres de los nodos no pueden ser nulos o vacíos.");
            }

            ConnectNode(nodeVehicle, nodeSparePart);
            ConnectNode(nodeSparePart, nodeVehicle);
        }

        private void ConnectNode(string nodeA, string nodeB)
        {
            if(!adjacencyList.ContainsKey(nodeA))
                adjacencyList[nodeA] = new List<string>();
            
            if(!adjacencyList[nodeA].Contains(nodeB))
                adjacencyList[nodeA].Add(nodeB);
        }

        public bool IsEmpty()
        {
            return adjacencyList.Count == 0;
        }

        public string GenerateDot()
        {
            StringBuilder graph = new StringBuilder();
            graph.AppendLine("graph UndirectedGraph {");
            graph.AppendLine("    node [shape=ellipse];");
            graph.AppendLine("    rankdir=TB;");
            graph.AppendLine("    subgraph cluster_0 {");
            graph.AppendLine("        label = \"Grafo No Dirigido\";");
            foreach (var node in adjacencyList)
            {
                string currentId = node.Key;
                graph.AppendLine($"        {currentId} [label=\"{currentId}\"];");
            }

            foreach (var node in adjacencyList.Where(node => node.Key.StartsWith("V")))
            {
                string currentId = node.Key;
                List<string> nodeConnections = node.Value;

                foreach (var connection in nodeConnections)
                {
                    if (connection.StartsWith("R"))
                    {
                        graph.AppendLine($"        {currentId} -- {connection};");
                    }
                }
            }

            if(!IsEmpty())
            {
                graph.AppendLine($"        {RankSameNodes("V")}");
                graph.AppendLine($"        {RankSameNodes("R")}");
            }

            graph.AppendLine("    }");
            graph.AppendLine("}");
            return graph.ToString();
        }

        private string RankSameNodes(string start)
        {
            var rank = "{ rank=same; ";
            foreach (var id in adjacencyList.Keys.Where(id => id.StartsWith(start)))
            {
                rank += $"{id}; ";
            }
            rank += "};";
            return rank;
        }
    }
}
```