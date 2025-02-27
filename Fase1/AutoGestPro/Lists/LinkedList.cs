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
            Console.WriteLine("-------------------------------------------------------------------------------");
            LinkedNode* current = head;
            while (current != null)
            {
                Console.WriteLine(current->ToString());
                current = current->Next;
            }
        }

        public LinkedNode* GetUser(int id)
        {
            LinkedNode* current = head;
            while (current != null)
            {
                if (current->Id == id)
                {
                    return current;
                }
                current = current->Next;
            }
            return null;
        }

        public bool Contains(int id)
        {
            return GetUser(id) != null;
        }

        public LinkedNode* UpdateUser(int id, string newNames, string newLastNames, string newEmail)
        {
            LinkedNode* user = GetUser(id);
            if (user!= null)
            {
                user->UpdateNode(newNames, newLastNames, newEmail);
            }
            return user;
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

        public string GenerateGraph()
        {
            var graph = "digraph G {\n";
            graph += "    node [shape=record];\n";
            graph += "    rankdir=LR;\n";
            graph += "    subgraph cluster_0 {\n";
            graph += "        label = \"Lista Simple Enlazada\";\n";
            
            LinkedNode* current = head;
            int index = 0;

            while (current != null)
            {
                graph += $"        n{index} {current->ToGraph()}\n";
                current = current->Next;
                index++;
            }

            for (int i = 0; i < index-1; i++)
            {
                graph += $"        n{i} -> n{i + 1};\n";
            }

            graph += "    }\n";
            graph += "}\n";
            return graph;
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