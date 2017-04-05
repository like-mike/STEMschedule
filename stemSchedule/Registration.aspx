<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="stemSchedule.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
        .auto-style2 {
            height: 23px;
        }
        .auto-style3 {
            width: 190px;
        }
        .auto-style4 {
            height: 23px;
            width: 190px;
        }
        .auto-style5 {
            width: 125px;
        }
        .auto-style6 {
            height: 23px;
            width: 125px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <table class="auto-style1">
            <tr>
                <td class="auto-style5">User Name:</td>
                <td class="auto-style3">
                    <asp:TextBox ID="UNTextBox" runat="server"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style5">E-Mail</td>
                <td class="auto-style3">
                    <asp:TextBox ID="emailTextBox" runat="server"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style6">Password</td>
                <td class="auto-style4">
                    <asp:TextBox ID="passTextBox" runat="server"></asp:TextBox>
                </td>
                <td class="auto-style2"></td>
            </tr>
            <tr>
                <td class="auto-style6">Confirm Password</td>
                <td class="auto-style4">
                    <asp:TextBox ID="cpassTextBox" runat="server"></asp:TextBox>
                </td>
                <td class="auto-style2"></td>
            </tr>
            <tr>
                <td class="auto-style6"></td>
                <td class="auto-style4">
                    <asp:Button ID="submitButton" runat="server" Text="Submit" OnClick="submitButton_Click" />
                </td>
                <td class="auto-style2"></td>
            </tr>
            <tr>
                <td class="auto-style5">&nbsp;</td>
                <td class="auto-style3">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style5">&nbsp;</td>
                <td class="auto-style3">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
