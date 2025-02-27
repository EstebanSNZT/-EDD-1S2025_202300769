using System;
using System.Runtime.InteropServices;

namespace Lists
{
    public unsafe class Queue
    {
        private QueueNode* front;
        private QueueNode* rear;

        public Queue()
        {
            front = null;
            rear = null;
        }

        public void Enqueue(int id, int sparePartId, int vehicleId, string details, double cost)
        {
            QueueNode* newNode = (QueueNode*)NativeMemory.Alloc((nuint)sizeof(QueueNode));
            *newNode = new QueueNode(id, sparePartId, vehicleId, details, cost);
            if (rear == null)
            {
                front = newNode;
                rear = newNode;
            }
            else
            {
                rear->Next = newNode;
                rear = newNode;
            }
        }

        public bool IsEmpty()
        {
            if (front == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Print()
        {
            QueueNode* current = front;
            while (current != null)
            {
                Console.WriteLine(current->ToString());
                current = current->Next;
            }
        }

        public string GenerateGraph()
        {
            var graph = "digraph G {\n";
            graph += "    node [shape=record];\n";
            graph += "    rankdir=LR;\n";
            graph += "    subgraph cluster_0 {\n";
            graph += "        label = \"Cola\";\n";

            QueueNode* current = front;
            int index = 0;

            while (current != null)
            {
                graph += $"        n{index} {current->ToGraph(index+1)}\n";
                current = current->Next;
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