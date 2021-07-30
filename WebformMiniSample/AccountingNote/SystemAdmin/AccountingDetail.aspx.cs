using AccountingNote.DBSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AccountingNote.SysteamAdmin
{
    public partial class AccountingDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserLoginInfo"] == null)//第一次做好的登入檢查
            {
                Response.Redirect("/Login.aspx");
                return;
            }


            string account = this.Session["UserLoginInfo"] as string;
            var drUserInfo = UserInfoManager.GetUserInfoByAccount(account);

            if (drUserInfo == null)
            {
                Response.Redirect("/Login.aspx");
                return;
            }


            //到這邊
            if (!this.IsPostBack)//7
            {
                //5判斷是新增還是編輯模式

                if (this.Request.QueryString["ID"] == null)//近來這頁的時候先檢查是否網址有帶特定參數
                {
                    this.btnDelete.Visible = false;//是新噌模式 刪除隱藏 也不用顯示

                }

                else
                {
                    this.btnDelete.Visible = true;//控制按鈕選項

                    string idText = this.Request.QueryString["ID"];//為了資安先把輸入內容轉為字串在轉型
                    int id;
                    if (int.TryParse(idText, out id))//能否轉型成數字不然不要了
                    {
                        var drAccounting = AccountingManager.GetAccounting(id, drUserInfo["UserID"].ToString());//能讀出來就去查資料庫(數字的部分

                        if (drAccounting == null)
                        {
                            this.ltMsg.Text = "資料不存在";
                            this.btnSave.Visible = false;
                            this.btnDelete.Visible = false;
                        }
                        else
                        {
                            if (drAccounting["UserID"].ToString() == drUserInfo["ID"].ToString())
                            {
                                this.ddlActType.SelectedValue = drAccounting["ActType"].ToString();
                                this.txtAmount.Text = drAccounting["Amount"].ToString();
                                this.txtCaption.Text = drAccounting["Caption"].ToString();
                                this.txtDesc.Text = drAccounting["Body"].ToString();
                            }

                        }
                    }
                    else
                    {
                        this.ltMsg.Text = "請確認輸入的值";
                        this.btnSave.Visible = false;
                        this.btnDelete.Visible = false;

                    }
                }
            }
        }

            //if (!this.IsPostBack)
            //{
            //    if (this.Request.QueryString["ID"] == null)
            //    {
            //        this.btnDelete.Visible = false;
            //    }
            //    else
            //    {
            //        this.btnDelete.Visible = true;
            //        string idText = this.Request.QueryString["ID"];
            //        int id;
            //        if (int.TryParse(idText, out id))
            //        {
            //            var drAccounting = AccountingManager.GetAccounting(id,drUserInfo["ID"].ToString());
                        
                        
            //            if (drAccounting == null)
            //            {

            //                this.ltMsg.Text = "資料不存在";
            //                this.btnSave.Visible = false;
            //                this.btnDelete.Visible = false;
            //            }
            //            else
            //            {
            //                this.ddlActType.SelectedValue = drAccounting["ActType"].ToString();
            //                this.txtAmount.Text = drAccounting["Amount"].ToString();
            //                this.txtCaption.Text = drAccounting["Caption"].ToString();
            //                this.txtDesc.Text = drAccounting["body"].ToString();
            //            }
            //        }
            //        else
            //        {
            //            this.ltMsg.Text = "ID is required.";
            //            this.btnSave.Visible = false;
            //            this.btnDelete.Visible = false;
            //        }



            //    }
                
            //    string account = this.Session["UserLoginInfo"] as string;//取值
            //    var dr = UserInfoManager.GetUserInfoByAccount(account);

            //    if (dr == null)
            //    {
            //        Response.Redirect("/Login.aspx");
            //        return;

            //    }

            //    string userID = dr["ID"].ToString();
            //    string actTypeText = this.ddlActType.SelectedValue;
            //    string amountText = this.txtAmount.Text;
            //    string caption = this.txtCaption.Text;
            //    string body = this.txtDesc.Text;

            //    int amount = Convert.ToInt32(amountText);
            //    int actType = Convert.ToInt32(actTypeText);

            //    string idText = this.Request.QueryString["ID"];


            //    //string idText = this.Request.QueryString["ID"];
            //   

            //    Response.Redirect("/SystemAdmin/AccountingList.aspx");



              

                //不知道哪裡錯了先註解
                //string userID = dr["ID"].ToString();
                //string actTypeText = this.ddlActType.SelectedValue;
                //string amountText = this.txtAmount.Text;
                //string caption = this.txtCaption.Text;
                //string body = this.txtDesc.Text;

                //int amount = Convert.ToInt32(amountText);
                //int actType = Convert.ToInt32(actTypeText);

                //AccountingManager.CreateAccounting(userID,caption,amount,actType,body);
                //Response.Redirect("/SystemAdmin/AccountingList.aspx");


      

        protected void btnSave_Click(object sender, EventArgs e)//第一次做好的存檔紐
        { 
            List<string> msgList = new List<string>();

            if(!this.CheckInput(out msgList))
            {
                this.ltMsg.Text = string.Join("<br/>", msgList);//做字串結合
                return;
            }
           

            string account = this.Session["UserLoginInfo"] as string;//1取值
            var dr = UserInfoManager.GetUserInfoByAccount(account);

            if (dr == null)
            {
                Response.Redirect("/Login.aspx");
                return;
            }
            
        
            string userID =dr["ID"].ToString();//2新增輸出欄位從INFO?
            string actTypeText = this.ddlActType.SelectedValue;
            string amountText = this.txtAmount.Text;
            string caption = this.txtCaption.Text;
            string body = this.txtDesc.Text;

            int amount = Convert.ToInt32(amountText);
            int actType = Convert.ToInt32(actTypeText);


            //4把資料放進資料庫
            AccountingManager.CreateAccounting(userID, caption, amount, actType, body);

            string idText = this.Request.QueryString["ID"];//6按存檔判斷是要新增還是編輯
            if (string.IsNullOrWhiteSpace(idText))
            {
                //空字串跑新增
                AccountingManager.CreateAccounting(userID, caption, amount, actType, body);
            }
            else
            {
                int id;
                if (int.TryParse(idText, out id))
                {
                    AccountingManager.UpdateAccounting(id,userID, caption, amount, actType, body);
                }
            }



            //4.1假設成功就導向別頁
            Response.Redirect("/SystemAdmin/AccountingList.aspx");





        }

        private bool CheckInput(out List<string> errorMsgList)//out回傳
        {
            List<string> msgList = new List<string>();
            //type
            if(this.ddlActType.SelectedValue !="0"&&this.ddlActType.SelectedValue != "1")//SelectedValue取得值是否為零或一
            {
                msgList.Add("Type must be 0 or 1.");
            }

            //Amount
            if(string.IsNullOrWhiteSpace(this.txtAmount.Text))
            
                msgList.Add("必填入數字");           

            else
            {
                int tempInt;
                if(!int.TryParse(this.txtAmount.Text,out tempInt))
                {
                    msgList.Add("填入數字必須為整數");
                }
                if(tempInt <0 || tempInt > 1000000)////3警告輸入者
                {
                    msgList.Add("填入數字必須在零到一百萬間");
                }

            }

            //這邊應該要再改但看起來沒問題啊
            errorMsgList = msgList;

            if (msgList.Count == 0)
                return true;
            else
                return false;

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string idText = this.Request.QueryString["ID"];
            if (string.IsNullOrWhiteSpace(idText))
                return;
            int id;

            if(int.TryParse(idText,out id))
            {
                AccountingManager.DeleteAccount(id);

            }
            Response.Redirect("/SystemAdmin/AccountingList.aspx");

        }
    }
}