using System;
using System.Runtime.InteropServices;
using Gtk;

namespace Matrix
{
    public unsafe class SparseMatrix
    {
        public HeaderList rows;
        public HeaderList cols;

        // Constructor de la clase MatrizDispersa
        public SparseMatrix()
        {
            rows = new HeaderList();
            cols = new HeaderList();
        }

        public void Insert(int posX, int posY, string Details)
        {
            InternalNode* newNode = (InternalNode*)NativeMemory.Alloc((nuint)sizeof(InternalNode));
            if (newNode == null) throw new InvalidOperationException("No se pudo asignar memoria para el nuevo nodo.");
            *newNode = new InternalNode(posX, posY, Details);

            HeaderNode* nodeX = rows.GetHeader(posX);
            HeaderNode* nodeY = cols.GetHeader(posY);

            if (nodeX == null)
            {
                rows.InsertHeaderNode(posX);
                nodeX = rows.GetHeader(posX);
            }

            if (nodeY == null)
            {
                cols.InsertHeaderNode(posY);
                nodeY = cols.GetHeader(posY);
            }

            if (nodeX == null || nodeY == null)
            {
                throw new InvalidOperationException("Error al crear los encabezados.");
            }

            if (nodeX->Access == null)
            {
                nodeX->Access = newNode;
            }
            else
            {
                InternalNode* tmp = nodeX->Access;
                while (tmp != null)
                {
                    if (newNode->PosY < tmp->PosY)
                    {
                        if (tmp->Left != null)
                        {
                            newNode->Right = tmp;
                            newNode->Left = tmp->Left;
                            tmp->Left->Right = newNode;
                            tmp->Left = newNode;
                            break;
                        }
                        else
                        {
                            tmp->Left = newNode;
                            newNode->Right = tmp;
                            nodeX->Access = newNode;
                            break;
                        }
                    }
                    else if (newNode->PosX == tmp->PosX && newNode->PosY == tmp->PosY)
                    {
                        break;
                    }
                    else
                    {
                        if (tmp->Right == null)
                        {
                            tmp->Right = newNode;
                            newNode->Left = tmp;
                            break;
                        }
                        else
                        {
                            tmp = tmp->Right;
                        }
                    }
                }
            }

            if (nodeY->Access == null)
            {
                nodeY->Access = newNode;
            }
            else
            {
                InternalNode* tmp2 = nodeY->Access;
                while (tmp2 != null)
                {
                    if (newNode->PosX < tmp2->PosX)
                    {
                        if (tmp2->Up != null)
                        {
                            newNode->Down = tmp2;
                            newNode->Up = tmp2->Up;
                            tmp2->Up->Down = newNode;
                            tmp2->Up = newNode;
                            break;
                        }
                        else
                        {
                            tmp2->Up = newNode;
                            newNode->Down = tmp2;
                            nodeY->Access = newNode;
                            break;
                        }
                    }
                    else if (newNode->PosX == tmp2->PosX && newNode->PosY == tmp2->PosY)
                    {
                        break;
                    }
                    else
                    {
                        if (tmp2->Down == null)
                        {
                            tmp2->Down = newNode;
                            newNode->Up = tmp2;
                            break;
                        }
                        else
                        {
                            tmp2 = tmp2->Down;
                        }
                    }
                }
            }
        }

        public void Print()
        {
            HeaderNode* y_col = cols.first;
            Console.Write("\t");

            while (y_col != null)
            {
                Console.Write(y_col->id + "\t");
                y_col = y_col->Next;
            }
            Console.WriteLine();

            HeaderNode* x_row = rows.first;
            while (x_row != null)
            {
                Console.Write(x_row->id + "\t");

                InternalNode* internalNode = x_row->Access;
                HeaderNode* y_col_iter = cols.first;

                while (y_col_iter != null)
                {
                    if (internalNode != null && internalNode->PosY == y_col_iter->id)
                    {
                        Console.Write(internalNode->GetDetails() + "\t");
                        internalNode = internalNode->Right;
                    }
                    else
                    {
                        Console.Write("0\t");
                    }
                    y_col_iter = y_col_iter->Next;
                }
                Console.WriteLine();
                x_row = x_row->Next;
            }
        }

        ~SparseMatrix()
        {
            HeaderNode* row = rows.first;
            while (row != null)
            {
                InternalNode* internalNode = row->Access;
                while (internalNode != null)
                {
                    InternalNode* tmp = internalNode;
                    internalNode = internalNode->Right;
                    if (tmp != null)
                    {
                        NativeMemory.Free(tmp);
                    }
                }

                HeaderNode* tmp_row = row;
                row = row->Next;
                if (tmp_row != null)
                {
                    NativeMemory.Free(tmp_row);
                }
            }

            HeaderNode* col = cols.first;
            while (col != null)
            {
                InternalNode* internalNode = col->Access;
                while (internalNode != null)
                {
                    InternalNode* tmp2 = internalNode;
                    internalNode = internalNode->Down;
                    if (tmp2 != null)
                    {
                        NativeMemory.Free(tmp2);
                    }
                }

                HeaderNode* tmp_col = col;
                col = col->Next;
                if (tmp_col != null)
                {
                    NativeMemory.Free(tmp_col);
                }
            }
        }
    }

}