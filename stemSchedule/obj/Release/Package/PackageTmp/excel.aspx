<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="excel.aspx.cs" Inherits="stemSchedule.excel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />
    <h3>Import / Export database data from/to Excel.</h3>
<div>
    <table>
        <tr>
            <td>Select File : </td>
            <td>
                <asp:FileUpload ID="FileUpload1" runat="server" />
                </td>
            <td>
                <asp:Button ID="btnImport" runat="server" Text="Import Data" OnClick="btnImport_Click" />
            </td>
        </tr>
    </table>
    <div>
        <br />
        <asp:Label ID="lblMessage" runat="server"  Font-Bold="true" />
        <br />
        <asp:GridView ID="GridView1" runat="server">
        </asp:GridView>
        <br />
        <asp:Button ID="btnExport" runat="server" Text="Export Data" OnClick="btnExport_Click" />
    </div>
</div>
    </form>
    </body>
</html>
