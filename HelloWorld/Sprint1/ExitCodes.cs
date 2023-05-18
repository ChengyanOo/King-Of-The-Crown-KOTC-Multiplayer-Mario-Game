
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint1
{
    internal enum ExitCodes: int
    {
        Success = 0,
        InvalidFilename = 3,
        XmlFormatError = 4,
        XmlSpriteFormatError = 5,
    }
}
