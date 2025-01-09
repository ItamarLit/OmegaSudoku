using System.Collections.Generic;
using System;

namespace OmegaSudoku.MinHeap
{
    class Heap
    {
        /// <summary>
        /// This class represents a minimum heap that will hold Nodes 
        /// of possiblity numbers for every empty cell on the board.
        /// </summary>
        private List<BoardCell> NodeList { get; }

        public Heap()
        {
            // set the node list
            NodeList = new List<BoardCell>();
        }

        public void CreateMinHeap(List<BoardCell> CellList)
        {
            int startingListIndex = 0;
            // this func will only be called once and will create the min heap
            foreach(BoardCell bc in CellList)
            {
                // insert the value into the first index
                NodeList.Insert(startingListIndex, bc);
                // heapify
                MinHeapify(startingListIndex);
            }
        }

        public void DeleteMinHeap()
        {
            // delete the first index element
            if(NodeList.Count != 0)
            {
                NodeList.RemoveAt(0);
            }
        }

        private void SwapNodes(int indexA, int indexB)
        {
            BoardCell temp = NodeList[indexA];
            NodeList[indexA] = NodeList[indexB];
            NodeList[indexB] = temp;
        }

        public BoardCell GetMinValue()
        {
            return NodeList[0];
        }

        public void MinHeapify(int position)
        {
            int leftNode = position * 2 + 1;
            int rightNode = position * 2 + 2;
            int smallestIndex = position;
            if(leftNode < NodeList.Count && NodeList[leftNode].NumberOfPossibilites() < NodeList[position].NumberOfPossibilites())
            {
                smallestIndex = leftNode;
            }
            else if (rightNode < NodeList.Count && NodeList[rightNode].NumberOfPossibilites() < NodeList[position].NumberOfPossibilites())
            {
                smallestIndex = rightNode;
            }
            // check if we need swap the values
            if(smallestIndex != position)
            {
                SwapNodes(position, smallestIndex);
                // recursively change the values until the heap is a Min heap
                MinHeapify(smallestIndex);
            }

        }

        public void PrintHeap()
        {
            foreach(BoardCell bc in NodeList)
            {
                Console.Write(bc.NumberOfPossibilites());
            }
        }
    }
}
