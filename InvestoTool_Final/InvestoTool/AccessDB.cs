using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace InvestoTool
{
    public interface IAccessDB
    {
        void initialize_db();
        bool delete_db();
        void write_db(Double profit_margin, int threshold, int num_nodes, string pathway);
        Object[,] read_db(int num_rows);
    }
    public class AccessDB : IAccessDB
    {
        private static String DBname = "my_db.db";
        //private static String path = 'D:\Dropbox\Classes\60-311 intro to software eng\project\test'
        private static SQLiteConnection m_dbConnection; //singleton 

        /*~~~DB creation and deletion methods~~~*/

        //initialize_db() is a method that ensures a db file is created, a db connection to it exists, and the initial table exists
        public void initialize_db()
        {
            if (!File.Exists(DBname))
            {                      //if db file dne, create it
                SQLiteConnection.CreateFile(DBname);        //create db file
            }
            if (Object.ReferenceEquals(null, m_dbConnection))   //if we haven't created connection to db, create it
            {
                m_dbConnection = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;"); //connection to db. for open()/close()/query updates
                m_dbConnection.Open();              //create new table if it dne already
                string query = "create table if not exists history (id INTEGER PRIMARY KEY AUTOINCREMENT, DateTime datetime, profit_margin INTEGER, threshold REAL, num_nodes INTEGER, pathway TEXT)";
                SQLiteCommand command = new SQLiteCommand(query, m_dbConnection); //SQLite command
                command.ExecuteNonQuery();          //execute command, if it's not a select query so it only returns int
                m_dbConnection.Close();             //must close connection to update SQLite
            }
        }
        //delete_db() will delete the entire db 
        public bool delete_db()
        {
            bool deleted = false;
            if (File.Exists(DBname))
            {
                try
                {
                    File.Delete(DBname);    //SQLite db is not like SQL. simply delete the file instead of using SQLite commands.
                    deleted = true;
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                Console.WriteLine("DB file doesn't exist.");
                deleted = true;
            }
            return deleted;
        }

        /*~~~SQLite table update/retrieval methods~~~*/

        //write_db insert new rows into table
        public void write_db(Double profit_margin, int threshold, int num_nodes, string pathway)
        {
            var time = DateTime.Now;            //gets system date and time
            string formattedTime = time.ToString("yyyy, MM, dd, hh, mm, ss"); //parse date and time

            m_dbConnection.Open();
            string query = "insert into history (DateTime, profit_margin, threshold, num_nodes, pathway) values (\"" + formattedTime + "\", " + profit_margin + ", " + threshold + ", " + num_nodes + ", \"" + pathway + "\")";
            Console.WriteLine(query);
            SQLiteCommand command = new SQLiteCommand(query, m_dbConnection); //SQLite command
            command.ExecuteNonQuery();          //execute command, if it's not a select query so it only returns int
            m_dbConnection.Close();             //must close connection to update SQLite
        }
        //read_db retrieves rows from table 'history' and returns an array of multiple types
        //returns a 2D array
        public Object[,] read_db(int num_rows)
        {
            Object[,] myarray = new Object[num_rows, 5];  //[row,column]

            m_dbConnection.Open();                       //retrieve latest rows from table by ordering by autoincrementing id and taking limit num_rows given

            string query = "SELECT count(*) FROM history";
            SQLiteCommand command = new SQLiteCommand(query, m_dbConnection); //SQLite command
            SQLiteDataReader r = command.ExecuteReader();
            r.Read();
            int rows = r.GetInt32(0);

            if (rows < num_rows)
            {
                num_rows = rows;
            }

            query = "SELECT DateTime, profit_margin, threshold, num_nodes, pathway FROM history ORDER BY id DESC LIMIT " + num_rows;
            command = new SQLiteCommand(query, m_dbConnection); //SQLite command
            var reader = command.ExecuteReader();
            for (int i = 0; i < num_rows; i++)
            {
                reader.Read();                          //read next line
                myarray[i, 0] = reader.GetString(0);    //store values into myarray
                myarray[i, 1] = reader.GetDouble(1);
                myarray[i, 2] = reader.GetDouble(2);
                myarray[i, 3] = reader.GetInt32(3);
                myarray[i, 4] = reader.GetString(4);
            }
            m_dbConnection.Close();                     //must close connection to update SQLite
            return myarray;
        }
    }
}
