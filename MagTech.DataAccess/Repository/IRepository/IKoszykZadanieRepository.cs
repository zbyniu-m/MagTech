using MagTech.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagTech.DataAccess.Repository.IRepository
{
    public interface IKoszykZadanieRepository: IRepository<KoszykZadanie>
    {
        void Update(KoszykZadanie obj);
    }
}
