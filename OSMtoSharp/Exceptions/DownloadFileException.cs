using System;
using System.Collections.Generic;
using System.Text;

namespace OSMtoSharp
{
    class DownloadFileException : Exception
    {
        public DownloadFileException()
        {
        }

        public DownloadFileException(string message) : base(message)
        {
        }

        public DownloadFileException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
