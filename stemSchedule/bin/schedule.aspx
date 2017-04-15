<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="schedule.aspx.cs" Inherits="stemSchedule.schedule" %>


<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>


<!DOCTYPE html>
<html lang="en">
	<head runat="server">
		<meta charset="utf-8">
		<meta http-equiv="X-UA-Compatible" content="IE=edge">
		<title>STEMschedule - Schedule</title>
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
			.auto-style25 {
				margin-right: 0px;
			}
			.auto-style26 {
				width: 397px;
			}
		</style>

		
	</head>
	<body>
		<form id="form2" runat="server">
		<div class="container">
			<div class="header">
				<h1 class="header-heading">&nbsp;<strong>STEMschedule</strong></h>
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
					<asp:SqlDataSource ID="schedule_SqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:RegistrationConnectionString %>" SelectCommand="SELECT [Days], [StartTime], [EndTime], [Classroom], [Department], [CourseNumber], [Instructor], [CRN] FROM [schedule]"></asp:SqlDataSource>
					<asp:SqlDataSource ID="UserData_SqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:RegistrationConnectionString %>" SelectCommand="SELECT * FROM [UserData]"></asp:SqlDataSource>
					<asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:RegistrationConnectionString %>" SelectCommand="SELECT * FROM [classrooms]"></asp:SqlDataSource>
					<asp:SqlDataSource ID="SqlDataSource_courses" runat="server" ConnectionString="<%$ ConnectionStrings:RegistrationConnectionString %>" SelectCommand="SELECT [classes] FROM [classes] ORDER BY [classes]"></asp:SqlDataSource>
					<asp:SqlDataSource ID="SqlDataSource_dept" runat="server" ConnectionString="<%$ ConnectionStrings:RegistrationConnectionString %>" SelectCommand="SELECT * FROM [department]"></asp:SqlDataSource>
					<asp:SqlDataSource ID="SqlDataSource_Rooms" runat="server" ConnectionString="<%$ ConnectionStrings:RegistrationConnectionString %>" SelectCommand="SELECT DISTINCT [room] FROM [classrooms]" OnSelecting="SqlDataSource_Rooms_Selecting"></asp:SqlDataSource>
					<br/>
					<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="auto-style25" DataSourceID="schedule_SqlDataSource" BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px" CellPadding="4" CellSpacing="2" ForeColor="Black">
						<Columns>
							<asp:BoundField DataField="CRN" HeaderText="CRN" SortExpression="CRN" />
							<asp:BoundField DataField="Days" HeaderText="Days" SortExpression="Days" />
							<asp:BoundField DataField="StartTime" HeaderText="Start" SortExpression="StartTime" />
							<asp:BoundField DataField="EndTime" HeaderText="End" SortExpression="EndTime" />
							<asp:BoundField DataField="Classroom" HeaderText="Room" SortExpression="Classroom" />
							<asp:BoundField DataField="Department" HeaderText="Dept" SortExpression="Department" />
							<asp:BoundField DataField="CourseNumber" HeaderText="Course#" SortExpression="CourseNumber" />
							<asp:BoundField DataField="Instructor" HeaderText="Instructor" SortExpression="Instructor" />
						</Columns>
						<FooterStyle BackColor="#CCCCCC" />
						<HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
						<PagerStyle BackColor="#CCCCCC" ForeColor="Black" HorizontalAlign="Left" />
						<RowStyle BackColor="White" />
						<SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
						<SortedAscendingCellStyle BackColor="#F1F1F1" />
						<SortedAscendingHeaderStyle BackColor="#808080" />
						<SortedDescendingCellStyle BackColor="#CAC9C9" />
						<SortedDescendingHeaderStyle BackColor="#383838" />
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
								<asp:DropDownList ID="DropDownList_instructor" runat="server" AutoPostBack="True" DataSourceID="UserData_SqlDataSource" DataTextField="UserName" DataValueField="UserName" OnSelectedIndexChanged="DropDownList_instructor_SelectedIndexChanged">
								</asp:DropDownList>
							</td>
						</tr>
						<tr>
							<td class="auto-style21">Show Room:</td>
							<td class="auto-style22">
								<asp:DropDownList ID="DropDownList_rooms" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource_Rooms" DataTextField="room" DataValueField="room" OnSelectedIndexChanged="DropDownList_rooms_SelectedIndexChanged">
								</asp:DropDownList>
							</td>
							<td class="auto-style23"></td>
							<td class="auto-style24"></td>
						</tr>
						</table>
								<asp:Button ID="button_addAppear" runat="server" Text="Add Class" OnClick="button_addAppear_Click" />
								<asp:Button ID="Button_showDelete" runat="server" Text="Delete Class" OnClick="Button_showDelete_Click" />
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
									<td>CRN</td>
									<td>
										<asp:TextBox ID="TextBoxCRN" runat="server"></asp:TextBox>
									</td>
									<td>&nbsp;</td>
									<td>&nbsp;</td>
								</tr>
							</table>
						</asp:Panel>
					</asp:Panel>
					<asp:Panel ID="Panel_delete" runat="server">
						<table class="auto-style1">
							<tr>
								<td class="auto-style26">
									<asp:Label ID="Label1" runat="server" Text="Enter class CRN to delete:"></asp:Label>
								</td>
								<td>
									<asp:TextBox ID="TextBox_delete" runat="server"></asp:TextBox>
								</td>
							</tr>
							<tr>
								<td class="auto-style26">
									<asp:Label ID="Label2" runat="server" Text="Confirm CRN(Enter again):"></asp:Label>
								</td>
								<td>
									<asp:TextBox ID="TextBox_confirmDelete" runat="server"></asp:TextBox>
								</td>
							</tr>
							<tr>
								<td class="auto-style26">&nbsp;</td>
								<td>
									<asp:Button ID="Button_delete" runat="server" Text="Delete" OnClick="Button_delete_Click1" />
									<asp:Button ID="Button_deleteHide" runat="server" Text="Hide" OnClick="Button_deleteHide_Click" />
								</td>
							</tr>
						</table>
					</asp:Panel>
					<br/>
					<asp:Panel ID="Panel_confirmAdd" runat="server" Visible="False">
						<table class="auto-style1">
							<tr>
								<td>
									<asp:Label ID="Label_conflict" runat="server" Text="Conflict: asdfasfd and asdfasdfasdfas"></asp:Label>
									<asp:Button ID="Button_showConflict" runat="server" OnClick="Button_showConflict_Click" Text="Show" />
								</td>
							</tr>
							<tr>
								<td>
									<asp:Button ID="Button_confirmAdd" runat="server" OnClick="Button_confirmAdd_Click" Text="Confirm" />
									<asp:Button ID="Button_dontAdd" runat="server" OnClick="Button_dontAdd_Click" Text="Do Not Add" />
								</td>
							</tr>
						</table>
					</asp:Panel>
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
