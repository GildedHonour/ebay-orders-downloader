using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace EbayOrdersDownloaderConsole
{
    public class DALProvider : IDisposable
    {
        [ThreadStatic]
        static SqlConnection commonConnection;
        private static string _connectionStringName = "orderManager";

        public void Dispose()
        {
            if (commonConnection != null)
            {
                commonConnection.Dispose();
            }
        }

        public static string ConnectionStringName
        {
            get { return _connectionStringName; }
            set { _connectionStringName = value; }
        }

        private static SqlConnection GetConnectionInternal()
        {
            var connection = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString);
            connection.Open();
            return connection;
        }

        internal static SqlConnection GetConnection()
        {
            if (commonConnection != null && commonConnection.State != ConnectionState.Open)
            {
                commonConnection.Dispose();
                return GetConnectionInternal();
            }

            return commonConnection ?? GetConnectionInternal();
        }

        internal static void CloseConnection(SqlConnection connection)
        {
            if (connection != commonConnection)
            {
                connection.Dispose();
            }
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

        protected static SqlParameter CreateParameter(string name, object value, bool skipEmpty, bool isOutput = false)
        {
            if (skipEmpty)
            {
                if (value == null)
                {
                    return null;
                }


                if (value is string && string.IsNullOrEmpty((string)value))
                {
                    return null;
                }
            }

            var param = new SqlParameter
            {
                ParameterName = string.Format("@{0}", name),
                Direction = isOutput == false ? ParameterDirection.Input : ParameterDirection.Output,
                Value = value ?? DBNull.Value,
                SourceColumn = name
            };

            return param;
        }


        protected static T ExecuteReader<T>(IDbCommand command, Func<IDataReader, T> func, bool isSingleRow = false)
        {
            var combehavior = isSingleRow ? CommandBehavior.SingleRow : CommandBehavior.Default;
            var connection = GetConnection();
            try
            {
                command.Connection = connection;
                //connection.Open();

                using (var reader = command.ExecuteReader(combehavior))
                {
                    return func(reader);
                }
            }
            finally
            {
                CloseConnection(connection);
            }

        }


        protected static void ExecuteReader(IDbCommand command, Action<IDataReader> action, bool isSingleRow = false)
        {
            var combehavior = isSingleRow ? CommandBehavior.SingleRow : CommandBehavior.Default;

            var connection = GetConnection();
            try
            {
                command.Connection = connection;
                //connection.Open();

                using (var reader = command.ExecuteReader(combehavior))
                {
                    action(reader);
                }
            }
            finally
            {
                CloseConnection(connection);
            }
        }

        protected static int ExecuteNonQuery(IDbCommand command)
        {
            var connection = GetConnection();
            try
            {
                command.Connection = connection;
                connection.Open();
                return command.ExecuteNonQuery();
            }
            finally
            {
                CloseConnection(connection);
            }
        }

        protected static int CommandExecute(IDbCommand command, Func<IDbCommand, int> commandAction)
        {
            bool alreadyExists = commonConnection != null;
            if (!alreadyExists)
            {
                commonConnection = GetConnectionInternal();
            }

            try
            {
                command.Connection = commonConnection;
                //connection.Open();
                return commandAction(command);
            }
            finally
            {
                if (!alreadyExists)
                {
                    commonConnection.Dispose();
                }
            }
        }


        protected static void CommandExecute(IDbCommand command, Action<IDbCommand> commandAction)
        {
            var connection = GetConnection();
            try
            {
                command.Connection = connection;
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
            try
            {
                command.Connection = connection;
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
