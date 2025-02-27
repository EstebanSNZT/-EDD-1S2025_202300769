using System;
using System.Runtime.InteropServices;

namespace Matrix
{
    public unsafe class HeaderList
    {
        public HeaderNode* first;
        public HeaderNode* last;
        public int size;

        public HeaderList()
        {
            first = null;
            last = null;
            size = 0;
        }

        public void InsertHeaderNode(int id)
        {
            HeaderNode* newNode = (HeaderNode*)NativeMemory.Alloc((nuint)sizeof(HeaderNode));
            if (newNode == null) throw new InvalidOperationException("No se pudo asignar memoria para el nuevo nodo.");
            *newNode = new HeaderNode(id);

            size++;

            if (first == null)
            {
                first = newNode;
                last = newNode;
            }
            else
            {
                if (newNode->id < first->id)
                {
                    newNode->Next = first;
                    first->Prev = newNode;
                    first = newNode;
                }
                else if (newNode->id > last->id)
                {
                    last->Next = newNode;
                    newNode->Prev = last;
                    last = newNode;
                }
                else
                {
                    HeaderNode* tmp = first;
                    while (tmp != null)
                    {
                        if (newNode->id < tmp->id)
                        {
                            newNode->Next = tmp;
                            newNode->Prev = tmp->Prev;
                            tmp->Prev->Next = newNode;
                            tmp->Prev = newNode;
                            return;
                        }

                        tmp = tmp->Next;
                    }
                }
            }
        }

        public HeaderNode* GetHeader(int id)
        {
            HeaderNode* current = first;

            while (current != null)
            {
                if (id == current->id) return current;
                current = current->Next;
            }

            return null;
        }

        ~HeaderList()
        {
            if (first == null) return;

            while (first != null)
            {
                HeaderNode* tmp = first;
                first = first->Next;
                NativeMemory.Free(tmp);
            }
        }
    }
}