using AccountingNote.DBSource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Accounting.Auth
{
    public class AuthManager
    {
        public static bool IsLogined()
        {
            if (HttpContext.Current.Session["UserLoginInfo"]==null)
                return false;
            else
                return true;

        }

        public static UserInfoModel GetCurrentUser()
        {
            string account = HttpContext.Current.Session["UserLoginInfo"] as string;
                if (account == null)
                return null;

            DataRow dr = UserInfoManager.GetUserInfoByAccount(account);

            if (dr == null)
                return null;

            UserInfoModel model = new UserInfoModel();
            model.ID = dr["ID"].ToString();
            model.Account = dr["Account"].ToString();
            model.Name = dr["Name"].ToString();
            model.Email = dr["Email"].ToString();



            return model;
        }

        public static void Logout()
        {
           // string account=
           // HttpContext.Current.Session["UserLoginInfo"]= s
           //null;


        }

        public static bool TryLogin(string account, string pwd, string errowMsg)
        {
if(string.IsNullOrWhiteSpace(account)||string.IsNullOrWhiteSpace(pwd))

            {
                errowMsg = "必須輸入數值";
                return false;


            }
            var dr = UserInfoManager.GetUserInfoByAccount(account);//去資料庫查資料

            if (dr == null)
            {
                errowMsg = $"Account:{account}不存在.";
                return false;
            }

            if (string.Compare(dr["Account"].ToString(),account, true) == 0 &&
                string.Compare(dr["PWD"].ToString(),pwd, false) == 0)


            {
                HttpContext.Current.Session["UserLoginInfo"] = dr["Account"].ToString();
                errowMsg = string.Empty;
                return true;
                
                //Response.Redirect("/SystemAdmin/UserInfo.aspx");
            }
            else
            {
              errowMsg = "登入失敗";
                return false;
            }


        }

    }
}
