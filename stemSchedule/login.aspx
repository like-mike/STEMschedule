<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="stemSchedule.login" %>


<!DOCTYPE html>
<html lang="en">
	<head runat="server">
		<meta charset="utf-8">
		<meta http-equiv="X-UA-Compatible" content="IE=edge">
		<title>STEMschedule - Login</title>
		<meta name="viewport" content="width=device-width, initial-scale=1">
		<meta name="Description" lang="en" content="ADD SITE DESCRIPTION">
		<meta name="author" content="ADD AUTHOR INFORMATION">
		<meta name="robots" content="index, follow">

		<!-- icons -->
		<link rel="apple-touch-icon" href="assets/img/apple-touch-icon.png">
		<link rel="shortcut icon" href="favicon.ico">

		<!-- Override CSS file - add your own CSS rules -->
		<link rel="stylesheet" href="Content/styles.css">
		<style type="text/css">
			.auto-style1 {
				width: 100%;
			}
			.auto-style2 {
				width: 76px;
			}
			.auto-style4 {
				width: 128px;
			}
		</style>
	</head>
	<body>
		<form id="form2" runat="server">
		<div class="container">
			<div class="header">
				<h1 class="header-heading">STEMscheduletest</h1>
			</div>
			<div class="nav-bar">
				<ul class="nav">
					
				</ul>
			</div>
			<div class="content">
				<div class="main">

					
					<hr>
					<br/>
					<br/>
					<br/>
					<table class="auto-style1">
						<tr>
							<td class="auto-style2">
					<asp:Label ID="Label1" runat="server" Text="Username: "></asp:Label>
							</td>
							<td class="auto-style4">
					<asp:TextBox ID="UNTextBox" runat="server"></asp:TextBox>
							</td>
							<td>
								&nbsp;</td>
						</tr>
						<tr>
							<td class="auto-style2">
					<asp:Label ID="Label2" runat="server" Text="Password:"></asp:Label>
							</td>
							<td class="auto-style4">
					<asp:TextBox ID="passTextBox" runat="server" TextMode="Password"></asp:TextBox>
							</td>
							<td>
								&nbsp;</td>
						</tr>
						<tr>
							<td class="auto-style2">
					<asp:Button ID="Button_Login" runat="server" Text="Login" OnClick="Button1_Click" />
							</td>
							<td class="auto-style4">&nbsp;</td>
							<td>&nbsp;</td>
						</tr>
					</table>
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

					
				<div class="container">

	
	  
	</div>
  </div>
  
</div>

					
			<div class="footer">
				© Copyright 2017
				</div>
		</form>
	</body>
</html>
