using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archiver.Data
{
    public class Node
    {
        public byte Symbol;
        public int Frequency;
        public Node bit0;
        public Node bit1;
    }
}
