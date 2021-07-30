using Accounting.Auth;
using AccountingNote.DBSource;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AccountingNote.SysteamAdmin
{
    public partial class UserInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)//可能是按鈕跳回並非真正登入所以要判斷
            {
                //if (this.Session["UserLoginInfo"] == null)
                //{
                //    Response.Redirect("/Login.aspx");
                //    return;//假設沒有登入過就返回登入頁
                //}

                if (!AuthManager.IsLogined())//檢查帳號
                {
                    Response.Redirect("/Login.aspx");
                    return;

                }

                string account = this.Session["UserLoginInfo"] as string;
                DataRow dr = UserInfoManager.GetUserInfoByAccount(account);
                //轉成字串，去查使用者存不存在

                var currentUser = AuthManager.GetCurrentUser();

                if (currentUser == null)//帳號不存在就跳回登入頁
                {
                    this.Session["UserLoginInfo"] = null;//清理以免造成上下迴圈
                    Response.Redirect("/Login.aspx");
                    return;
                }
                this.ltAccount.Text = currentUser.Account;
                this.ltName.Text = currentUser.Name;
                this.ltEmail.Text = currentUser.Email;
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            this.Session["UserLoginInfo"] = null;
            Response.Redirect("/Login.aspx");
        }
    }
}