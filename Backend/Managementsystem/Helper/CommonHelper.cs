using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Managementsystem.Helper
{
    public class CommonHelper
    {
        public object ConvertDataSetToJson(DataSet ds, int statusCode,string message)
        {
            List<object> result = new List<object>();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataTable table in ds.Tables)
                {
                    List<Dictionary<string, object>> tableData = new();

                    foreach (DataRow row in table.Rows)
                    {
                        var item = new Dictionary<string, object>();

                        foreach (DataColumn column in table.Columns)
                        {
                            item[column.ColumnName] = row[column];
                        }

                        tableData.Add(item);
                    }

                    result.Add(tableData);
                }
            }

            return new
            {
                statusCode = statusCode,
                message = message,
                result = result
            };
        }
    }
}
