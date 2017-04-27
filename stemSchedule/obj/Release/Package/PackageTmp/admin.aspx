<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="admin.aspx.cs" Inherits="stemSchedule.admin" %>


<!DOCTYPE html>
<html lang="en">
	<head runat="server">
		<meta charset="utf-8">
		<meta http-equiv="X-UA-Compatible" content="IE=edge">
		<title>STEMschedule - Admin</title>
		<meta name="viewport" content="width=device-width, initial-scale=1">
		<meta name="Description" lang="en" content="ADD SITE DESCRIPTION">
		<meta name="author" content="ADD AUTHOR INFORMATION">
		<meta name="robots" content="index, follow">

		<!-- icons -->
		<link rel="apple-touch-icon" href="assets/img/apple-touch-icon.png">
		<link rel="shortcut icon" href="favicon.ico">

		<!-- Override CSS file - add your own CSS rules -->
		<link rel="stylesheet" href="assets/css/styles.css">
		<style type="text/css">

		.auto-style1 {
			width: 100%;
		}
		.auto-style5 {
			width: 125px;
		}
		.auto-style6 {
			height: 23px;
			width: 125px;
		}
		.auto-style2 {
			height: 23px;
		}
			.auto-style8 {
				width: 355px;
			}
			.auto-style9 {
				height: 23px;
				width: 355px;
			}
			.auto-style10 {
				width: 465px;
			}
			.auto-style11 {
				width: 290px;
			}
			</style>
	</head>
	<body>
		<form id="form2" runat="server">
		<div class="container">
			<div class="header">
				<h1 class="header-heading">STEMschedule</h1>
			</div>
			<div class="nav-bar">
				<ul class="nav">
					
					<li>
						<asp:Button ID="Button_Settings" runat="server" BackColor="Black" BorderColor="Black" BorderStyle="Solid" ForeColor="White" Text="Schedule" OnClick="Button_Settings_Click" />
					</li>
					<asp:Button ID="Button_Logout" runat="server" Text="Logout" OnClick="Button_Logout_Click" BorderStyle="Solid" BackColor="Black" BorderColor="Black" ForeColor="White" />
						
				</ul>
			</div>
			<div class="content">
				<div class="main">

					
					<hr>
					<asp:SqlDataSource ID="SqlDataSource_userData" runat="server" ConnectionString="<%$ ConnectionStrings:RegistrationConnectionString %>" SelectCommand="SELECT [UserName], [Email] FROM [UserData]"></asp:SqlDataSource>
					<br/>
					<br/>
					<br/>
					<asp:Panel ID="Panel_user" runat="server">
						<asp:GridView ID="GridView_users" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource_userData">
							<Columns>
								<asp:CommandField ShowSelectButton="True" />
								<asp:BoundField DataField="UserName" HeaderText="UserName" SortExpression="UserName" />
								<asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
							</Columns>
						</asp:GridView>
						<asp:Button ID="Button_showAdd" runat="server" OnClick="Button_showAdd_Click" Text="Add User" />
						<asp:Button ID="Button_showDelete" runat="server" Text="Delete User" OnClick="Button_showDelete_Click1" />
						<br />
						<asp:Panel ID="Panel_addUser" runat="server" Visible="False">
							<table class="auto-style1">
								<tr>
									<td class="auto-style5">User Name:</td>
									<td class="auto-style8">
										<asp:TextBox ID="UNTextBox" runat="server"></asp:TextBox>
									</td>
									<td>&nbsp;</td>
								</tr>
								<tr>
									<td class="auto-style5">E-Mail</td>
									<td class="auto-style8">
										<asp:TextBox ID="emailTextBox" runat="server"></asp:TextBox>
									</td>
									<td>&nbsp;</td>
								</tr>
								<tr>
									<td class="auto-style6">Password</td>
									<td class="auto-style9">
										<asp:TextBox ID="passTextBox" runat="server"></asp:TextBox>
									</td>
									<td class="auto-style2"></td>
								</tr>
								<tr>
									<td class="auto-style6">Confirm Password</td>
									<td class="auto-style9">
										<asp:TextBox ID="cpassTextBox" runat="server"></asp:TextBox>
									</td>
									<td class="auto-style2"></td>
								</tr>
								<tr>
									<td class="auto-style6"></td>
									<td class="auto-style9">
										<asp:Button ID="submitButton" runat="server" OnClick="submitButton_Click" Text="Submit" />
										<asp:Button ID="Button_buttonHide" runat="server" OnClick="Button_buttonHide_Click" Text="Hide" />
									</td>
									<td class="auto-style2">&nbsp;</td>
								</tr>
							</table>
						</asp:Panel>
						<asp:Panel ID="Panel_delete" runat="server">
							<table class="auto-style1">
								<tr>
									<td class="auto-style10">Enter User Name to Delete</td>
									<td class="auto-style11">
										<asp:TextBox ID="TextBox_delete1" runat="server"></asp:TextBox>
									</td>
									<td>&nbsp;</td>
								</tr>
								<tr>
									<td class="auto-style10">Confirm User Name(Enter again)</td>
									<td class="auto-style11">
										<asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
									</td>
									<td>&nbsp;</td>
								</tr>
								<tr>
									<td class="auto-style10">
										<asp:Label ID="Label_deleteError" runat="server" ForeColor="Red" Text="Error"></asp:Label>
									</td>
									<td class="auto-style11">
										<asp:Button ID="Button_delete" runat="server" Text="Delete" OnClick="Button_delete_Click1" />
										<asp:Button ID="Button_hide" runat="server" Text="Hide" />
									</td>
									<td>&nbsp;</td>
								</tr>
							</table>
						</asp:Panel>
						<br />
					</asp:Panel>
					<br/>
					<br/>
					<br/>
					<br/>
					<br/>
					<br/>
					<br/>
					<br/>
					<br/>
					<br/>
					<br/>

					</form>
				<div class="container">

	
	  
	</div>
  </div>
  
</div>

					
			<div class="footer">
				© Copyright 2017
				</div>
</html>
