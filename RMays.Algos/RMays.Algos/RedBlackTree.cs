using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMays.Algos
{
    public class RedBlackTree
    {
        internal class RedBlackTreeNode
        {
            public bool IsRed { get; set; }
            public bool IsBlack
            {
                get
                {
                    return !IsRed;
                }
                set
                {
                    IsRed = !value;
                }
            }
            
        }
    }
}
