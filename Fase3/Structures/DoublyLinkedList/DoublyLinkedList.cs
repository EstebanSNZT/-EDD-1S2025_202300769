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