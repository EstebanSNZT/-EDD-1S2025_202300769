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
            LinkedNode* current = head;
            while (current != null)
            {
                Console.WriteLine(current->ToString());
                current = current->Next;
            }
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