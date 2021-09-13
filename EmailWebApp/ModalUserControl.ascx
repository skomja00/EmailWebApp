<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModalUserControl.ascx.cs" Inherits="TermProject.ModalUserControl" %>


<div ID="" class="mr-auto">
    <asp:UpdatePanel id="upBtnHelp" runat="server"
        ChildrenAsTriggers="false"
        UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button id="btnModalUserControl" runat="server" Text="missing ButtonText" CssClass="btn btn-default m-1" OnClick="btnToggleVisibility" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<!-- The Modal -->
<asp:UpdatePanel ID="upModalDiv" runat="server"
    UpdateMode="Always">
    <ContentTemplate>
        <!-- Modal content -->
        <div id="ModalHelp" class="modal-none" runat="server">
            <div id="ModalContent" class="modal-content" runat="server">
                <asp:Button runat="server" Text="Close" CssClass="btn btn-default" OnClick="btnToggleVisibility" />
                <div id="contentDiv" runat="server">
                    <%--Content inserted in the code-behind--%>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>