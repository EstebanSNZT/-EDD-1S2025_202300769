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