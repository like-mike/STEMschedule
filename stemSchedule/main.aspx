﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main.aspx.cs" Inherits="stemSchedule.main" %>

<!DOCTYPE html>
<html lang="en">
	<head runat="server">
<script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>
<!-- Compiled and minified CSS -->
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/materialize/0.96.1/css/materialize.min.css">

<!-- Compiled and minified JavaScript -->
<script src="https://cdnjs.cloudflare.com/ajax/libs/materialize/0.96.1/js/materialize.min.js"></script>
		</head>
 <nav class="red darken-4" role="navigation">
	<div class="nav-wrapper container"><a id="logo-container" href="#" class="brand-logo">STEMschedule</a>
	  <ul class="right hide-on-med-and-down">
		<li class="active"><a href="#">Schedule</a></li>
		<li><a href="#">Settings</a></li>
		<li><a href="#">Admin</a></li>
		<li><a href="#">Logout</a></li>
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
	  <div class="row center-left">
	   
  
	  </div>
	  <div class="row center">
		<!-- table here -->
		  <asp:GridView ID="GridView1" runat="server" class ="striped" AllowPaging="True" OnRowDataBound="GridView1_RowDataBound">
		  </asp:GridView>
	  </div>
	  
		<div style="float:left">
		<a href="http://materializecss.com/getting-started.html" id="download-button" class="btn-large waves-effect red darken-4">Delete Class</a>
		  <!-- Dropdown Trigger -->
		  </div>
		  <div style="float:right">
  <a class='dropdown-button btn red darken-4' href='#' data-activates='dropdown1' align="right">Drop Me!</a>
</div>
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
  <!-- Add Class Modal Structure -->
  <div id="modal1" class="modal modal-fixed-footer">
	<div class="modal-content">
	  <h4>Modal Header</h4>
	  <p>A bunch of text</p>
		 <table class="auto-style1">
			  <tr>
				  <td class="auto-style2">CRN</td>
				  <td>
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
				  <td>
					  <asp:TextBox ID="TextBox_Faculty" runat="server"></asp:TextBox>
				  </td>
				  <td>&nbsp;</td>
				  <td class="auto-style3">Year Taken</td>
				  <td>
					  <asp:TextBox ID="TextBox_YearTaken" runat="server"></asp:TextBox>
				  </td>
				  <td>&nbsp;</td>
			  </tr>
			  <tr>
				  <td class="auto-style2">Class</td>
				  <td>
					  <asp:TextBox ID="TextBox_Class" runat="server"></asp:TextBox>
				  </td>
				  <td>&nbsp;</td>
				  <td class="auto-style3">Major 1</td>
				  <td>
					  <asp:TextBox ID="TextBox_M1" runat="server"></asp:TextBox>
				  </td>
				  <td>&nbsp;</td>
			  </tr>
			  <tr>
				  <td class="auto-style2">Term</td>
				  <td>
					  <asp:TextBox ID="TextBox_Term" runat="server"></asp:TextBox>
				  </td>
				  <td>&nbsp;</td>
				  <td class="auto-style3">Major 2</td>
				  <td>
					  <asp:TextBox ID="TextBox_Major2" runat="server"></asp:TextBox>
				  </td>
				  <td>&nbsp;</td>
			  </tr>
			  <tr>
				  <td class="auto-style2">Classroom</td>
				  <td>
					  <asp:TextBox ID="TextBox_Classroom" runat="server"></asp:TextBox>
				  </td>
				  <td>&nbsp;</td>
				  <td class="auto-style3">Major 3</td>
				  <td>
					  <asp:TextBox ID="TextBox_Major3" runat="server"></asp:TextBox>
				  </td>
				  <td>&nbsp;</td>
			  </tr>
			  <tr>
				  <td class="auto-style2">Days</td>
				  <td>
					  <asp:TextBox ID="TextBox_Days" runat="server"></asp:TextBox>
				  </td>
				  <td>&nbsp;</td>
				  <td class="auto-style3">Major 4</td>
				  <td>
					  <asp:TextBox ID="TextBox_Major4" runat="server"></asp:TextBox>
				  </td>
				  <td>&nbsp;</td>
			  </tr>
			  <tr>
				  <td class="auto-style2">Start Time</td>
				  <td>
					  <asp:TextBox ID="TextBox_StartTime" TextMode="Time" runat="server"></asp:TextBox>
				  </td>
				  <td>&nbsp;</td>
				  <td class="auto-style3">Credits</td>
				  <td>
					  <asp:TextBox ID="TextBox_Credits" runat="server"></asp:TextBox>
				  </td>
				  <td>&nbsp;</td>
			  </tr>
			  <tr>
				  <td class="auto-style2">End Time</td>
				  <td>
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
				  <td>
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
		
		<asp:GridView ID="GridView2" runat="server">
            </asp:GridView>
&nbsp;<div align="left">
	   <button data-target="modal1" class="btn modal-trigger red darken-4">Add</button>
		<a href="http://materializecss.com/getting-started.html" id="download-button" class="waves-effect waves-light btn red darken-4">Delete</a>
		<button id="Button_Push" runat="server" OnServerClick="Button_Push_Click" class="btn modal-trigger red darken-4">Push</button>
  
	  
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