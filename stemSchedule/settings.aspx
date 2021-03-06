﻿ <%@ Page Language="C#" AutoEventWireup="true" CodeBehind="settings.aspx.cs" Inherits="stemSchedule.settings" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Settings -- STEMschedule</title>
	<style type="text/css">
		.auto-style1 {
			width: 100%;
		}
		.auto-style2 {
			width: 1453px;
		}
		.auto-style3 {
			width: 1453px;
			text-align: right;
		}
	</style>
</head>
<script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>
<!-- Compiled and minified CSS -->
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/materialize/0.96.1/css/materialize.min.css">
<form id="form2" runat="server">
<!-- Compiled and minified JavaScript -->
<script src="https://cdnjs.cloudflare.com/ajax/libs/materialize/0.96.1/js/materialize.min.js"></script>
 <nav class="red darken-4" role="navigation">
	<div class="nav-wrapper container"><a id="logo-container" href="#" class="brand-logo">STEMschedule</a>
	  <ul class="right hide-on-med-and-down">
		<li><a href="main.aspx">Schedule</a></li>
		<li class="active"><a href="#">Settings</a></li>
		<li><a href="administration.aspx">Admin</a></li>
		<li><a href="#" runat="server" onserverclick="Button_Logout_Click">Logout</a></li>
		
	  </ul>

	  <ul id="nav-mobile" class="side-nav">
		<li><a href="#">Navbar Link</a></li>
	  </ul>
	  <a href="#" data-activates="nav-mobile" class="button-collapse"><i class="material-icons">menu</i></a>
	</div>
  </nav>
  <div class="section no-pad-bot" id="index-banner">
<div class="container">
	 <div class="row">
	  
	  <div class="col s3"><asp:GridView ID="GridView_departments" runat="server" OnSelectedIndexChanged="GridView_departments_SelectedIndexChanged">
		  </asp:GridView>
	  
	  
	  <div style="float:left">
		<button data-target="modal_addDept" class="btn modal-trigger red darken-4">Add Department</button>
		  </div></div>
	  <div class="col s3"><asp:GridView ID="GridView_room" runat="server" OnRowDataBound="GridView_room_RowDataBound">
		  </asp:GridView>
	  
	  
		  <div style="float:left">
		  <button data-target="modal_addRoom" class="btn modal-trigger red darken-4">Add Room<br />
			  </button>
			  
		  </div></div>
		 <div class="col s6"> <button data-target="modal_addClass" class="btn modal-trigger red darken-4">Add Class<br />
			  </button></div>

	</div>
	  <br><br>
	  <div class="row center-left">
	   
  
	  </div>
	  <div class="row center">
		<!-- Login -->
		<div class="row">
	</div>
	  <div style="float:left">
	  <button data-target="modal_changePW" class="btn modal-trigger red darken-4">Change Password</button>
		
		</div>

	  </br></br>
	</br>
	  
	  
		  
		  <br><br>
	  
	  
		  

		  <br />
		  <br />
		  <button data-target="modal_addClass" class="btn modal-trigger red darken-4">Add Class<br />
			  </button>
		  
		  <br />
		  
	</div>
	
  </div>
		
	  </div>
	  
	  
	  
	  <br><br>

	
  
<!-- Modal Structure -->
  <div id="modal_changePW" class="modal modal-fixed-footer">
	<div class="modal-content">
	  <h4>Change Password</h4>
	  <div class="row">
		<div class="input-field col s6">
		  <input id="password" type="password" class="validate">
		  <label for="password">Enter New Password</label>
		</div>
	  </div>
	  <div class="row">
		<div class="input-field col s6">
		  <input id="password" type="password" class="validate">
		  <label for="password">Confirm Password</label>
		  <br><br><br>
		  <a class="waves-effect waves-light btn red darken-4">Confirm</a>
		</div>
	  </div>
	  <div class="modal-footer">
	  <a href="#!" class="modal-action modal-close waves-effect waves-green btn-flat ">Exit</a>
	</div>
	  
	  
	</div>
	
  </div>
  <div id="modal_addDept" class="modal modal-fixed-footer">
	<div class="modal-content">
	  <h4>Modal Header</h4>
	  <div class="row">
		<div class="input-field col s6">
			<input id="department" type="text" class="validate" runat="server">
		  <label for="department">New Department</label>
		</div>
	  </div>
	  <div class="row">
		<div class="input-field col s6">
		  <input id="email" type="text" class="validate">
		  <label for="UserName">Confirm New Department</label>
		  <br><br><br>
		  
			<asp:Button ID="Button_addDepartment" runat="server" OnClick="Button_addDepartment_Click1" Text="Confirm" />
			
		</div>
	  </div>
	</div>
	<div class="modal-footer">
	  <a href="#!" class="modal-action modal-close waves-effect waves-green btn-flat ">Agree</a>
	</div>
  </div>

	
	
  <div id="modal_addRoom" class="modal modal-fixed-footer">
	<div class="modal-content">
	  <table class="auto-style1">
				  <tr>
					  <td>Enter Room</td>
					  <td>
						  <asp:TextBox ID="TextBox_Room" runat="server"></asp:TextBox>
					  </td>
				  </tr>
				  <tr>
					  <td>Confirm room</td>
					  <td>
						  <asp:TextBox ID="TextBox_RoomConfirm" runat="server"></asp:TextBox>
					  </td>
				  </tr>
				  <tr>
					  <td>&nbsp;</td>
					  <td>
						  <asp:Button ID="Button_addRoom" runat="server" OnClick="Button_addRoom_Click" Text="Add Room" />
					  </td>
				  </tr>
			  </table>
	</div>
	<div class="modal-footer">
	  <a href="#!" class="modal-action modal-close waves-effect waves-green btn-flat ">Agree</a>
	</div>
  </div>

	<div id="modal_addClass" class="modal modal-fixed-footer">
	<div class="modal-content">
	  <table class="auto-style1">
			  <tr>
				  <td class="auto-style3">Class name:</td>
				  <td>
					  <asp:TextBox ID="TextBox_className" runat="server" Width="275px"></asp:TextBox>
				  </td>
			  </tr>
			  <tr>
				  <td class="auto-style3">Applicable Major(s):</td>
				  <td>
					  <asp:CheckBoxList ID="CheckBoxList_majors" runat="server" RepeatColumns="2">
					  </asp:CheckBoxList>
				  </td>
			  </tr>
			  <tr>
				  <td class="auto-style2">&nbsp;</td>
				  <td>
					  <asp:Button ID="Button_addClass" runat="server" Text="Add Class" OnClick="Button_addClass_Click1" />
				  </td>
			  </tr>
		  </table>
	</div>
	<div class="modal-footer">
	  <a href="#!" class="modal-action modal-close waves-effect waves-green btn-flat ">Agree</a>
	</div>
  </div>

	
  
  
  
	  <br><br>

	</div>
  </div>


<div class="container">
	<div class="section">

	  <!--   Icon Section   -->
	  <div class="row">
		
		
		  
			
	  
  
	  
<!-- Add Class Modal Structure -->
  <div id="modal1" class="modal modal-fixed-footer">
	<div class="modal-content">
	  <h4>Modal Header</h4>
	  <p>A bunch of text</p>
	

	
	</div>
	<div class="modal-footer">
	  <a href="#!" class="modal-action modal-close waves-effect waves-green btn-flat ">Agree</a>
	</div>
  </div>


		
		
		</form>

		

		
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
</html>
<script>
$(document).ready(function() {
  // the "href" attribute of .modal-trigger must specify the modal ID that wants to be triggered
  $('.modal-trigger').leanModal();
});
</script>