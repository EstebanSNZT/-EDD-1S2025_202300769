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

        public void Print()
        {
            QueueNode* current = front;
            while (current != null)
            {
                Console.WriteLine(current->ToString());
                current = current->Next;
            }
        }
    }
}