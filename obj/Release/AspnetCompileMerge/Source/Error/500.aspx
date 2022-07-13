<%@ Page Language="C#" MasterPageFile="~/CPanel.Master"  AutoEventWireup="true" CodeBehind="500.aspx.cs" Inherits="CMS.Error.InternalError" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <%  exception = exception ?? new Exception();
        switch (exception.HResult)
        {
            case -2146233078:
    %>
        <title>Forbidden Access</title>
        <h1><i class="icon-lock text-danger"></i> Forbidden</h1>
        You dont have access to this page. Please contact your system administrator
    <%
            break;
            default:
    %>
        <title>Internal Error</title>
        <h1><i class="icon-shield text-danger"></i> Error</h1>
        <p>Internal server error. Please contact your system administrator</p>
    <% if (ErrorText.Length > 0){ %>
        <div class="alert alert-warning" role="alert">
            <h5 class="alert-heading mb-0"><i class="icon-envelope"></i></h5>
            <hr class="mb-2 mt-2">
            <p class="mb-0"><i class="icon-close"></i> <%: ErrorText %></p>
        </div>
    <%
       }
            break;
            case 0:
    %>
        <h1><i class="icon-settings text-danger"></i> Under Maintenance</h1>
        <p>This module under maintenance. Please contact your system administrator</p>
    <%
            break;
       }
       
    %>
    
</asp:Content>
