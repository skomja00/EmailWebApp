<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateAcct.aspx.cs" Inherits="EmailWebApp.CreateAcct" %>

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
        <!--Start NavBar-->
        <nav class="navbar fixed-top navigation border border-primary rounded" id="mainNav">
            <%--Navigation Logo--%>
            <span class="border border-primary rounded bg-white p-3 ml-3">Email WebApp</span>
            <div class="btn-toolbar ml-2">
                <div class="btn-group">
                    <div>
                        <asp:LinkButton runat="server" CssClass="btn btn-default dropdown-toggle m-1" data-toggle="dropdown">
                            Menu<span class="caret"></span></asp:LinkButton>
                        <ul class="dropdown-menu">
                            <li><asp:Button ID="btnNavLogIn" class="btn btn-default" runat="server" 
                                Text="Login" OnClick="btnLogIn_Click"></asp:Button></li>
                            <li><asp:Button ID="btnNavCreateAccount" CssClass="btn btn-default" runat="server" 
                                Text="Create Account" OnClick="btnCreateAccount_Click"></asp:Button></li>
                        </ul>
                    </div>
                </div>
            </div>
            <mc:ModalControl id="mcCreateAcctModal" runat="server"></mc:ModalControl>
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
        <%--Start Create Account Card--%>
        <div class="w-75">
            <div id="createAcctCard" class="card">
                <div class="card-body">
                    <div class="container">
                        <div class="row justify-content-center mb-2">
                            <div class="col-auto">
                                <img src="Images/mail.svg"/>
                                &nbsp;
                                <asp:Label ID="lblTopMessage" CssClass="h4" runat="server" Text="Enter your email account information."></asp:Label>
                            </div>
                        </div>
                        <div class="row mb-1">
                            <%--Create Account UserName and Avatar--%>
                            <div class="col-md-6 mb-1">
                                <label for="username">Username</label>
                                <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" placeholder="jim"></asp:TextBox>
                                <div class="invalid-feedback">Your username is required.</div>
                            </div>
                            <div class="col-md-6 mb-1">
                                <label for="avatar">Select an avatar:</label> <br />
                                <asp:Image ID="imgAvatar" runat="server" ImageUrl="Images/default-avatar.svg" CssClass="imgAvatarW10" />
                                <asp:DropDownList ID="ddlAvatar" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAvatar_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row mb-1">
                            <%--Create Account Address and PhoneNumber--%>
                            <div class="col-md-6 mb-1">
                                <label for="txtAddress">Address</label>
                                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" placeholder="123 Main St"></asp:TextBox>
                                <div class="invalid-feedback">Your address is required.</div>
                            </div>
                            <div class="col-md-6 mb-1">
                                <label for="txtPhoneNumber">Phone Number</label>
                                <asp:TextBox ID="txtPhoneNumber" runat="server" CssClass="form-control" placeholder="(123) 345-7890"></asp:TextBox>
                                <div class="invalid-feedback">Your phone number is required.</div>
                            </div> 
                        </div>
                        <div class="row mb-1">
                            <%--Create Account Contact Email and Security Email--%>
                            <div class="col-md-6 mb-1">
                                <label for="txtEmail">Email Address</label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Unique, unused email address."></asp:TextBox>
                                <div class="invalid-feedback">Email address is required.</div>
                            </div>
                            <div class="col-md-6 mb-1">
                                <label for="txtEmailSecurity">Security email</label>
                                <asp:TextBox ID="txtEmailSecurity" runat="server" CssClass="form-control" placeholder="Security email"></asp:TextBox>
                                <div class="invalid-feedback">Your security email is required.</div>
                            </div> 
                        </div>
                        <div class="row mb-1">
                            <%--Create Account Password and Confirm Password--%>
                            <div class="col-md-6 mb-1">
                                <label for="txtPassword">Password</label>
                                <asp:TextBox ID="txtPassword" runat="server" Type="Password" CssClass="form-control" placeholder="Password"></asp:TextBox>
                                <div class="invalid-feedback">Your password is required.</div>
                            </div>
                            <div class="col-md-6 mb-1">
                                <label for="txtPasswordConfirm">Confirm Password</label>
                                <asp:TextBox ID="txtPasswordConfirm" runat="server"  Type="Password" CssClass="form-control" placeholder="Confirm"></asp:TextBox>
                                <div class="invalid-feedback">Your confirmation password is required.</div>
                            </div> 
                        </div>
                        <div class="row mb-1">
                            <%--Security Question #1--%>
                            <div class="col-md-10 mb-2">
                                <label for="lblSecQuestionCity">In what town or city was your first full time job?</label>
                                <asp:TextBox ID="txtSecQuestionCity" runat="server" CssClass="form-control" placeholder="In what town..."></asp:TextBox>
                                <div class="invalid-feedback">Your response is required.</div>
                            </div>
                        </div>                        
                        <div class="row mb-1">
                            <%--Security Question #2--%>
                            <div class="col-md-10 mb-2">
                                <label for="lblSecQuestionPhone">What were the last four digits of your childhood telephone number?</label>
                                <asp:TextBox ID="txtSecQuestionPhone" runat="server" CssClass="form-control" placeholder="What were the last four..."></asp:TextBox>
                                <div class="invalid-feedback">Your response is required.</div>
                            </div>
                        </div>                        
                        <div class="row mb-1">
                            <%--Security Question #3--%>
                            <div class="col-md-10 mb-2">
                                <label for="lblSecQuestionSchool">What primary school did you attend?</label>
                                <asp:TextBox ID="txtSecQuestionSchool" runat="server" CssClass="form-control" placeholder="What primary school..."></asp:TextBox>
                                <div class="invalid-feedback">Your response is required.</div>
                            </div>
                        </div>                        
                        <div class="row mb-1">
                            <%--Create Account Type of User--%>
                            <div class="col-md-12">
                                <h4 class="text-center">Please choose a type of user</h4>
                            </div>
                        </div>
                        <div class="row mb-1 justify-content-center">
                            <div class="col-auto"> 
                                <asp:RadioButtonList ID="rblUserType" runat="server" RepeatDirection="Horizontal" CssClass="table borderless table-responsive">
                                    <asp:ListItem>User</asp:ListItem>
                                    <asp:ListItem>Administrator</asp:ListItem>
                                </asp:RadioButtonList>
                                <div class="invalid-feedback">Type of user is required.</div>
                            </div>
                        </div>
                        <div class="row mb-1 text-center">
                            <%--Create Account Create or SignIn Buttons--%>
                            <div class="col-md-12">
                                <asp:Button ID="btnCreateAccount" runat="server" Text="Create Account" CssClass="btn btn-primary" OnClick="btnCreateAccount_Click" />
                                <asp:Button ID="btnLogIn" runat="server" Text="Go to Sign In" CssClass="btn btn-outline-secondary" OnClick="btnLogIn_Click" />
                            </div> 
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%--End Create Account Card--%>
    </form>
</body>
</html>
