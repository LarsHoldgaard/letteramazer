<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LetterAmazer.Websites.OfficeTool._Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

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

    <h3>Orderlines</h3>
    
    <a id="moreBtn" href="#">(+)</a>

    <asp:Panel runat="server" ID="OrderLinesPnl">
 
    </asp:Panel>
    
    <asp:Button runat="server" ID="SubmitBtn" Text="Create product" OnClick="SubmitBtn_Click"/>



    <script>
        var count = 0;
        $(document).ready(function() {
            $('#moreBtn').click(function() {
                addNewControl();
            });
        });
        
        function addNewControl() {

            var orderLineDivId = "orderlinesDiv" + count;
            var orderLineTitleId = "orderlines_title" + count;
            var orderLineValueId = "orderlines_value" + count;
            
            var parent = $('#<%=OrderLinesPnl.ClientID %>');

            var newCtl = '<div id="'+orderLineDivId+'">' +
                '<div id="' + orderLineTitleId + '"><input type="text" /></div>' +
                '<div id="' + orderLineValueId + '"><input type="text" /></div>' +
                '</div>';

            parent.append(newCtl);
        }

    </script>



</asp:Content>
