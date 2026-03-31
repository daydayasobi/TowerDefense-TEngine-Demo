using System;
using Mono.Data.Sqlite;
using UnityEngine;

namespace TEngine
{
    public class SQLiteHelper
    {
        private SqliteConnection _dbConnection;
        private SqliteCommand _dbCommand;
        private SqliteDataReader _dataReader;

        public SQLiteHelper(string connectionString)
        {
            try
            {
                _dbConnection = new SqliteConnection(connectionString);
                _dbConnection.Open();
            }
            catch (Exception exception)
            {
                Debug.Log(exception.Message);
            }
        }

        public SqliteDataReader ExecuteQuery(string queryString)
        {
            _dbCommand = _dbConnection.CreateCommand();
            _dbCommand.CommandText = queryString;
            _dataReader = _dbCommand.ExecuteReader();
            return _dataReader;
        }

        public void CloseConnection()
        {
            if (_dbCommand != null)
            {
                _dbCommand.Cancel();
            }

            _dbCommand = null;

            if (_dataReader != null)
            {
                _dataReader.Close();
            }

            _dataReader = null;

            if (_dbConnection != null)
            {
                _dbConnection.Close();
            }

            _dbConnection = null;
        }

        public SqliteDataReader CreateTable(string tableName, string[] colNames, string[] colTypes)
        {
            var queryString = "CREATE TABLE " + tableName + "( " + colNames[0] + " " + colTypes[0];
            for (var i = 1; i < colNames.Length; i++)
            {
                queryString += ", " + colNames[i] + " " + colTypes[i];
            }

            queryString += "  ) ";
            return ExecuteQuery(queryString);
        }
    }
}
