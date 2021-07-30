<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountingList.aspx.cs" Inherits="AccountingNote.SysteamAdmin.AccountingList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
      
     <table>
          <tr>
                 <td colspan="2">
                     <h1>流水帳管理系統 後台</h1>
                 </td>
          </tr>
          <tr>
           <td>
             <a href="UserInfo.aspx">使用者資訊</a><br />
             <a href="AccountingList.aspx">流水帳管理</a>
            </td>
           <td>
                <asp:Button ID="btnCreate" runat="server" Text="Add Accounting" OnClick="btnCreate_Click" />
              
               
               <asp:GridView ID="gvAccountList" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvAccountList_RowDataBound1" >

                    <Columns>
                        <asp:BoundField DataField="Caption" HeaderText="標題" />
                        <asp:BoundField DataField="Amount" HeaderText="金額" />
                       <asp:TemplateField HeaderText="In/Out">

                        <ItemTemplate>
                       <%--   <%# ((int)Eval("ActType") == 0) ? "支出" : "收入" %>--%>

                           <asp:Literal runat="server" ID="ltlActType"></asp:Literal>
                            <asp:Label ID="lblActType" runat="server" Text=""></asp:Label>


                        </ItemTemplate>
                           
                           </asp:TemplateField>


                        <asp:BoundField DataField="CreateDate" DataFormatString="{0:yyyy-Mm-dd}" HeaderText="建立日期" />
                       
                        <asp:TemplateField HeaderText="Act">
                            <ItemTemplate>
                                <a href="/SystemAdmin/AccountingDetail.aspx?ID=<%#Eval("ID")%>">Edit</a>
                              
                            </ItemTemplate>


                        </asp:TemplateField>
                       
                    </Columns>


                </asp:GridView>

                <asp:PlaceHolder ID="plcNoData" runat="server" Visible="false">
                    <p style="color:brown";">
                        No data in Accounting Note.
                   <%--做有沒有資料的檢查--%>

                    </p>

                </asp:PlaceHolder>
               
            </td>
          </tr>
        </table>


    </form>
</body>
</html>
