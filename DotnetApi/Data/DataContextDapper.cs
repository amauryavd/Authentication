using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DotnetApi.Data;

public class DataContextDapper
{
    private readonly IConfiguration _config;

    public DataContextDapper(IConfiguration config)
    {
        _config = config;
    }

    public IEnumerable<T> LoadData<T>(string sql)
    {
        IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        return dbConnection.Query<T>(sql);
    }

    public T LoadDataSingle<T>(string sql)
    {
        IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        return dbConnection.QuerySingle<T>(sql);
    }

    public bool Execute(string sql)
    {
        IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        return dbConnection.Execute(sql) > 0;
    }

    public int ExecuteWithRowCount(string sql)
    {
        IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        return dbConnection.Execute(sql);
    }

    public bool ExecuteSQLWithParameters(string sql, List<SqlParameter> parameters)
    {
        SqlCommand sqlCmd = new SqlCommand(sql);
        foreach(SqlParameter param in parameters)
        {
            sqlCmd.Parameters.Add(param);
        }
        SqlConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        dbConnection.Open();
        sqlCmd.Connection = dbConnection;
        int rowsAffected = sqlCmd.ExecuteNonQuery();
        dbConnection.Close();
        return rowsAffected > 0;
    }
}