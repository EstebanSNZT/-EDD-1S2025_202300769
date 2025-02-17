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
            while (current!= null)
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