<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main.aspx.cs" Inherits="stemSchedule.main" %>

<!DOCTYPE html>
<html lang="en">
	<head runat="server">
		<title>STEMschedule</title>
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
			.auto-style5 {
				width: 639px;
			}
			.auto-style6 {
				width: 614px;
			}
			.auto-style7 {
				width: 606px;
			}
			.auto-style8 {
				width: 314px;
			}
			.auto-style9 {
				width: 98px;
			}
			.auto-style10 {
                width: 389px;
            }
			</style>
		</head>



<!-- Dropdown Structure -->
<ul id="dropdown_admin" class="dropdown-content">
  <li><a href="#" runat="server" onserverclick="Button_AddUserShow_Click">Add User</a></li>
  <li><a href="#" runat="server" onserverclick="Button_DeleteUserShow_Click">Delete User</a></li>
	<li><a href="#" runat="server" onserverclick="Button_ChangePwShow_Click">Change Password</a></li>
	<li class="divider"></li>
	<li><a href="#" runat="server" onserverclick="Button_CopyShow_Click">Copy Classes</a></li>
	<li><a href="#" runat="server" onserverclick="Button_DeleteClassesShow_Click">Delete Classes</a></li>
	
	
</ul>

	<!-- Dropdown Structure -->
<ul id="dropdown_settings" class="dropdown-content">
  <li><a href="#" runat="server" onserverclick="Button_AddMajorShow_Click">Add Major</a></li>
	<li><a href="#" runat="server" onserverclick="Button_DeleteMajorShow_Click">Delete Major</a></li>
	<li class="divider"></li>
  <li><a href="#" runat="server" onserverclick="Button_AddInstructorShow_Click">Add Instructor</a></li>
	<li><a href="#" runat="server" onserverclick="Button_DeleteInstructorShow_Click">Delete Instructor</a></li>
	<li class="divider"></li>
	<li><a href="#" runat="server" onserverclick="Button_AddRoomShow_Click">Add Room</a></li>
	<li><a href="#" runat="server" onserverclick="Button_DeleteRoomShow_Click">Delete Room</a></li>
	<li class="divider"></li>
	
	<li><a href="#" runat="server" onserverclick="Button_AddClassShow_Click">Add Class</a></li>
	<li><a href="#" runat="server" onserverclick="Button_DeleteClassShow_Click">Delete Class</a></li>

</ul>


 <nav class="red darken-4" role="navigation">
	<div class="nav-wrapper container"><a id="logo-container" href="#" class="brand-logo">STEMschedule</a>
	  <ul class="right hide-on-med-and-down">
		<li class="active"><a href="#">Schedule</a></li>
		<li><a class="dropdown-button" href="#!" data-constrainwidth="false" data-activates="dropdown_admin">Admin</a></li>
		<li><a class="dropdown-button" href="#!" data-constrainwidth="false" data-activates="dropdown_settings">Settings</a></li>
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
	   
  
		  <h4>Public Schedule</h4>
	   
  
	  </div>
	  <div class="row center">
		<!-- table here -->
		  <asp:GridView ID="GridView1" runat="server" class ="striped" OnRowDataBound="GridView1_RowDataBound" UpdateMode="Conditional" PersistedSelection="true" AutoGenerateSelectButton="True" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnRowCreated="GridView1_RowCreated" OnDataBound="GridView1_DataBound">
		  </asp:GridView>
	  </div>
	  
		<div class="left">
		
		  <!-- Dropdown Trigger -->
			
			<button id="Button_changePrivate" runat="server" onserverclick="Button_changePrivate_Click" class="waves-effect waves-light btn red darken-4" >Selected Private</button>
			
			<button id="btnExport" runat="server" onserverclick="ExportToExcel" class="waves-effect waves-light btn red darken-4" >Export To Excel</button>
			<button id="button_checkConflict" runat="server" onserverclick="checkSpecific" class="waves-effect waves-light btn red darken-4" >Check Conflict</button>
			<button id="button8" runat="server" onserverclick="Button_Finalize_Click" class="waves-effect waves-light btn red darken-4" >Confirm</button>
			
		  </div>
		  <div style="float:right">
			  <button id="Button2" runat="server" onserverclick="Button_ShowAll_Click" class="waves-effect waves-light btn red darken-4" >Show All</button>
  <button id="Button7" runat="server" onserverclick="Button_SearchShow_Click"  class="btn modal-trigger red darken-4">Search</button></div>
  
	  </div>
	  
	  <br>
	  
	  <asp:GridView ID="GridView1_Hidden" runat="server" Visible="False">
	  </asp:GridView>
	  
	  <br>

	</div>
  </div>
<!-- Modal Structure modal modal-fixed-footer-->
  <!-- Add Class Modal Structuremodal modal-fixed-footer -->
  <div id="modal1" class="modal">
	<div class="modal-content">
		<asp:Label ID="Label_AddClassDesc" runat="server" Text="Warning: Editing a class will changed stored user for the class" Font-Size="large" Visible="false"></asp:Label>
		<br />
	
		 <table class="auto-style1">
			  <tr>
				  <td class="auto-style2">CRN</td>
				  <td class="auto-style5">
					  <div class="input-field col s6">
		  <input id="CRN_Text" type="text" class="validate" runat="server">
		  <label for="CRN_Text">Leave BLANK to submit random CRN</label>
		</div>
				  </td>
				  <td>&nbsp;</td>
				  <td class="auto-style3">Year</td>
				  <td>
					  <div class="input-field col s6">
		  <input id="Year_Text" type="text" class="validate" runat="server">
		  <label for="Year_Text">eg. 2017, etc.</label>
		</div></td>
				  <td>&nbsp;</td>
			  </tr>
			  <tr>
				  <td class="auto-style2">Instructor</td>
				  <td class="auto-style5">
					  <asp:DropDownList ID="DropDownList_instructor" runat="server" class="browser-default" Width="200px"></asp:DropDownList>
				  </td>
				  <td>&nbsp;</td>
				  <td class="auto-style3">Start Time</td>
				  <td>
					  <asp:TextBox ID="TextBox_StartTime" TextMode="Time" runat="server"></asp:TextBox>
				  </td>
				  <td>&nbsp;</td>
			  </tr>
			  <tr>
				  <td class="auto-style2">Class</td>
				  <td class="auto-style5">
					  <asp:DropDownList ID="DropDownList_class" runat="server" class="browser-default" Width="200px">
					  </asp:DropDownList>
				  </td>
				  <td>&nbsp;</td>
				  <td class="auto-style3">End Time</td>
				  <td>
					  <asp:TextBox ID="TextBox_EndTime" TextMode="Time" runat="server"></asp:TextBox>
				  </td>
				  <td>&nbsp;</td>
			  </tr>
			  <tr>
				  <td class="auto-style2">Term</td>
				  <td class="auto-style5">
					  <asp:DropDownList ID="DropDownList_term" runat="server" class="browser-default" Width="200px">
						  <asp:ListItem Selected="True" Value="Autumn">Autumn</asp:ListItem>
						  <asp:ListItem Value="Winter">Winter</asp:ListItem>
						  <asp:ListItem Value="Spring">Spring</asp:ListItem>
						  <asp:ListItem Value="Summer">Summer</asp:ListItem>
					  </asp:DropDownList>
				  </td>
				  <td>&nbsp;</td>
				  <td class="auto-style3">Days</td>
				  <td rowspan="6">
					  <asp:CheckBoxList ID="CheckBoxList_days" runat="server">
						  <asp:ListItem Value="1">Monday</asp:ListItem>
						  <asp:ListItem Value="2">Tuesday</asp:ListItem>
						  <asp:ListItem Value="3">Wednesday</asp:ListItem>
						  <asp:ListItem Value="4">Thursday</asp:ListItem>
						  <asp:ListItem Value="5">Friday</asp:ListItem>
						  <asp:ListItem Value="6">Saturday</asp:ListItem>
						  <asp:ListItem Value="7">Sunday</asp:ListItem>
					  </asp:CheckBoxList>
				  </td>
				  <td>&nbsp;</td>
			  </tr>
			  <tr>
				  <td class="auto-style2">Classroom</td>
				  <td class="auto-style5">
					  <asp:DropDownList ID="DropDownList_Classroom" runat="server" class="browser-default" Width="200px"></asp:DropDownList>
				  </td>
				  <td>&nbsp;</td>
				  <td class="auto-style3">&nbsp;</td>
				  <td>&nbsp;</td>
			  </tr>
			  <tr>
				  <td class="auto-style2">Enrollment</td>
				  <td class="auto-style5">
					  <div class="input-field col s6">
		  <input id="Enrollment_Text" type="text" class="validate" runat="server">
		  <label for="Enrollment_Text"></label>
		</div>
				  </td>
				  <td>&nbsp;</td>
				  <td class="auto-style3">&nbsp;</td>
				  <td>&nbsp;</td>
			  </tr>
			  <tr>
				  <td class="auto-style2">Credits</td>
				  <td class="auto-style5">
					  <div class="input-field col s6">
		  <input id="Credits_Text" type="text" class="validate" runat="server">
		  <label for="Credits_Text"></label>
		</div>
				  </td>
				  <td>&nbsp;</td>
				  <td class="auto-style3">&nbsp;</td>
				  <td>&nbsp;</td>
			  </tr>
			  <tr>
				  <td class="auto-style2">&nbsp;</td>
				  <td class="auto-style5">
					  &nbsp;</td>
				  <td>&nbsp;</td>
				  <td class="auto-style3">&nbsp;</td>
				  <td>&nbsp;</td>
			  </tr>
			  <tr>
				  <td class="auto-style2">&nbsp;</td>
				  <td class="auto-style5">
					  
				  </td>
				  <td>&nbsp;</td>
				  <td class="auto-style3">&nbsp;</td>
				  <td>&nbsp;</td>
			  </tr>
		  </table>
	</div>
	<div class="modal-footer">
		<a href="#!" class="modal-action modal-close waves-effect waves-green btn-flat ">Exit</a>
	  <a href="#" runat="server" onserverclick="Button_AddClass_Click1" class="modal-action modal-close waves-effect waves-green btn-flat ">Add Class</a>
		
	</div>
  </div>

	  <br><br>

	</div>
  </div>

		<!-- Modal Structure -->
  <div id="modal_deleteClasses" class="modal">
	<div class="modal-content">
	  <h4>Delete Classes</h4>
	  <table align="center" class="auto-style1">
		  <tr>
			  <td class="auto-style10">
				  <asp:DropDownList ID="DropDownList_deleteSelectMajor" runat="server" class="browser-default" Width="200px">
			  </asp:DropDownList>
			  </td>
			  <td>
				  <asp:DropDownList ID="DropDownList_deleteSelectQuarters" runat="server" class="browser-default" Width="200px">
					  <asp:ListItem>Autumn</asp:ListItem>
					  <asp:ListItem>Winter</asp:ListItem>
					  <asp:ListItem>Spring</asp:ListItem>
					  <asp:ListItem>Summer</asp:ListItem>
			  </asp:DropDownList>
			  </td>
			  <td>
				  &nbsp;</td>
		  </tr>
		  <tr>
			  <td class="auto-style10">
                  <button id="Button4" runat="server" onserverclick="Button_deleteUpdateMajor_Click"  class="btn modal-trigger red darken-4">Update</button>
				  
			  </td>
			  <td>
				  <asp:DropDownList ID="DropDownList_deleteSelectYear" runat="server" class="browser-default" Width="200px">
			  </asp:DropDownList>
			  </td>
			  <td>
				  &nbsp;</td>
		  </tr>
		  <tr>
			  <td class="auto-style10">Select Class(es):</td>
			  <td>
				  <asp:CheckBoxList ID="CheckBoxList_deleteSelect" runat="server">
				  </asp:CheckBoxList>
			  </td>
			  <td>
				  &nbsp;</td>
		  </tr>
		  <tr>
			  <td class="auto-style10">&nbsp;</td>
			  <td>
                  <button id="Button9" runat="server" onserverclick="Button_deleteSelectAll_Click"  class="btn modal-trigger red darken-4">Select All</button>
                  <button id="Button10" runat="server" onserverclick="Button_deleteUnselectAll_Click"  class="btn modal-trigger red darken-4">Unselect All</button></td>
				  
				  
			  </td>
			  <td>
                  
		  </tr>
		  </table>
		
	</div>
	<div class="modal-footer">
	  <a href="#!" class="modal-action modal-close waves-effect waves-green btn-flat">Exit</a>
        <a href="#" runat="server" onserverclick="Button_deleteClasses_Click" class="modal-action modal-close waves-effect waves-green btn-flat ">Delete Class</a>
	</div>
  </div>

		

<!-- Modal Structure -->
  <div id="modal_search" class="modal">
	<div class="modal-content">
	  <h4>Search</h4>
	  <p>Select 'Save as My Default' to save search query as default view when logging in/selecting 'Show All' button</p>
	  <p>Leave all fields BLANK to show all classes in database</p>


	</div>
	<div class="modal-footer">
		<table align="center">
			<tr>
				<td class="auto-style8">Search by Class name/number</td>
				<td class="auto-style9">
					<asp:CheckBox ID="CheckBox_className" runat="server" Text="Order By" />
				</td>
				<td>
					<div class="input-field col s6">
		  <input id="ClassSearch_Text" type="text" class="validate" runat="server">
		  <label for="ClassSearch_Text">eg. CSC1230, etc.</label>
		</div>

				</td>
			</tr>
			<tr>
				<td class="auto-style8">Search by CRN</td>
				<td class="auto-style9">
					<asp:CheckBox ID="CheckBox_CRN" runat="server" Text="Order By" />
				</td>
				<td>
					 <div class="input-field col s6">
		  <input id="CRNSearch_Text" type="text" class="validate" runat="server">
		  <label for="CRNSearch_Text">eg. CSC1230, etc.</label>
		</div>

				</td>
			</tr>
			<tr>
				<td class="auto-style8">Search by Instructor</td>
				<td class="auto-style9">
					<asp:CheckBox ID="CheckBox_instructor" runat="server" Text="Order By" />
				</td>
				<td>
					<asp:DropDownList ID="DropDownList_searchInstructor" runat="server" class="browser-default" Width="200px"></asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td class="auto-style8">Search by Major</td>
				<td class="auto-style9">&nbsp;</td>
				<td>
					<asp:DropDownList ID="DropDownList_searchMajor" runat="server" class="browser-default" Width="200px"></asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td class="auto-style8">Search by Calendar Year</td>
				<td class="auto-style9">
					<asp:CheckBox ID="CheckBox_calYear" runat="server" Text="Order By" />
				</td>
				<td>
					<asp:DropDownList ID="DropDownList_searchCalYear" runat="server" class="browser-default" Width="200px">
						
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td class="auto-style8">Search by Term</td>
				<td class="auto-style9">&nbsp;</td>
				<td>
					<asp:DropDownList ID="DropDownList_searchTerm" runat="server" class="browser-default" Width="200px">
						<asp:ListItem></asp:ListItem>
						<asp:ListItem>Autumn</asp:ListItem>
						<asp:ListItem>Winter</asp:ListItem>
						<asp:ListItem>Spring</asp:ListItem>
						<asp:ListItem>Summer</asp:ListItem>
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td class="auto-style8">Search by Class Year</td>
				<td class="auto-style9">&nbsp;</td>
				<td>
					<asp:DropDownList ID="DropDownList_searchClassYear" runat="server" class="browser-default" Width="200px">
						<asp:ListItem> </asp:ListItem>
						<asp:ListItem>Freshman</asp:ListItem>
						<asp:ListItem>Sophomore</asp:ListItem>
						<asp:ListItem>Junior</asp:ListItem>
						<asp:ListItem>Senior</asp:ListItem>
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td class="auto-style8">&nbsp;</td>
				<td class="auto-style9">&nbsp;</td>
				<td>
					<asp:CheckBox ID="CheckBox_conflicts" runat="server" Text="Show Conflicts" />
				</td>
			</tr>
			<tr>
				<td class="auto-style8">&nbsp;</td>
				<td class="auto-style9">&nbsp;</td>
				<td>
					<asp:CheckBox ID="CheckBox_default" runat="server" Text="Save as My Default" />
					
					
				</td>
			</tr>
		</table>
	  <a href="#!" class="modal-action modal-close waves-effect waves-green btn-flat">Exit</a>
		<a href="#" runat="server" onserverclick="Button_search_Click1" class="modal-action modal-close waves-effect waves-green btn-flat ">Search</a>
	</div>
  </div>
		  


<!-- Modal Structure -->
  <div id="modal_deleteUser" class="modal">
	<div class="modal-content">
	  <h4>Delete User</h4>
	  <p>Warning: Deleting User will not delete their classes</p>

	</div>
	<div class="modal-footer">
		<table  align="center" class="auto-style1" style="width:75%">
			<tr>
				<td>
					<asp:DropDownList ID="DropDownList_deleteUser" runat="server" class="browser-default" Width="200px">
			  </asp:DropDownList>

				</td>
			</tr>
			<tr>
				<td>
					<div class="input-field col s6">
		  <input id="confirmUserNameDelete_Text" type="text" class="validate" runat="server">
		  <label for="confirmUserNameDelete_Text">Confirm User Name to Delete</label>
		</div>

				</td>
			</tr>
		</table>
	  <a href="#!" class="modal-action modal-close waves-effect waves-green btn-flat">Exit</a>
		<a href="#" runat="server" onserverclick="Button_deleteUser_Click" class="modal-action modal-close waves-effect waves-green btn-flat ">Delete User</a>
	</div>
  </div>
		  

		<!-- Modal Structure modal modal-fixed-footer-->
  <!-- Modal Structure -->
  <div id="modal_settings" class="modal">
	<div class="modal-content">
	  <h4>Add New User</h4>
		
	  <table align="center" class="auto-style1" style="width:75%">
			<tr>
				<td>
					<div class="input-field col s6">
		  <input id="newUser_Text" type="text" class="validate" runat="server">
		  <label for="newUser_Text">Enter New User Name</label>
		</div>

				</td>
			</tr>
			<tr>
				<td>
					<div class="input-field col s6">
		  <input id="confirmUser_Text" type="text" class="validate" runat="server">
		  <label for="confirmUser_Text">Confirm New User Name</label>
		</div>

				</td>
			</tr>
			<tr>
				<td>
					<div class="input-field col s6">
		  <input id="newUPass_Text" type="password" class="validate" runat="server">
		  <label for="newUPass_Text">Enter New Password</label>
		</div>

				</td>
			</tr>
			<tr>
				<td>
					<div class="input-field col s6">
		  <input id="confirmNewUPass_Text" type="password" class="validate" runat="server">
		  <label for="confirmNewUPass_Text">Confirm New Password</label>
					</td>
			</tr>
		</table>
	</div>
	<div class="modal-footer">
	   
		<a href="#!" class="modal-action modal-close waves-effect waves-green btn-flat ">Exit</a>
	   
		<a href="#" runat="server" onserverclick="Button_UserAdd_Click" class="modal-action modal-close waves-effect waves-green btn-flat ">Add User</a>
	  
	</div>
  </div>

		<!-- Modal Structure -->
  <div id="modal_Login" class="modal">
	<div class="modal-content">
	  <h4>Login</h4>
	  <p>Please enter username and password.</p>



		<table align="center" class="auto-style1" style="width:75%">
			<tr>
				<td>
					<div class="input-field col s6">
		  <input id="UserName" type="text" class="validate" runat="server">
		  <label for="UserName">Enter User Name</label>
		</div>
				</td>
			</tr>
			<tr>
				<td>
					<div class="input-field col s6">
		  <input id="Password" type="password" class="validate" runat="server">
		  <label for="UserName">Enter Password</label>
		</div>
				</td>
			</tr>
		</table>



	</div>
	<div class="modal-footer">
	  <a href="#" runat="server" onserverclick="Button_Login_Click" class="modal-action modal-close waves-effect waves-green btn-flat">Login</a>
	</div>
  </div>

				<!-- Modal Structure -->
  <div id="modal_chgPass" class="modal">
	<div class="modal-content">
	  <h4>Change Password</h4>
	  <p>Please select user name and enter new password.</p>



		<table align="center" class="auto-style1" style="width:75%">
			<tr>
				<td>
					<asp:DropDownList ID="DropDownList_chgPass" runat="server" class="browser-default" Width="200px">
			  </asp:DropDownList>

					</td>
			</tr>
			<tr>
				<td>
					<div class="input-field col s6">
		  <input id="chgPass_Text" type="password" class="validate" runat="server">
		  <label for="chgPass_Text">Enter New Password</label>
		</div>
				</td>
			</tr>
			<tr>
				<td>
					<div class="input-field col s6">
		  <input id="confirmChgPass_Text" type="password" class="validate" runat="server">
		  <label for="confirmChgPass_Text">Confirm New Password</label>
		</div>
				</td>
			</tr>
		</table>



	</div>
	<div class="modal-footer">
		<a href="#!" class="modal-action modal-close waves-effect waves-green btn-flat ">Exit</a>
	  <a href="#" runat="server" onserverclick=" Button_chgPw_Click" class="modal-action modal-close waves-effect waves-green btn-flat">Change Password</a>
	</div>
  </div>

<!-- Modal Structure -->
  <div id="modal_addMajor" class="modal">
	<div class="modal-content">
	  <h4>Add New Major</h4>
	  



		<table align="center" class="auto-style1" style="width:75%">
			<tr>
				<td>
					<div class="input-field col s6">
		  <input id="addDepartment_Text" type="text" class="validate" runat="server">
		  <label for="addDepartment_Text">Enter Major Name</label>
		</div>
				</td>
			</tr>
			<tr>
				<td>
					<div class="input-field col s6">
		  <input id="confirmAddDepartment_Text" type="text" class="validate" runat="server">
		  <label for="confirmAddDepartment_Text">Confirm New Major Name</label>
		</div>
				</td>
			</tr>
		</table>



	</div>
	<div class="modal-footer">
		<a href="#!" class="modal-action modal-close waves-effect waves-green btn-flat ">Exit</a>
	  <a href="#" runat="server" onserverclick="Button_AddMajor_Click" class="modal-action modal-close waves-effect waves-green btn-flat">Add Department</a>
	</div>
  </div>


		<!-- Modal Structure -->
  <div id="modal_addInstructor" class="modal">
	<div class="modal-content">
	  <h4>Add New Instructor</h4>
	  



		<table align="center" class="auto-style1" style="width:75%">
			<tr>
				<td>
					<div class="input-field col s6">
		  <input id="addInstructor_Text" type="text" class="validate" runat="server">
		  <label for="addInstructor_Text">Enter Instructor Name</label>
		</div>
				</td>
			</tr>
			<tr>
				<td>
					<div class="input-field col s6">
		  <input id="confirmAddInstructor_Text" type="text" class="validate" runat="server">
		  <label for="confirmAddInstructor_Text">Confirm New Instructor Name</label>
		</div>
				</td>
			</tr>
		</table>



	</div>
	<div class="modal-footer">
		<a href="#!" class="modal-action modal-close waves-effect waves-green btn-flat ">Exit</a>
	  <a href="#" runat="server" onserverclick="Button_AddInstructor_Click" class="modal-action modal-close waves-effect waves-green btn-flat">Add Instructor</a>
	</div>
  </div>

		 <!-- Modal Structure -->
  <div id="modal_addRoom" class="modal">
	<div class="modal-content">
	  <h4>Add New Classroom</h4>
	  



		<table align="center" class="auto-style1" style="width:75%">
			<tr>
				<td>
					<div class="input-field col s6">
		  <input id="addRoom_Text" type="text" class="validate" runat="server">
		  <label for="addRoom_Text">Enter Classroom Name</label>
		</div>
				</td>
			</tr>
			<tr>
				<td>
					<div class="input-field col s6">
		  <input id="confirmAddRoom_Text" type="text" class="validate" runat="server">
		  <label for="cconfirmAddRoom_Text">Confirm New Classroom Name</label>
		</div>
				</td>
			</tr>
		</table>



	</div>
	<div class="modal-footer">
		<a href="#!" class="modal-action modal-close waves-effect waves-green btn-flat ">Exit</a>
	  <a href="#" runat="server" onserverclick="Button_AddRoom_Click" class="modal-action modal-close waves-effect waves-green btn-flat">Add Classroom</a>
	</div>
  </div>

		<!-- Modal Structure -->
  <div id="modal_addClass" class="modal">
	<div class="modal-content">
	  <h4>Add New Class</h4>
	  



		<table align="center" class="auto-style1" style="width:75%">
			<tr>
				<td>
					<div class="input-field col s6">
		  <input id="addClass_Text" type="text" class="validate" runat="server">
		  <label for="addClass_Text">Enter Class Name</label>
		</div>
				</td>
			</tr>
			<tr>
				<td>
					<div class="input-field col s6">
		  <input id="confirmAddClass_Text" type="text" class="validate" runat="server">
		  <label for="confirmAddClass_Text">Confirm New Class Name</label>
		</div>
				</td>
			</tr>
			<tr>
				<td>
					<p>Applicable Major(s):</p>
					
				  <asp:CheckBoxList ID="CheckBoxList_majors" runat="server" RepeatColumns="2">
					  </asp:CheckBoxList>
					</td>
			</tr>
			<tr>
				<td>
					Select Year(s):<br />
					<asp:CheckBoxList ID="CheckBoxList_addClassYear" runat="server">
						<asp:ListItem Value="1">Freshman</asp:ListItem>
						<asp:ListItem Value="1">Sophomore</asp:ListItem>
						<asp:ListItem Value="3">Junior</asp:ListItem>
						<asp:ListItem Value="4">Senior</asp:ListItem>
					</asp:CheckBoxList>
					</td>
			</tr>
		</table>



	</div>
	<div class="modal-footer">
		<a href="#!" class="modal-action modal-close waves-effect waves-green btn-flat ">Exit</a>
	  <a href="#" runat="server" onserverclick="Button_addClass_Click" class="modal-action modal-close waves-effect waves-green btn-flat">Add Class</a>
	</div>
  </div>

		<!-- Modal Structure -->
  <div id="modal_deleteMajor" class="modal">
	<div class="modal-content">
	  <h4>Delete Major</h4>
	 

	</div>
	<div class="modal-footer">
		<table  align="center" class="auto-style1" style="width:75%">
			<tr>
				<td>
					<asp:DropDownList ID="DropDownList_deleteMajor" runat="server" class="browser-default" Width="200px">
			  </asp:DropDownList>

				</td>
			</tr>
			<tr>
				<td>
					<div class="input-field col s6">
		  <input id="confirmMajorDelete_Text" type="text" class="validate" runat="server">
		  <label for="confirmMajorDelete_Text">Confirm Major to Delete</label>
		</div>

				</td>
			</tr>
		</table>
	  <a href="#!" class="modal-action modal-close waves-effect waves-green btn-flat">Exit</a>
		<a href="#" runat="server" onserverclick="Button_deleteMajor_Click" class="modal-action modal-close waves-effect waves-green btn-flat ">Delete Major</a>
	</div>
  </div>

		<!-- Modal Structure -->
  <div id="modal_deleteInstructor" class="modal">
	<div class="modal-content">
	  <h4>Delete Instructor</h4>
	 

	</div>
	<div class="modal-footer">
		<table  align="center" class="auto-style1" style="width:75%">
			<tr>
				<td>
					<asp:DropDownList ID="DropDownList_deleteInstructor" runat="server" class="browser-default" Width="200px">
			  </asp:DropDownList>

				</td>
			</tr>
			<tr>
				<td>
					<div class="input-field col s6">
		  <input id="confirmInstructorDelete_Text" type="text" class="validate" runat="server">
		  <label for="confirmInstructorDelete_Text">Confirm Instructor to Delete</label>
		</div>

				</td>
			</tr>
		</table>
	  <a href="#!" class="modal-action modal-close waves-effect waves-green btn-flat">Exit</a>
		<a href="#" runat="server" onserverclick="Button_deleteInstructor_Click" class="modal-action modal-close waves-effect waves-green btn-flat ">Delete Instructor</a>
	</div>
  </div>

		<!-- Modal Structure -->
  <div id="modal_deleteRoom" class="modal">
	<div class="modal-content">
	  <h4>Delete Room</h4>
	 

	</div>
	<div class="modal-footer">
		<table  align="center" class="auto-style1" style="width:75%">
			<tr>
				<td>
					<asp:DropDownList ID="DropDownList_deleteRoom" runat="server" class="browser-default" Width="200px">
			  </asp:DropDownList>

				</td>
			</tr>
			<tr>
				<td>
					<div class="input-field col s6">
		  <input id="confirmRoomDelete_Text" type="text" class="validate" runat="server">
		  <label for="confirmRoomDelete_Text">Confirm Room to Delete</label>
		</div>

				</td>
			</tr>
		</table>
	  <a href="#!" class="modal-action modal-close waves-effect waves-green btn-flat">Exit</a>
		<a href="#" runat="server" onserverclick="Button_deleteRoom_Click" class="modal-action modal-close waves-effect waves-green btn-flat ">Delete Room</a>
	</div>
  </div>

		<!-- Modal Structure -->
  <div id="modal_deleteClass" class="modal">
	<div class="modal-content">
	  <h4>Delete Class</h4>
	 

	</div>
	<div class="modal-footer">
		<table  align="center" class="auto-style1" style="width:75%">
			<tr>
				<td>
					<asp:DropDownList ID="DropDownList_deleteClass" runat="server" class="browser-default" Width="200px">
			  </asp:DropDownList>

				</td>
			</tr>
			<tr>
				<td>
					<div class="input-field col s6">
		  <input id="confirmClassDelete_Text" type="text" class="validate" runat="server">
		  <label for="confirmClassDelete_Text">Confirm Room to Delete</label>
		</div>

				</td>
			</tr>
		</table>
	  <a href="#!" class="modal-action modal-close waves-effect waves-green btn-flat">Exit</a>
		<a href="#" runat="server" onserverclick="Button_deleteClass_Click" class="modal-action modal-close waves-effect waves-green btn-flat ">Delete Class</a>
	</div>
  </div>


		<!-- Modal Structure -->
  <div id="modal_copy" class="modal">
	<div class="modal-content">
	  <h4>Copy Classes</h4>
	  <table align="center">
		  <tr>
			  <td class="auto-style6">
				  <asp:DropDownList ID="DropDownList_copy" runat="server" class="browser-default" Width="200px" OnSelectedIndexChanged="DropDownList_copy_SelectedIndexChanged">
			  </asp:DropDownList>

				  
				  <button id="Button_copyUpdate" runat="server" onserverclick="Button_copyUpdate_Click"  class="btn modal-trigger red darken-4">Update</button>

			  </td>
			  <td class="auto-style7">
				  
			  <asp:DropDownList ID="DropDownList_CopyTerm" runat="server" class="browser-default" Width="200px" OnSelectedIndexChanged="DropDownList_copy_SelectedIndexChanged">
				  <asp:ListItem Selected="True">Autumn</asp:ListItem>
				  <asp:ListItem>Winter</asp:ListItem>
				  <asp:ListItem>Spring</asp:ListItem>
				  <asp:ListItem>Summer</asp:ListItem>
			  </asp:DropDownList>
				  <asp:DropDownList ID="DropDownList_copiedYear" runat="server" class="browser-default" Width="200px" OnSelectedIndexChanged="DropDownList_copy_SelectedIndexChanged">
			  </asp:DropDownList>
				  <div class="input-field col s6">
		  <input id="CopyYear_Text" type="text" class="validate" runat="server">
		  <label for="CopyYear_Text">Enter New Year</label>
		</div>
			  </td>
			  <td>
				  

			  </td>
		  </tr>
		  <tr>
			  <td class="auto-style6">
				  Select Class(es):</td>
			  <td class="auto-style7">
				  
				  <asp:CheckBoxList ID="CheckBoxList_copyAll" runat="server" OnSelectedIndexChanged="CheckBoxList_copyAll_SelectedIndexChanged">
				  </asp:CheckBoxList>
			  </td>
			  <td>
				  &nbsp;</td>
		  </tr>
		  <tr>
			  <td class="auto-style6">
				  

			  </td>
			  <td class="auto-style7">
				  
				  
				  <button id="Button5" runat="server" onserverclick="Button_selectAll_Click"  class="btn modal-trigger red darken-4">Select All</button>
				  <button id="Button6" runat="server" onserverclick="Button_unselectAll_Click"  class="btn modal-trigger red darken-4">Unselect All</button>


			  </td>
			  <td>
				  &nbsp;</td>
		  </tr>
		  </table>
		
		<asp:panel runat="server" ID="Panel_copyAll">
		</asp:panel>


	</div>
	<div class="modal-footer">
		<a href="#!" class="modal-action modal-close waves-effect waves-green btn-flat ">Exit</a>
		<a href="#" runat="server" onserverclick="Button_copy_Click" class="modal-action modal-close waves-effect waves-green btn-flat ">Copy</a>
	  
	</div>
  </div>







<div class="container">
	<div class="section">

	  <!--   Icon Section   -->
	  <div class="row">
		<div class="col s12">
			<div class="row center">
	   
  
		  
		  <h4>Private Schedule</h4>
	   
  
	  </div>
		
		<asp:GridView ID="GridView2" runat="server" AutoGenerateSelectButton="True" OnSelectedIndexChanged="GridView2_SelectedIndexChanged" OnRowDataBound="GridView2_RowDataBound" OnRowCreated="GridView2_RowCreated">
			</asp:GridView>
 &nbsp;<div align="left">
	   <button id="Button_addSession" runat="server" onserverclick="Button_addSessionShow_Click"  class="btn modal-trigger red darken-4">Add</button>
	 <button id="Button_editSession" runat="server" onserverclick="Button_editSessionShow_Click"  class="btn modal-trigger red darken-4">Edit</button>
		<button id="Button1" runat="server" onserverclick="Button_delete_Click" class="waves-effect waves-light btn red darken-4" >Delete</button>
		<button id="Button_Push" runat="server" OnServerClick="Button_Push_Click" class="btn modal-trigger red darken-4">Selected Public</button>
	 
		<!--<button data-target="modal2" class="btn modal-trigger red darken-4">Import</button>-->
  
	  
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

	
				<br />
	</div>
	
			&nbsp;<asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="Button" Visible="False" />
		  </div>
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