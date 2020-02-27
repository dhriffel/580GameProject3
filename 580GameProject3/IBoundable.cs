using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _580GameProject4
{
    public interface IBoundable
    {
        BoundingRectangle bounds { get; }
    }
}
