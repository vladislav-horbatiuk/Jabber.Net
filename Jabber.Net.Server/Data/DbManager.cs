using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;


namespace Jabber.Net.Server.Data
{
    public class DbManager : IDisposable
    {
        private readonly string connectionStringName;
        private IDbConnection connection;
        private ISqlDialect dialect = SqlDialect.Default;


        public DbManager(string connectionStringName)
        {
            Args.NotNull(connectionStringName, "connectionStringName");
            this.connectionStringName = connectionStringName;
        }

        public void Dispose()
        {
            if (connection != null)
            {
                connection.Dispose();
                connection = null;
            }
        }

        public IDbTransaction BeginTransaction()
        {
            return GetConnection().BeginTransaction();
        }

        public IEnumerable<object[]> ExecList(ISqlInstruction sql)
        {
            using (var command = CreateCommand(sql))
            using (var reader = command.ExecuteReader())
            {
                var result = new List<object[]>();
                var fieldCount = reader.FieldCount;
                while (reader.Read())
                {
                    var row = new object[fieldCount];
                    for (var i = 0; i < fieldCount; i++)
                    {
                        row[i] = reader[i];
                        if (DBNull.Value.Equals(row[i])) row[i] = null;
                    }
                    result.Add(row);
                }
                return result;
            }
        }

        public T ExecScalar<T>(ISqlInstruction sql)
        {
            using (var command = CreateCommand(sql))
            {
                var result = command.ExecuteScalar();
                return result == null || result == DBNull.Value ? default(T) : (T)Convert.ChangeType(result, typeof(T));
            }
        }

        public int ExecuteNonQuery(ISqlInstruction sql)
        {
            using (var command = CreateCommand(sql))
            {
                return command.ExecuteNonQuery();
            }
        }

        public int ExecBatch(IEnumerable<ISqlInstruction> batch)
        {
            if (batch == null) throw new ArgumentNullException("batch");

            var affected = 0;
            using (var tx = BeginTransaction())
            {
                foreach (var sql in batch)
                {
                    affected += ExecuteNonQuery(sql);
                }
                tx.Commit();
            }
            return affected;
        }


        private IDbConnection GetConnection()
        {
            if (connection == null)
            {
                var cs = ConfigurationManager.ConnectionStrings[connectionStringName];
                var provider = DbProviderFactories.GetFactory(cs.ProviderName);
                connection = provider.CreateConnection();
                connection.ConnectionString = cs.ConnectionString;
                // 2 tries
                try
                {
                    connection.Open();
                }
                catch
                {
                    connection.Open();
                }

                dialect = SqlDialect.GetDialect(cs.ProviderName);
            }
            return connection;
        }

        private IDbCommand CreateCommand(ISqlInstruction sql)
        {
            var command = GetConnection().CreateCommand();
            var parameters = sql.GetParameters().ToArray();
            var parts = sql.ToString(dialect).Split('?');
            var result = new StringBuilder();
            for (var i = 0; i < parts.Length - 1; i++)
            {
                var p = command.CreateParameter();
                p.ParameterName = "p" + i;
                if (parameters[i] == null)
                {
                    p.Value = DBNull.Value;
                }
                else if (parameters[i] is Enum)
                {
                    p.Value = ((Enum)parameters[i]).ToString("d");
                }
                else
                {
                    p.Value = parameters[i];
                }
                command.Parameters.Add(p);
                result.AppendFormat("{0}@{1}", parts[i], p.ParameterName);
            }
            result.Append(parts[parts.Length - 1]);
            command.CommandText = result.ToString();
            return command;
        }
    }
}