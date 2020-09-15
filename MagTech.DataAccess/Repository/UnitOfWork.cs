using MagTech.Data;
using MagTech.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagTech.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            BazaArtykulow = new BazaArtykulowRepository(_db);
            SP_Call = new SP_Call(_db);
            RaportOperacji = new RaportOperacji(_db);
            NaglowekKoszykZadania = new NaglowekKoszykZadaniaRepository(_db);
        }

        public IBazaArtykulowRepository BazaArtykulow { get; private set; }
        public IRaportOperacji RaportOperacji { get; private set; }
        public ISP_Call SP_Call { get; private set; }

        public INaglowekKoszykZadaniaRepository NaglowekKoszykZadania { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
