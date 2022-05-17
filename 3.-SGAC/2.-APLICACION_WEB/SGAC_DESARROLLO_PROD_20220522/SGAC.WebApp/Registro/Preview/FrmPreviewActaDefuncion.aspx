<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmPreviewActaDefuncion.aspx.cs" Inherits="SGAC.WebApp.Registro.Preview.FrmPreviewActaDefuncion" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">
<head runat="server">
    <title></title>
    <meta http-equiv="content-type" content="text/html;charset=utf-8"/>
    <meta http-equiv="Content-Style-Type" content="text/css"/>
    <style type="text/css">
        @page 
        {        
            margin:  0in auto 0in auto;
            mso-header-margin:0in; 
            mso-footer-margin:0in; 
            mso-paper-source:0;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
    <asp:Button ID="Button1" runat="server" Text="Button" onclick="Button1_Click" />
        <div align="left">&nbsp;&nbsp;</div>
        <div align="left">&nbsp;&nbsp;</div>
        <div align="left">&nbsp;&nbsp;</div>
        <div align="left">&nbsp;&nbsp;</div>
        <div align="left">&nbsp;&nbsp;</div>
        <div align="left">&nbsp;&nbsp;</div>

        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:223px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt"><asp:Literal ID="lt_dia_1" runat="server"></asp:Literal></span></font></div>
        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:2px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt"><asp:Literal ID="lt_dia_2" runat="server"></asp:Literal></span></font></div>

        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:20px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt"><asp:Literal ID="lt_mes_1" runat="server"></asp:Literal></span></font></div>
        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:2px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt"><asp:Literal ID="lt_mes_2" runat="server"></asp:Literal></span></font></div>

        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:20px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt"><asp:Literal ID="lt_anio_1" runat="server"></asp:Literal></span></font></div>
        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:2px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt"><asp:Literal ID="lt_anio_2" runat="server"></asp:Literal></span></font></div>
        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:2px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt"><asp:Literal ID="lt_anio_3" runat="server"></asp:Literal></span></font></div>
        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:2px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt"><asp:Literal ID="lt_anio_4" runat="server"></asp:Literal></span></font></div>

        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:115px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt"><asp:Literal ID="lt_hora_1" runat="server"></asp:Literal></span></font></div>
        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:3px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt"><asp:Literal ID="lt_hora_2" runat="server"></asp:Literal></span></font></div>
        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:3px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt"></span></font></div>
        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:3px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt"><asp:Literal ID="lt_minuto_1" runat="server"></asp:Literal></span></font></div>
        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:3px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt"><asp:Literal ID="lt_minuto_2" runat="server"></asp:Literal></span></font></div>
        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:15px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt"><asp:Literal ID="lt_am_1" runat="server"></asp:Literal></span></font></div>
        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:3px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt"><asp:Literal ID="lt_am_2" runat="server"></asp:Literal></span></font></div>

        <div align="left">&nbsp;&nbsp;</div>
        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:15px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt"><asp:Literal ID="lt_lo_1" runat="server"></asp:Literal></span></font></div>
        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:3px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt"><asp:Literal ID="lt_lo_2" runat="server"></asp:Literal></span></font></div>
        <div style="line-height: 150%; width:75%; display: inline-block; margin-left:18px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt"><asp:Literal ID="lt_lo_3" runat="server"></asp:Literal></span></font></div>

        <div align="left">&nbsp;&nbsp;</div>
        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:15px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[D]</span></font></div>
        <div style="line-height: 150%; width:45%; display: inline-block; margin-left:18px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[DEPARTAMENTO]</span></font></div>
        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:15px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[P_1]</span></font></div>
        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:5px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[P_1]</span></font></div>
        <div style="line-height: 150%; width:44%; display: inline-block; margin-left:20px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[PROVINCIA]</span></font></div>

        <div align="left">&nbsp;&nbsp;</div>
        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:15px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[PR]</span></font></div>
        <div style="line-height: 150%; width:45%; display: inline-block; margin-left:18px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[PR_1]</span></font></div>
        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:15px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[CE_1]</span></font></div>
        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:5px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[CE_2]</span></font></div>
        <div style="line-height: 150%; width:44%; display: inline-block; margin-left:20px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[CENTROPOBLADO]</span></font></div>

        <div align="left">&nbsp;&nbsp;</div>
        <div align="left">&nbsp;&nbsp;</div>

        <div style="line-height: 150%; width:100%; display: inline-block; margin-left:15px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[NOMBRES]</span></font></div>
        <div style="line-height: 150%; width:100%; display: inline-block; margin-left:15px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[APELLIDO_1]</span></font></div>
        <div style="line-height: 150%; width:100%; display: inline-block; margin-left:15px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[APELLIDO_2]</span></font></div>

        <div align="left">&nbsp;&nbsp;</div>

        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:95px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[DI]</span></font></div>
        <div style="line-height: 150%; width:20%; display: inline-block; margin-left:225px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[NRO_DOC]</span></font></div>
        <div style="line-height: 150%; width:10%; display: inline-block; margin-left:325px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[EDAD]</span></font></div>

        <div align="left">&nbsp;&nbsp;</div>

        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:75px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[NACIONALIDAD]</span></font></div>
        <div style="line-height: 150%; width:68%; display: inline-block; margin-left:155px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[APELLIDO_1]</span></font></div>

        <div align="left">&nbsp;&nbsp;</div>
        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:15px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[L]</span></font></div>
        <div style="line-height: 150%; width:95%; display: inline-block; margin-left:18px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[LUGAROCURRENCIA]</span></font></div>

        <div align="left">&nbsp;&nbsp;</div>
        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:15px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[D]</span></font></div>
        <div style="line-height: 150%; width:45%; display: inline-block; margin-left:18px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[DEPARTAMENTO]</span></font></div>
        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:15px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[P_1]</span></font></div>
        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:5px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[P_1]</span></font></div>
        <div style="line-height: 150%; width:44%; display: inline-block; margin-left:20px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[PROVINCIA]</span></font></div>

        <div align="left">&nbsp;&nbsp;</div>
        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:15px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[PR]</span></font></div>
        <div style="line-height: 150%; width:45%; display: inline-block; margin-left:18px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[PR_1]</span></font></div>
        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:15px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[CE_1]</span></font></div>
        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:5px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[CE_2]</span></font></div>
        <div style="line-height: 150%; width:44%; display: inline-block; margin-left:20px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[CENTROPOBLADO]</span></font></div>

        <div align="left">&nbsp;&nbsp;</div>
        <div align="left">&nbsp;&nbsp;</div>

        <div style="line-height: 150%; width:100%; display: inline-block; margin-left:15px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[NOMBRES]</span></font></div>
        <div style="line-height: 150%; width:100%; display: inline-block; margin-left:15px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[APELLIDO_1]</span></font></div>
        <div style="line-height: 150%; width:100%; display: inline-block; margin-left:15px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[APELLIDO_2]</span></font></div>

        <div align="left">&nbsp;&nbsp;</div>
        <div align="left">&nbsp;&nbsp;</div>

        <div style="line-height: 150%; width:100%; display: inline-block; margin-left:15px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[NOMBRES]</span></font></div>
        <div style="line-height: 150%; width:100%; display: inline-block; margin-left:15px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[APELLIDO_1]</span></font></div>
        <div style="line-height: 150%; width:100%; display: inline-block; margin-left:15px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[APELLIDO_2]</span></font></div>

        <div align="left">&nbsp;&nbsp;</div>
        <div align="left">&nbsp;&nbsp;</div>


        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:120px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[DIA_1]</span></font></div>
        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:3px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[DIA_2]</span></font></div>

        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:35px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[MES_1]</span></font></div>
        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:3px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[MES_2]</span></font></div>

       <%-- <div style="line-height: 150%; width:1%; display: inline-block; margin-left:35px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[ANIO_1]</span></font></div>
        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:3px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[ANIO_2]</span></font></div>
        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:3px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[ANIO_3]</span></font></div>
        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:3px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[ANIO_4]</span></font></div>


        <div align="left">&nbsp;&nbsp;</div>
        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:15px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[L]</span></font></div>
        <div style="line-height: 150%; width:95%; display: inline-block; margin-left:18px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[LUGAROCURRENCIA]</span></font></div>

        <div align="left">&nbsp;&nbsp;</div>
        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:15px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[D]</span></font></div>
        <div style="line-height: 150%; width:45%; display: inline-block; margin-left:18px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[DEPARTAMENTO]</span></font></div>
        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:15px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[P_1]</span></font></div>
        <div style="line-height: 150%; width:1%; display: inline-block; margin-left:5px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[P_1]</span></font></div>
        <div style="line-height: 150%; width:44%; display: inline-block; margin-left:20px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[PROVINCIA]</span></font></div>

        <div align="left">&nbsp;&nbsp;</div>
        <div align="left">&nbsp;&nbsp;</div>

        <div style="line-height: 150%; width:100%; display: inline-block; margin-left:15px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[NOMBRES]</span></font></div>
        <div style="line-height: 150%; width:100%; display: inline-block; margin-left:15px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[APELLIDO_1]</span></font></div>
        <div style="line-height: 150%; width:100%; display: inline-block; margin-left:15px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[APELLIDO_2]</span></font></div>

        <div align="left">&nbsp;&nbsp;</div>
        <div style="line-height: 150%; width:50%; display: inline-block; margin-left:215px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[NOMBRES]</span></font></div>
        <div style="line-height: 150%; width:90%; display: inline-block; margin-left:95px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[REGISTRADOR]</span></font></div>
        <div style="line-height: 150%; width:50%; display: inline-block; margin-left:45px;"><font face="Courier New" color="#010101" size="2"><span style=" font-size:10pt">[DNI]</span></font></div>
--%>
    </form>
</body>
</html>
