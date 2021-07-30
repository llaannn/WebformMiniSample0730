using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingNote.DBSource
{
    public class AccountingManager
    {
        public static string GetConnectionstring()
        {
            string val = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            return val;

        }

        public static DataTable GetAccountingList(string userID)//帳號間不該看到彼此
        {
            string connStr = GetConnectionstring();
            string dbCommandString =
                    @"SELECT ID,Caption,Amount,ActType,CreateDate,Body
                    FROM Accounting
                    WHERE UserID = @userID
                ";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand command = new SqlCommand(dbCommandString, conn))
                {
                    command.Parameters.AddWithValue("@userID", userID);

                    try
                    {
                        conn.Open();
                        var reader = command.ExecuteReader();

                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        return dt;

                        //if (dt.Rows.Count == 0)
                        //    return null;

                        //DataRow dr = dt.Rows[0];
                        //return dr;

                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog(ex);
                        Console.WriteLine(ex.ToString());
                        return null;
                    }
                }
            }

        }

        /// <summary>建立流水帳的項目</summary>
        /// <param name="userID"></param>
        /// <param name="caption"></param>
        /// <param name="amount"></param>
        /// <param name="actType"></param>
        /// <param name="body"></param>
        public static void CreateAccounting(string userID, string caption, int amount, int actType, string body) //第一個做的 沒有回傳值
        {
            //確認輸入值
            if (amount < 0 || amount > 1000000)
                throw new ArgumentException("Amount must between 0 and 1,000,000.");

            if (actType < 0 || actType > 1)
                throw new ArgumentException("ActType must be 0 or 1.");


            //連線資料庫

            string connStr = GetConnectionstring();

            string dbCommand =
                @"SELECT INSERT INTO [dbo].[Accounting]
                  (
	                UserID
	                ,Caption
	                ,Amount
	                ,ActType
	                ,CreateDate
	                ,Body
                  )
                    VALUES
                  (
                    @userID
	                ,@caption
	                ,@amount
	                ,@actType
	                ,@createDate
	                ,@body
                 )
                ";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand comm = new SqlCommand(dbCommand, conn))
                {
                    comm.Parameters.AddWithValue("@userID", userID);
                    comm.Parameters.AddWithValue("@caption", caption);
                    comm.Parameters.AddWithValue("@amount", amount);
                    comm.Parameters.AddWithValue("@actType", actType);
                    comm.Parameters.AddWithValue("@createDate", DateTime.Now);
                    comm.Parameters.AddWithValue("@body", body);

                    try
                    {
                        conn.Open();
                        //var reader =
                        comm.ExecuteNonQuery();

                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog(ex);

                    }
                }
            }

        }
        
        /// <summary>更新流水帳內容 </summary>
        /// <param name="ID"></param>
        /// <param name="userID"></param>
        /// <param name="caption"></param>
        /// <param name="amount"></param>
        /// <param name="actType"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static bool UpdateAccounting(int ID, string userID, string caption, int amount, int actType, string body)//需要帶入自己的ID
        {
            if (amount < 0 || amount > 1000000)
                throw new ArgumentException("amount從一到一百萬之間");

            if (actType < 0 || actType > 1)
                throw new ArgumentException("actType必須為零或一");

            string connStr = GetConnectionstring();

            string dbCommand =
                $@" UPDATE [Accounting]
                    SET
	                    UserID  = @userID
	                    ,Caption = @caption
	                    ,Amount  = @amount
	                    ,ActType = @actType
	                    ,CreateDate = @createDate
	                    ,Body = @body           
                    WHERE 
                         ID =@id ";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand comm = new SqlCommand(dbCommand, conn))
                {
                    comm.Parameters.AddWithValue("@userID", userID);
                    comm.Parameters.AddWithValue("@Caption", caption);
                    comm.Parameters.AddWithValue("@Amount", amount);
                    comm.Parameters.AddWithValue("@ActType", actType);
                    comm.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                    comm.Parameters.AddWithValue("@body", body);
                    comm.Parameters.AddWithValue("@id", ID);

                    try
                    {
                        conn.Open();
                        int effectRows = comm.ExecuteNonQuery();


                        if (effectRows == 1)
                            return true;
                        else
                            return false;

                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog(ex);
                        return false;
                    }
                }
            }

        }
        
        /// <summary>查詢內容 </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DataRow GetAccounting(int id,string userID)
        {
            string connStr = GetConnectionstring();
            string dbCommand =
                $@" SELECT
                ID
                ,Caption
                ,Amount
                ,ActType
                ,CreateDate
                ,Body
                FROM  Accounting
                WHERE id = @id AND UserID=@userID;

                ";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand comm = new SqlCommand(dbCommand, conn))
                {
                    comm.Parameters.AddWithValue("@id", id);
                    comm.Parameters.AddWithValue("@userID", userID);

                    try
                    {
                        conn.Open();
                        var reader = comm.ExecuteReader();

                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        if (dt.Rows.Count == 0)
                            return null;

                        return dt.Rows[0];

                    }

                    catch (Exception ex)
                    {
                        return null;
                    }

                }

            }

        }

        public static void DeleteAccount(int ID)
        {
            string connStr = GetConnectionstring();
            string dbCommand =
                $@"DELETE[Accountins]
                   WHERE ID = @id

                ";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand comm = new SqlCommand(dbCommand, conn))
                {
                    comm.Parameters.AddWithValue("@id", ID);

                    try
                    {
                        conn.Open();
                        comm.ExecuteNonQuery();

                    }

                    catch (Exception ex)
                    {
                        Logger.WriteLog(ex);
                    }

                }
            }


        }
    }
}
   