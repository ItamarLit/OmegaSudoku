using System;
using OmegaSudoku.MinHeap;
using System.Collections.Generic;

namespace OmegaSudoku
{
    class Program
    {
        static void Main(string[] args)
        {
            BoardCell b1 = new BoardCell(0, 0, 9, 8, -1);
            BoardCell b2 = new BoardCell(0, 0, 9, 3, -1);
            BoardCell b3 = new BoardCell(0, 0, 9, 5, -1);
            BoardCell b4 = new BoardCell(0, 0, 9, 3, -1);
            BoardCell b5 = new BoardCell(0, 0, 9, 7, -1);
            Heap m = new Heap();
            List<BoardCell> l = new List<BoardCell>();
            l.Add(b1);
            l.Add(b2);
            l.Add(b3);
            l.Add(b4);
            l.Add(b5);
            m.CreateMinHeap(l);
            m.PrintHeap();
        }
    }
}
