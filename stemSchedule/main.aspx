﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main.aspx.cs" Inherits="stemSchedule.main" %>

<!DOCTYPE html>
<html lang="en">
	<head runat="server">
		<title>Schedule -- STEMschedule</title>
<script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>
<!-- Compiled and minified CSS -->
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/materialize/0.96.1/css/materialize.min.css">

<!-- Compiled and minified JavaScript -->
<script src="https://cdnjs.cloudflare.com/ajax/libs/materialize/0.96.1/js/materialize.min.js"></script>
		<style type="text/css">
			.auto-style1 {
				width: 100%;
			}
			.auto-style2 {
				width: 345px;
			}
			.auto-style3 {
				width: 361px;
			}
			.auto-style4 {
				width: 453px;
			}
		    .auto-style5 {
                width: 639px;
            }
		</style>
		</head>
 <nav class="red darken-4" role="navigation">
	<div class="nav-wrapper container"><a id="logo-container" href="#" class="brand-logo">STEMschedule</a>
	  <ul class="right hide-on-med-and-down">
		<li class="active"><a href="#">Schedule</a></li>
		<li><a href="settings.aspx">Settings</a></li>
		<li><a href="admin.aspx">Admin</a></li>
		<li><a href="#" runat="server" onserverclick="Button_Logout_Click">Logout</a></li>
	  </ul>

	  <ul id="nav-mobile" class="side-nav">
		<li><a href="#">Navbar Link</a></li>
	  </ul>
	  <a href="#" data-activates="nav-mobile" class="button-collapse"><i class="material-icons">menu</i></a>
	</div>
  </nav>

	<form id="form1" runat="server">
  <div class="section no-pad-bot" id="index-banner">
<div class="container">
	  <br><br>
	  <div class="row center">
	   
  
	      <asp:Label ID="Label_showSearch" runat="server" BackColor="#B71C1C" Font-Bold="True" Font-Size="Large" ForeColor="White" Text="Showing: ALL" Visible="False"></asp:Label>
	   
  
	  </div>
	  <div class="row center">
		<!-- table here -->
		  <asp:GridView ID="GridView1" runat="server" class ="striped" OnRowDataBound="GridView1_RowDataBound" UpdateMode="Conditional" PersistedSelection="true" AutoGenerateSelectButton="True" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
		  </asp:GridView>
	  </div>
	  
		<div class="left">
		
		  <!-- Dropdown Trigger -->
			
			<button id="Button_changePrivate" runat="server" onserverclick="Button_changePrivate_Click" class="waves-effect waves-light btn red darken-4" >Selected Private</button>
			
			<button id="btnExport" runat="server" onserverclick="ExportToExcel" class="waves-effect waves-light btn red darken-4" >Export To Excel</button>
			
		  </div>
		  <div style="float:right">
			  
  <button data-target="modal_search" class="btn modal-trigger red darken-4">Search</button></div>
  <!-- Dropdown Structure -->
  <ul id='dropdown1' class='dropdown-content'>
	<li><a href="#!">one</a></li>
	<li><a href="#!">two</a></li>
	<li class="divider"></li>
	<li><a href="#!">three</a></li>
	<li><a href="#!"><i class="material-icons">view_module</i>four</a></li>
	<li><a href="#!"><i class="material-icons">cloud</i>five</a></li>
  </ul>
	  </div>
	  
	  <br>
	  
	  <br>

	</div>
  </div>
<!-- Modal Structure modal modal-fixed-footer-->
  <!-- Add Class Modal Structuremodal modal-fixed-footer -->
  <div id="modal1" class="modal modal-fixed-footer">
	<div class="modal-content">
	  <h4>Modal Header</h4>
	  <p>A bunch of text</p>
		 <table class="auto-style1">
			  <tr>
				  <td class="auto-style2">CRN</td>
				  <td class="auto-style5">
					  <asp:TextBox ID="TextBox_CRN" runat="server"></asp:TextBox>
				  </td>
				  <td>&nbsp;</td>
				  <td class="auto-style3">Enrollment</td>
				  <td>
					  <asp:TextBox ID="TextBox_Enrollment" runat="server"></asp:TextBox>
				  </td>
				  <td>&nbsp;</td>
			  </tr>
			  <tr>
				  <td class="auto-style2">Faculty</td>
				  <td class="auto-style5">
					  <asp:TextBox ID="TextBox_Faculty" runat="server"></asp:TextBox>
				  </td>
				  <td>&nbsp;</td>
				  <td class="auto-style3">Year Taken</td>
				  <td>
					  <asp:DropDownList ID="DropDownList_year" runat="server" class="browser-default" Width="200px">
						  <asp:ListItem Selected="True" Value="Freshman">Freshman</asp:ListItem>
					      <asp:ListItem Value="Sophomore">Sophomore</asp:ListItem>
                          <asp:ListItem Value="Junior">Junior</asp:ListItem>
                          <asp:ListItem Value="Senior">Senior</asp:ListItem>
					  </asp:DropDownList>
				  </td>
				  <td>&nbsp;</td>
			  </tr>
			  <tr>
				  <td class="auto-style2">Class</td>
				  <td class="auto-style5">
					  <asp:DropDownList ID="DropDownList_class" runat="server" class="browser-default" Width="200px">
						  <asp:ListItem Selected="True">None</asp:ListItem>
					  </asp:DropDownList>
				  </td>
				  <td>&nbsp;</td>
				  <td class="auto-style3">Credits</td>
				  <td>
					  <asp:TextBox ID="TextBox_Credits" runat="server"></asp:TextBox>
				  </td>
				  <td>&nbsp;</td>
			  </tr>
			  <tr>
				  <td class="auto-style2">Term</td>
				  <td class="auto-style5">
					  <asp:DropDownList ID="DropDownList_term" runat="server" class="browser-default" Width="200px">
						  <asp:ListItem Selected="True" Value="1">Autumn</asp:ListItem>
					      <asp:ListItem Value="2">Winter</asp:ListItem>
                          <asp:ListItem Value="3">Spring</asp:ListItem>
                          <asp:ListItem Value="4">Summer</asp:ListItem>
					  </asp:DropDownList>
				  </td>
				  <td>&nbsp;</td>
				  <td class="auto-style3">&nbsp;</td>
				  <td>
					  &nbsp;</td>
				  <td>&nbsp;</td>
			  </tr>
			  <tr>
				  <td class="auto-style2">Classroom</td>
				  <td class="auto-style5">
					  <asp:TextBox ID="TextBox_Classroom" runat="server"></asp:TextBox>
				  </td>
				  <td>&nbsp;</td>
				  <td class="auto-style3">&nbsp;</td>
				  <td>
					  &nbsp;</td>
				  <td>&nbsp;</td>
			  </tr>
			  <tr>
				  <td class="auto-style2">Days</td>
				  <td class="auto-style5">
					  <asp:TextBox ID="TextBox_Days" runat="server"></asp:TextBox>
				  </td>
				  <td>&nbsp;</td>
				  <td class="auto-style3">&nbsp;</td>
				  <td>
					  &nbsp;</td>
				  <td>&nbsp;</td>
			  </tr>
			  <tr>
				  <td class="auto-style2">Start Time</td>
				  <td class="auto-style5">
					  <asp:TextBox ID="TextBox_StartTime" TextMode="Time" runat="server"></asp:TextBox>
				  </td>
				  <td>&nbsp;</td>
				  <td class="auto-style3">&nbsp;</td>
				  <td>
					  &nbsp;</td>
				  <td>&nbsp;</td>
			  </tr>
			  <tr>
				  <td class="auto-style2">End Time</td>
				  <td class="auto-style5">
					  <asp:TextBox ID="TextBox_EndTime" TextMode="Time" runat="server"></asp:TextBox>
				  </td>
				  <td>&nbsp;</td>
				  <td class="auto-style3">&nbsp;</td>
				  <td>
					  &nbsp;</td>
				  <td>&nbsp;</td>
			  </tr>
			  <tr>
				  <td class="auto-style2">&nbsp;</td>
				  <td class="auto-style5">
					  <asp:Button ID="Button_AddClass" runat="server" Text="Add Class" OnClick="Button_AddClass_Click1" />
				  </td>
				  <td>&nbsp;</td>
				  <td class="auto-style3">&nbsp;</td>
				  <td>&nbsp;</td>
				  <td>&nbsp;</td>
			  </tr>
		  </table>
	</div>
	<div class="modal-footer">
	  <a href="#!" class="modal-action modal-close waves-effect waves-green btn-flat ">Exit</a>
	</div>
  </div>

	  <br><br>

	</div>
  </div>


<div class="container">
	<div class="section">

	  <!--   Icon Section   -->
	  <div class="row">
		<div class="col s12">
		
		<asp:GridView ID="GridView2" runat="server" AutoGenerateSelectButton="True" OnSelectedIndexChanged="GridView2_SelectedIndexChanged" OnRowDataBound="GridView2_RowDataBound">
		    </asp:GridView>
 &nbsp;<div align="left">
	   <button data-target="modal1" class="btn modal-trigger red darken-4">Add</button>
		<button id="Button1" runat="server" onserverclick="Button_delete_Click" class="waves-effect waves-light btn red darken-4" >Delete</button>
		<button id="Button_Push" runat="server" OnServerClick="Button_Push_Click" class="btn modal-trigger red darken-4">Selected Public</button>
		<button data-target="modal2" class="btn modal-trigger red darken-4">Import</button>
  
	  
<!-- Add Class Modal Structure -->
  <div id="modal2" class="modal modal-fixed-footer" data-backdrop="static">
	<div class="modal-content">
	  <h4>Import Excel Spreadsheet</h4>
	  <p>A bunch of text</p>
		<div>
	<input type="file" id="myFile" name="myFile" />
<asp:Button ID="btnUpload" runat="server" Text="Upload"
			OnClick="btnUpload_Click" />
			<asp:Button ID="button_Save" runat="server" OnClick="button_Save_Click" Text="Save" />
<br />
<asp:Label ID="Label1" runat="server" Text="Has Header ?" />
<asp:RadioButtonList ID="rbHDR" runat="server">
	<asp:ListItem Text = "Yes" Value = "Yes" Selected = "True" >
	</asp:ListItem>
	<asp:ListItem Text = "No" Value = "No"></asp:ListItem>
</asp:RadioButtonList>
<asp:GridView ID="GridView3" runat="server">
</asp:GridView>
	</div>
	</div>
	<div class="modal-footer">
	  <a href="#!" class="modal-action modal-close waves-effect waves-green btn-flat ">Close</a>
	</div>
  </div>

	<!-- Search Modal -->
  <div id="modal_search" class="modal modal-fixed-footer" data-backdrop="static">
	<div class="modal-content">
		<table class="auto-style1">
				<tr>
					<td class="auto-style4">
						Search by CRN</td>
					<td class="auto-style2">
						<asp:TextBox ID="TextBox_searchCRN" runat="server" Width="201px"></asp:TextBox>
					</td>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td class="auto-style4">
						Search by Instructor</td>
					<td class="auto-style2">
			  
  <asp:DropDownList ID="DropDownList_searchInstructor" runat="server" class="browser-default" Width="200px">
				  <asp:ListItem Selected="True" Value="0">All</asp:ListItem>
				  <asp:ListItem Value="1">Freshman</asp:ListItem>
				  <asp:ListItem Value="2">Sophomore</asp:ListItem>
				  <asp:ListItem Value="3">Junior</asp:ListItem>
				  <asp:ListItem Value="4">Senior</asp:ListItem>
			  </asp:DropDownList>
			  
					</td>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td class="auto-style4">
						<asp:RadioButtonList ID="RadioButtonList_ShowClasses" runat="server">
							<asp:ListItem Selected="True">Show &quot;Everyone&#39;s&quot; Classes</asp:ListItem>
							<asp:ListItem>Show Only My Classes</asp:ListItem>
							<asp:ListItem>Show Only Conflicts</asp:ListItem>
						</asp:RadioButtonList>
					</td>
					<td class="auto-style2">&nbsp;</td>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td class="auto-style4">Show Major</td>
					<td class="auto-style2">
  <asp:DropDownList ID="DropDownList_ShowDept" runat="server" class="browser-default" Width="200px">
	  <asp:ListItem Selected="True">Show Only</asp:ListItem>
				  
			  </asp:DropDownList>
			  
					</td>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td class="auto-style4">Show Classroom</td>
					<td class="auto-style2">&nbsp;</td>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td class="auto-style4">Show Year</td>
					<td class="auto-style2">
			  
  <asp:DropDownList ID="DropDownList_ShowYear" runat="server" class="browser-default" Width="200px">
				  <asp:ListItem Selected="True" Value="0">All</asp:ListItem>
				  <asp:ListItem Value="1">Freshman</asp:ListItem>
				  <asp:ListItem Value="2">Sophomore</asp:ListItem>
				  <asp:ListItem Value="3">Junior</asp:ListItem>
				  <asp:ListItem Value="4">Senior</asp:ListItem>
			  </asp:DropDownList>
			  
					</td>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td class="auto-style4">Show Day of Week</td>
					<td class="auto-style2">
			  
						&nbsp;</td>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td class="auto-style4">&nbsp;</td>
					<td class="auto-style2">
						<button id="Button_ApplySort" runat="server" OnServerClick="Button_ApplySort_Click" class="btn modal-trigger red darken-4">Search</button>
					</td>
					<td>&nbsp;</td>
				</tr>
			</table>
		</div>
	  
	
	</div>
	            <br />
	</div>
	
			&nbsp;</div>
		</div>

		

		
	  </div>

	</div>
	<br><br>

	<div class="section">

	</div>
  </div>

  <footer class="page-footer grey">
	<div class="container">
	  <div class="row">
		<div class="col l6 s12">
		  <h5 class="white-text">Company Bio</h5>
		  <p class="grey-text text-lighten-4">We are a team of college students</p>


		</div>
		<div class="col l3 s12">
		  <h5 class="white-text">Settings</h5>
		  <ul>
			<li><a class="white-text" href="#!">Link 1</a></li>
			<li><a class="white-text" href="#!">Link 2</a></li>
			<li><a class="white-text" href="#!">Link 3</a></li>
			<li><a class="white-text" href="#!">Link 4</a></li>
		  </ul>
		</div>
		<div class="col l3 s12">
		  <h5 class="white-text">Connect</h5>
		  <ul>
			<li><a class="white-text" href="#!">Link 1</a></li>
			<li><a class="white-text" href="#!">Link 2</a></li>
			<li><a class="white-text" href="#!">Link 3</a></li>
			<li><a class="white-text" href="#!">Link 4</a></li>
		  </ul>
		</div>
	  </div>
	</div>
	<div class="footer-copyright">
	  <div class="container">
	  For Educational Purposes Only
	  </div>
	</div>
  </footer>
		</form>


<script>
$(document).ready(function() {
  // the "href" attribute of .modal-trigger must specify the modal ID that wants to be triggered
  $('.modal-trigger').leanModal();
});
</script>