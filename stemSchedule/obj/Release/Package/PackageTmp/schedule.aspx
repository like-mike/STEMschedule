<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="schedule.aspx.cs" Inherits="stemSchedule.schedule" %>


<!DOCTYPE html>
<html lang="en">
	<head runat="server">
		<meta charset="utf-8">
		<meta http-equiv="X-UA-Compatible" content="IE=edge">
		<title>One-column fixed-width responsive layout</title>
		<meta name="viewport" content="width=device-width, initial-scale=1">
		<meta name="Description" lang="en" content="ADD SITE DESCRIPTION">
		<meta name="author" content="ADD AUTHOR INFORMATION">
		<meta name="robots" content="index, follow">


		<link rel="stylesheet" href="Content/styles.css">
		<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>
		<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>

		<!-- icons -->
		<link rel="apple-touch-icon" href="assets/img/apple-touch-icon.png">
		<link rel="shortcut icon" href="favicon.ico">

		<!-- Override CSS file - add your own CSS rules -->

		
		<style type="text/css">
			.auto-style1 {
				width: 100%;
			}
			.auto-style13 {
				width: 186px;
			}
			.auto-style14 {
				width: 191px;
			}
			.auto-style15 {
				width: 214px;
			}
			.auto-style16 {
				width: 379px;
			}
			.auto-style17 {
				width: 186px;
				height: 55px;
			}
			.auto-style18 {
				width: 191px;
				height: 55px;
			}
			.auto-style19 {
				width: 214px;
				height: 55px;
			}
			.auto-style20 {
				width: 379px;
				height: 55px;
			}
			.auto-style21 {
				width: 186px;
				height: 67px;
			}
			.auto-style22 {
				width: 191px;
				height: 67px;
			}
			.auto-style23 {
				width: 214px;
				height: 67px;
			}
			.auto-style24 {
				width: 379px;
				height: 67px;
			}
		</style>

		
	</head>
	<body>
		<form id="form2" runat="server">
		<div class="container">
			<div class="header">
				<h1 class="header-heading">&nbsp;<strong>STEMschedule</strong></h1>
			</div>
			<div class="nav-bar">
				<ul class="nav">
					<li>
						<asp:Button ID="Button_Settings" runat="server" BackColor="Black" BorderColor="Black" BorderStyle="Solid" ForeColor="White" Text="Settings" OnClick="Button_Settings_Click" />
					</li>
					<li>
					<asp:Button ID="Button_Logout" runat="server" Text="Logout" OnClick="Button_Logout_Click" BorderStyle="Solid" BackColor="Black" BorderColor="Black" ForeColor="White" />
						
						<asp:Button ID="Button_Admin" runat="server" BackColor="Black" BorderColor="Black" BorderStyle="Solid" ForeColor="White" Text="Admin" OnClick="Button_Settings_Click" />
						
					</li>
						
				</ul>
			</div>
			<div class="content">
				<div class="main">

					<h2>Schedule</h2>
					<hr>
					<asp:SqlDataSource ID="schedule_SqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:stemscheduleConnectionString %>" SelectCommand="SELECT [Days], [StartTime], [EndTime], [Classroom], [Department], [CourseNumber], [Instructor] FROM [schedule]" ProviderName="<%$ ConnectionStrings:stemscheduleConnectionString.ProviderName %>"></asp:SqlDataSource>
					<asp:SqlDataSource ID="UserData_SqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:stemscheduleConnectionString %>" SelectCommand="SELECT * FROM [userdata]" ProviderName="<%$ ConnectionStrings:stemscheduleConnectionString.ProviderName %>"></asp:SqlDataSource>
					<asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:RegistrationConnectionString %>" SelectCommand="SELECT * FROM [classrooms]"></asp:SqlDataSource>
					<asp:SqlDataSource ID="SqlDataSource_courses" runat="server" ConnectionString="<%$ ConnectionStrings:RegistrationConnectionString %>" SelectCommand="SELECT [classes] FROM [classes] ORDER BY [classes]"></asp:SqlDataSource>
					<asp:SqlDataSource ID="SqlDataSource_dept" runat="server" ConnectionString="<%$ ConnectionStrings:RegistrationConnectionString %>" SelectCommand="SELECT * FROM [department]"></asp:SqlDataSource>
					<br/>
					<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="schedule_SqlDataSource" AllowPaging="True" AllowSorting="True">
					</asp:GridView>
					<table class="auto-style1">
						<tr>
							<td class="auto-style17">Show Only:</td>
							<td class="auto-style18">
								<asp:DropDownList ID="DropDownList_SortBy" runat="server" AutoPostBack="True" Height="16px" OnSelectedIndexChanged="DropDownList_SortBy_SelectedIndexChanged">
									<asp:ListItem>ALL</asp:ListItem>
									<asp:ListItem Value="My Classes"></asp:ListItem>
									<asp:ListItem>CSC</asp:ListItem>
									<asp:ListItem>EE</asp:ListItem>
									<asp:ListItem></asp:ListItem>
								</asp:DropDownList>
							</td>
							<td class="auto-style19">By Instructor:</td>
							<td class="auto-style20">
								<asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource2" DataTextField="UserName" DataValueField="UserName">
								</asp:DropDownList>
							</td>
						</tr>
						<tr>
							<td class="auto-style21">&nbsp;Sort By:</td>
							<td class="auto-style22"></td>
							<td class="auto-style23"></td>
							<td class="auto-style24"></td>
						</tr>
						<tr>
							<td class="auto-style13">&nbsp;</td>
							<td class="auto-style14">
								&nbsp;</td>
							<td class="auto-style15">&nbsp;</td>
							<td class="auto-style16">&nbsp;</td>
						</tr>
					</table>
								<asp:Button ID="button_addAppear" runat="server" Text="Add Class" OnClick="button_addAppear_Click" />
								<asp:Button ID="Button3" runat="server" Text="Delete Class" />
					<br/>
					<asp:Panel ID="Panel1" runat="server">
						<asp:Panel ID="Panel_addClass" runat="server" Visible="False">
							<table id="addTable1" class="auto-style1">
								<tr>
									<td>Start Time</td>
									<td>
										<asp:TextBox ID="TextBox_startTime" runat="server" TextMode="Time"></asp:TextBox>
									</td>
									<td>&nbsp;</td>
									<td>Classroom</td>
									<td>
										<asp:DropDownList ID="DropDownList_classroom" runat="server" DataSourceID="SqlDataSource3" DataTextField="room" DataValueField="room">
										</asp:DropDownList>
									</td>
									<td>&nbsp;</td>
								</tr>
								<tr>
									<td>End Time</td>
									<td>
										<asp:TextBox ID="TextBox_endTime" runat="server" TextMode="Time"></asp:TextBox>
									</td>
									<td>&nbsp;</td>
									<td>Department</td>
									<td>
										<asp:DropDownList ID="DropDownList_dept" runat="server" DataSourceID="SqlDataSource_dept" DataTextField="department" DataValueField="department">
										</asp:DropDownList>
									</td>
									<td>&nbsp;</td>
								</tr>
								<tr>
									<td>Days</td>
									<td>
										<asp:DropDownList ID="DropDownList_Days" runat="server" AutoPostBack="True">
											<asp:ListItem>MWF</asp:ListItem>
											<asp:ListItem>TTH</asp:ListItem>
											<asp:ListItem>WF</asp:ListItem>
											<asp:ListItem>M</asp:ListItem>
											<asp:ListItem>T</asp:ListItem>
											<asp:ListItem>W</asp:ListItem>
											<asp:ListItem>Th</asp:ListItem>
											<asp:ListItem>F</asp:ListItem>
										</asp:DropDownList>
									</td>
									<td>&nbsp;</td>
									<td>Course Number</td>
									<td>
										<asp:DropDownList ID="DropDownList_courses" runat="server" DataSourceID="SqlDataSource_courses" DataTextField="classes" DataValueField="classes">
										</asp:DropDownList>
									</td>
									<td>&nbsp;</td>
								</tr>
								<tr>
									<td>
										<asp:Button ID="Button_addClass" runat="server" OnClick="Button_addClass_Click" Text="Submit" />
									</td>
									<td>
										<asp:Button ID="Button_AddHide" runat="server" OnClick="Button_AddHide_Click1" Text="Hide" />
									</td>
									<td>&nbsp;</td>
									<td>&nbsp;</td>
									<td>&nbsp;</td>
									<td>&nbsp;</td>
									<td>&nbsp;</td>
								</tr>
							</table>
						</asp:Panel>
					</asp:Panel>
					<asp:Label ID="Label_errorConflict" runat="server" ForeColor="Red" Text="Cannot add class. Conflict with " Visible="False"></asp:Label>
					<br/>
					<br/>
					<div class="container">

	
	  
	</div>
  </div>
  
</div>

					
			<div class="footer">
				© Copyright 2016
				</div>
		</form>
	</body>
</html>
