<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="start.aspx.cs" Inherits="stemSchedule.start" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>
<!-- Compiled and minified CSS -->
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/materialize/0.96.1/css/materialize.min.css">

<!-- Compiled and minified JavaScript -->
<script src="https://cdnjs.cloudflare.com/ajax/libs/materialize/0.96.1/js/materialize.min.js"></script>
 <form id="form1" runat="server">
 <nav class="red darken-4" role="navigation">
	<div class="nav-wrapper container"><a id="logo-container" href="#" class="brand-logo">STEMschedule</a>
	  <ul class="right hide-on-med-and-down">
		<li class="active"><a href="#">Login</a></li>
		
	  </ul>

	  <ul id="nav-mobile" class="side-nav">
		<li><a href="#">Navbar Link</a></li>
	  </ul>
	  <a href="#" data-activates="nav-mobile" class="button-collapse"><i class="material-icons">menu</i></a>
	</div>
  </nav>
  <div class="section no-pad-bot" id="index-banner">
<div class="container">
	  <br><br>
	  <div class="row center-left">
	   
  
	  </div>
	  <div class="row center">
		<!-- Login -->
		<div class="row">
	  <div class="row">
		<div class="input-field col s6">
		  <input id="UserName" type="text" class="validate" runat="server">
		  <label for="UserName">User Name</label>
		</div>
	  </div>
	  
	  <div class="row">
		<div class="input-field col s6">
		  <input id="Password" type="password" class="validate" runat="server">
		  <label for="Password">Password</label>
		</div>
	  </div>
	  <div style="float:left">
		<button id="Button1" runat="server" OnServerClick="Button1_Click" class="btn-large waves-effect red darken-4">Login</button>
		  </div>
	  
	        
	
  </div>
		
	  </div>
	  
		
		  
	  </div>
	  
	  <br><br>

	</div>
  </div>
<!-- Modal Structure -->
  <div id="modal1" class="modal modal-fixed-footer">
	<div class="modal-content">
	  <h4>Modal Header</h4>
	  <p>A bunch of text</p>
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


<script>
$(document).ready(function() {
  // the "href" attribute of .modal-trigger must specify the modal ID that wants to be triggered
  $('.modal-trigger').leanModal();
});
</script>
</form>

