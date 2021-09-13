<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminClient.aspx.cs" Inherits="EmailWebApp.AdminClient" %>

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
<body>
    <form id="form1" class="p-1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <!-- Navigation -->
        <nav class="navbar fixed-top navigation border border-primary rounded">
            <%--Navigation Logo--%>
            <span class="border border-primary rounded bg-white p-3 m-3">Email WebApp</span>
            <%--Navigation drop down--%>
            <div class="btn-toolbar">
                <div class="btn-group">
                    <div>
                        <asp:LinkButton runat="server" CssClass="btn btn-default dropdown-toggle" data-toggle="dropdown">
                            Menu<span class="caret"></span></asp:LinkButton>
                        <%--More info on BootStrap "Auto Margins" (ie. mr-auto)--%> 
                        <%--https://getbootstrap.com/docs/4.0/utilities/flex/#auto-margins--%>
                        <ul class="dropdown-menu mr-auto">
                            <li><asp:Button ID="btnNavLogout" CssClass="btn btn-default" runat="server" 
                                Text="Logout" OnClick="btnNavLogout_Click"></asp:Button></li>
                        </ul>
                    </div>
                </div>
            </div>
            <%--Navigation Help--%>
            <mc:ModalControl id="hmAccountClientModal" runat="server"></mc:ModalControl>
            <div class="align-content-center ml-auto">
                <div id="svgAvatar" runat="server">
                    <svg viewBox="0 0 534 534" width="50" height="50">
                        <use href="..."/>
                    </svg>
                </div>
                <div>
				    <asp:Label ID="lblNavUserName" runat="server"></asp:Label>
                </div>
            </div>
        </nav>
        <%--End Navigation--%>
        <div class="mainContent">
            <div id="emailList" class="container-fluid" runat="server">
                <div>
                    <div>
                        <h3>Account List</h3>
                        <p>Note Active Column: Banned accounts will not be active (ie. "no"). Otherwise UnBanned/Active will be "yes".</p>
                        <asp:Button ID="btnBan" runat="server" Text="Ban" OnClick="btnBan_Click" CssClass="mb-3"/>
                        <asp:Button ID="btnUnBan" runat="server" Text="UnBan" OnClick="btnUnBan_Click" CssClass="mb-3"/>
                        <%--Account List--%>
                            <%--onselectedindexchanged="gvAccounts_SelectedIndexChanged"--%>
                        <asp:GridView ID="gvAccounts" runat="server" AutoGenerateColumns="false" 
                            ShowHeaderWhenEmpty="true" GridLines="Horizontal" 
                            AutoGenerateSelectButton="false"
                            CssClass="table table-border table-hover">
                            <Columns>
                                <asp:TemplateField HeaderText="Select">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelectEmail" runat="server" AutoPostBack="true" oncheckedchanged="chkSelectEmail_CheckedChanged" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Active" HeaderText="Active" />
                                <asp:BoundField DataField="UserName" HeaderText="UserName" />
                                <asp:BoundField DataField="PhoneNumber" HeaderText="PhoneNumber" />
                                <asp:BoundField DataField="UserAddress" HeaderText="UserAddress" />
                                <asp:BoundField DataField="CreatedEmailAddress" HeaderText="EmailAddress" />
                                <asp:TemplateField HeaderText="Avatar">
                                    <ItemTemplate>
                                        <asp:Image ID="gvAccountsAvatar" runat="server" ImageUrl="Images/" CssClass="imgAvatarW10"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <RowStyle VerticalAlign="Middle" />
                            <EmptyDataTemplate>
                                <div class="text-center">You have no accounts.</div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                        <%--Flagged Email List--%>
                        <h3>Flagged Email List</h3>
                        <p>This list contains all email in the system "Banned" or in a "Flag" folder.</p>
                            <%--onselectedindexchanged="gvFlaggedEmails_SelectedIndexChanged"--%>
                        <asp:GridView ID="gvFlaggedEmails" runat="server" AutoGenerateColumns="false" 
                            ShowHeaderWhenEmpty="true" GridLines="Horizontal" 
                            AutoGenerateSelectButton="false"
                            CssClass="table table-border table-hover">
                            <Columns>
                                <asp:BoundField DataField="CreatedEmailAddressSend" HeaderText="SenderEmail" />
                                <asp:BoundField DataField="CreatedEmailAddressRecv" HeaderText="ReceiveEmail" />
                                <asp:BoundField DataField="EmailSubject" HeaderText="Subject" />
                                <asp:BoundField DataField="EmailBody" HeaderText="Content" />
                                <asp:BoundField DataField="DateTimeStamp" HeaderText="DateTimeStamp" />
                            </Columns>
                            <RowStyle VerticalAlign="Middle" />
                            <EmptyDataTemplate>
                                <div class="text-center">You have no flagged emails.</div>
                            </EmptyDataTemplate>
                        </asp:GridView>                    
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>

