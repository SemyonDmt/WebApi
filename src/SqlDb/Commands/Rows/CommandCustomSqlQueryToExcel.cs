using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using SqlDb.Requests.Rows;

namespace SqlDb.Commands.Rows
{
    public class CommandCustomSqlQueryToExcel : ICommand
    {
        private readonly RequestCustomSqlQuery _request;
        private readonly string _tableName;
        public byte[] Data { get; private set; }
        public bool IsSuccessful { get; private set; }
        public string Error { get; private set; }

        public CommandCustomSqlQueryToExcel(string tableName, string select, string order, string filter, int page, int size)
        {
            _request = new RequestCustomSqlQuery(tableName, select, order, filter, page, size, RequestFormat.Reader);
            _tableName = tableName;
        }

        public async Task ExecuteAsync(DbCommand command)
        {
            try
            {
                command.CommandText = _request.Sql();
                command.Parameters.AddRange(_request.Parameters());

                using (var reader = await command.ExecuteReaderAsync(CancellationToken.None))
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add(_tableName);
                    worksheet.Cells[1, 1].LoadFromDataReader(reader, true, _tableName, TableStyles.Light1);
                    Data = package.GetAsByteArray();
                    IsSuccessful = true;
                }
            }
            catch (Exception)
            {
                //Todo Реализовать разновидности ошибок
                Error = "Error";
            }
        }
    }
}