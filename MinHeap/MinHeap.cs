using System;
using System.Collections.Generic;

namespace OmegaSudoku.MinHeap
{
    class MinHeap
    {
        /// <summary>
        /// This class represents a minimum heap that will hold Nodes 
        /// of possiblity numbers for every empty cell on the board.
        /// </summary>
        private List<BoardCell> NodeList { get; }

        public MinHeap()
        {
            // set the node list
            NodeList = new List<BoardCell>();
        }

        public void InsertMinHeap()
        {
        
        }

        public void DeleteMinHeap()
        {
            // delete the first index element
            if(NodeList.Count != 0)
            {
                NodeList.RemoveAt(0);
            }
        }

        private void SwapNodes()
        {

        }

        public BoardCell GetMinValue()
        {
            return NodeList[0];
        }
        public void MinHeapify()
        {
            
        }
    }
}
