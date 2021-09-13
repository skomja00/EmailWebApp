<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="EmailWebApp.ForgotPassword" %>

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
                                Text="Login" OnClick="btnLogin_Click"></asp:Button></li>
                            <li><asp:Button ID="btnCreateAccount" CssClass="btn btn-default m-1" runat="server" 
                                Text="Create Account" OnClick="btnCreateAccount_Click"></asp:Button></li>
						</ul>
					</div>
				</div>
			</div>


            <mc:ModalControl id="hmForgotPassModal" runat="server"></mc:ModalControl>
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
        <%--Security Questions--%> 
        <div id="securityCard" class="w-85 text-black" runat="server">
            <div class="card">
                <div class="card-body w-100">
                    <h3 class="text-center">Please answer the Security Questions to help us verify your identity.</h3>
                    <div class="form-group">
                        <div class="container">
                            <div class="row mb-3">
                                <div class="col-sm-3 text-right">
                                    <span class="mr-1">Email Address:</span>
                                </div>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="form-text text w-100"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="col-sm-3 text-right">
                                    <span class="mr-1">Security Question:</span>
                                </div>
                                <div class="col-sm-8">
                                    <asp:Label ID="lblQuestionCity" runat="server" Text="In what town or city was your first full time job?"></asp:Label>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="col-sm-3 text-right">
                                    <span class="mr-1">Answer:</span>
                                </div>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtResponseCity" runat="server" CssClass="form-text text w-100" placeholder="In what town or city..."></asp:TextBox>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="col-sm-3 text-right">
                                    <span class="mr-1">Security Question:</span>
                                </div>
                                <div class="col-sm-8">
                                    <asp:Label ID="lblQuestionPhone" runat="server" Text="What were the last four digits of your childhood telephone number?"></asp:Label>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="col-sm-3 text-right">
                                    <span class="mr-1">Answer:</span>
                                </div>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtResponsePhone" runat="server" CssClass="form-text w-100" placeholder="What were the last four..."></asp:TextBox>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="col-sm-3 text-right">
                                    <span class="mr-1">Security Question:</span>
                                </div>
                                <div class="col-sm-8">
                                    <asp:Label ID="lblQuestionSchool" runat="server" Text="What primary school did you attend?"></asp:Label>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="col-sm-3 text-right">
                                    <span class="mr-1">Answer:</span>
                                </div>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtResponseSchool" runat="server" CssClass="form-text w-100" placeholder="What primary school..."></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <%--btn-block: block level buttons span the full width of a parent--%>
                        <asp:Button ID="btnCheckQuestions" Text="Submit" runat="server" CssClass="btn btn-block btn-primary m-2" OnClick="btnCheckQuestions_Click"/>
                    </div>
                </div>
            </div>
        </div>
        <%--End Security Questions--%> 
        <%--New Password--%> 
        <div id="newPasswordCard" class="w-75 text-black" runat="server">
            <div class="card">
                <div class="card-body w-100">
                    <h3 class="text-center">You answered correctly. Please enter your new password.</h3>
                    <div class="form-group">
                        <div class="container">
                            <div class="row mb-3">
                                <div class="col-sm-3 text-right">
                                    <span class="mr-1">New Password:</span>
                                </div>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-text text w-100" TextMode="Password" placeholder="Type your new password..."></asp:TextBox>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="col-sm-3 text-right">
                                    <span class="mr-1">Confirm:</span>
                                </div>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtPasswordConfirm" runat="server" CssClass="form-text text w-100" TextMode="Password" placeholder="Confirm password..."></asp:TextBox>
                                </div>
                            </div>
                            <%--btn-block: block level buttons span the full width of a parent--%>
                            <asp:Button ID="btnSaveNewPassword" Text="Submit" runat="server" CssClass="btn btn-block btn-primary m-2" OnClick="btnSaveNewPassword_Click"/>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%--End New Password--%> 
    </form>
</body>
</html>


