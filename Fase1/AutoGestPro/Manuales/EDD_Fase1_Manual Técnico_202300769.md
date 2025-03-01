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

Para almacenar los datos en memoria se utilizaron estructuras de datos abstractas. Se utilizo la lista simple para almacenar los usuarios, la lista doblemente enlazada para los vehículos, la lista circular para los repuestos, la cola para los servicios, la pila para las facturas y la matriz dispersa se utilizó para crear una bitacora con los datos del id de autos, el id de los vehiculos y el detalle del servicio que se le aplicaria al vehículo.

## Lógica del programa

## Lista Enlazada (LinkedList)

Fue la estructura abstracta más utilizada ya que almacenaba los usuarios. Su facilidad en manipulación la con los apuntadores la hizo muy buena para realizar varios metodos requeridos para la gestión de los usuarios.

### Código

```csharp
using System;
using System.Runtime.InteropServices;

namespace Lists
{
    public unsafe class LinkedList
    {
        private LinkedNode* head;

        public LinkedList()
        {
            head = null;
        }

        public void Insert(int id, string names, string lastNames, string email, string password)
        {
            LinkedNode* newNode = (LinkedNode*)NativeMemory.Alloc((nuint)sizeof(LinkedNode));
            *newNode = new LinkedNode(id, names, lastNames, email, password);

            if (head == null)
            {
                head = newNode;   
            }
            else
            {
                LinkedNode* current = head;
                while (current->Next != null)
                {
                    current = current->Next;
                }
                current->Next = newNode;
            }
        }

        public void Delete(int id)
        {
            if (head == null) return;

            if (head->Id == id)
            {
                LinkedNode* temp = head;
                head = head->Next;
                temp->FreeData();
                NativeMemory.Free(temp);
                return;
            }

            LinkedNode* current = head;

             while (current->Next != null && current->Next->Id != id)
            {
                current = current->Next;
            }

            if (current->Next != null)
            {
                LinkedNode* temp = current->Next;
                current->Next = current->Next->Next;
                temp->FreeData();
                NativeMemory.Free(temp);
            }
        }

        public void Print()
        {
            Console.WriteLine("-------------------------------------------------------------------------------");
            LinkedNode* current = head;
            while (current != null)
            {
                Console.WriteLine(current->ToString());
                current = current->Next;
            }
        }

        public LinkedNode* GetUser(int id)
        {
            LinkedNode* current = head;
            while (current != null)
            {
                if (current->Id == id)
                {
                    return current;
                }
                current = current->Next;
            }
            return null;
        }

        public bool Contains(int id)
        {
            return GetUser(id) != null;
        }

        public LinkedNode* UpdateUser(int id, string newNames, string newLastNames, string newEmail)
        {
            LinkedNode* user = GetUser(id);
            if (user!= null)
            {
                user->UpdateNode(newNames, newLastNames, newEmail);
            }
            return user;
        }

        public bool IsEmpty()
        {
            if (head == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GenerateGraph()
        {
            var graph = "digraph G {\n";
            graph += "    node [shape=record];\n";
            graph += "    rankdir=LR;\n";
            graph += "    subgraph cluster_0 {\n";
            graph += "        label = \"Lista Simple Enlazada\";\n";
            
            LinkedNode* current = head;
            int index = 0;

            while (current != null)
            {
                graph += $"        n{index} {current->ToGraph()}\n";
                current = current->Next;
                index++;
            }

            for (int i = 0; i < index-1; i++)
            {
                graph += $"        n{i} -> n{i + 1};\n";
            }

            graph += "    }\n";
            graph += "}\n";
            return graph;
        }

        ~LinkedList()
        {
            while (head != null)
            {
                LinkedNode* temp = head;
                head = head->Next;
                temp->FreeData();
                NativeMemory.Free(temp);
            }
        }
    }
}
```

### Nodo de la lista enlazada (LinkedNode)

```csharp
using System;
using System.Runtime.InteropServices;

namespace Lists
{
    public unsafe struct LinkedNode 
    {
        public int Id;
        public IntPtr Names;
        public IntPtr LastNames;
        public IntPtr Email;
        public IntPtr Password;
        public LinkedNode* Next;

        public LinkedNode(int id, string names, string lastNames, string email, string password)
        {
            Id = id;
            Names = Marshal.StringToHGlobalUni(names);
            LastNames = Marshal.StringToHGlobalUni(lastNames);
            Email = Marshal.StringToHGlobalUni(email);
            Password = Marshal.StringToHGlobalUni(password);
            Next = null;
        }

        public void FreeData()
        {
            if (Names != IntPtr.Zero) Marshal.FreeHGlobal(Names);
            if (LastNames != IntPtr.Zero) Marshal.FreeHGlobal(LastNames);
            if (Email != IntPtr.Zero) Marshal.FreeHGlobal(Email);
            if (Password != IntPtr.Zero) Marshal.FreeHGlobal(Password);
        }

        public string ToGraph()
        {
            return $"[label = \"{{<data> ID: {Id} \\n Nombre: {GetNames() + " " + GetLastNames()} \\n Correo: {GetEmail()} \\n Contraseña: {GetPassword()}}}\"];";
        }

        public void UpdateNode(string newNames, string newLastNames, string newEmail)
        {
            SetNames(newNames);
            SetLastNames(newLastNames);
            SetEmail(newEmail);
        }

        public void SetNames(string newNames)
        {
            if (!string.IsNullOrEmpty(newNames) && newNames != GetNames())
            {
                if (Names != IntPtr.Zero) Marshal.FreeHGlobal(Names);
                Names = Marshal.StringToHGlobalUni(newNames);
            }
        }

        public void SetLastNames(string newLastNames)
        {
            if (!string.IsNullOrEmpty(newLastNames) && newLastNames!= GetLastNames())
            {
                if (LastNames!= IntPtr.Zero) Marshal.FreeHGlobal(LastNames);
                LastNames = Marshal.StringToHGlobalUni(newLastNames);
            }
        }

        public void SetEmail(string newEmail)
        {
            if (!string.IsNullOrEmpty(newEmail) && newEmail!= GetEmail())
            {
                if (Email!= IntPtr.Zero) Marshal.FreeHGlobal(Email);
                Email = Marshal.StringToHGlobalUni(newEmail);
            }
        }

        public void SetPassword(string newPassword)
        {
            if (!string.IsNullOrEmpty(newPassword) && newPassword!= GetPassword())
            {
                if (Password!= IntPtr.Zero) Marshal.FreeHGlobal(Password);
                Password = Marshal.StringToHGlobalUni(newPassword);
            }
        }

        public string? GetNames() => Marshal.PtrToStringUni(Names);
        public string? GetLastNames() => Marshal.PtrToStringUni(LastNames);
        public string? GetEmail() => Marshal.PtrToStringUni(Email);
        public string? GetPassword() => Marshal.PtrToStringUni(Password);

        public override string ToString() => $"Id: {Id}, Nombres: {GetNames()}, Apellidos: {GetLastNames()}, Correo: {GetEmail()}, Contraseña: {GetPassword()}";
    }
}
```

## Lista Doblemente Enlazada (DoubleList)

La lista doblemente enlazada fue utilizada para almacenar la información de los vehiculos. Al tener 2 nodos en ambas direcciones la hace mucho más facil de manipular a la hora de eliminar nodos en cualquier parte. Fue de las listas mas utilizadas en el programa.

### Nodo (DoubleNode)
```csharp
using System;
using System.Runtime.InteropServices;

namespace Lists
{
    public unsafe struct DoubleNode
    {
        public int Id;
        public int UserId;
        public IntPtr Brand;
        public int Model;
        public IntPtr Plate;
        public int ServiceCounter;
        public DoubleNode* Next;
        public DoubleNode* Prev;

        public DoubleNode(int id, int userId, string brand, int model, string plate)
        {
            Id = id;
            UserId = userId;
            Brand = Marshal.StringToHGlobalUni(brand);
            Model = model;
            Plate = Marshal.StringToHGlobalUni(plate);
            ServiceCounter = 0;
            Next = null;
            Prev = null;
        }

        public void FreeData()
        {
            if (Brand != IntPtr.Zero) Marshal.FreeHGlobal(Brand);
            if (Plate != IntPtr.Zero) Marshal.FreeHGlobal(Plate);
        }

        public string ToGraph()
        {
            return $"[label = \"{{<data> ID: {Id} \\n ID_Usuario: {UserId} \\n Marca: {GetBrand()} \\n Modelo: {Model} \\n Placa: {GetPlate()}}}\"];";
        }

        public static DoubleNode* CloneNode(DoubleNode* node)
        {
            if (node == null) return null;

            DoubleNode* newNode = (DoubleNode*)NativeMemory.Alloc((nuint)sizeof(DoubleNode));
            *newNode = new DoubleNode(node->Id, node->UserId, node->GetBrand(), node->Model, node->GetPlate());
            newNode->ServiceCounter = node->ServiceCounter;
            return newNode;
        }

        public string? GetBrand() => Marshal.PtrToStringUni(Brand);
        public string? GetPlate() => Marshal.PtrToStringUni(Plate);

        public override string ToString() => $"Id: {Id}, Id Usuario: {UserId}, Marca: {GetBrand()}, Modelo: {Model}, Placa: {GetPlate()}, Servicios: {ServiceCounter}";
    }
}
```

### Lista
```csharp
using System;
using Gtk;
using System.Runtime.InteropServices;

namespace Lists
{
    public unsafe class DoubleList
    {
        public DoubleNode* head;
        public DoubleNode* tail;

        public DoubleList()
        {
            head = null;
            tail = null;
        }

        public void Insert(int id, int userId, string brand, int model, string plate)
        {
            DoubleNode* newNode = (DoubleNode*)NativeMemory.Alloc((nuint)sizeof(DoubleNode));
            *newNode = new DoubleNode(id, userId, brand, model, plate);

            if (head == null)
            {
                head = tail = newNode;
            }
            else
            {
                tail->Next = newNode;
                newNode->Prev = tail;
                tail = newNode;
            }
        }

        public DoubleNode* GetVehicle(int id)
        {
            DoubleNode* current = head;
            while (current != null)
            {
                if (current->Id == id) return current;
                current = current->Next;
            }
            return null;
        }

        public bool IsEmpty()
        {
            if (head == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Contains(int id)
        {
            return GetVehicle(id) != null;
        }

        public TreeView GenerateTreeView(int userId)
        {
            TreeView treeView = new TreeView();
            ListStore listStore = new ListStore(typeof(string), typeof(string), typeof(string), typeof(string));

            CellRendererText cellRenderer = new CellRendererText();
            cellRenderer.FontDesc = Pango.FontDescription.FromString("Arial 12");

            treeView.AppendColumn("ID", cellRenderer, "text", 0);
            treeView.AppendColumn("Marca", cellRenderer, "text", 1);
            treeView.AppendColumn("Modelo", cellRenderer, "text", 2);
            treeView.AppendColumn("Placa", cellRenderer, "text", 3);

            DoubleNode* current = head;

            while (current != null)
            {
                if (current->UserId == userId)
                {
                    listStore.AppendValues(
                        current->Id.ToString(),
                        current->GetBrand(),
                        current->Model.ToString(),
                        current->GetPlate()
                    );
                }
                current = current->Next;
            }
            treeView.Model = listStore;
            return treeView;
        }

        public string GenerateGraph()
        {
            var graph = "digraph G {\n";
            graph += "    node [shape=record];\n";
            graph += "    rankdir=LR;\n";
            graph += "    subgraph cluster_0 {\n";
            graph += "        label = \"Lista Doblemente Enlazada\";\n";
            DoubleNode* current = head;
            int index = 0;

            while (current != null)
            {
                graph += $"        n{index} {current->ToGraph()}\n";
                current = current->Next;
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

        public void Delete(int userId)
        {
            if (head == null) return;

            DoubleNode* current = head;
            while (current != null)
            {
                if (current->UserId == userId)
                {
                    if (current->Prev != null)
                        current->Prev->Next = current->Next;
                    else
                        head = current->Next;

                    if (current->Next != null)
                        current->Next->Prev = current->Prev;
                    else
                        tail = current->Prev;
                    current->FreeData();
                    NativeMemory.Free(current);
                }
                current = current->Next;
            }
        }

        public void Print()
        {
            Console.WriteLine("-------------------------------------------------------------------------------");
            DoubleNode* current = head;
            while (current != null)
            {
                Console.WriteLine(current->ToString());
                current = current->Next;
            }

        }

        public void Truncate(int n)
        {
            if (n <= 0)
            {
                Clean();
                return;
            }

            DoubleNode* current = head;
            int count = 1;

            while (current != null && count < n)
            {
                current = current->Next;
                count++;
            }

            if (current == null)
            {
                return;
            }

            DoubleNode* nodeDelete = current->Next;
            current->Next = null;

            while (nodeDelete != null)
            {
                DoubleNode* temp = nodeDelete;
                nodeDelete = nodeDelete->Next;
                temp->FreeData();
                NativeMemory.Free(temp);
            }

            tail = current;
        }

        public void SortByModel()
        {
            if (head == null || head->Next == null) return;

            bool swapped;
            do
            {
                swapped = false;
                DoubleNode* current = head;
                DoubleNode* last = null;

                while (current->Next != last)
                {
                    if (current->Model > current->Next->Model)
                    {
                        DoubleNode* nextNode = current->Next;
                        DoubleNode* prevNode = current->Prev;

                        if (prevNode != null)
                            prevNode->Next = nextNode;
                        else
                            head = nextNode;

                        current->Next = nextNode->Next;
                        nextNode->Prev = prevNode;

                        if (current->Next != null)
                            current->Next->Prev = current;

                        nextNode->Next = current;
                        current->Prev = nextNode;

                        swapped = true;
                    }
                    else
                    {
                        current = current->Next;
                    }
                }
                last = current;
            } while (swapped);

            DoubleNode* tmp = head;
            while (tmp->Next != null)
            {
                tmp = tmp->Next;
            }
            tail = tmp;
        }

        public void SortByNumServices()
        {
            if (head == null || head->Next == null) return;

            bool swapped;
            do
            {
                swapped = false;
                DoubleNode* current = head;
                DoubleNode* last = null;

                while (current->Next != last)
                {
                    if (current->ServiceCounter < current->Next->ServiceCounter)
                    {
                        DoubleNode* nextNode = current->Next;
                        DoubleNode* prevNode = current->Prev;

                        if (prevNode != null)
                            prevNode->Next = nextNode;
                        else
                            head = nextNode;

                        current->Next = nextNode->Next;
                        nextNode->Prev = prevNode;

                        if (current->Next != null)
                            current->Next->Prev = current;

                        nextNode->Next = current;
                        current->Prev = nextNode;

                        swapped = true;
                    }
                    else
                    {
                        current = current->Next;
                    }
                }
                last = current;
            } while (swapped);

            DoubleNode* tmp = head;
            while (tmp->Next != null)
            {
                tmp = tmp->Next;
            }
            tail = tmp;
        }

        public DoubleList Duplicate()
        {
            DoubleList newList = new DoubleList();
            DoubleNode* current = head;

            while (current != null)
            {
                DoubleNode* clonedNode = DoubleNode.CloneNode(current);
                if (newList.head == null)
                {
                    newList.head = newList.tail = clonedNode;
                }
                else
                {
                    newList.tail->Next = clonedNode;
                    clonedNode->Prev = newList.tail;
                    newList.tail = clonedNode;
                }
                current = current->Next;
            }

            return newList;
        }

        public string Top5Model()
        {
            DoubleList topVehicles = Duplicate();
            topVehicles.SortByModel();
            topVehicles.Truncate(5);

            string message = "Top 5 vehículos más antiguos\n\n";

            DoubleNode* current = topVehicles.head;
            int number = 1;

            while (current != null)
            {
                message += $"Número {number} -- ID: {current->Id}, Marca: {current->GetBrand()}, Modelo: {current->Model}\n";
                current = current->Next;
                number++;
            }

            return message;
        }

        public string Top5Services()
        {
            DoubleList topVehicles = Duplicate();
            topVehicles.SortByNumServices();
            topVehicles.Truncate(5);

            string message = "Top 5 vehículos con más servicios\n\n";

            DoubleNode* current = topVehicles.head;
            int number = 1;

            while (current != null)
            {
                message += $"Número {number} -- ID: {current->Id}, Marca: {current->GetBrand()}, Cantidad de servicios: {current->ServiceCounter}\n";
                current = current->Next;
                number++;
            }

            return message;
        }

        public void Clean()
        {
            while (head != null)
            {
                DoubleNode* temp = head;
                head = head->Next;
                temp->FreeData();
                NativeMemory.Free(temp);
            }
            tail = null;
        }

        ~DoubleList()
        {
            Clean();
        }
    }
}
```
## Lista Circular (CircularList)

La lista doblemente enlazada fue utilizada para almacenar la información de los repuestos. Posiblemente la lista menos utilizada en el programa y de las que, personalmente, menos me gustan. Para mi no tiene nada realmente destacable más que agregar un apuntador al principio que solo ayuda a complicar un poco las cosas.

### Nodo (CircularNode)
```csharp
using System;
using System.Runtime.InteropServices;

namespace Lists 
{
    public unsafe struct CircularNode 
    {
        public int Id;
        public IntPtr SparePart;
        public IntPtr Details;
        public double Cost;
        public CircularNode* Next;

        public CircularNode(int id, string sparePart, string details, double cost)
        {
            Id = id;
            SparePart = Marshal.StringToHGlobalUni(sparePart);
            Details = Marshal.StringToHGlobalUni(details);
            Cost = cost;
            Next = null;
        }

        public void FreeData()
        {
            if (SparePart != IntPtr.Zero) Marshal.FreeHGlobal(SparePart);
            if (Details!= IntPtr.Zero) Marshal.FreeHGlobal(Details);
        }

        public string ToGraph()
        {
            return $"[label = \"{{<data> ID: {Id} \\n Repuesto: {GetSparePart()} \\n Detalles: {GetDetails()} \\n Costos: {Cost}}}\"];";
        }

        public string? GetSparePart() => Marshal.PtrToStringUni(SparePart);
        public string? GetDetails() => Marshal.PtrToStringUni(Details);

        public override string ToString() => $"Id: {Id}, Repuesto: {GetSparePart()}, Detalles: {GetDetails()}, Costo: {Cost}";
    }
}
```

### Lista
```csharp
using System;
using System.Runtime.InteropServices;

namespace Lists
{
    public unsafe class CircularList
    {
        private CircularNode* head;

        public CircularList()
        {
            head = null;
        }

        public void Insert(int id, string sparePart, string details, double cost)
        {
            CircularNode* newNode = (CircularNode*)NativeMemory.Alloc((nuint)sizeof(CircularNode));
            *newNode = new CircularNode(id, sparePart, details, cost);

            if (head == null)
            {
                head = newNode;
                head->Next = head;
            }
            else
            {
                CircularNode* current = head;
                while (current->Next != head)
                {
                    current = current->Next;
                }
                current->Next = newNode;
                newNode->Next = head;
            }
        }

        public CircularNode* GetSparePart(int id)
        {
            if (head == null) return null;

            CircularNode* current = head;
            do
            {
                if (current->Id == id)
                {
                    return current;
                }
                current = current->Next;
            } while (current!= head);
            
            return null;
        }

        public void Delete(int id)
        {
            if (head == null) return;

            if (head->Id == id && head->Next == head)
            {
                head->FreeData();
                NativeMemory.Free(head);
                head = null;
                return;
            }

            CircularNode* current = head;
            CircularNode* prev = null;
            do
            {
                if (current->Id == id)
                {
                    if (prev != null)
                    {
                        prev->Next = current->Next;
                    }
                    else
                    {
                        CircularNode* last = head;
                        while (last->Next != head)
                        {
                            last = last->Next;
                        }
                        head = head->Next;
                        last->Next = head;
                    }
                    current->FreeData();
                    NativeMemory.Free(current);
                    return;
                }
                prev = current;
                current = current->Next;
            } while (current != head);
        }

        public void Print()
        {
            Console.WriteLine("-------------------------------------------------------------------------------");
            CircularNode* current = head;
            do
            {
                Console.WriteLine(current->ToString());
                current = current->Next;
            } while (current != head);
        }

        public string GenerateGraph()
        {
            var graph = "digraph G {\n";
            graph += "    node [shape=record];\n";
            graph += "    rankdir=LR;\n";
            graph += "    subgraph cluster_0 {\n";
            graph += "        label = \"Lista Circular\";\n";

            CircularNode* current = head;
            int index = 0;

            do
            {
                graph += $"        n{index} {current->ToGraph()}\n";
                current = current->Next;
                index++;
            } while (current != head);

            for (int i = 0; i < index; i++)
            {
                graph += $"        n{i} -> n{(i + 1) % index};\n";
            }

            graph += "    }\n";
            graph += "}\n";
            return graph;

        }

        public bool IsEmpty()
        {
            if (head == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        ~CircularList()
        {
            if (head == null) return;

            CircularNode* temp = head;
            CircularNode* next = null;

            do
            {
                next = temp->Next;
                NativeMemory.Free(temp);
                temp = next;
            } while (temp != head);

            head = null;
        }
    }
}
```

## Cola (Queue)

La cola fue utilizada para almacenar la información de los servicios. Fue muy poco utilizada en este proyecto. En otros contextos puede ser más util pero en este proyecto se le saco poco provecho ya que ni fue necesario utilizar su método de descolar.

### Nodo (QueueNode)
```csharp
using System;
using System.Runtime.InteropServices;

namespace Lists 
{
    public unsafe struct QueueNode 
    {
        public int Id;
        public int SparePartId;
        public int VehicleId;
        public IntPtr Details;
        public double Cost;
        public QueueNode* Next;

        public QueueNode(int id, int sparePartId, int vehicleId, string details, double cost) 
        {
            Id = id;
            SparePartId = sparePartId;
            VehicleId = vehicleId;
            Details = Marshal.StringToHGlobalUni(details);
            Cost = cost;
            Next = null;
        }

        public void FreeData()
        {
            if (Details != IntPtr.Zero) Marshal.FreeHGlobal(Details);
        }

        public string ToGraph(int serviceNum)
        {
            return $"[label = \"{{<data> Servicio {serviceNum} \\n ID: {Id} \\n Id_Repuesto: {SparePartId} \\n Id_Vehículo: {VehicleId} \\n Detalles: {GetDetails()} \\n Costo: {Cost}}}\"];";
        }

        public string? GetDetails() => Marshal.PtrToStringUni(Details);
    
        public override string ToString() => $"Id: {Id}, Id Repuesto: {SparePartId}, Id Vehículo: {VehicleId}, Detalles: {GetDetails()}, Costo: {Cost}";
    }
}
```
### Lista
```csharp
using System;
using System.Runtime.InteropServices;

namespace Lists 
{
    public unsafe struct QueueNode 
    {
        public int Id;
        public int SparePartId;
        public int VehicleId;
        public IntPtr Details;
        public double Cost;
        public QueueNode* Next;

        public QueueNode(int id, int sparePartId, int vehicleId, string details, double cost) 
        {
            Id = id;
            SparePartId = sparePartId;
            VehicleId = vehicleId;
            Details = Marshal.StringToHGlobalUni(details);
            Cost = cost;
            Next = null;
        }

        public void FreeData()
        {
            if (Details != IntPtr.Zero) Marshal.FreeHGlobal(Details);
        }

        public string ToGraph(int serviceNum)
        {
            return $"[label = \"{{<data> Servicio {serviceNum} \\n ID: {Id} \\n Id_Repuesto: {SparePartId} \\n Id_Vehículo: {VehicleId} \\n Detalles: {GetDetails()} \\n Costo: {Cost}}}\"];";
        }

        public string? GetDetails() => Marshal.PtrToStringUni(Details);
    
        public override string ToString() => $"Id: {Id}, Id Repuesto: {SparePartId}, Id Vehículo: {VehicleId}, Detalles: {GetDetails()}, Costo: {Cost}";
    }
}
```

## Pila (Stack)

La pila fue utilizada para almacenar la información de los facturas. Su peculiaridad de sacar lo primero que se ingresa con la función pop es realmente utíl en este tipo de situaciones como el manejo de facturas y cobros.

### Nodo (StackNode)
```csharp
using System;
using System.Runtime.InteropServices;

namespace Lists 
{
    public unsafe struct StackNode 
    {
        public int Id;
        public int OrderId;
        public double TotalCost;
        public StackNode* Next;

        public StackNode(int id, int orderId, double totalCost) 
        {
            Id = id;
            OrderId = orderId;
            TotalCost = totalCost;
            Next = null;
        }

        public string ToGraph(int invoiceNum)
        {
            return $"[label = \"{{<data> Factura {invoiceNum} \\n ID: {Id} \\n Id_Orden: {OrderId} \\n Total: {TotalCost}}}\"];";
        }
        
        public override string ToString() => $"Id: {Id}, Id_Orden: {OrderId}, Total: {TotalCost}";
    }
}
```

### Lista
```csharp
using System;
using System.Runtime.InteropServices;

namespace Lists
{
    public unsafe class Stack
    {
        private static int lastId = 0;
        private StackNode* top;
        private int count = 0;

        public Stack()
        {
            top = null;
        }

        public void Push(int orderId, double totalCost)
        {
            int newId = ++lastId;

            StackNode* newNode = (StackNode*)NativeMemory.Alloc((nuint)sizeof(StackNode));
            *newNode = new StackNode(newId, orderId, totalCost);
            newNode->Next = top;
            top = newNode;
            count++;
        }

        public StackNode* Pop()
        {
            if (top == null) return null;
            StackNode* temp = top;
            top = top->Next;
            count--;
            return temp;
        }

        public bool IsEmpty()
        {
            if (top == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GenerateGraph()
        {
            var graph = "digraph G {\n";
            graph += "    node [shape=record];\n";
            graph += "    rankdir=TB;\n";
            graph += "    subgraph cluster_0 {\n";
            graph += "        label = \"Pila\";\n";

            StackNode* current = top;
            int index = count;

            while (current != null)
            {
                graph += $"        n{index} {current->ToGraph(index)}\n";
                current = current->Next;
                index--;
            }

            for (int i = count; i > 1; i--)
            {
                graph += $"        n{i} -> n{i - 1};\n";
            }

            graph += "    }\n";
            graph += "}\n";
            return graph;
        }

        public void Print()
        {
            Console.WriteLine("-------------------------------------------------------------------------------");
            StackNode* current = top;
            while (current != null)
            {
                Console.WriteLine(current->ToString());
                current = current->Next;
            }
        }
    }
}
```

### Matriz Dispersa (SparseMatrix)

La matriz dispersa fue utilizada para almacenar una bitacora de la relación de vehículos con repuesto a la hora de crear un servicio. Es realmente útil para relacionar datos de distinto tipo y de mantener todo ordenado y fácil de acceder. Creo que no se le saco el provecho necesario a esta estructura en este proyecto ya que para su complejidad de creación y que tenga caracteristicas tan buenas fue utilizada para una cosa bastante simple en mi opinión. Para lo que siento que se le saco provecho fue para la gráfica. Me gusto esta estructura.

### Nodo Encabezado (HeaderNode)
```csharp
namespace Matrix
{
    public unsafe struct HeaderNode
    {
        public int id;
        public HeaderNode* Next;
        public HeaderNode* Prev;
        public InternalNode* accessFirst;
        public InternalNode* accessLast;

        public HeaderNode(int id)
        {
            this.id = id;
            Next = null;
            Prev = null;
            accessFirst = null;
        }
    }
}
```

### Nodo Interno (InternalNode)
```csharp
using System;
using System.Runtime.InteropServices;

namespace Matrix
{
    public unsafe struct InternalNode
    {
        public IntPtr Details;
        public int PosX;     
        public int PosY;
        public InternalNode* Up;
        public InternalNode* Down; 
        public InternalNode* Right;
        public InternalNode* Left;

        public InternalNode(int posX, int posY, string details)
        {
            PosX = posX;
            PosY = posY;
            Details = Marshal.StringToHGlobalUni(details);
            Up = null;
            Down = null;
            Right = null;
            Left = null;
        }   

        public string? GetDetails() => Marshal.PtrToStringUni(Details);
    }
}
```

### Lista Encabezado (HeaderList)

Esta lista es muy útil para el acceso de nodo. Teniendo un acceso por el primer nodo ingresado y otro por el ultimo ingresado lo hizo muy fácil de recorrer a la hora de realizar la gráfica.

```csharp
using System;
using System.Runtime.InteropServices;

namespace Matrix
{
    public unsafe class HeaderList
    {
        public HeaderNode* first;
        public HeaderNode* last;
        public int size;

        public HeaderList()
        {
            first = null;
            last = null;
            size = 0;
        }

        public void InsertHeaderNode(int id)
        {
            HeaderNode* newNode = (HeaderNode*)NativeMemory.Alloc((nuint)sizeof(HeaderNode));
            if (newNode == null) throw new InvalidOperationException("No se pudo asignar memoria para el nuevo nodo.");
            *newNode = new HeaderNode(id);

            size++;

            if (first == null)
            {
                first = newNode;
                last = newNode;
            }
            else
            {
                if (newNode->id < first->id)
                {
                    newNode->Next = first;
                    first->Prev = newNode;
                    first = newNode;
                }
                else if (newNode->id > last->id)
                {
                    last->Next = newNode;
                    newNode->Prev = last;
                    last = newNode;
                }
                else
                {
                    HeaderNode* tmp = first;
                    while (tmp != null)
                    {
                        if (newNode->id < tmp->id)
                        {
                            newNode->Next = tmp;
                            newNode->Prev = tmp->Prev;
                            tmp->Prev->Next = newNode;
                            tmp->Prev = newNode;
                            return;
                        }

                        tmp = tmp->Next;
                    }
                }
            }
        }

        public HeaderNode* GetHeader(int id)
        {
            HeaderNode* current = first;

            while (current != null)
            {
                if (id == current->id) return current;
                current = current->Next;
            }

            return null;
        }

        ~HeaderList()
        {
            if (first == null) return;

            while (first != null)
            {
                HeaderNode* tmp = first;
                first = first->Next;
                NativeMemory.Free(tmp);
            }
        }
    }
}
```

### Matriz
```csharp
using System;
using System.Runtime.InteropServices;
using Gtk;

namespace Matrix
{
    public unsafe class SparseMatrix
    {
        public HeaderList rows;
        public HeaderList cols;

        // Constructor de la clase MatrizDispersa
        public SparseMatrix()
        {
            rows = new HeaderList();
            cols = new HeaderList();
        }

        public void Insert(int posX, int posY, string Details)
        {
            InternalNode* newNode = (InternalNode*)NativeMemory.Alloc((nuint)sizeof(InternalNode));
            if (newNode == null) throw new InvalidOperationException("No se pudo asignar memoria para el nuevo nodo.");
            *newNode = new InternalNode(posX, posY, Details);

            HeaderNode* nodeX = rows.GetHeader(posX);
            HeaderNode* nodeY = cols.GetHeader(posY);

            if (nodeX == null)
            {
                rows.InsertHeaderNode(posX);
                nodeX = rows.GetHeader(posX);
            }

            if (nodeY == null)
            {
                cols.InsertHeaderNode(posY);
                nodeY = cols.GetHeader(posY);
            }

            if (nodeX == null || nodeY == null)
            {
                throw new InvalidOperationException("Error al crear los encabezados.");
            }

            if (nodeX->accessFirst == null)
            {
                nodeX->accessFirst = newNode;
                nodeX->accessLast = newNode;
            }
            else
            {
                if (newNode->PosY < nodeX->accessFirst->PosY)
                {
                    newNode->Right = nodeX->accessFirst;
                    nodeX->accessFirst->Left = newNode;
                    nodeX->accessFirst = newNode;
                }
                else if (newNode->PosY > nodeX->accessLast->PosY)
                {
                    nodeX->accessLast->Right = newNode;
                    newNode->Left = nodeX->accessLast;
                    nodeX->accessLast = newNode;
                }
                else
                {
                    InternalNode* tmp = nodeX->accessFirst;
                    while (tmp != null)
                    {
                        if (newNode->PosY < tmp->PosY)
                        {

                            newNode->Right = tmp;
                            newNode->Left = tmp->Left;
                            tmp->Left->Right = newNode;
                            tmp->Left = newNode;
                            break;
                        }
                        else if (newNode->PosX == tmp->PosX && newNode->PosY == tmp->PosY)
                        {
                            break;
                        }
                        else
                        {
                            tmp = tmp->Right;
                        }
                    }
                }
            }

            if (nodeY->accessFirst == null)
            {
                nodeY->accessFirst = newNode;
                nodeY->accessLast = newNode;
            }
            else
            {
                if (newNode->PosX < nodeY->accessFirst->PosX)
                {
                    newNode->Down = nodeY->accessFirst;
                    nodeY->accessFirst->Up = newNode;
                    nodeY->accessFirst = newNode;
                }
                else if (newNode->PosX > nodeY->accessLast->PosX)
                {
                    nodeY->accessLast->Down = newNode;
                    newNode->Up = nodeY->accessLast;
                    nodeY->accessLast = newNode;
                }
                else
                {
                    InternalNode* tmp2 = nodeY->accessFirst;
                    while (tmp2 != null)
                    {
                        if (newNode->PosX < tmp2->PosX)
                        {
                            newNode->Down = tmp2;
                            newNode->Up = tmp2->Up;
                            tmp2->Up->Down = newNode;
                            tmp2->Up = newNode;
                            break;
                        }
                        else if (newNode->PosX == tmp2->PosX && newNode->PosY == tmp2->PosY)
                        {
                            break;
                        }
                        else
                        {
                            tmp2 = tmp2->Down;
                        }
                    }
                }
            }
        }

        public void Print()
        {
            HeaderNode* y_col = cols.first;
            Console.Write("\t");

            while (y_col != null)
            {
                Console.Write(y_col->id + "\t");
                y_col = y_col->Next;
            }
            Console.WriteLine();

            HeaderNode* x_row = rows.first;
            while (x_row != null)
            {
                Console.Write(x_row->id + "\t");

                InternalNode* internalNode = x_row->accessFirst;
                HeaderNode* y_col_iter = cols.first;

                while (y_col_iter != null)
                {
                    if (internalNode != null && internalNode->PosY == y_col_iter->id)
                    {
                        Console.Write(internalNode->GetDetails() + "\t");
                        internalNode = internalNode->Right;
                    }
                    else
                    {
                        Console.Write("0\t");
                    }
                    y_col_iter = y_col_iter->Next;
                }
                Console.WriteLine();
                x_row = x_row->Next;
            }
        }

        public string GenerateGraph()
        {
            var graph = "digraph G {\n";
            graph += "    node [shape=box width=1.4];\n";
            graph += "    n0 [label=\"\" group=0];\n";

            HeaderNode* y_col = cols.first;
            HeaderNode* x_row = rows.first;

            while (x_row != null)
            {
                graph += $"    V{x_row->id} [label=\"V{x_row->id}\" group=0];\n";
                x_row = x_row->Next;
            }

            int i = 1;
            while (y_col != null)
            {
                graph += $"    R{y_col->id} [label=\"R{y_col->id}\" group={i}];\n";
                y_col = y_col->Next;
                i++;
            }

            i = 1;
            y_col = cols.first;
            while (y_col != null)
            {
                InternalNode* tmp = y_col->accessFirst;
                while (tmp != null)
                {
                    graph += $"    N{tmp->PosX}_{tmp->PosY} [label=\"Detalles: {tmp->GetDetails()}\" group={i}];\n";
                    tmp = tmp->Down;
                }
                y_col = y_col->Next;
                i++;
            }

            graph += $"    n0";
            y_col = cols.first;
            while (y_col != null)
            {
                graph += $" -> R{y_col->id}";
                y_col = y_col->Next;
            }
            graph += ";\n";

            graph += $"    ";
            y_col = cols.last;
            while (y_col != null)
            {
                graph += $"R{y_col->id} -> ";
                y_col = y_col->Prev;
            }
            graph += "n0;\n";

            x_row = rows.first;
            while (x_row != null)
            {
                graph += $"    V{x_row->id}";
                InternalNode* tmp1 = x_row->accessFirst;
                while (tmp1 != null)
                {
                    graph += $" -> N{tmp1->PosX}_{tmp1->PosY}";
                    tmp1 = tmp1->Right;
                }
                graph += ";\n";

                graph += $"    ";
                tmp1 = x_row->accessLast;
                while (tmp1 != null)
                {
                    graph += $"N{tmp1->PosX}_{tmp1->PosY} -> ";
                    tmp1 = tmp1->Left;
                }
                graph += $"V{x_row->id};\n";
                x_row = x_row->Next;
            }

            graph += $"    n0";
            x_row = rows.first;
            while (x_row != null)
            {
                graph += $" -> V{x_row->id}";
                x_row = x_row->Next;
            }
            graph += ";\n";

            graph += $"    ";
            x_row = rows.last;
            while (x_row != null)
            {
                graph += $"V{x_row->id} -> ";
                x_row = x_row->Prev;
            }
            graph += "n0;\n";

            y_col = cols.first;
            while (y_col != null)
            {
                graph += $"    R{y_col->id}";
                InternalNode* tmp2 = y_col->accessFirst;
                while (tmp2 != null)
                {
                    graph += $" -> N{tmp2->PosX}_{tmp2->PosY}";
                    tmp2 = tmp2->Down;
                }
                graph += ";\n";

                graph += $"    ";
                tmp2 = y_col->accessLast;
                while (tmp2 != null)
                {
                    graph += $"N{tmp2->PosX}_{tmp2->PosY} -> ";
                    tmp2 = tmp2->Up;
                }
                graph += $"R{y_col->id};\n";
                y_col = y_col->Next;
            }

            graph += $"    {{ rank=same; n0; ";
            y_col = cols.first;
            while (y_col != null)
            {
                graph += $"R{y_col->id}; ";
                y_col = y_col->Next;
            }
            graph += "};\n";

            x_row = rows.first;
            while (x_row != null)
            {
                graph += $"    {{ rank=same; V{x_row->id}; ";
                InternalNode* tmp3 = x_row->accessFirst;
                while (tmp3 != null)
                {
                    graph += $"N{tmp3->PosX}_{tmp3->PosY}; ";
                    tmp3 = tmp3->Right;
                }
                graph += "};\n";
                x_row = x_row->Next;
            }

            graph += "}\n";
            return graph;
        }

        ~SparseMatrix()
        {
            HeaderNode* x_row = rows.first;
            while (x_row != null)
            {
                InternalNode* internalNode = x_row->accessFirst;
                while (internalNode != null)
                {
                    InternalNode* tmp = internalNode;
                    internalNode = internalNode->Right;
                    if (tmp != null)
                    {
                        NativeMemory.Free(tmp);
                    }
                }

                HeaderNode* tmp_row = x_row;
                x_row = x_row->Next;
                if (tmp_row != null)
                {
                    NativeMemory.Free(tmp_row);
                }
            }

            HeaderNode* y_col = cols.first;
            while (y_col != null)
            {
                InternalNode* internalNode = y_col->accessFirst;
                while (internalNode != null)
                {
                    InternalNode* tmp2 = internalNode;
                    internalNode = internalNode->Down;
                    if (tmp2 != null)
                    {
                        NativeMemory.Free(tmp2);
                    }
                }

                HeaderNode* tmp_col = y_col;
                y_col = y_col->Next;
                if (tmp_col != null)
                {
                    NativeMemory.Free(tmp_col);
                }
            }
        }
    }

}
```