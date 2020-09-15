using Dapper;
using MagTech.DataAccess.Repository.IRepository;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;


namespace MagTech.DataAccess.Repository
{
    public class FromHanelToSqliteRepository : IFromHanelToSqliteRepository
    {
        private  IConfiguration _configuration { get; }
        private static string ConnectionString ="";
        public FromHanelToSqliteRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectionString = configuration.GetConnectionString("Sqlite");
        }
#nullable enable
        public void GetFromAmdToSqlite()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            string? amdFileToRead = GetTheNewestTypeOfFileFromDirInput("amd");
            if(amdFileToRead !=null)
            {
                try
                {
                    var query = @"INSERT OR REPLACE INTO
                                        'main'.'ZbiorArtykolow'
                                        (
                                        'Id',
                                        'NumerArtykulu',
                                        'NumerRegalu', 
                                        'NumerPrzedzialu',
                                        'NumerPolki', 
                                        'StanWMiejscuSkladowania', 
                                        'OznaczenieFifo','WielkoscPojemnika',
                                        'StanMinimalny',
                                        'NumerSzeregu',
                                        'NazwaArtykulu', 
                                        'GrupaMaterialowa',
                                        'Typ',
                                        'Tag',
                                        'DodatkoweInformacje
                                        ')
                                        VALUES
                                        (
                                        (
                                        SELECT 'Id' FROM 'main'.'ZbiorArtykolow' WHERE 
                                        'NumerArtykulu'= @NumerArtykulu AND 
                                        'NumerRegalu' = @NumerRegalu AND
                                        'NumerPrzedzialu'= @NumerPrzedzialu AND
                                        'NumerPolki' = @NumerPolki
                                        ),
                                        @NumerArtykulu, 
                                        @NumerRegalu,
                                        @NumerPrzedzialu,
                                        @NumerPolki,
                                        @StanWMiejscuSkladowania, 
                                        @OznaczenieFifo,
                                        @WielkoscPojemnika, 
                                        @StanMinimalny, 
                                        @NumerSzeregu, 
                                        @NazwaArtykulu, 
                                        @GrupaMaterialowa, 
                                        @Typ, 
                                        @Tag, 
                                        (
                                        SELECT 'DodatkoweInformacje' FROM 'main'.'ZbiorArtykolow' WHERE
                                        'NumerArtykulu'= @NumerArtykulu AND 
                                        'NumerRegalu' = @NumerRegalu AND
                                        'NumerPrzedzialu'= @NumerPrzedzialu AND
                                        'NumerPolki' = @NumerPolki
                                        ))";
                    StreamReader sr = new StreamReader(_configuration.GetSection("ExchangeFileDir").GetSection("Input").Value + amdFileToRead);
                    //Read the first line of text
                    var line = sr.ReadLine();
                    //Continue to read until you reach end of file
                    while (line != null)
                    {
                        // Delete *$ from the begining of line
                        line = line.Substring(2, line.Length - 2); 
                        var row = line.Split("$");
                        //Prepare string row to insert into SqliteDB
                        foreach(var column in row)
                        {
                            //Remove unwanted chars
                            switch (column.Substring(0,1))
                            {
                                case "S": //Numer artykułu 0
                                    column.Substring(1, column.Length - 1).Trim();
                                    break;
                                case "L": //Numer regału 1
                                    column.Substring(1, column.Length - 1).Trim();
                                    break;
                                case "F": //Numer przedziału 2
                                    column.Substring(1, column.Length - 1).Trim();
                                    break;
                                case "T": //Numer półki 3
                                    column.Substring(1, column.Length - 1).Trim();
                                    break;
                                case "B": //Stan w miejscu składowania 4
                                    column.Substring(1, column.Length - 1).Trim();
                                    break;
                                case "I": //Oznaczenie FIFO 5
                                    column.Substring(1, column.Length - 1).Trim();
                                    break;
                                case "G": //Wielkość pojemnika 6
                                    column.Substring(1, column.Length - 1).Trim();
                                    break;
                                case "R": //Stan minimalny 7
                                    column.Substring(1, column.Length - 1).Trim();
                                    break;
                                case "O": //Numer szeregu 8
                                    column.Substring(1, column.Length - 1).Trim();
                                    break;                                  
                                case "N": //Nazwa artykułu 9                                  
                                    column.Substring(1, column.Length - 1).TrimEnd();
                                    break;
                                case "H":
                                    switch(column.Substring(0, 3))
                                    {
                                        case "H01": //Grupa materiałowa 10
                                            column.Substring(3, column.Length - 3).Trim();
                                            break;
                                        case "H02": //Typ 11
                                            column.Substring(3, column.Length - 3).Trim();
                                            break;
                                        case "H03": //Tag 12
                                            column.Substring(3, column.Length - 3).Trim();
                                            break;
                                    }
                                    break;
                            }
                            var parameter = new DynamicParameters();
                            parameter.Add("@NumerArtykulu", row[0].ToString()); 
                            parameter.Add("@NumerRegalu", Int16.Parse(row[1]));
                            parameter.Add("@NumerPrzedzialu", Int16.Parse(row[2]));
                            parameter.Add("@NumerPolki", Int16.Parse(row[3]));
                            parameter.Add("@StanWMiejscuSkladowania", Int16.Parse(row[4]));
                            parameter.Add("@OznaczenieFifo", Int16.Parse(row[5]));
                            parameter.Add("@WielkoscPojemnika", row[6].ToString());
                            parameter.Add("@StanMinimalny", Int16.Parse(row[7]));
                            parameter.Add("@NumerSzeregu", Int16.Parse(row[8]));
                            parameter.Add("@NazwaArtykulu", row[9].ToString());
                            parameter.Add("@GrupaMaterialowa", row[10].ToString());
                            parameter.Add("@Typ", row[11].ToString());
                            parameter.Add("@Tag", row[12].ToString());
                            Execute(query, parameter);
                        }
                    }
                    //close the file
                    sr.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: " + e.Message);
                }
            

                //Ustawić wszystkie stany na 0,

                //wczytać plik AMD i wrzucić go do bazy

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

            }
            stopwatch.Stop();
            Debug.WriteLine("Procedure GetFromOpjToSqlite takes: " + stopwatch.Elapsed);
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

        private void Execute(string query, DynamicParameters param)
        {
            using (SqliteConnection sqliteCon = new SqliteConnection(ConnectionString))
            {
                sqliteCon.Open();
                sqliteCon.Execute(query, param, commandType: System.Data.CommandType.Text);
            }
        }



    }
}
