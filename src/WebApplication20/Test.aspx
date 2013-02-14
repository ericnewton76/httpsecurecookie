<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="WebApplication20.Test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
	<p>
	<asp:TextBox runat="server" ID="txtCookieValue" /> <asp:Button runat="server" ID="btnSet" Text="Set" OnClick="btnSet_Click" /> <asp:Button runat="server" ID="btnExpire" Text="Expire it" OnClick="btnExpire_Click" />
	</p>

	<p>
	<asp:TextBox runat="server" ID="txt1" Width="600px" ReadOnly="true" /><br />
	<asp:TextBox runat="server" ID="txt2" TextMode="MultiLine" Rows="5" Width="600px" ReadOnly="true" />
	</p>

    </div>
    </form>
</body>
</html>
