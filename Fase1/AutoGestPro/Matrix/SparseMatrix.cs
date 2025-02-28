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

            if (nodeX->accessFirst == null)
            {
                nodeX->accessFirst = newNode;
                nodeX->accessLast = newNode;
            }
            else
            {
                if (newNode->PosY < nodeX->accessFirst->PosY)
                {
                    newNode->Right = nodeX->accessFirst;
                    nodeX->accessFirst->Left = newNode;
                    nodeX->accessFirst = newNode;
                }
                else if (newNode->PosY > nodeX->accessLast->PosY)
                {
                    nodeX->accessLast->Right = newNode;
                    newNode->Left = nodeX->accessLast;
                    nodeX->accessLast = newNode;
                }
                else
                {
                    InternalNode* tmp = nodeX->accessFirst;
                    while (tmp != null)
                    {
                        if (newNode->PosY < tmp->PosY)
                        {

                            newNode->Right = tmp;
                            newNode->Left = tmp->Left;
                            tmp->Left->Right = newNode;
                            tmp->Left = newNode;
                            break;
                        }
                        else if (newNode->PosX == tmp->PosX && newNode->PosY == tmp->PosY)
                        {
                            break;
                        }
                        else
                        {
                            tmp = tmp->Right;
                        }
                    }
                }
            }

            if (nodeY->accessFirst == null)
            {
                nodeY->accessFirst = newNode;
                nodeY->accessLast = newNode;
            }
            else
            {
                if (newNode->PosX < nodeY->accessFirst->PosX)
                {
                    newNode->Down = nodeY->accessFirst;
                    nodeY->accessFirst->Up = newNode;
                    nodeY->accessFirst = newNode;
                }
                else if (newNode->PosX > nodeY->accessLast->PosX)
                {
                    nodeY->accessLast->Down = newNode;
                    newNode->Up = nodeY->accessLast;
                    nodeY->accessLast = newNode;
                }
                else
                {
                    InternalNode* tmp2 = nodeY->accessFirst;
                    while (tmp2 != null)
                    {
                        if (newNode->PosX < tmp2->PosX)
                        {
                            newNode->Down = tmp2;
                            newNode->Up = tmp2->Up;
                            tmp2->Up->Down = newNode;
                            tmp2->Up = newNode;
                            break;
                        }
                        else if (newNode->PosX == tmp2->PosX && newNode->PosY == tmp2->PosY)
                        {
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

                InternalNode* internalNode = x_row->accessFirst;
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

        public string GenerateGraph()
        {
            var graph = "digraph G {\n";
            graph += "    node [shape=box width=1.4];\n";
            graph += "    n0 [label=\"\" group=0];\n";

            HeaderNode* y_col = cols.first;
            HeaderNode* x_row = rows.first;

            while (x_row != null)
            {
                graph += $"    V{x_row->id} [label=\"V{x_row->id}\" group=0];\n";
                x_row = x_row->Next;
            }

            int i = 1;
            while (y_col != null)
            {
                graph += $"    R{y_col->id} [label=\"R{y_col->id}\" group={i}];\n";
                y_col = y_col->Next;
                i++;
            }

            i = 1;
            y_col = cols.first;
            while (y_col != null)
            {
                InternalNode* tmp = y_col->accessFirst;
                while (tmp != null)
                {
                    graph += $"    N{tmp->PosX}_{tmp->PosY} [label=\"Detalles: {tmp->GetDetails()}\" group={i}];\n";
                    tmp = tmp->Down;
                }
                y_col = y_col->Next;
                i++;
            }

            graph += $"    n0";
            y_col = cols.first;
            while (y_col != null)
            {
                graph += $" -> R{y_col->id}";
                y_col = y_col->Next;
            }
            graph += ";\n";

            graph += $"    ";
            y_col = cols.last;
            while (y_col != null)
            {
                graph += $"R{y_col->id} -> ";
                y_col = y_col->Prev;
            }
            graph += "n0;\n";

            x_row = rows.first;
            while (x_row != null)
            {
                graph += $"    V{x_row->id}";
                InternalNode* tmp1 = x_row->accessFirst;
                while (tmp1 != null)
                {
                    graph += $" -> N{tmp1->PosX}_{tmp1->PosY}";
                    tmp1 = tmp1->Right;
                }
                graph += ";\n";

                graph += $"    ";
                tmp1 = x_row->accessLast;
                while (tmp1 != null)
                {
                    graph += $"N{tmp1->PosX}_{tmp1->PosY} -> ";
                    tmp1 = tmp1->Left;
                }
                graph += $"V{x_row->id};\n";
                x_row = x_row->Next;
            }

            graph += $"    n0";
            x_row = rows.first;
            while (x_row != null)
            {
                graph += $" -> V{x_row->id}";
                x_row = x_row->Next;
            }
            graph += ";\n";

            graph += $"    ";
            x_row = rows.last;
            while (x_row != null)
            {
                graph += $"V{x_row->id} -> ";
                x_row = x_row->Prev;
            }
            graph += "n0;\n";

            y_col = cols.first;
            while (y_col != null)
            {
                graph += $"    R{y_col->id}";
                InternalNode* tmp2 = y_col->accessFirst;
                while (tmp2 != null)
                {
                    graph += $" -> N{tmp2->PosX}_{tmp2->PosY}";
                    tmp2 = tmp2->Down;
                }
                graph += ";\n";

                graph += $"    ";
                tmp2 = y_col->accessLast;
                while (tmp2 != null)
                {
                    graph += $"N{tmp2->PosX}_{tmp2->PosY} -> ";
                    tmp2 = tmp2->Up;
                }
                graph += $"R{y_col->id};\n";
                y_col = y_col->Next;
            }

            graph += $"    {{ rank=same; n0; ";
            y_col = cols.first;
            while (y_col != null)
            {
                graph += $"R{y_col->id}; ";
                y_col = y_col->Next;
            }
            graph += "};\n";

            x_row = rows.first;
            while (x_row != null)
            {
                graph += $"    {{ rank=same; V{x_row->id}; ";
                InternalNode* tmp3 = x_row->accessFirst;
                while (tmp3 != null)
                {
                    graph += $"N{tmp3->PosX}_{tmp3->PosY}; ";
                    tmp3 = tmp3->Right;
                }
                graph += "};\n";
                x_row = x_row->Next;
            }

            graph += "}\n";
            return graph;
        }

        ~SparseMatrix()
        {
            HeaderNode* x_row = rows.first;
            while (x_row != null)
            {
                InternalNode* internalNode = x_row->accessFirst;
                while (internalNode != null)
                {
                    InternalNode* tmp = internalNode;
                    internalNode = internalNode->Right;
                    if (tmp != null)
                    {
                        NativeMemory.Free(tmp);
                    }
                }

                HeaderNode* tmp_row = x_row;
                x_row = x_row->Next;
                if (tmp_row != null)
                {
                    NativeMemory.Free(tmp_row);
                }
            }

            HeaderNode* y_col = cols.first;
            while (y_col != null)
            {
                InternalNode* internalNode = y_col->accessFirst;
                while (internalNode != null)
                {
                    InternalNode* tmp2 = internalNode;
                    internalNode = internalNode->Down;
                    if (tmp2 != null)
                    {
                        NativeMemory.Free(tmp2);
                    }
                }

                HeaderNode* tmp_col = y_col;
                y_col = y_col->Next;
                if (tmp_col != null)
                {
                    NativeMemory.Free(tmp_col);
                }
            }
        }
    }

}