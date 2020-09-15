using Dapper;
using MagTech.Data;
using MagTech.DataAccess.Repository.IRepository;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagTech.DataAccess.Repository
{
    public class SP_Call : ISP_Call
    {
        private readonly ApplicationDbContext _db;
        private static string ConnectionString = "";

        public SP_Call(ApplicationDbContext db)
        {
            _db = db;
            ConnectionString = db.Database.GetDbConnection().ConnectionString;
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Execute(string query, DynamicParameters param = null)
        {
            using (SqliteConnection sqliteCon = new SqliteConnection(ConnectionString))
            {
                sqliteCon.Open();
                sqliteCon.Execute(query, param, commandType:System.Data.CommandType.Text);
            }
        }

        public T Singel<T>(string procedureName, DynamicParameters param = null)
        {
            using (SqliteConnection sqliteCon = new SqliteConnection(ConnectionString))
            {
                sqliteCon.Open();                
                return (T)Convert.ChangeType(sqliteCon.ExecuteScalar<T>(procedureName, param, commandType: System.Data.CommandType.Text), typeof(T));
            }
        }

        public T OneRecord<T>(string procedureName, DynamicParameters param = null)
        {
            using (SqliteConnection sqliteCon = new SqliteConnection(ConnectionString))
            {
                sqliteCon.Open();
                var value = sqliteCon.Query<T>(procedureName, param, commandType: System.Data.CommandType.Text);
                return (T)Convert.ChangeType(value.FirstOrDefault(), typeof(T));
            }
        }

        public IEnumerable<T> List<T>(string procedureName, DynamicParameters param = null)
        {
            using (SqliteConnection sqliteCon = new SqliteConnection(ConnectionString))
            {
                sqliteCon.Open();
                return sqliteCon.Query<T>(procedureName, param, commandType: System.Data.CommandType.Text);
            }
        }

        public Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string procedureName, DynamicParameters param = null)
        {
            using (SqliteConnection sqliteCon = new SqliteConnection(ConnectionString))
            {
                sqliteCon.Open();
                var result = SqlMapper.QueryMultiple(sqliteCon, procedureName, param, commandType: System.Data.CommandType.Text);
                var item1 = result.Read<T1>().ToList();
                var item2 = result.Read<T2>().ToList();

                if(item1 != null && item2 !=null)
                {
                    return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(item1, item2);
                }      
            }
            return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(new List<T1>(), new List<T2>());
        }
    } 
}

