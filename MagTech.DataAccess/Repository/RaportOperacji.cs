using MagTech.Data;
using MagTech.DataAccess.Repository.IRepository;
using MagTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MagTech.DataAccess.Repository
{
    class RaportOperacji : Repository<MagTech.Models.RaportOperacji>, IRaportOperacji
    {
        private readonly ApplicationDbContext _db;

        public RaportOperacji(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        
    }
}
