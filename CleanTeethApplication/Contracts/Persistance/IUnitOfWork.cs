using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication.Contracts.Persistance
{
    public interface IUnitOfWork
    {
        Task Commit();

        Task Rollback();
    }
}
