using System;
using System.IO;

namespace LibraryForLab4
{
    public interface ILab
    {
        string Input { get; set; }
        string Output { get; set; }

        void Run();
    }
}
