using System;
using System.Runtime.InteropServices;

namespace Lists
{
    public unsafe class Stack
    {
        private static int lastId = 0;
        private StackNode* top;

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
        }

        public StackNode* Pop()
        {
            if (top == null) return null;
            StackNode* temp = top;
            top = top->Next;
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