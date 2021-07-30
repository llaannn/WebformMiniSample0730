
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accounting.Auth;
using AccountingNote.DBSource;

namespace AccountingNote
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if(this.Session["UserLogin"] !=null)
            {
                this.plcLogin.Visible = false;
                Response.Redirect("/SystemAdmin/UserInfo.aspx");
            }

            else
            {
                this.plcLogin.Visible = true;
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            //string inp_Account = "帳號";
            //string db_Password = "密碼";

            string inp_Account = this.txtAccount.Text;
            string inp_PWD = this.txtPWD.Text;

            if(!AuthManager.TryLogin(inp_Account,inp_PWD, out msg))
            {
                this.ltlMsg.Text = "需要填入帳號或密碼";
                return;
            }
            //var dr =  UserInfoManager

            var dr = UserInfoManager.GetUserInfoByAccount(inp_Account);//去資料庫查資料

            if(dr == null)
            {
                this.ltlMsg.Text = "帳號不存在";
                return;
            }

            if(string.Compare(dr["Account"].ToString(), inp_Account, true)== 0 && 
                string.Compare(dr["PWD"].ToString(), inp_PWD, false)== 0)
             
                
            {
                this.Session["UserLoginInfo"] = dr["Account"].ToString();
                Response.Redirect("/SystemAdmin/UserInfo.aspx");
            }
            else
            {
                this.ltlMsg.Text = "登入失敗";
                return;
            }
         
        }
    }
}