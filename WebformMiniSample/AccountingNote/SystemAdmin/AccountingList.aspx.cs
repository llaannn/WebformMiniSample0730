using Accounting.Auth;
using AccountingNote.DBSource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AccountingNote.SysteamAdmin
{
    public partial class AccountingList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!AuthManager.IsLogined())//檢查帳號
            {
                Response.Redirect("/Login.aspx");
                return;

            }


            string account = this.Session["UserLoginInfo"] as string; //1先取得登入的人是誰
            var dr = UserInfoManager.GetUserInfoByAccount(account);//2先取得帳號

            if (dr == null)
            {
                Response.Redirect("/Login.aspx");
                return;


            }

            //3 取得使用者資料之後才確認ID

            //G&P互換 有沒有值來做判斷
            var dt = AccountingManager.GetAccountingList(dr["ID"].ToString());
            if (dt.Rows.Count > 0)
            {
                this.gvAccountList.DataSource = dt;
                this.gvAccountList.DataBind();

            }
            else
            {
                this.gvAccountList.Visible = false;
                this.plcNoData.Visible = true;

            }


        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            Response.Redirect("/SystemAdmin/AccountingDetail.aspx");
        }


        protected void gvAccountList_RowDataBound1(object sender, GridViewRowEventArgs e)
        {
            var row = e.Row;

            if (row.RowType == DataControlRowType.DataRow)
            {
                //Literal lt1 = row.FindControl("actType") as Literal;

                Label lb1 = row.FindControl("lblActType") as Label;

                var dr = row.DataItem as DataRowView;
                int actType = dr.Row.Field<int>("ActType");

                if (actType == 0)


                {
                   //lt1.Text= "支出";

                    lb1.Text = "支出";
                }
                else


                {
                   //lt1.Text= "收入";
                    lb1.Text = "收入";
                }

                if (dr.Row.Field<int>("Amonut") > 1500)
                {
                    lb1.ForeColor = Color.Red;
                }
            }
        }
    }
}
