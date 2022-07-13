<%@ Page Language="C#" MasterPageFile="~/CPanel.Master" AutoEventWireup="true" CodeBehind="Display.aspx.cs" Inherits="CMS.Monitoring.Display" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="Header" runat="server">
    <%: Scripts.Render("~/Content/js/dev/devextreme") %> 
    <%: Styles.Render("~/Content/css/dev/devextreme") %> 
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="mdc-layout-grid">
        <div class="mdc-layout-grid__inner">
                <%
                    DataTable server = M_Server.SelectList();
                    foreach(DataRow row in server.Rows)
                    {
                        string output = string.Empty;
                        output += "<div class='mdc-layout-grid__cell stretch-card mdc-layout-grid__cell--span-12'>";
                        output += "<div class='mdc-card info-card info-card--success d-inline'>";
                        output += "<div class='text-center'>";
                        output += "<h4 class='card-title mb-0'>"+ row["server_name"].ToString().ToUpper() +"</h4>";
                        output += "</div>";
                        output += "<div class='d-flex justify-content-between'>";
                        output += string.Format("<div id='gaugeCPUServer{0}' style='width:250px'></div>", row["server_id"].ToString());
                        output += string.Format("<div id='gaugeRAMServer{0}' style='width:250px'></div>", row["server_id"].ToString());
                        output += string.Format("<div id='gaugeDISKServer{0}' style='width:250px'></div>", row["server_id"].ToString());
                        output += string.Format("<div id='gaugeNETWORKServer{0}' style='width:250px'></div>", row["server_id"].ToString());
                        output += "</div>";
                        output += "</div>";
                        output += "</div>";
                        Response.Write(output);
                    }
                %>
            </div>
    </div>
    
    <script>
    window.paceOptions = {
        ajax: false,
        restartOnRequestAfter: false,
    };
    </script>
    <script type="text/javascript">
        $(function () {
            var options = {
                scale: {
                    startValue: 0,
                    endValue: 100,
                    tickInterval: 10,
                    label: {
                        useRangeColors: true,
                        customizeText: function (arg) {
                            return arg.valueText + " %";
                        }
                    }
                },
                rangeContainer: {
                    palette: "pastel",
                    ranges: [
                        { startValue: 0, endValue: 60 },
                        { startValue: 60, endValue: 85 },
                        { startValue: 85, endValue: 100 }
                    ]
                },
                title: {
                    font: { size: 18 },
                    horizontalAlignment: "center",
                    verticalAlignment: "bottom",
                },
                valueIndicator: {
                    color: "#483D8B"
                },
            };

        <% 
        foreach(DataRow row in server.Rows)
        {
            string serverId = row["server_id"].ToString();
            string data = "var gaugeCPUServer"+ serverId +" = $('#gaugeCPUServer"+ serverId +"').dxCircularGauge(options).dxCircularGauge({ title: { text: 'CPU' } }).dxCircularGauge('instance');";
            data += "var gaugeRAMServer"+ serverId +" = $('#gaugeRAMServer"+ serverId +"').dxCircularGauge(options).dxCircularGauge({ title: { text: 'RAM' } }).dxCircularGauge('instance');";
            data += "var gaugeDISKServer"+ serverId +" = $('#gaugeDISKServer"+ serverId +"').dxCircularGauge(options).dxCircularGauge({ title: { text: 'DISK' } }).dxCircularGauge('instance');";
            data += "var gaugeNETWORKServer"+ serverId +" = $('#gaugeNETWORKServer"+ serverId +"').dxCircularGauge(options).dxCircularGauge({ title: { text: 'NETWORK' } }).dxCircularGauge('instance');";
            Response.Write(data);
        }
        %>
            
            
        <% 
        foreach(DataRow row in server.Rows)
        {
            string serverId = row["server_id"].ToString();
            string data = "function getServerInfo"+ serverId +"() {";
            //data += "$.getJSON('http://localhost:64041/api/check/cpu').done(function (data) {";
            data += "$.getJSON('http://"+ row["ip_address"] +"/srvmon/api/resource/get').done(function (data) {";
            data += "gaugeCPUServer"+ serverId +".option('value', data.CPU.value);";
            data += "gaugeRAMServer"+ serverId +".option('value', data.RAM.value);";
            data += "gaugeDISKServer"+ serverId +".option('value', data.DISK.value);";
            data += "gaugeNETWORKServer"+ serverId +".option('value', data.NETWORK.value);";
            data += "})";
            data += "}";
            data += "getServerInfo"+ serverId +"();";
            data += "repeatEvery(getServerInfo" + serverId + ", (2 * 1000));";
            Response.Write(data);
        }
        %>
        });
    </script>
</asp:Content>

