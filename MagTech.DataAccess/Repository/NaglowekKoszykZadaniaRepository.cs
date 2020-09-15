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
    public class NaglowekKoszykZadaniaRepository : Repository<NaglowekKoszykZadania>, INaglowekKoszykZadaniaRepository
    {
        private readonly ApplicationDbContext _db;

        public NaglowekKoszykZadaniaRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(NaglowekKoszykZadania obj)
        {
            throw new NotImplementedException();
        }
    }
}
