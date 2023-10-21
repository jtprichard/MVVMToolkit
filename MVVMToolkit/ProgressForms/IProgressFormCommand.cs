using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.MVVMToolkit.ProgressForms
{
    public interface IProgressFormCommand
    {
        void Execute(IProgress<int> progress);
    }
}
