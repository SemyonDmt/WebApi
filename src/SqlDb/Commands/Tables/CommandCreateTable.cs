using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using SqlDb.Model;
using SqlDb.Model.Structure.Columns;
using SqlDb.Requests.Tables;
using Column = SqlDb.Model.Column;

namespace SqlDb.Commands.Tables
{
    public class CommandCreateTable : ICommand
    {
        private readonly RequestCreateTable _request;
        private readonly RequestCreateProcInsertRow _requestInsert;
        private readonly RequestCreateProcUpdateRow _requestUpdate;
        private readonly RequestCreateProcDeleteRow _requestDelete;
        public bool IsSuccessful { get; private set; }
        public string Error { get; private set; }

        public CommandCreateTable(string tableName, Column[] columns)
        {
            var col = Convert(columns);
            _request = new RequestCreateTable(tableName, col);
            _requestInsert = new RequestCreateProcInsertRow(tableName, col);
            _requestUpdate = new RequestCreateProcUpdateRow(tableName, col);
            _requestDelete = new RequestCreateProcDeleteRow(tableName);
        }

        public async Task ExecuteAsync(DbCommand command)
        {
            DbTransaction transaction = null;
            try
            {
                transaction = command.Connection.BeginTransaction();
                command.Transaction = transaction;

                if (await TryCreateTable(command) &&
                    await TryCreateInsertProc(command) &&
                    await TryCreateUpdateProc(command) &&
                    await TryCreateDeleteProc(command))
                {
                    transaction.Commit();
                    IsSuccessful = true;
                    return;
                }

                transaction.Rollback();
                Error = "Error";
            }
            catch (Exception)
            {
                transaction?.Rollback();
                //Todo Реализовать разновидности ошибок
                Error = "Error";
            }
        }

        private async Task<bool> TryCreateTable(DbCommand command)
        {
            command.CommandText = _request.Sql();
            return await command.ExecuteNonQueryAsync(CancellationToken.None) == -1;
        }

        private async Task<bool> TryCreateInsertProc(DbCommand command)
        {
            command.CommandText = _requestInsert.Sql();
            return await command.ExecuteNonQueryAsync(CancellationToken.None) == -1;
        }

        private async Task<bool> TryCreateUpdateProc(DbCommand command)
        {
            command.CommandText = _requestUpdate.Sql();
            return await command.ExecuteNonQueryAsync(CancellationToken.None) == -1;
        }

        private async Task<bool> TryCreateDeleteProc(DbCommand command)
        {
            command.CommandText = _requestDelete.Sql();
            return await command.ExecuteNonQueryAsync(CancellationToken.None) == -1;
        }

        private Model.Structure.Columns.Column[] Convert(Column[] tableColumns)
        {
            var list = new List<Model.Structure.Columns.Column>();

            foreach (var tableColumn in tableColumns)
            {
                switch (tableColumn.ColumnType)
                {
                    case ColumnType.none:
                        break;
                    case ColumnType.@int:
                        list.Add(new ColumnNumeric(tableColumn.Name, tableColumn.IsNull));
                        break;
                    case ColumnType.nvarchar:
                        list.Add(new ColumnString(tableColumn.Name, tableColumn.IsNull, tableColumn.Size ?? 0));
                        break;
                    case ColumnType.datetime:
                        list.Add(new ColumnDatetime(tableColumn.Name, tableColumn.IsNull));
                        break;
                    case ColumnType.bit:
                        list.Add(new ColumnBoolean(tableColumn.Name, tableColumn.IsNull));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return list.ToArray();
        }
    }
}