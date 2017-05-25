<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="administration.aspx.cs" Inherits="stemSchedule.administation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Settings -- STEMschedule</title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
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
          <li><a href="settings.aspx">Settings</a></li>
		<li class="active"><a href="#">Admin</a></li>
		
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
      
      <div class="col s12">
	  <center> <button id="Button_changePrivate" runat="server" onserverclick="Button_ShowUser" class="waves-effect waves-light btn red darken-4" >Add/Delete User</button></center>
	 
	  

    
	
  </div>
		
	  </div>
	  
	  
	  
	  <asp:Panel ID="Panel1" runat="server">
          <asp:GridView ID="GridView_users" runat="server">
          </asp:GridView>
          <asp:Panel ID="Panel2" runat="server">
          </asp:Panel>
          <br />
          <center> <button id="Button1" runat="server" onserverclick="Button_showAddUser" class="waves-effect waves-light btn red darken-4" >Add User</button>
              <button id="Button2" runat="server" onserverclick="Button_showDeleteUser" class="waves-effect waves-light btn red darken-4" >Delete User</button>


          </center>
          <asp:Panel ID="Panel_addUser" runat="server" Visible="False">
              <table class="auto-style1">
                  <tr>
                      <td class="auto-style5">User Name:</td>
                      <td class="auto-style8">
                          <asp:TextBox ID="UNTextBox" runat="server"></asp:TextBox>
                      </td>
                  </tr>
                  <tr>
                      <td class="auto-style5">E-Mail</td>
                      <td class="auto-style8">
                          <asp:TextBox ID="emailTextBox" runat="server"></asp:TextBox>
                      </td>
                  </tr>
                  <tr>
                      <td class="auto-style6">Password</td>
                      <td class="auto-style9">
                          <asp:TextBox ID="passTextBox" runat="server"></asp:TextBox>
                      </td>
                  </tr>
                  <tr>
                      <td class="auto-style6">Confirm Password</td>
                      <td class="auto-style9">
                          <asp:TextBox ID="cpassTextBox" runat="server"></asp:TextBox>
                      </td>
                  </tr>
                  <tr>
                      <td class="auto-style6"></td>
                      <td class="auto-style9">
                          
                          <button id="Button3" runat="server" onserverclick="submitButton_Click" class="waves-effect waves-light btn red darken-4" >Submit</button>
                          <button id="Button4" runat="server" onserverclick="Button_buttonHide_Click" class="waves-effect waves-light btn red darken-4" >Hide</button>
                          
                      </td>
                  </tr>
              </table>
          </asp:Panel>
          <br />
          <br />
          <br />
          <br />
          <br />
     </asp:Panel>
	  
	  
	  
	  <br><br>

	
  

	  
	  
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