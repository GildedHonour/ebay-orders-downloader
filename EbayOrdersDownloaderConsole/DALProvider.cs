using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace EbayOrdersDownloaderConsole
{
    public abstract class DALProvider
    {
        protected static SqlConnection GetConnection()
        {
            var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["orderManager"].ConnectionString);
            connection.Open();
            return connection;
        }

        protected static void CloseConnection(SqlConnection connection)
        {
            connection.Dispose();
        }

        protected static SqlCommand BuildCommand(string commandtText, params SqlParameter[] parameters)
        {
            var cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = commandtText
            };
            FillCommand(cmd, parameters);
            return cmd;
        }

        protected static void FillCommand(SqlCommand command, params SqlParameter[] parameters)
        {
            command.Parameters.AddRange(parameters.Where(n => n != null).ToArray());
        }

        protected static SqlParameter CreateParameter(string name, object value)
        {
            var param = new SqlParameter
            {
                ParameterName = string.Format("@{0}", name),
                Direction = ParameterDirection.Input,
                Value = value ?? DBNull.Value,
                SourceColumn = name
            };

            return param;
        }

        protected static int ExecuteNonQuery(IDbCommand command)
        {
            var connection = GetConnection();
            command.Connection = connection;
            try
            {
                //connection.Open();
                return command.ExecuteNonQuery();
            }
            finally
            {
                CloseConnection(connection);
            }
        }

        protected static void CommandExecute(IDbCommand command, Action<IDbCommand> commandAction)
        {
            var connection = GetConnection();
            command.Connection = connection;
            try
            {
                //connection.Open();
                commandAction(command);
            }
            finally
            {
                CloseConnection(connection);
            }
        }

        protected static object ExecuteScalar(IDbCommand command)
        {
            var connection = GetConnection();
            command.Connection = connection;
            try
            {
                //connection.Open();
                return command.ExecuteScalar();
            }
            finally
            {
                CloseConnection(connection);
            }
        }
    }
}
