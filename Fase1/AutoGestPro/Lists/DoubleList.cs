using System;
using Gtk;
using System.Runtime.InteropServices;

namespace Lists
{
    public unsafe class DoubleList
    {
        private DoubleNode* head;
        private DoubleNode* tail;

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
                    if (current->Model < current->Next->Model)
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
                newList.Insert(current->Id, current->UserId, current->GetBrand(), current->Model, current->GetPlate());
                current = current->Next;
            }
            return newList;
        }

        ~DoubleList()
        {
            while (head != null)
            {
                DoubleNode* temp = head;
                head = head->Next;
                temp->FreeData();
                NativeMemory.Free(temp);
            }
        }
    }
}