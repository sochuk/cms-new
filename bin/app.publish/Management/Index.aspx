<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CPanel.Master"  CodeBehind="Index.aspx.cs" Inherits="CMS.Management.Index" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<% 
    Response.Write("<br/> <i>HttpContext.Current.Request.Url.Host</i>: " + HttpContext.Current.Request.Url.Host);
    Response.Write("<br/> <i>HttpContext.Current.Request.Url.Authority</i> :" + HttpContext.Current.Request.Url.Authority);
    Response.Write("<br/> <i>HttpContext.Current.Request.Url.Port</i> :" + HttpContext.Current.Request.Url.Port);
    Response.Write("<br/> <i>HttpContext.Current.Request.Url.AbsolutePath</i> :" + HttpContext.Current.Request.Url.AbsolutePath);
    Response.Write("<br/> <i>HttpContext.Current.Request.ApplicationPath</i> :" + HttpContext.Current.Request.ApplicationPath);
    Response.Write("<br/> <i>HttpContext.Current.Request.Url.AbsoluteUri</i> :" + HttpContext.Current.Request.Url.AbsoluteUri);
    Response.Write("<br/> <i>HttpContext.Current.Request.Url.PathAndQuery</i> :" + HttpContext.Current.Request.Url.PathAndQuery);
    Response.Write("<br/> <i>HttpContext.Current.Request.Url.LocalPath</i> :" + HttpContext.Current.Request.Url.LocalPath);
    Response.Write("<br/> <i>HttpContext.Current.Request.Url.OriginalString</i> :" + HttpContext.Current.Request.Url.OriginalString);
    Response.Write("<br/> <i>HttpContext.Current.Request.Url.Query</i> :" + HttpContext.Current.Request.Url.Query);
%>
</asp:Content>