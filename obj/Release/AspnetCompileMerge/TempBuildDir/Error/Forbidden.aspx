<%@ Page Language="C#" MasterPageFile="~/CPanel.Master" AutoEventWireup="true" CodeBehind="Forbidden.aspx.cs" Inherits="CMS.Error.Forbidden" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1><i class="icon-lock text-danger"></i> Forbidden</h1>
    You dont have access to this page. Please contact your system administrator
    <%: ErrorPage %>
</asp:Content>
