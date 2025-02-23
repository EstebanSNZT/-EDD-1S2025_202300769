using System;
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

        public void Delete(int id)
        {
            if (head == null) return;

            DoubleNode* current = head;
            while (current != null)
            {
                if (current->Id == id)
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
                    return;
                }
                current = current->Next;
            }
        }

        public void Print()
        {
            DoubleNode* current = head;
            while (current != null)
            {
                Console.WriteLine(current->ToString());
                current = current->Next;
            }

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