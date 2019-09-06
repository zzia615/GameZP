using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections;

/// <summary>
/// DBA 的摘要说明
/// </summary>
public class DBA
{
    public static string strConnection;
    static DBA()
    {
        DBA.strConnection = ConfigurationSettings.AppSettings["ConnString"];
    }
    
    public static DataTable GetDBToDataTable(string sqlCommand)
    {
        SqlConnection connection1 = new SqlConnection(DBA.strConnection);
        SqlDataAdapter adapter1 = new SqlDataAdapter();
        adapter1.SelectCommand = new SqlCommand(sqlCommand, connection1);
        try
        {
            connection1.Open();
        }
        catch
        {
            return null;
        }
        DataTable table1 = new DataTable();
        try
        {
            adapter1.Fill(table1);
        }
        catch (Exception exception1)
        {
            //WebFun.ScriptMessage("SQL错误:" + exception1.Message, true);
            //return null;
        }
        adapter1.Dispose();
        connection1.Close();
        connection1.Dispose();
        //WebFun.ScriptMessage("SQL错误:" + connection1.State.ToString(), true);
        return table1;
    }
    
}
