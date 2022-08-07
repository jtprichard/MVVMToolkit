using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.MVVMToolkit.Dialogs.Data
{
    public interface IListItem
    {
        int Id { get; set; }

        string Description { get; set; }

        bool IsLocked { get; set; }

        IListItem Dependency { get; set; }


    }
}
