using MagTech.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagTech.DataAccess.Repository.IRepository
{
    public interface INaglowekKoszykZadaniaRepository : IRepository<NaglowekKoszykZadania>
    {
        void Update(NaglowekKoszykZadania obj);
    }
}
