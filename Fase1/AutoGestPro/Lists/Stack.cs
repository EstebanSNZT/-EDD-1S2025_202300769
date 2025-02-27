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
            StackNode* current = top;
            while (current != null)
            {
                Console.WriteLine(current->ToString());
                current = current->Next;
            }
        }
    }
}