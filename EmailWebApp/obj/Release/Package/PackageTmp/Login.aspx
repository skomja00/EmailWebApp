<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="EmailWebApp.Login" %>

<%@ Register src="ModalUserControl.ascx" TagName="ModalControl" TagPrefix="mc"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Email WebApp</title>
	<link rel="icon" type="image/svg" href="Images/mail.svg" />
	<meta name="viewport" content="width=device-width, initial-scale=1" />
	
    <%--Bootstrap Icons CDN--%>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.4.1/font/bootstrap-icons.css">

	<%--Bootstrap CSS CDN--%>
	<link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
	
	<%--local <style> & CSS--%>
	<link href="Style/Modal.css" rel="stylesheet" />
    <link href="Style/Common.css" rel="stylesheet" />

	<script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
	<script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js"></script>
	<script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
</head>
<body class="d-flex align-items-center justify-content-center">
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <%--Start NavBar--%>
        <nav class="navbar fixed-top navigation border border-primary rounded">
            <%--Navigation Logo--%>
			<span class="border border-primary rounded bg-white p-3 m-3">Email WebApp</span>
			<%--Navigation: Login/Create Account Dropdown--%>
			<div class="btn-toolbar ml-2">
				<div class="btn-group">
					<div>
                        <asp:LinkButton runat="server" CssClass="btn btn-default dropdown-toggle m-1" data-toggle="dropdown">
                            Menu<span class="caret"></span></asp:LinkButton>
                        <ul class="dropdown-menu">
                            <li><asp:Button ID="btnLogin" class="btn btn-default m-1" runat="server" 
                                Text="Login" onclick="btnLogin_Click"></asp:Button></li>
                            <li><asp:Button ID="btnForgotPassword" CssClass="btn btn-default m-1" runat="server" 
                                Text="Forgot Passord?" OnClick="btnForgotPassword_Click"></asp:Button></li>
                            <li><asp:Button ID="btnCreateAccount" CssClass="btn btn-default m-1" runat="server" 
                                Text="Create Account" OnClick="btnCreateAccount_Click"></asp:Button></li>
						</ul>
					</div>
				</div>
			</div>
            <mc:ModalControl id="hmLoginModal" runat="server"></mc:ModalControl>
            <div class="form-inline mt-2 mt-md-0">
                <div class="text-center p-1">
                    <%--viewBox is an attribute of the <svg> element. 
                            x coordinate in the scaled viewBox coordinate system to use for the top left corner of the SVG viewport
                            y coordinate in the scaled viewBox coordinate system to use for the top left corner of the SVG viewport 
                            width in coordinates/px units within the SVG code/doc scaled to fill the available width
                            height in coordinates/px units within the SVG code/doc scaled to fill the available height
                            For more info see: https://css-tricks.com/scale-svg/--%>        
                    <div id="svgAvatar" runat="server">
                        <svg viewBox="0 0 534 534" width="50" height="50">
                            <use href="Images/..."/>
                        </svg>
                    </div>
                    <div>
				        <asp:Label ID="lblUserName" runat="server" Width="50" Height="50"></asp:Label>
                    </div>
			    </div>
            </div>
        </nav>
        <%--End NavBar--%>
        <%--Start Login--%> 
        <div id="loginCard" class="w-75">
            <div class="card">
                <div class="card-body">
                    <h3 class="text-center">Please Log In</h3>
                    <div class="form-group">
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-text text w-100" placeholder="Email address"></asp:TextBox>
                        <asp:TextBox ID="txtPass" runat="server" CssClass="form-text w-100" placeholder="Password" TextMode="Password" ></asp:TextBox>
                        <div class="checkbox mb-3 text-center">
                            <label for="chkRemember">
                                <asp:CheckBox ID="chkRemember" runat="server" value="remember-me"/>Remember me
                            </label>
                        </div>
                        <div class="text-center">
                            <%--btn-block: block level buttons span the full width of a parent--%>
                            <asp:Button ID="btnSignIn" Text="LogIn" runat="server" CssClass="btn btn-block btn-primary mb-2" OnClick="btnLogin_Click"/>
                            <asp:Button ID="btnCreateAcct" Text="Create Account" runat="server" CssClass="btn btn-block btn-outline-secondary" OnClick="btnCreateAccount_Click"/>
                        </div>
                        <div>
                            <p class="text-center mt-1">Please click "Help/About" on each page to get tips and technicals details. Thank you for visiting.</p>
                        </div>
                    </div>
                </div>
            </div>
            <div>
                <asp:HyperLink ID="lnkForgotPassword" NavigateUrl="ForgotPassword.aspx" runat="server"><u>Forgot Password?</u></asp:HyperLink>
            </div>
        </div>
        <%--End Login--%> 
    </form>
    </body>
</html>

