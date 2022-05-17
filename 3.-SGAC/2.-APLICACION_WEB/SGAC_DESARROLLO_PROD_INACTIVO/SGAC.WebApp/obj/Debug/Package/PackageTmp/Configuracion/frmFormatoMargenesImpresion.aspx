<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmFormatoMargenesImpresion.aspx.cs" Inherits="SGAC.WebApp.Configuracion.frmFormatoMargenesImpresion" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlOficinaConsular.ascx" TagName="ctrlOficinaConsular" TagPrefix="uc1" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlToolBarConfirm.ascx" TagPrefix="ToolBar" TagName="ToolBarContent" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlValidation.ascx" TagName="Validation" TagPrefix="Label" %>
<%@ Register Src="~/Accesorios/SharedControls/ctrlPageBar.ascx" TagName="PageBar" TagPrefix="PageBarContent" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/smoothness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/toastr.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="../Scripts/site.js" type="text/javascript"></script>
    
    <style type="text/css">
        .style2
        {
            width: 102px;
        }
        .style3
        {
            width: 100px;
        }
    </style>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <table class="mTblTituloM" align="center">
            <tr>
                <td>
                    <h2><asp:Label ID="lblTituloLibros" runat="server" Text="Configuración Márgenes de Documentos"></asp:Label></h2>
                </td>
            </tr>
        </table>

        <table style="width: 90%" align="center">
            <tr>
                <td align="left">
                    <div id="tabs">
                        <ul>
                            <li><a href="#tab-1">
                                <asp:Label ID="lblTabConsulta" runat="server" Text="Consulta"></asp:Label></a></li>
                            <li><a href="#tab-2">
                                <asp:Label ID="lblTabRegistro" runat="server" Text="Registro"></asp:Label></a></li>
                        </ul>
                        <div id="tab-1">
                            <asp:UpdatePanel ID="updConsulta" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>  
                                <asp:HiddenField ID="hdn_sOficinaConsularId" runat="server" Value="0" />                              
                                <table width="100%">  
                                    <tr>
                                        <td><asp:Label ID="lblOficinaConsularConsulta" runat="server" Text="Oficina Consular:"></asp:Label>
                                        </td>
                                        <td>
                                        <uc1:ctrlOficinaConsular ID="ddlOficinaConsularConsulta" runat="server" 
                                                Width="400px" />
                                        </td>
                                        <td colspan="2">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label ID="Label1" runat="server" Text="Documento:"></asp:Label></td>
                                        
                                        <td><asp:DropDownList ID="ddlDocumentoConsulta" runat="server">
                                            </asp:DropDownList></td>
                                        <td colspan="2">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <ToolBar:ToolBarContent ID="ctrlToolBarConsulta" runat="server"></ToolBar:ToolBarContent>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%;" colspan="2">
                                            <Label:Validation ID="ctrlValidacion" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                    <td colspan="2">
                                        <asp:GridView ID="gdvDocumentos" runat="server" CssClass="mGrid" SelectedRowStyle-CssClass="slt" Width="100%"
                                            AutoGenerateColumns="False" GridLines="None"
                                            OnRowCommand="gdvDocumentos_RowCommand">
                                            <AlternatingRowStyle CssClass="alt" />
                                            <Columns>
                                                <asp:BoundField HeaderText="Correlativo" DataField="mado_iCorrelativo">
                                                    <HeaderStyle CssClass="ColumnaOculta" />
                                                    <ItemStyle CssClass="ColumnaOculta" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="oficina" DataField="mado_sOficinaConsular">
                                                    <HeaderStyle CssClass="ColumnaOculta" />
                                                    <ItemStyle CssClass="ColumnaOculta" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Oficina Consular" DataField="OFICINA">
                                                <ItemStyle HorizontalAlign="Center" Width="200px" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="idDocumento" DataField="mado_sTipDocImpresion">
                                                    <HeaderStyle CssClass="ColumnaOculta" />
                                                    <ItemStyle CssClass="ColumnaOculta" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Documento" DataField="FORMATO">
                                                    <ItemStyle HorizontalAlign="Center" Width="250px" />
                                                </asp:BoundField>                                                
                                                <asp:BoundField HeaderText="Seccion Impresión" DataField="mado_sSeccion">
                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnConsultar" CommandName="Consultar" ToolTip="Consultar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                    runat="server" ImageUrl="../Images/img_gridbuscar.gif" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="50px"  />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnEditar" CommandName="Editar" ToolTip="Editar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                                                    runat="server" ImageUrl="../Images/img_grid_modify.png" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                        </asp:TemplateField>
                                            </Columns>
                                            <SelectedRowStyle CssClass="slt" />
                                        </asp:GridView>
                                        <PageBarContent:PageBar ID="ctrlPaginador" runat="server" />
                                    </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="tab-2">
                            <asp:UpdatePanel ID="updMantenimiento" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                
                                 <table width="100%">
                                <tr>
                                    <asp:HiddenField ID="hCodigo" runat="server" value="0"/>
                                    <asp:HiddenField ID="hEditarVer" runat="server" value="E"/>
                                        <td colspan="1" class="style2">
                                            <asp:Button ID="btnNuevo" runat="server" Text="      Nuevo" 
                                                CssClass="btnNew" onclick="btnNuevo_Click"  /> 
                                        </td>
                                        <td colspan="1" class="style2">
                                            <asp:Button ID="btnGrabarCorrelativa" runat="server" Text="      Grabar" 
                                                CssClass="btnSave" onclick="btnGrabarCorrelativa_Click"  /> 
                                        </td>
                                        <td colspan="1" class="style2">
                                            <asp:Button ID="btnEliminar" runat="server" Text="      Eliminar" 
                                                CssClass="btnDelete" onclick="btnEliminar_Click"  /> 
                                        </td>

                                        <td colspan="1">
                                        
                                        </td>

                                        
                                    </tr>
                                </table>
                           

                                <table width="100%">  
                                    

                                    <tr>
                                        <td class="style2"><asp:Label ID="lblOficinaConsular" runat="server" Text="Oficina Consular:"></asp:Label></td>
                                        <td colspan="2"><uc1:ctrlOficinaConsular ID="ddlOficinaConsularRegistro" runat="server" Width="400px" />
                                       
                                    </tr>

                                    <tr>
                                        <td class="style2"><asp:Label ID="lblTipDocumento" runat="server" Text="Documento:"></asp:Label></td>
                                        <td colspan="2">
                                        <asp:DropDownList ID="ddlDocumento" runat="server" Width="400px"></asp:DropDownList>      

                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="style2"><asp:Label ID="lblSecciones" runat="server" Text="Sección:"></asp:Label></td>
                                        <td colspan="2">
                                            <asp:DropDownList ID="ddlSeccion" runat="server">
                                            <asp:ListItem>1</asp:ListItem>
                                            <asp:ListItem>2</asp:ListItem>
                                            <asp:ListItem>3</asp:ListItem>
                                            <asp:ListItem>4</asp:ListItem>
                                            </asp:DropDownList>
                                    </tr>
                                    <tr>
                                        <td class="style2"><asp:Label ID="Label2" runat="server" Text="Descripción Sección:"></asp:Label></td>
                                        <td colspan="2">
                                        <asp:TextBox ID="txtDescripcion" runat="server" Height="39px" TextMode="MultiLine" 
                                                Width="290px"></asp:TextBox>
                                    </tr>
                                    <tr>
                                        <td class="style2"><asp:Label ID="Label3" runat="server" Text="Margen Superior:"></asp:Label></td>
                                        <td colspan="2">
                                            <asp:DropDownList ID="ddlMargenSuperior" runat="server">
                                            <asp:ListItem>-20</asp:ListItem>
                                            <asp:ListItem>-19</asp:ListItem>
                                            <asp:ListItem>-18</asp:ListItem>
                                            <asp:ListItem>-17</asp:ListItem>
                                            <asp:ListItem>-16</asp:ListItem>
                                            <asp:ListItem>-15</asp:ListItem>
                                            <asp:ListItem>-14</asp:ListItem>
                                            <asp:ListItem>-13</asp:ListItem>
                                            <asp:ListItem>-12</asp:ListItem>
                                            <asp:ListItem>-11</asp:ListItem>
                                            <asp:ListItem>-10</asp:ListItem>
                                            <asp:ListItem>-9</asp:ListItem>
                                            <asp:ListItem>-8</asp:ListItem>
                                            <asp:ListItem>-7</asp:ListItem>
                                            <asp:ListItem>-6</asp:ListItem>
                                            <asp:ListItem>-5</asp:ListItem>
                                            <asp:ListItem>-4</asp:ListItem>
                                            <asp:ListItem>-3</asp:ListItem>
                                            <asp:ListItem>-2</asp:ListItem>
                                            <asp:ListItem>-1</asp:ListItem>
                                            <asp:ListItem>0</asp:ListItem>
                                            <asp:ListItem>1</asp:ListItem>
                                            <asp:ListItem>2</asp:ListItem>
                                            <asp:ListItem>3</asp:ListItem>
                                            <asp:ListItem>4</asp:ListItem>
                                            <asp:ListItem>5</asp:ListItem>
                                            <asp:ListItem>6</asp:ListItem>
                                            <asp:ListItem>7</asp:ListItem>
                                            <asp:ListItem>8</asp:ListItem>
                                            <asp:ListItem>9</asp:ListItem>
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>11</asp:ListItem>
                                            <asp:ListItem>12</asp:ListItem>
                                            <asp:ListItem>13</asp:ListItem>
                                            <asp:ListItem>14</asp:ListItem>
                                            <asp:ListItem>15</asp:ListItem>
                                            <asp:ListItem>16</asp:ListItem>
                                            <asp:ListItem>17</asp:ListItem>
                                            <asp:ListItem>18</asp:ListItem>
                                            <asp:ListItem>19</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
                                            </asp:DropDownList>
                                    </tr>
                                    <tr>
                                        <td class="style2"><asp:Label ID="Label4" runat="server" Text="Margen Izquierdo:"></asp:Label></td>
                                        <td colspan="2">
                                            <asp:DropDownList ID="ddlMargenIzquierdo" runat="server">
                                            <asp:ListItem>-20</asp:ListItem>
                                            <asp:ListItem>-19</asp:ListItem>
                                            <asp:ListItem>-18</asp:ListItem>
                                            <asp:ListItem>-17</asp:ListItem>
                                            <asp:ListItem>-16</asp:ListItem>
                                            <asp:ListItem>-15</asp:ListItem>
                                            <asp:ListItem>-14</asp:ListItem>
                                            <asp:ListItem>-13</asp:ListItem>
                                            <asp:ListItem>-12</asp:ListItem>
                                            <asp:ListItem>-11</asp:ListItem>
                                            <asp:ListItem>-10</asp:ListItem>
                                            <asp:ListItem>-9</asp:ListItem>
                                            <asp:ListItem>-8</asp:ListItem>
                                            <asp:ListItem>-7</asp:ListItem>
                                            <asp:ListItem>-6</asp:ListItem>
                                            <asp:ListItem>-5</asp:ListItem>
                                            <asp:ListItem>-4</asp:ListItem>
                                            <asp:ListItem>-3</asp:ListItem>
                                            <asp:ListItem>-2</asp:ListItem>
                                            <asp:ListItem>-1</asp:ListItem>
                                            <asp:ListItem>0</asp:ListItem>
                                            <asp:ListItem>1</asp:ListItem>
                                            <asp:ListItem>2</asp:ListItem>
                                            <asp:ListItem>3</asp:ListItem>
                                            <asp:ListItem>4</asp:ListItem>
                                            <asp:ListItem>5</asp:ListItem>
                                            <asp:ListItem>6</asp:ListItem>
                                            <asp:ListItem>7</asp:ListItem>
                                            <asp:ListItem>8</asp:ListItem>
                                            <asp:ListItem>9</asp:ListItem>
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>11</asp:ListItem>
                                            <asp:ListItem>12</asp:ListItem>
                                            <asp:ListItem>13</asp:ListItem>
                                            <asp:ListItem>14</asp:ListItem>
                                            <asp:ListItem>15</asp:ListItem>
                                            <asp:ListItem>16</asp:ListItem>
                                            <asp:ListItem>17</asp:ListItem>
                                            <asp:ListItem>18</asp:ListItem>
                                            <asp:ListItem>19</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
                                            </asp:DropDownList>
                                    </tr>
                                    
                                    </table>

                            </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <script language="javascript" type="text/javascript">
    $(function () {
            $('#tabs').tabs();
        });

        function showpopupother(type_msg, title, msg, resize, height, width) {
            showdialog(type_msg, title, msg, resize, height, width);
        }
        </script>
</asp:Content>
