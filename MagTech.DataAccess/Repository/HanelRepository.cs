using Dapper;
using MagTech.DataAccess.Repository.IRepository;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MagTech.DataAccess.Repository
{
    public class HanelRepository : IHanelRepository
    {
        private static string ConnectionString = "";
        private readonly IConfiguration _configuration;

        public HanelRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectionString = configuration.GetConnectionString("Sqlite");
        }             



        public void SetReadAMBBackup()
        {
            try
            {
                string data = "READ AMDBACKUP AMD" + DateTime.Now.ToString("yyMMddHHmmss");
                string path = Path.GetTempFileName();
                File.WriteAllText(path, data);
                CopyFileToDirOut(path);
                File.Delete(path);

            }
            catch (Exception)
            {

            }

        }

        public void SetReadOPJournalBackup()
        {
            try 
            { 
            string data = "READ OPJOURNALBACKUP OPJ" + DateTime.Now.ToString("yyMMddHHmmss");
            string path = Path.GetTempFileName();
            File.WriteAllText(path, data);
            CopyFileToDirOut(path);
            File.Delete(path);
            }
            catch (Exception)
            {
                
            }
        }
       
        private void CopyFileToDirOut(string path)
        {
            Random rnd = new Random();            
            int losowy = rnd.Next(1000);
            string DirOut = _configuration.GetSection("ExchangeFileDir").GetSection("Output").Value;
            string uniqueFileName = DirOut + losowy + "_" + DateTime.Now.ToString("yyMMddHHmmss") + ".req";
            File.Copy(path, uniqueFileName, true);            
        }

#nullable enable
        public void GetFromAmdToSqlite()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            string? amdFileToRead = GetTheNewestTypeOfFileFromDirInput("amd");
            if (amdFileToRead != null)
            {
                try
                {
                    var query = "UPDATE ZbiorArtykolow SET StanWMiejscuSkladowania=0 ";
                    using (SqliteConnection sqliteCon = new SqliteConnection(ConnectionString))
                    {
                        SqliteCommand cmd = new SqliteCommand(query, sqliteCon);
                        sqliteCon.Open();                        
                        cmd.ExecuteNonQuery();
                        sqliteCon.Close();
                    }
                    query = "INSERT OR REPLACE INTO ZbiorArtykolow (Id,NumerArtykulu,NumerRegalu,NumerPrzedzialu,NumerPolki,StanWMiejscuSkladowania,OznaczenieFifo,WielkoscPojemnika,StanMinimalny,NumerSzeregu,NazwaArtykulu,GrupaMaterialowa,Typ,Tag,DodatkoweInformacje) VALUES((SELECT Id FROM ZbiorArtykolow WHERE NumerArtykulu= @NumerArtykulu AND NumerRegalu= @NumerRegalu AND NumerPrzedzialu= @NumerPrzedzialu AND NumerPolki = @NumerPolki),@NumerArtykulu,@NumerRegalu,@NumerPrzedzialu,@NumerPolki,@StanWMiejscuSkladowania,@OznaczenieFifo,@WielkoscPojemnika,@StanMinimalny,@NumerSzeregu,@NazwaArtykulu,@GrupaMaterialowa,@Typ,@Tag,(SELECT DodatkoweInformacje FROM ZbiorArtykolow WHERE NumerArtykulu= @NumerArtykulu AND NumerRegalu = @NumerRegalu AND NumerPrzedzialu= @NumerPrzedzialu AND NumerPolki = @NumerPolki))";
                    StreamReader sr = new StreamReader(amdFileToRead);
                    //Read the first line of text
                    var line = sr.ReadLine();
                    //Continue to read until you reach end of file
                    while (line != null)
                    {
                        // Delete *$ from the begining of line
                        line = line.Substring(2, line.Length - 3);
                        var row = line.Split("$");
                        //Prepare string row to insert into SqliteDB
                        for (int i = 0; i < row.Length; i++)
                        {
                            string firstSign = row[i].Substring(0, 1);
                            //Remove unwanted chars
                            switch (firstSign)
                            {
                                case "S": //Numer artykułu 0
                                    row[i] = Convert.ToString(row[i].Substring(1, row[i].Length - 1).Trim());                  
                                    break;
                                case "L": //Numer regału 1
                                    row[i] = row[i] = row[i].Substring(1, row[i].Length - 1).Trim();                                   
                                    break;
                                case "F": //Numer przedziału 2
                                    row[i] = row[i].Substring(1, row[i].Length - 1).Trim();                                   
                                    break;
                                case "T": //Numer półki 3
                                    row[i] = row[i].Substring(1, row[i].Length - 1).Trim();                                    
                                    break;
                                case "B": //Stan w miejscu składowania 4
                                    row[i] = row[i].Substring(1, row[i].Length - 1).Trim();                                    
                                    break;
                                case "I": //Oznaczenie FIFO 5
                                    row[i] = row[i].Substring(1, row[i].Length - 1).Trim();                                   
                                    break;
                                case "G": //Wielkość pojemnika 6
                                    row[i] = row[i].Substring(1, row[i].Length - 1).Trim();                                   
                                    break;
                                case "R": //Stan minimalny 7
                                    row[i] = row[i].Substring(1, row[i].Length - 1).Trim();                                   
                                    break;
                                case "O": //Numer szeregu 8
                                    row[i] = row[i].Substring(1, row[i].Length - 1).Trim();                                   
                                    break;
                                case "N": //Nazwa artykułu 9                                  
                                    row[i] = row[i].Substring(1, row[i].Length - 1).TrimEnd();                                    
                                    break;
                                case "H":
                                    switch (row[i].Substring(0, 3))
                                    {
                                        case "H01": //Grupa materiałowa 10
                                            row[i] = row[i].Substring(3, row[i].Length - 3).Trim();                                            
                                            break;
                                        case "H02": //Typ 11
                                            row[i] = row[i].Substring(3, row[i].Length - 3).Trim();                                            
                                            break;
                                        case "H03": //Tag 12
                                            row[i] = row[i].Substring(3, row[i].Length - 3).Trim();                                           
                                            break;
                                    }
                                    break;
                                default:
                                    row[i] = row[i].Remove(0);
                                    break;
                            }                       
                        }
                        using (SqliteConnection sqliteCon = new SqliteConnection(ConnectionString))
                        {
                            SqliteCommand cmd = new SqliteCommand(query, sqliteCon); 
                            sqliteCon.Open();                           
                            cmd.Parameters.AddWithValue("@NumerArtykulu", row[0].ToString());
                            cmd.Parameters.AddWithValue("@NumerRegalu", Int16.Parse(row[1]));
                            cmd.Parameters.AddWithValue("@NumerPrzedzialu", Int16.Parse(row[2]));
                            cmd.Parameters.AddWithValue("@NumerPolki", Int16.Parse(row[3]));
                            cmd.Parameters.AddWithValue("@StanWMiejscuSkladowania", Int16.Parse(row[4]));
                            cmd.Parameters.AddWithValue("@OznaczenieFifo", Int16.Parse(row[5]));
                            cmd.Parameters.AddWithValue("@WielkoscPojemnika", row[6].ToString());
                            cmd.Parameters.AddWithValue("@StanMinimalny", Int16.Parse(row[7]));
                            cmd.Parameters.AddWithValue("@NumerSzeregu", Int16.Parse(row[8]));
                            cmd.Parameters.AddWithValue("@NazwaArtykulu", row[9].ToString());
                            cmd.Parameters.AddWithValue("@GrupaMaterialowa", row[10].ToString());
                            cmd.Parameters.AddWithValue("@Typ", row[11].ToString());
                            cmd.Parameters.AddWithValue("@Tag", row[12].ToString());
                            cmd.ExecuteNonQueryAsync();
                            sqliteCon.Close();
                        }
                        line = sr.ReadLine();
                    }
                    //close the file
                    sr.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: " + e.Message);
                }  
            }
            stopwatch.Stop();
            Debug.WriteLine("Procedure GetFromAmdToSqlite takes: " + stopwatch.Elapsed);
        }
#nullable enable
        public void GetFromOpjToSqlite()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            string? opjFileToRead = GetTheNewestTypeOfFileFromDirInput("opj");
            if (opjFileToRead != null)
            {
                DateTime? maxDataFromDb = null;
                using (SqliteConnection sqliteCon = new SqliteConnection(ConnectionString))
                {
                    string query = "Select Max(Data) FROM RaportyOperacji";
                    SqliteCommand cmd = new SqliteCommand(query, sqliteCon);
                    sqliteCon.Open();
                    var ansFromDb = cmd.ExecuteScalar();
                    if (ansFromDb != DBNull.Value && ansFromDb != null)
                    {
                        maxDataFromDb = DateTime.Parse(s: ansFromDb.ToString());
                    }                    
                    sqliteCon.Close();
                }
                if (maxDataFromDb != null)
                {
                    {
                        string query = "INSERT INTO RaportyOperacji(Data,Proces,NumerArtykulu,Ilosc,Uzytkownik,StanowiskoKosztow,NumerMPK,Zadanie,MiejsceSkladowania,WielkoscPojemnika,Status) VALUES (@Data,@Proces,@NumerArtykulu,@Ilosc,@Uzytkownik,@StanowiskoKosztow,@NumerMPK,@Zadanie,@MiejsceSkladowania,@WielkoscPojemnika,@Status)";
                        StreamReader sr = new StreamReader(opjFileToRead);
                        //Read the first line of text
                        var line = sr.ReadLine();
                        //Continue to read until you reach end of file
                        while (line != null)
                        {
                            // Delete *$ from the begining of line
                            line = line.Substring(2, line.Length - 3);
                            var row = line.Split("$");
                            //Prepare string row to insert into SqliteDB
                            for (int i = 0; i < row.Length; i++)
                            {                                
                                string firstSign = row[i].Substring(0, 1);
                                //Remove unwanted chars
                                switch (firstSign)
                                {
                                    case "S": //Numer artykułu 0
                                        row[i] = Convert.ToString(row[i].Substring(1, row[i].Length - 1).Trim());
                                        break;
                                    case "V": //Rodzaj operacji 1
                                        row[i] = row[i] = row[i].Substring(1, row[i].Length - 1).Trim();
                                        break;
                                    case "Q": //Ilość 2
                                        row[i] = row[i].Substring(1, row[i].Length - 1).Trim();
                                        break;
                                    case "G": //Wielkość pojemnika 3
                                        row[i] = row[i].Substring(1, row[i].Length - 1).Trim();
                                        break;
                                    case "W": //Status 4
                                        row[i] = row[i].Substring(1, row[i].Length - 1).Trim();
                                        break;
                                    case "U":
                                        switch (row[i].Substring(0, 3))
                                        {
                                            case "U01": //Data zadanie 5
                                                row[i] = row[i].Substring(3, row[i].Length - 3).Trim();
                                                break;
                                            case "U02": //Data DDMMYY 6
                                                row[i] = row[i].Substring(3, row[i].Length - 3).Trim();
                                                break;
                                            case "U03": //Godzina HHMM 7
                                                row[i] = row[i].Substring(3, row[i].Length - 3).Trim();
                                                break;
                                            case "U04": // Stanowisko kosztów 8
                                                row[i] = row[i].Substring(3, row[i].Length - 3).Trim();
                                                break;
                                            case "U06": // Operator/wypożyczający 9
                                                row[i] = row[i].Substring(3, row[i].Length - 3).Trim();
                                                break;
                                            case "U07": // Miejsce składowania (LLTTTFFFOO) 10
                                                row[i] = row[i].Substring(3, row[i].Length - 3).Trim();
                                                break;
                                            case "U15": // Stanowisko kosztów - zlecenie 11
                                                row[i] = row[i].Substring(3, row[i].Length - 3).Trim();
                                                break;
                                        }
                                        break;
                                    default:
                                        row[i] = row[i].Remove(0);
                                        break;                                
                                }
                            }
                            if (maxDataFromDb < DateTime.ParseExact(row[6].ToString() + row[7].ToString(), "ddMMyyHHmm", CultureInfo.InvariantCulture))
                            {   
                            using (SqliteConnection sqliteCon = new SqliteConnection(ConnectionString))
                            {
                                SqliteCommand cmd = new SqliteCommand(query, sqliteCon);
                                sqliteCon.Open();
                                cmd.Parameters.AddWithValue("@Data", DateTime.ParseExact(row[6].ToString() + row[7].ToString(), "ddMMyyHHmm", CultureInfo.InvariantCulture));
                                cmd.Parameters.AddWithValue("@Proces", row[1].ToString());
                                cmd.Parameters.AddWithValue("@NumerArtykulu", row[0].ToString());
                                cmd.Parameters.AddWithValue("@Ilosc", Int32.Parse(row[2]));
                                cmd.Parameters.AddWithValue("@Uzytkownik", row[9].ToString());
                                cmd.Parameters.AddWithValue("@StanowiskoKosztow", row[8].ToString());
                                cmd.Parameters.AddWithValue("@Zadanie", row[5].ToString());                                
                                cmd.Parameters.AddWithValue("@WielkoscPojemnika", row[3].ToString());
                                cmd.Parameters.AddWithValue("@Status", row[4].ToString());
                                switch (row.Length)
                                {
                                    case 11:
                                        cmd.Parameters.AddWithValue("@MiejsceSkladowania", row[10].ToString());
                                        cmd.Parameters.AddWithValue("@NumerMPK", "");
                                        break;
                                    case 12:
                                        cmd.Parameters.AddWithValue("@MiejsceSkladowania", row[10].ToString());
                                        cmd.Parameters.AddWithValue("@NumerMPK", row[11].ToString());
                                        break;
                                }
                                cmd.ExecuteNonQueryAsync();
                                sqliteCon.Close();
                            }
                            }
                            line = sr.ReadLine();
                        }
                        //close the file
                        sr.Close();
                    }
                }
                else
                {
                    string query = "INSERT INTO RaportyOperacji(Data,Proces,NumerArtykulu,Ilosc,Uzytkownik,StanowiskoKosztow,NumerMPK,Zadanie,MiejsceSkladowania,WielkoscPojemnika,Status) VALUES (@Data,@Proces,@NumerArtykulu,@Ilosc,@Uzytkownik,@StanowiskoKosztow,@NumerMPK,@Zadanie,@MiejsceSkladowania,@WielkoscPojemnika,@Status)";
                    StreamReader sr = new StreamReader(opjFileToRead);
                    //Read the first line of text
                    var line = sr.ReadLine();
                    //Continue to read until you reach end of file
                    while (line != null)
                    {
                        // Delete *$ from the begining of line
                        line = line.Substring(2, line.Length - 3);
                        var row = line.Split("$");
                        //Prepare string row to insert into SqliteDB
                        for (int i = 0; i < row.Length; i++)
                        {
                            
                            string firstSign = row[i].Substring(0, 1);
                            //Remove unwanted chars
                            switch (firstSign)
                            {
                                case "S": //Numer artykułu 0
                                    row[i] = Convert.ToString(row[i].Substring(1, row[i].Length - 1).Trim());
                                    break;
                                case "V": //Rodzaj operacji 1
                                    row[i] = row[i] = row[i].Substring(1, row[i].Length - 1).Trim();
                                    break;
                                case "Q": //Ilość 2
                                    row[i] = row[i].Substring(1, row[i].Length - 1).Trim();
                                    break;
                                case "G": //Wielkość pojemnika 3
                                    row[i] = row[i].Substring(1, row[i].Length - 1).Trim();
                                    break;
                                case "W": //Status 4
                                    row[i] = row[i].Substring(1, row[i].Length - 1).Trim();
                                    break;
                                case "U":
                                    switch (row[i].Substring(0, 3))
                                    {
                                        case "U01": //Data zadanie 5
                                            row[i] = row[i].Substring(3, row[i].Length - 3).Trim();
                                            break;
                                        case "U02": //Data DDMMYY 6
                                            row[i] = row[i].Substring(3, row[i].Length - 3).Trim();
                                            break;
                                        case "U03": //Godzina HHMM 7
                                            row[i] = row[i].Substring(3, row[i].Length - 3).Trim();
                                            break;
                                        case "U04": // Stanowisko kosztów 8
                                            row[i] = row[i].Substring(3, row[i].Length - 3).Trim();
                                            break;
                                        case "U06": // Operator/wypożyczający 9
                                            row[i] = row[i].Substring(3, row[i].Length - 3).Trim();
                                            break;
                                        case "U07": // Miejsce składowania (LLTTTFFFOO) 10
                                            row[i] = row[i].Substring(3, row[i].Length - 3).Trim();
                                            break;
                                        case "U15": // Stanowisko kosztów - zlecenie 11
                                            row[i] = row[i].Substring(3, row[i].Length - 3).Trim();
                                            break;
                                    }
                                    break;
                                default:
                                    row[i] = row[i].Remove(0);
                                    break;
                            
                            }
                        }
                        using (SqliteConnection sqliteCon = new SqliteConnection(ConnectionString))
                        {
                            SqliteCommand cmd = new SqliteCommand(query, sqliteCon);
                            sqliteCon.Open();
                            cmd.Parameters.AddWithValue("@Data", DateTime.ParseExact(row[6].ToString() + row[7].ToString(), "ddMMyyHHmm", CultureInfo.InvariantCulture));
                            cmd.Parameters.AddWithValue("@Proces", row[1].ToString());
                            cmd.Parameters.AddWithValue("@NumerArtykulu", row[0].ToString());
                            cmd.Parameters.AddWithValue("@Ilosc", Int32.Parse(row[2]));
                            cmd.Parameters.AddWithValue("@Uzytkownik", row[9].ToString());
                            cmd.Parameters.AddWithValue("@StanowiskoKosztow", row[8].ToString());
                            cmd.Parameters.AddWithValue("@Zadanie", row[5].ToString()); 
                            cmd.Parameters.AddWithValue("@WielkoscPojemnika", row[3].ToString());
                            cmd.Parameters.AddWithValue("@Status", row[4].ToString());
                            switch (row.Length)
                            {
                                case 11:
                                    cmd.Parameters.AddWithValue("@MiejsceSkladowania", row[10].ToString());
                                    cmd.Parameters.AddWithValue("@NumerMPK", "");
                                    break;
                                case 12:
                                    cmd.Parameters.AddWithValue("@MiejsceSkladowania", row[10].ToString());
                                    cmd.Parameters.AddWithValue("@NumerMPK", row[11].ToString());
                                    break;
                            }                                                     
                            cmd.ExecuteNonQueryAsync();
                            sqliteCon.Close();
                        }
                        line = sr.ReadLine();
                    }
                    //close the file
                    sr.Close();

                }
                //pobierz datę z bazy danych

                // czytaj linie tak długo jak


            }
            stopwatch.Stop();
            Debug.WriteLine("Procedure GetFromOpjToSqlite takes: " + stopwatch.Elapsed);
        }

        public void GetFromAmdToSqlite2()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            string? amdFileToRead = GetTheNewestTypeOfFileFromDirInput("amd");
            if (amdFileToRead != null)
            {
                try
                {
                    StreamReader sr = new StreamReader(amdFileToRead);
                    //Read the first line of text
                    var line = sr.ReadLine();
                    //Continue to read until you reach end of file
                    DataTable datatable = CreateAMDTable();
                    while (line != null)
                    {
                        // Delete *$ from the begining of line
                        line = line.Substring(2, line.Length - 3);
                        var row = line.Split("$");
                        //Prepare string row to insert into SqliteDB
                        for (int i = 0; i < row.Length; i++)
                        {
                            string firstSign = row[i].Substring(0, 1);
                            //Remove unwanted chars
                            switch (firstSign)
                            {
                                case "S": //Numer artykułu 0
                                    row[i] = Convert.ToString(row[i].Substring(1, row[i].Length - 1).Trim());
                                    break;
                                case "L": //Numer regału 1
                                    row[i] = row[i] = row[i].Substring(1, row[i].Length - 1).Trim();
                                    break;
                                case "F": //Numer przedziału 2
                                    row[i] = row[i].Substring(1, row[i].Length - 1).Trim();
                                    break;
                                case "T": //Numer półki 3
                                    row[i] = row[i].Substring(1, row[i].Length - 1).Trim();
                                    break;
                                case "B": //Stan w miejscu składowania 4
                                    row[i] = row[i].Substring(1, row[i].Length - 1).Trim();
                                    break;
                                case "I": //Oznaczenie FIFO 5
                                    row[i] = row[i].Substring(1, row[i].Length - 1).Trim();
                                    break;
                                case "G": //Wielkość pojemnika 6
                                    row[i] = row[i].Substring(1, row[i].Length - 1).Trim();
                                    break;
                                case "R": //Stan minimalny 7
                                    row[i] = row[i].Substring(1, row[i].Length - 1).Trim();
                                    break;
                                case "O": //Numer szeregu 8
                                    row[i] = row[i].Substring(1, row[i].Length - 1).Trim();
                                    break;
                                case "N": //Nazwa artykułu 9                                  
                                    row[i] = row[i].Substring(1, row[i].Length - 1).TrimEnd();
                                    break;
                                case "H":
                                    switch (row[i].Substring(0, 3))
                                    {
                                        case "H01": //Grupa materiałowa 10
                                            row[i] = row[i].Substring(3, row[i].Length - 3).Trim();
                                            break;
                                        case "H02": //Typ 11
                                            row[i] = row[i].Substring(3, row[i].Length - 3).Trim();
                                            break;
                                        case "H03": //Tag 12
                                            row[i] = row[i].Substring(3, row[i].Length - 3).Trim();
                                            break;
                                    }
                                    break;
                                default:
                                    row[i] = row[i].Remove(0);
                                    break;

                            }
                        }
                        datatable.Rows.Add(row[0].ToString(), Int16.Parse(row[1]), Int16.Parse(row[2]), Int16.Parse(row[3]), Int16.Parse(row[4]), Int16.Parse(row[5]), row[6].ToString(), Int16.Parse(row[7]), Int16.Parse(row[8]), row[9].ToString(), row[10].ToString(), row[11].ToString(), row[12].ToString());

                        line = sr.ReadLine();
                    }
                    //close the file
                    sr.Close();
                    SqliteCommand cmd;
                    SqliteConnection sqliteCon = new SqliteConnection(ConnectionString);
                    
                    sqliteCon.Open();     
                        using (var transaction = sqliteCon.BeginTransaction())
                        {
                        var query = "UPDATE ZbiorArtykolow SET StanWMiejscuSkladowania=0 ";
                        cmd = new SqliteCommand(query, sqliteCon, transaction);
                        cmd.ExecuteNonQuery();
                        for (var i=0; i< datatable.Rows.Count; i++)
                            {
                            cmd = new SqliteCommand();
                            cmd.Connection = sqliteCon;
                            cmd.Transaction = transaction;
                            cmd.CommandType = CommandType.Text;
                                cmd.CommandText = "INSERT OR REPLACE INTO ZbiorArtykolow (Id,NumerArtykulu,NumerRegalu,NumerPrzedzialu,NumerPolki,StanWMiejscuSkladowania,OznaczenieFifo,WielkoscPojemnika,StanMinimalny,NumerSzeregu,NazwaArtykulu,GrupaMaterialowa,Typ,Tag,DodatkoweInformacje) VALUES((SELECT Id FROM ZbiorArtykolow WHERE NumerArtykulu= @NumerArtykulu AND NumerRegalu= @NumerRegalu AND NumerPrzedzialu= @NumerPrzedzialu AND NumerPolki = @NumerPolki),@NumerArtykulu,@NumerRegalu,@NumerPrzedzialu,@NumerPolki,@StanWMiejscuSkladowania,@OznaczenieFifo,@WielkoscPojemnika,@StanMinimalny,@NumerSzeregu,@NazwaArtykulu,@GrupaMaterialowa,@Typ,@Tag,(SELECT DodatkoweInformacje FROM ZbiorArtykolow WHERE NumerArtykulu= @NumerArtykulu AND NumerRegalu = @NumerRegalu AND NumerPrzedzialu= @NumerPrzedzialu AND NumerPolki = @NumerPolki))";
                                cmd.Parameters.AddWithValue("@NumerArtykulu", datatable.Rows[i][0].ToString());
                                cmd.Parameters.AddWithValue("@NumerRegalu", datatable.Rows[i][1]);
                                cmd.Parameters.AddWithValue("@NumerPrzedzialu", datatable.Rows[i][2]);
                                cmd.Parameters.AddWithValue("@NumerPolki", datatable.Rows[i][3]);
                                cmd.Parameters.AddWithValue("@StanWMiejscuSkladowania", datatable.Rows[i][4]);
                                cmd.Parameters.AddWithValue("@OznaczenieFifo", datatable.Rows[i][5]);
                                cmd.Parameters.AddWithValue("@WielkoscPojemnika", datatable.Rows[i][6].ToString());
                                cmd.Parameters.AddWithValue("@StanMinimalny", datatable.Rows[i][7]);
                                cmd.Parameters.AddWithValue("@NumerSzeregu", datatable.Rows[i][8]);
                                cmd.Parameters.AddWithValue("@NazwaArtykulu", datatable.Rows[i][9].ToString());
                                cmd.Parameters.AddWithValue("@GrupaMaterialowa", datatable.Rows[i][10].ToString());
                                cmd.Parameters.AddWithValue("@Typ", datatable.Rows[i][11].ToString());
                                cmd.Parameters.AddWithValue("@Tag", datatable.Rows[i][12].ToString());
                                cmd.ExecuteNonQuery();
                            }
                            transaction.Commit();                        
                    }
                    sqliteCon.Close();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Exception: " + e.Message);
                }
            }
            stopwatch.Stop();
            Debug.WriteLine("Procedure GetFromAmdToSqlite2 takes: " + stopwatch.Elapsed);

        }

        private static DataTable CreateAMDTable()
        {
            DataTable orderDetailTable = new DataTable("AMD");
            // Define all the columns once.
            DataColumn[] cols ={
                                  new DataColumn("NumerArtykulu",typeof(String)),
                                  new DataColumn("NumerRegalu",typeof(Int32)),
                                  new DataColumn("NumerPrzedzialu",typeof(Int32)),
                                  new DataColumn("NumerPolki",typeof(Int32)),
                                  new DataColumn("StanWMiejscuSkladowania",typeof(Int32)),
                                  new DataColumn("OznaczenieFifo",typeof(Int32)),
                                  new DataColumn("WielkoscPojemnika",typeof(String)),
                                  new DataColumn("StanMinimalny",typeof(Int32)),
                                  new DataColumn("NumerSzeregu",typeof(Int32)),
                                  new DataColumn("NazwaArtykulu",typeof(String)),
                                  new DataColumn("GrupaMaterialowa",typeof(String)),
                                  new DataColumn("Typ",typeof(String)),
                                  new DataColumn("Tag",typeof(String))
                              };

            orderDetailTable.Columns.AddRange(cols);            
            return orderDetailTable;
        }



#nullable enable
        private string? GetTheNewestTypeOfFileFromDirInput(string fileType)
        {
            var directoryInfo = new DirectoryInfo(_configuration.GetSection("ExchangeFileDir").GetSection("Input").Value);
            if (directoryInfo.GetFiles("*." + fileType).Length > 0)
            {
                var myFile = directoryInfo.GetFiles("*." + fileType)
                 .OrderByDescending(f => f.LastWriteTime)
                 .First();
                return myFile.ToString();
            }
            else
            {
                return null;
            }
        }      


    }
}

