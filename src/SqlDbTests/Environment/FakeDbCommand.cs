using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace SqlDbTests.Environment
{
    public class FakeDbCommand : DbCommand
    {
        #region ConfigureOutputParameters

        private readonly List<Action<DbParameterCollection>> _actionAfterExecute = new List<Action<DbParameterCollection>>();

        public void SetOutputParameterValueAfterExecute(string parameterName, object val)
        {
            void Act(DbParameterCollection collection)
            {
                foreach (DbParameter parameter in collection)
                {
                    if (parameter.ParameterName != parameterName)
                        continue;

                    parameter.Value = val;
                    return;
                }
            }

            _actionAfterExecute.Add(Act);
        }

        #endregion

        #region FakeExecuteScalarAsync
        public object ConfigureExecuteScalarAsync { get; set; }
        public int CountCallExecuteScalarAsync { get; private set; }

        public override Task<object> ExecuteScalarAsync(CancellationToken cancellationToken)
        {
            //ToDo Добавить проверку на CommandText, посмотреть какой Exceptions нужно бросать
            //ToDo Подумать насчет проверки DbParameterCollection

            CountCallExecuteScalarAsync++;
            _actionAfterExecute.ForEach(a => a(_dbParameterCollection));
            return Task.FromResult(ConfigureExecuteScalarAsync);
        }
        #endregion

        #region FakeDbParameterCollection

        private readonly DbParameterCollection _dbParameterCollection = new SqlCommand().Parameters;
        public int CountCallDbParameterCollection { get; private set; }

        protected override DbParameterCollection DbParameterCollection
        {
            get
            {
                CountCallDbParameterCollection++;
                return _dbParameterCollection;
            }
        }

        #endregion

        #region FakeCommandText

        private string _commandText;
        public int CountCallCommandText { get; private set; }

        public override string CommandText
        {
            get => _commandText;
            set
            {
                CountCallCommandText++;
                _commandText = value;
            }
        }

        #endregion

        #region DbConnection

        public DbConnection ConfigureDbConnection { get; set; }
        protected override DbConnection DbConnection { get => ConfigureDbConnection; set => ConfigureDbConnection = value; }

        #endregion


        #region NotUsed
        public override void Cancel()
        {
            throw new System.NotImplementedException();
        }

        public override int ExecuteNonQuery()
        {
            throw new System.NotImplementedException();
        }

        public override object ExecuteScalar()
        {
            throw new System.NotImplementedException();
        }

        public override void Prepare()
        {
            throw new System.NotImplementedException();
        }

        public override int CommandTimeout { get; set; }
        public override CommandType CommandType { get; set; }
        public override UpdateRowSource UpdatedRowSource { get; set; }


        protected override DbTransaction DbTransaction { get; set; }
        public override bool DesignTimeVisible { get; set; }

        protected override DbParameter CreateDbParameter()
        {
            throw new System.NotImplementedException();
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}