using MagTech.Data;
using MagTech.DataAccess.Repository.IRepository;
using MagTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagTech.DataAccess.Repository
{
   public  class BazaArtykulowRepository : Repository<BazaArtykulow>, IBazaArtykulowRepository
    {
        private readonly ApplicationDbContext _db;

        public BazaArtykulowRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
        }

        public void Update(BazaArtykulow bazaArtykulow)
        {
            var objFromDb = _db.ZbiorArtykolow.FirstOrDefault(s => s.Id == bazaArtykulow.Id);
            objFromDb.DodatkoweInformacje = bazaArtykulow.DodatkoweInformacje;
            objFromDb.GrupaMaterialowa = bazaArtykulow.GrupaMaterialowa;
            objFromDb.NazwaArtykulu = bazaArtykulow.NazwaArtykulu;
            objFromDb.NumerArtykulu = bazaArtykulow.NumerArtykulu;
            objFromDb.NumerPolki = bazaArtykulow.NumerPolki;
            objFromDb.NumerPrzedzialu = bazaArtykulow.NumerPrzedzialu;
            objFromDb.NumerRegalu = bazaArtykulow.NumerRegalu;
            objFromDb.NumerSzeregu = bazaArtykulow.NumerSzeregu;
            objFromDb.OznaczenieFifo = bazaArtykulow.OznaczenieFifo;
            objFromDb.StanMinimalny = bazaArtykulow.StanMinimalny;
            objFromDb.StanWMiejscuSkladowania = bazaArtykulow.StanWMiejscuSkladowania;
            objFromDb.Tag = bazaArtykulow.Tag;
            objFromDb.Typ = bazaArtykulow.Typ;
            objFromDb.WielkoscPojemnika = bazaArtykulow.WielkoscPojemnika;

            _db.SaveChanges();
        }
    }
}
