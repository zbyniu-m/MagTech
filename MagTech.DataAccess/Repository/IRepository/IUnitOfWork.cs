using System;
using System.Collections.Generic;
using System.Text;

namespace MagTech.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IBazaArtykulowRepository BazaArtykulow { get; }
        ISP_Call SP_Call { get; }
        IRaportOperacji RaportOperacji { get; }

        INaglowekKoszykZadaniaRepository NaglowekKoszykZadania { get; }
        void Save();
    }
}
