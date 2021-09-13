<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountClient.aspx.cs" Inherits="EmailWebApp.AccountClient" %>

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

    <%--font-awesome icon, SVG, font, and CSS toolkit--%>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.3/css/all.min.css" integrity="sha512-iBBXm8fW90+nuLcSKlbmrPcLa0OT92xO1BIsZ+ywDWZCvqsWgccV3gFoRBv0z+8dLJgyAHIhR35VZc2oM/gI1w==" crossorigin="anonymous" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <%--Start NavBar--%>
        <nav class="navbar fixed-top navigation border border-primary rounded">
            <%--Navigation Logo--%>
			<span class="border border-primary rounded bg-white p-3 m-3">Email WebApp</span>
            <%--Navigation drop down--%>
            <div class="btn-toolbar">
                <div class="btn-group">
                    <div>
                        <asp:LinkButton runat="server" CssClass="btn btn-default dropdown-toggle" data-toggle="dropdown">
                            Menu<span class="caret"></span></asp:LinkButton>
                        <ul class="dropdown-menu">
                            <li><asp:Button ID="btnNavLogout" CssClass="btn btn-default" runat="server" 
                                Text="Logout" OnClick="btnNavLogout_Click"></asp:Button></li>
                        </ul>
                    </div>
                </div>
            </div>
            <%--Navigation Help--%>
            <mc:ModalControl id="hmAccountClientModal" runat="server"></mc:ModalControl>
            <div class="align-content-center">
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
            <div class="container-fluid" runat="server">
                <div class="row">
                    <%--sidebar--%>
                    <div class="col-sm-2 sidebarContent">
                        <div class="sidebar-heading text-center">
                            <asp:Button ID="btnCreate" runat="server" Text="Create Email" CssClass="btn btn-primary" OnClick="btnCreate_Click" />
                        </div>
                        <div class="list-group list-group-flush text-center">
                            <asp:LinkButton ID="btnInbox" runat="server" CssClass="list-group-item list-group-item-action font-weight-normal" 
                                OnClick="btnInbox_Click">Inbox</asp:LinkButton>
                            <asp:LinkButton ID="btnSent" runat="server" CssClass="list-group-item list-group-item-action font-weight-normal" 
                                OnClick="btnSent_Click">Sent</asp:LinkButton>
                            <asp:LinkButton ID="btnFlag" runat="server" CssClass="list-group-item list-group-item-action font-weight-normal" 
                                OnClick="btnFlag_Click">Flag</asp:LinkButton>
                            <asp:LinkButton ID="btnJunk" runat="server" CssClass="list-group-item list-group-item-action font-weight-normal" 
                                OnClick="btnJunk_Click">Junk</asp:LinkButton>
                            <asp:LinkButton ID="btnTrash" runat="server" CssClass="list-group-item list-group-item-action font-weight-normal" 
                                OnClick="btnTrash_Click">Trash</asp:LinkButton>
                            <asp:DropDownList ID="ddlCustomTags" runat="server" CssClass="list-group-item list-group-item-action" 
                                AutoPostBack="True" 
                                placeholder="Select Folder..."
                                OnSelectedIndexChanged="ddlCustomTags_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                    </div>
                    <%--main email content--%>
                    <div class="col-sm-10">
                        <%--email list--%>
                        <div id="emailList" runat="server">
                            <!--email list controls-->
                            <div class="form-inline">
                                <asp:DropDownList ID="ddlMoveTo" runat="server"
                                    CssClass="w-auto"
                                    AutoPostBack="true" 
                                    placeholder="Move To..."
                                    OnSelectedIndexChanged="ddlMoveTo_SelectedIndexChanged" Width="204px"></asp:DropDownList>
                                <asp:Button ID="btnFlagEmail" runat="server" Text="Flag" CssClass="btn btn-secondary m-1 p-1" OnClick="btnFlagEmail_Click" Width="40px"/>
                                <asp:Button ID="btnUnFlagEmail" runat="server" Text="UnFlag" CssClass="btn btn-secondary m-1 p-1" OnClick="btnUnFlagEmail_Click" Width="60px"/>
                                <asp:TextBox ID="txtAddTag" runat="server" CssClass="m-1 p-1" placeholder="Add folder..."  ></asp:TextBox>
                                <asp:Button ID="btnAddTag" runat="server" Text="Add Folder" CssClass="btn btn-secondary m-1 p-1" OnClick="btnAddTag_Click" Width="93px" />
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="m-1 p-1" placeholder="Search Email..."></asp:TextBox>
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-secondary m-1 p-1" OnClick="btnSearch_Click" Width="57px" />
                            </div>
                            <asp:GridView ID="gvEmail" runat="server" AutoGenerateColumns="false" 
                                ShowHeaderWhenEmpty="true" GridLines="Horizontal" 
                                AutoGenerateSelectButton="True"
                                onselectedindexchanged="gvEmail_SelectedIndexChanged"
                                CssClass="table table-border table-hover"
                                OnRowDataBound="gvEmail_OnRowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelectEmail" runat="server" AutoPostBack="true" oncheckedchanged="chkSelectEmail_CheckedChanged" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="UserNameSend" HeaderText="Sent" />
                                    <asp:TemplateField HeaderText="Recv">
                                        <ItemTemplate>
                                            <asp:PlaceHolder ID="RecvEmail" runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:BoundField DataField="EmailSubject" HeaderText="Subject" />
                                    <asp:BoundField DataField="EmailBody" HeaderText="Content" />
                                    <asp:BoundField DataField="DateTimeStamp" HeaderText="DateTimeStamp" DataFormatString="{0:MM/dd/yyyy h:mm tt}" />
                                </Columns>
                                <RowStyle VerticalAlign="Middle" />
                                <EmptyDataTemplate>
                                    <div class="text-center">You have no email.</div>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                        <%--create email--%>
                        <div class="row d-flex" id="createEmail" runat="server">
                            <div class="container-fluid" runat="server">
                                <asp:Label ID="lblCreateEmail" runat="server"><h3>New Message</h3></asp:Label>

                                <asp:Label ID="lblSendEmailAddr" runat="server">From: </asp:Label>
                                <asp:TextBox ID="txtSendEmailAddr" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
 
                                <asp:Label ID="lblRecvEmailAddr" runat="server">To: </asp:Label>
                                <asp:TextBox ID="txtRecvEmailAddr" runat="server" CssClass="form-control" placeholder="one@university.edu ; two@university.edu; etc..."></asp:TextBox>

                                <asp:Label ID="lblEmailSubject" runat="server">Subject: </asp:Label>
                                <asp:TextBox ID="txtEmailSubject" runat="server" CssClass="form-control" placeholder="Subject..."></asp:TextBox>

                                <asp:Label ID="lblEmailBody" runat="server">Email: </asp:Label>
                                <asp:TextBox ID="txtEmailBody" runat="server" CssClass="form-control h-auto" TextMode="MultiLine" placeholder="Compose email..."></asp:TextBox>

                                <asp:Button ID="btnSendEmail" runat="server" Text="Send" OnClick="btnSendEmail_Click" CssClass="mt-2"/>
                            </div>
                        </div>
                        <%--select email--%>
                        <div class="row d-flex" id="selectEmail" runat="server">
                            <div class="container-fluid" runat="server">
                                <asp:Label ID="lblSelectMessage" runat="server"><h3>Email Message</h3></asp:Label>

                                <asp:Label ID="lblSelectEmailAvatar" runat="server">Avatar: </asp:Label>
                                <asp:Image ID="imgSelectEmailAvatar" runat="server" ImageUrl="Images/" CssClass="imgAvatarW8 m-1"/><br/>

                                <asp:Label ID="lblSelectFrom" runat="server">From: </asp:Label>
                                <asp:TextBox ID="txtSelectFrom" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
 
                                <asp:Label ID="lblSelectRecv" runat="server">To: </asp:Label>
                                <asp:TextBox ID="txtSelectRecv" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>

                                <asp:Label ID="lblSelectSubject" runat="server">Subject: </asp:Label>

                                <asp:TextBox ID="txtSelectSubject" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>

                                <asp:Label ID="lblSelectDateTimeStamp" runat="server">DateTimeStamp: </asp:Label>
                                <asp:TextBox ID="txtSelectDateTimeStamp" runat="server" CssClass="form-control" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>

                                <asp:Label ID="lblSelectEmail" runat="server">Email: </asp:Label>
                                <asp:TextBox ID="txtSelectEmail" runat="server" CssClass="form-control" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <%--search email results--%>
                        <div class="row d-flex" id="searchEmail" runat="server">
                            <div class="container-fluid" runat="server">
                                <h3>Search results ...</h3>
                                <asp:GridView ID="gvSearch" runat="server" 
                                    AutoGenerateColumns="false"
                                    CssClass="table table-border table-hover"
                                    ShowHeaderWhenEmpty="true"
                                    emptydatatext="Search results not found.">
                                    <Columns>
                                        <asp:BoundField DataField="UserName" HeaderText="ReceiveUserName" />
                                        <asp:BoundField DataField="CreatedEmailAddress" HeaderText="ReceiveEmail" />
                                        <asp:BoundField DataField="UserNameSend" HeaderText="SendUserName" />
                                        <asp:BoundField DataField="CreatedEmailAddressSend" HeaderText="SendEmail" />
                                        <asp:BoundField DataField="EmailSubject" HeaderText="Subject" />
                                        <asp:BoundField DataField="EmailBody" HeaderText="Email Body" />
                                        <asp:BoundField DataField="DateTimeStamp" HeaderText="Create" DataFormatString="{0:MM/dd/yyyy h:mm tt}" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
