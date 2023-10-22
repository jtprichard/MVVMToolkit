using System;

namespace PB.MVVMToolkit.ProgressForms
{
    public interface IProgressFormCommand
    {
        void Execute(IProgress<int> progress);
    }
}
