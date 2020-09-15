using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MagTech.DataAccess.Repository.IRepository
{
    public interface IHanelRepository
    {
            public void SetReadAMBBackup();

            public void SetReadOPJournalBackup();

        public void GetFromAmdToSqlite();
        public void GetFromAmdToSqlite2();

        public void GetFromOpjToSqlite();
    }
}
