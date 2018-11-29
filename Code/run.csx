#r "System.Configuration"
#r "System.Data"

using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

public static void Run(Stream myBlob, string name, TraceWriter log)
{
    log.Info($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

    string tableN = name.Substring(name.LastIndexOf('/') + 1);
    string schema_name = "";
    var str = ConfigurationManager.ConnectionStrings["sqldb_con"].ConnectionString;
    using (SqlConnection conn = new SqlConnection(str))
    {
        conn.Open();

        log.Info($"Connection Opened.");
        
        DataTable tableColumns = new DataTable();
        DataTable sourceData = new DataTable();
        var text = "SELECT COLUMN_NAME, DATA_TYPE, TABLE_SCHEMA FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'" + tableN + "'";
        SqlCommand cmd = new SqlCommand(text, conn);
        tableColumns.Load(cmd.ExecuteReader());
        var CountofColumnsinTable = tableColumns.Rows.Count;
        log.Info($"Reading columns: {tableN} with {CountofColumnsinTable} columns");
        foreach (DataRow row in tableColumns.Rows)
        {
            //log.Info($"Column: {row["COLUMN_NAME"].ToString()} Data Type: {row["DATA_TYPE"].ToString()}");
            schema_name = row["TABLE_SCHEMA"].ToString();
            string SQLdtp = row["DATA_TYPE"].ToString();
            //log.Info($"SQL Data Type: {row["DATA_TYPE"]}");
            if (SQLdtp == "int")
            {
                DataColumn col = new DataColumn { ColumnName = row["COLUMN_NAME"].ToString()};
                col.DataType = System.Type.GetType("System.Int32");
                sourceData.Columns.Add(col);
                log.Info($"Created Column: {row["COLUMN_NAME"]} with Int32 Type");
            }
            else if (SQLdtp == "nvarchar")
            {
                DataColumn col = new DataColumn { ColumnName = row["COLUMN_NAME"].ToString()};
                col.DataType = System.Type.GetType("System.String");
                sourceData.Columns.Add(col);
                log.Info($"Created Column: {row["COLUMN_NAME"]} with String Type");
            }
            else if (SQLdtp == "float")
            {
                DataColumn col = new DataColumn { ColumnName = row["COLUMN_NAME"].ToString()};
                col.DataType = System.Type.GetType("System.Decimal");
                sourceData.Columns.Add(col);
                log.Info($"Created Column: {row["COLUMN_NAME"]} with Decimal Type");
            }
            else if (SQLdtp == "datetime2")
            {
                DataColumn col = new DataColumn { ColumnName = row["COLUMN_NAME"].ToString()};
                col.DataType = System.Type.GetType("System.DateTime");
                sourceData.Columns.Add(col);
                log.Info($"Created Column: {row["COLUMN_NAME"]} with DateTime Type");
            }
        }
        log.Info($"Schema Name: {schema_name} with {sourceData.Columns.Count} Columns");
        //Truncate existing table
        var text2 = "TRUNCATE TABLE [" + schema_name + "].[" + tableN + "]";
        SqlCommand cmd2 = new SqlCommand(text2, conn);
        cmd2.ExecuteNonQuery();
        log.Info($"Table: {tableN} Truncated succesfully"); 

        //Load values from blob and insert rows
        var reader = new StreamReader(myBlob);
        
        while (!reader.EndOfStream)
            {
                var dataline = reader.ReadLine();
                var columnvalues = dataline.Split('|');
                //log.Info($"Array Length: {columnvalues.Length} for ID {columnvalues[0]}");
                
                DataRow rowtoInsert = sourceData.NewRow();
                for (int i=0; i < columnvalues.Length; i++)
                {
                    String ColType = sourceData.Columns[i].DataType.Name;
                    //log.Info($"{i} Column Type is: {sourceData.Columns[i].DataType.Name}");
                    if (columnvalues[i].ToString() != "")
                    {
                        rowtoInsert[i] = columnvalues[i]; 
                    }
                }
                //Load Data in Datatable
                sourceData.Rows.Add(rowtoInsert);
            }
            log.Info("SourceData Loaded Success !");
            SqlBulkCopy bcp = new SqlBulkCopy(str);
            bcp.BulkCopyTimeout = 300;
            bcp.DestinationTableName = "[" + schema_name + "].[" + tableN + "]";
            bcp.WriteToServer(sourceData);
    }
}
