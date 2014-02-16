<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" ValidateRequest="false" 
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LetterAmazer.Websites.OfficeTool._Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <div class="officeTool">
    <h1>Office tool</h1>

    <h3>Fulfillement partners</h3>
    <asp:DropDownList ID="FulfilmentPartnersDdl" AutoPostBack="True"
        runat="server">
    </asp:DropDownList>

    <h3>Offices</h3>
    <asp:DropDownList ID="OfficesDdl" runat="server" AutoPostBack="True">
    </asp:DropDownList>

    <hr />

    <table>
        <tr>
            <td>
                <p>
                    <b>Letter color:</b><br />
                    <asp:DropDownList runat="server" ID="LetterColorDdl"></asp:DropDownList>
                </p>
            </td>
            <td>
                <p>
                    <b>Letter processing:</b><br />
                    <asp:DropDownList runat="server" ID="LetterProcessingDdl" />
                </p>
            </td>
            <td>
                <p>
                    <b>Letter size:</b><br />
                    <asp:DropDownList runat="server" ID="LetterSizeDdl" />
                </p>
            </td>

        </tr>
        <tr>
            <td>
                <p>
                    <b>Letter type:</b><br />
                    <asp:DropDownList runat="server" ID="LetterTypeDdl" />
                </p>
            </td>
            <td>
                <p>
                    <b>Letter weight:</b><br />
                    <asp:DropDownList runat="server" ID="LetterWeightDdl" />
                </p>
            </td>
            <td></td>

        </tr>
    </table>
    
    <h3>Location</h3>
    
        <div class="locationType">
    <asp:RadioButtonList ID="TypeOfLocationRbl" runat="server" >
        <asp:ListItem>ROW</asp:ListItem>
        <asp:ListItem Selected="True">Continent</asp:ListItem>
        <asp:ListItem>Country</asp:ListItem>
    </asp:RadioButtonList>
    </div>
    Select continent: <asp:DropDownList runat="server" ID="ContinentsDdl"/>
        <br/>
    Select country:  <asp:DropDownList runat="server" ID="CountryDll"/>

    <h3>Orderlines</h3>
    
    <span id="moreBtn">(+)</span>

    <asp:Panel runat="server" ID="OrderLinesPnl">
 
    </asp:Panel>
    
    <asp:Button runat="server" ID="SubmitBtn" Text="Create product" OnClick="SubmitBtn_Click"/>

    </div>

    <script>
        var count = 0;
        $(document).ready(function () {
            addNewControl();

            $('#moreBtn').click(function() {
                addNewControl();
            });

        });
        
        function addNewControl() {

            var orderLineDivId = "orderlinesDiv" + count;
            var orderLineTitleId = "orderlines_title_" + count;
            var orderLineValueId = "orderlines_value_" + count;
            
            var parent = $('#<%=OrderLinesPnl.ClientID %>');

            var newCtl = '<div id="'+orderLineDivId+'">' +
                '<table><tr><td><div><input id="' + orderLineTitleId + '" name="' + orderLineTitleId + '" type="text" /></div></td>' +
                '<td><div><input id="' + orderLineValueId + '" name="' + orderLineValueId + '" type="text" /></div></td>' +
                '</tr></table></div>';

            parent.append(newCtl);
            count++;
        }

    </script>



</asp:Content>
