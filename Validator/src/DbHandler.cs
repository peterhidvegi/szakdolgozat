using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Validator.src;

namespace Validator.src
{
    class DbHandler
    {

        BackgroundWorker dbWorker;
        MySqlConnection mysqlConn;

        public DbHandler(Types.Status status, int userId, int currentEventId, int nextEventId, int nodeId)
        {
            setAtStart();
            dbWorker.RunWorkerAsync(sqlUpdate(status,userId,currentEventId,nextEventId,nodeId));
        }

        public DbHandler(Types.Status status, int userId, int currentEventId, int nodeId, DateTime date)
        {
            setAtStart();
            dbWorker.RunWorkerAsync(sqlUpdate(status, userId, currentEventId, nodeId, date));
        }

        public DbHandler(string status, int userId, int currentEventId, int nextEventId, int nodeId, DateTime date)
        {
            setAtStart();
            dbWorker.RunWorkerAsync(sqlUpdate(status, userId, currentEventId, nextEventId, nodeId, date));
        }

        public DbHandler(string name,string pass)
        {
            setAtStart();
            dbWorker.RunWorkerAsync(sqlInsert(name,pass));
        }

        private void setAtStart()
        {
            dbWorker = new BackgroundWorker();
            mysqlConn = App.getEPSMySqlConnection();
            dbWorker.DoWork += DbWorker_DoWork;
        }

        private void DbWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string sql = e.Argument.ToString();

            if (sql != "")
            {
                if (mysqlConn.State != ConnectionState.Open)
                    mysqlConn.Open();

                MySqlCommand innerinsertCommand = new MySqlCommand(sql, mysqlConn);
                innerinsertCommand.ExecuteNonQuery();
                mysqlConn.Close();
            }
        }

        private string sqlUpdate(Types.Status status, int userId , int currentEventId, int nextEventId, int nodeId)
        {
            return "UPDATE " + App.dbName + " SET validation = '"+status+"', user_id = " + userId + ", validation_time = '" + DateTime.Now + "' WHERE event_id >= " + currentEventId + " AND event_id <" + nextEventId + " AND node_id = " + nodeId;
        }

        private string sqlUpdate(Types.Status status, int userId , int currentEventId, int nodeId, DateTime date)
        {
            return "UPDATE " + App.dbName + " SET validation = '" + status + "', user_id = " + userId + ", validation_time = '" + DateTime.Now + "' WHERE event_id >= " + currentEventId + " AND event_id <" + int.MaxValue + " AND node_id = " + nodeId +
                                   " AND YEAR(timestamp) = '" + date.Year + "' AND MONTH(timestamp) = '" + date.Month + "' AND DAY(timestamp) = '" + date.Day + "'";
        }

        private string sqlUpdate(string status, int userId, int currentEventId, int nextEventId, int nodeId, DateTime date)
        {
            return "UPDATE " + App.dbName + " SET validation = '" + status + "', user_id = " + userId + ", validation_time = '" + DateTime.Now + "' WHERE event_id >= " + currentEventId + " AND event_id < " + nextEventId + " AND node_id = " + nodeId +
                             " AND YEAR(timestamp) = '" + date.Year + "' AND MONTH(timestamp) = '" + date.Month + "' AND DAY(timestamp) = '" + date.Day + "'";
        }

        private string sqlInsert(string name,string pass)
        {
            return "INSERT INTO user (username, password) VALUES ('" + name + "', '" + pass + "')";
        }
    }
}
