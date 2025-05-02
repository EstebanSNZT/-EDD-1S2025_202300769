# Manual Técnico

## Introducción
Este manual tiene el propósito de orientar a quienes estén interesados en comprender el funcionamiento interno del programa para la gestión de un taller AutoGest Pro, así como proporcionar las clase y estructuras abstractas utilizadas en su desarrollo. Con esto, buscamos proporcionar orientación a la gente en el desarrollo de sus propias aplicaciones o simplemente acercar a nuevas personas al mundo de la programación con C# y creación de interfaces gráficas con la libreria para interfaces en linux GTK. Comprender el funcionamiento interno del programa AutoGest Pro permitirá a los desarrolladores aprender buenas prácticas de programación y mejorar su capacidad para crear software eficiente y confiable en C#.

## Objetivos

### General
Proporcionar orientación a aquellos interesados en el funcionamiento interno del programa AutoGest Pro, ofreciendo una descripción detallada de su estructura y su elaboración.

### Específicos
- Proporcionar una detallada inspección del código del programa AutoGest Pro, destacando las variables, módulos, funciones y subrutinas clave utilizados en su desarrollo.

- Facilitar la comprensión y aplicación del código para los desarrolladores en sus futuros proyectos.

### Alcances del sistema
El propósito de este manual es las principales estructuras abstractas utilizadas en el programa AutoGest Por. Su objetivo es orientar a aquellos interesados en el desarrollo de este programa, brindándoles una comprensión completa de su estructura y funcionamiento interno, para que puedan aplicarlos en sus futuros proyectos. Este manual es una herramienta esencial tanto para desarrolladores novatos como para aquellos con experiencia que deseen profundizar en el funcionamiento del programa AutoGest Pro.

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

Para almacenar los datos en memoria se utilizaron estructuras de datos abstractas. Se utilizo la lista simple para almacenar los usuarios, la lista doblemente enlazada para los vehículos, el árbol AVL para los repuestos, el árbol binario de búsqueda para los servicios y el árbol B para las facturas.

El software fue diseñado para simplificar y optimizar las tareas de gestión para un taller de vehiculos. Siendo administrador se permite realizar cargas masivas, de usuarios, vehiculos y repuestos asi como administrar los dos primeros. Puede actualizar los repuestos, ver los repuesto en distinto recorridos como tambien puede generar servicios, generar reporte del login y reportes graficos de las estructuras. Como usuario de la aplicación puedes insertar vehiculos, ver tus servicios en diferentes recoridos y facturas pendientes. Las facturas puedes cancelarlas.


## Lógica del programa

## Lista Enlazada (LinkedList)

Las lista enlazada es muy sencilla de manejar al contar solo con un apuntador. La típica, muy sencilla y simple pero muy util. En este proyecto se uso menos pero aun así se le saco el uso necesario.

### Lista

```csharp
using Classes;

namespace Structures
{
    public class LinkedList
    {
        private LinkedNode head;

        public LinkedList()
        {
            head = null;
        }

        public void Insert(User user)
        {
            LinkedNode newNode = new LinkedNode(user);

            if (head == null)
            {
                head = newNode;
            }
            else
            {
                LinkedNode current = head;
                while (current.Next != null)
                {
                    current = current.Next;
                }
                current.Next = newNode;
            }
        }

        public bool Delete(int id)
        {
            if (head == null) return false;

            if (head.Data.Id == id)
            {
                head = head.Next;
                return true;
            }

            LinkedNode current = head;

            while (current.Next != null && !(current.Next.Data.Id == id))
            {
                current = current.Next;
            }

            if (current.Next != null)
            {
                current.Next = current.Next.Next;
                return true;
            }

            return false;
        }

        public void Print()
        {
            Console.WriteLine("-------------------------------------------------------------------------------");
            LinkedNode current = head;
            while (current != null)
            {
                Console.WriteLine(current.Data.ToString());
                current = current.Next;
            }
        }

        public User Get(int id)
        {
            LinkedNode current = head;
            while (current != null)
            {
                if (current.Data.Id == id)
                {
                    return current.Data;
                }
                current = current.Next;
            }
            return null;
        }

        public User GetByEmail(string email)
        {
            LinkedNode current = head;
            while (current != null)
            {
                if (current.Data.Email.Equals(email))
                {
                    return current.Data;
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

        public string GenerateDot()
        {
            var graph = "digraph LinkedList {\n";
            graph += "    node [shape=record];\n";
            graph += "    rankdir=LR;\n";
            graph += "    subgraph cluster_0 {\n";
            graph += "        label = \"Lista Simple Enlazada\";\n";

            LinkedNode current = head;
            int index = 0;

            while (current != null)
            {
                graph += $"        n{index} {current.Data.ToDotNode()}\n";
                current = current.Next;
                index++;
            }

            for (int i = 0; i < index - 1; i++)
            {
                graph += $"        n{i} -> n{i + 1};\n";
            }

            graph += "    }\n";
            graph += "}\n";
            return graph;
        }
    }
}
```

### Nodo (LinkedNode)
```csharp
using Classes;

namespace Structures
{
    public class LinkedNode
    {
        public User Data { get; set; }
        public LinkedNode Next { get; set; }

        public LinkedNode(User data)
        {
            Data = data;
            Next = null;
        }
    }
}

```

# Lista Doblemente Enlazada (DoublyLinkedList)

En este proyecto fue una lista menos utiliza que en el pasado pero aun asi de gran importancia. Es para mi de las listas la mejor por su facilidad de uso.
Es de mis estructuras favoritas.

### Lista

```csharp
using Classes;

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

        public void Insert(Vehicle data)
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
        }

        public string GenerateDot()
        {
            var graph = "digraph DoublyLinkedList {\n";
            graph += "    node [shape=record];\n";
            graph += "    rankdir=LR;\n";
            graph += "    subgraph cluster_0 {\n";
            graph += "        label = \"Lista Doblemente Enlazada\";\n";
            DoublyLinkedNode current = head;
            int index = 0;

            while (current != null)
            {
                graph += $"        n{index} {current.Data.ToDotNode()}\n";
                current = current.Next;
                index++;
            }

            for (int i = 0; i < index; i++)
            {
                if (i < index - 1)
                {
                    graph += $"        n{i} -> n{i + 1};\n";
                }
                if (i > 0)
                {
                    graph += $"        n{i} -> n{i - 1};\n";
                }
            }

            graph += "    }\n";
            graph += "}\n";
            return graph;
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
    }
}
```

## Árbol AVL (AVLTree)

En un principio me costo comprender sus rotaciones pero es una estructura muy buen. Puede parecesr complicada pero es fácil de comprender a la larga. Me gusto bastente comprender sus rotaciones y como se ordena y acomoda sola. Se convirtio en una de mis estructuras favoritas.

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

        public void Insert(SparePart data)
        {
            root = InsertRecursive(root, data);
        }

        private AVLNode InsertRecursive(AVLNode node, SparePart data)
        {
            if (node == null) return new AVLNode(data);

            if (data.Id < node.Data.Id)
                node.Left = InsertRecursive(node.Left, data);
            else if (data.Id > node.Data.Id)
                node.Right = InsertRecursive(node.Right, data);
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

        public string GenerateDot()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("digraph ALVTree {");
            sb.AppendLine("    node [shape=box];");
            sb.AppendLine("    rankdir=TB;");
            sb.AppendLine("    subgraph cluster_0 {");
            sb.AppendLine("        label = \"Árbol AVL\";");
            GenerateDotNodes(root, sb);
            GenerateDotConnections(root, sb);
            sb.AppendLine("    }"); 
            sb.AppendLine("}");
            return sb.ToString();
        }

        public void GenerateDotNodes(AVLNode node, StringBuilder sb)
        {
            if (node == null) return;
            sb.Append("        ").Append(node.Data.ToDotNode()).AppendLine();
            GenerateDotNodes(node.Left, sb);
            GenerateDotNodes(node.Right, sb);
        }

        public void GenerateDotConnections(AVLNode node, StringBuilder sb)
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
    }
}
```

## Árbol Binario (BinaryTree)

Me gustó pero no como el árbol AVL. Sigue siendo util para buscar pero como se desacomoda el O(n) aumenta por lo que no me parece tan util como el AVL.

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

        public void Insert(Service data)
        {
            if (root == null)
                root = new BinaryNode(data);
            else
                InsertRecursive(root, data);
        }

        private void InsertRecursive(BinaryNode node, Service data)
        {
            if (data.Id < node.Data.Id)
            {
                if (node.Left == null)
                    node.Left = new BinaryNode(data);
                else
                    InsertRecursive(node.Left, data);
            }
            else if (data.Id > node.Data.Id)
            {
                if (node.Right == null)
                    node.Right = new BinaryNode(data);
                else
                    InsertRecursive(node.Right, data);
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
            sb.Append("        ").Append(node.Data.ToDotNode()).AppendLine();
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
    }
}

```

## Árbol B

Es al momento la estrutura de datos más complicada que me ha tocado ver y hacer. Es tan complicada que para mi no merece la pena tanto. Puede tener sus beneficios pero no creo que los valga.

### Árbol
```csharp
using Classes;
using Gtk;
using Global;
using System.Text;

namespace Structures
{
    public class BTree
    {
        private BNode root;
        private readonly int order;
        private readonly int maxKeys;
        private readonly int minKeys;

        public BTree(int order)
        {
            this.order = order;
            maxKeys = order - 1;
            minKeys = (order / 2) - 1;
            root = new BNode(order, maxKeys, minKeys);
        }

        public void Insert(Invoice invoice)
        {
            if (root.IsFull())
            {
                BNode newRoot = new BNode(order, maxKeys, minKeys);
                newRoot.IsLeaf = false;
                newRoot.Children.Add(root);
                SplitChild(newRoot, 0);
                root = newRoot;
            }

            InsertNotFilled(root, invoice);
        }

        private void SplitChild(BNode father, int index)
        {
            BNode child = father.Children[index];
            BNode newChild = new BNode(order, maxKeys, minKeys);
            newChild.IsLeaf = child.IsLeaf;
            Invoice middleInvoice = child.Keys[minKeys];

            for (int i = minKeys + 1; i < maxKeys; i++)
            {
                newChild.Keys.Add(child.Keys[i]);
            }

            if (!child.IsLeaf)
            {
                for (int i = minKeys + 1; i < order; i++)
                {
                    newChild.Children.Add(child.Children[i]);
                }
                child.Keys.RemoveRange(minKeys + 1, child.Children.Count - (minKeys + 1));
            }

            child.Keys.RemoveRange(minKeys, child.Keys.Count - minKeys);

            father.Children.Insert(index + 1, newChild);

            father.InsertInvoice(middleInvoice);
        }

        public void InsertNotFilled(BNode node, Invoice invoice)
        {
            if (node.IsLeaf)
            {
                node.InsertInvoice(invoice);
            }
            else
            {
                int i = node.GetInsertIndex(invoice.Id);

                if (node.Children[i].IsFull())
                {
                    SplitChild(node, i);

                    if (invoice.Id > node.Keys[i].Id)
                    {
                        i++;
                    }
                }

                InsertNotFilled(node.Children[i], invoice);
            }
        }

        public Invoice Get(int id)
        {
            return GetRecursive(root, id);
        }

        public Invoice GetRecursive(BNode node, int id)
        {
            int i = node.GetSearchIndex(id);

            if (i < node.Keys.Count && id == node.Keys[i].Id)
            {
                return node.Keys[i];
            }

            if (node.IsLeaf)
            {
                return null;
            }

            return GetRecursive(node.Children[i], id);
        }

        public ListStore GetUserInvoices(int userId)
        {
            ListStore result = new ListStore(typeof(int), typeof(int), typeof(string));
            GetUserInvoicesRecursive(root, result, userId);
            return result;
        }

        public void GetUserInvoicesRecursive(BNode node, ListStore result, int userId)
        {
            if (node == null) return;

            foreach (var invoice in node.Keys)
            {
                Service service = GlobalStructures.ServicesTree.Get(invoice.ServiceId);
                if (service != null)
                {
                    Vehicle vehicle = GlobalStructures.VehiclesList.Get(service.VehicleId);
                    if (vehicle != null && vehicle.UserId == userId)
                        result.AppendValues(invoice.Id, invoice.ServiceId, invoice.Total.ToString("F2"));
                }
            }

            if (!node.IsLeaf)
            {
                foreach (var child in node.Children)
                {
                    GetUserInvoicesRecursive(child, result, userId);
                }
            }
        }

        public void Delete(int id)
        {
            DeleteRecursive(root, id);

            if (root.Keys.Count == 0 && !root.IsLeaf)
            {
                BNode oldRoot = root;
                root = root.Children[0];
            }
        }

        private void DeleteRecursive(BNode node, int id)
        {
            int index = node.GetSearchIndex(id);

            if (index < node.Keys.Count && node.Keys[index].Id == id)
            {
                if (node.IsLeaf)
                {
                    node.Keys.RemoveAt(index);
                }
                else
                {
                    DeleteInternalNode(node, index);
                }
            }
            else
            {
                if (node.IsLeaf)
                {
                    return;
                }

                bool lastChild = index == node.Keys.Count;

                if (node.Children[index].IsUnderflow())
                {
                    FillChild(node, index);
                }

                if (lastChild && index > node.Children.Count - 1)
                {
                    DeleteRecursive(node.Children[index - 1], id);
                }
                else
                {
                    DeleteRecursive(node.Children[index], id);
                }
            }
        }

        private void DeleteInternalNode(BNode node, int index)
        {
            Invoice invoice = node.Keys[index];

            if (node.Children[index].Keys.Count > minKeys)
            {
                Invoice predecessor = GetPredecessor(node, index);
                node.Keys[index] = predecessor;
                DeleteRecursive(node.Children[index], predecessor.Id);
            }
            else if (node.Children[index + 1].Keys.Count > minKeys)
            {
                Invoice successor = GetSuccessor(node, index);
                node.Keys[index] = successor;
                DeleteRecursive(node.Children[index + 1], successor.Id);
            }
            else
            {
                MergeNodes(node, index);
                DeleteRecursive(node.Children[index], invoice.Id);
            }
        }

        private Invoice GetPredecessor(BNode node, int index)
        {
            BNode current = node.Children[index];
            while (!current.IsLeaf)
            {
                current = current.Children[current.Keys.Count];
            }
            return current.Keys[current.Keys.Count - 1];
        }

        private Invoice GetSuccessor(BNode node, int index)
        {
            BNode current = node.Children[index + 1];
            while (!current.IsLeaf)
            {
                current = current.Children[0];
            }
            return current.Keys[0];
        }

        private void FillChild(BNode node, int index)
        {
            if (index > 0 && node.Children[index - 1].Keys.Count > minKeys)
            {
                BorrowFromPrevious(node, index);
            }
            else if (index < node.Keys.Count && node.Children[index + 1].Keys.Count > minKeys)
            {
                BorrowFromNext(node, index);
            }
            else
            {
                if (index < node.Keys.Count)
                {
                    MergeNodes(node, index);
                }
                else
                {
                    MergeNodes(node, index - 1);
                }
            }
        }

        private void MergeNodes(BNode node, int index)
        {
            BNode child = node.Children[index];
            BNode sibling = node.Children[index + 1];

            child.Keys.Add(node.Keys[index]);

            for (int i = 0; i < sibling.Keys.Count; i++)
            {
                child.Keys.Add(sibling.Keys[i]);
            }

            if (!child.IsLeaf)
            {
                for (int i = 0; i < sibling.Children.Count; i++)
                {
                    child.Children.Add(sibling.Children[i]);
                }
            }

            node.Keys.RemoveAt(index);
            node.Children.RemoveAt(index + 1);
        }

        private void BorrowFromPrevious(BNode node, int index)
        {
            BNode child = node.Children[index];
            BNode sibling = node.Children[index - 1];

            child.Keys.Insert(0, node.Keys[index - 1]);

            if (!child.IsLeaf)
            {
                child.Children.Insert(0, sibling.Children[sibling.Keys.Count]);
                sibling.Children.RemoveAt(sibling.Keys.Count);
            }

            node.Keys[index - 1] = sibling.Keys[sibling.Keys.Count - 1];
            sibling.Keys.RemoveAt(sibling.Keys.Count - 1);
        }

        private void BorrowFromNext(BNode node, int index)
        {
            BNode child = node.Children[index];
            BNode sibling = node.Children[index + 1];

            child.Keys.Add(node.Keys[index]);

            if (!child.IsLeaf)
            {
                child.Children.Add(sibling.Children[0]);
                sibling.Children.RemoveAt(0);
            }

            node.Keys[index] = sibling.Keys[0];
            sibling.Keys.RemoveAt(0);
        }

        public bool IsEmpty()
        {
            return root.Keys.Count == 0;
        }

        public string GenerateDot()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("digraph BTree {");
            sb.AppendLine("    node [shape=record];");
            sb.AppendLine("    rankdir=TB;");
            sb.AppendLine("    subgraph cluster_0 {");
            sb.AppendLine("        label = \"Árbol B\";");

            int nodeCounter = 0;
            GenerateDotRecursive(root, sb, ref nodeCounter);

            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }

        private void GenerateDotRecursive(BNode node, StringBuilder sb, ref int nodeCounter)
        {
            if (node == null)
                return;

            int currentNode = nodeCounter++;

            StringBuilder nodeLabel = new StringBuilder();
            nodeLabel.Append($"        n{currentNode} [label=\"");

            for (int i = 0; i < node.Keys.Count; i++)
            {
                if (i > 0)
                    nodeLabel.Append("|");
                nodeLabel.Append($"<f{i}> |Id: {node.Keys[i].Id}, Orden: {node.Keys[i].ServiceId}, Total: {node.Keys[i].Total}|");
            }

            if (node.Keys.Count > 0)
                nodeLabel.Append($"<f{node.Keys.Count}>");

            nodeLabel.Append("\"];");
            sb.AppendLine(nodeLabel.ToString());

            if (!node.IsLeaf)
            {
                for (int i = 0; i <= node.Keys.Count; i++)
                {
                    int childPosition = nodeCounter;
                    GenerateDotRecursive(node.Children[i], sb, ref nodeCounter);
                    sb.AppendLine($"        n{currentNode}:f{i} -> n{childPosition};");
                }
            }
        }
    }
}
```

### Nodo (BNode)
```csharp
using Classes;

namespace Structures
{
    public class BNode
    {
        public List<Invoice> Keys { get; set; }
        public List<BNode> Children { get; set; }
        public bool IsLeaf { get; set; }
        private int maxKeys;
        private int minKeys;

        public BNode(int order, int maxKeys, int minKeys)
        {
            Keys = new List<Invoice>(maxKeys);
            Children = new List<BNode>(order);
            IsLeaf = true;
            this.maxKeys = maxKeys;
            this.minKeys = minKeys;
        }

        public void InsertInvoice(Invoice invoice)
        {
            Keys.Insert(GetInsertIndex(invoice.Id), invoice);
        }

        public int GetInsertIndex(int id)
        {
            int i = Keys.Count - 1;

            while (i >= 0 && id < Keys[i].Id)
            {
                i--;
            }

            return i + 1;
        }

        public int GetSearchIndex(int id)
        {
            int i = 0;

            while (i < Keys.Count && id > Keys[i].Id)
            {
                i++;
            }

            return i;
        }

        public bool IsFull()
        {
            return Keys.Count == maxKeys;
        }

        public bool IsUnderflow()
        {
            return Keys.Count < minKeys;
        }

    }
}

```