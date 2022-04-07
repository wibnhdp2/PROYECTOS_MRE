<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/Principales/Principal.Master"
    AutoEventWireup="true" CodeBehind="frmRegistroLinea.aspx.cs" Inherits="SolCARDIP_REGLINEA.Paginas.Principales.frmRegistroLinea" %>

<%@ Register Src="../../UserControl/cuUploadFile.ascx" TagName="cuUploadFile" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href='<%= VirtualPathUtility.ToAbsolute("~/Estilos/all.min.css")%>' rel="stylesheet"
        type="text/css" />
    <!-- Custom styles for this template-->
    <link href='<%= VirtualPathUtility.ToAbsolute("~/Estilos/sb-admin-2.css")%>' rel="stylesheet"
        type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="h3 mb-4 text-gray-800">
        Solicitud de Emisión de Carné de Identidad</h1>
    <div id="Tabs" role="tabpanel">
        <ul class="nav nav-tabs" id="myTab" role="tablist">
            <li class="nav-item"><a class="nav-link active" id="TipEmisionTab" runat="server"
                data-toggle="tab" href="#ContentPlaceHolder1_TipEmision" role="tab" aria-controls="TipEmision"
                aria-selected="true">Tipo de Emisión</a> </li>
            <li class="nav-item"><a class="nav-link disabled" id="RelacionDepTab" runat="server"
                data-toggle="tab" href="#ContentPlaceHolder1_RelacionDep" role="tab" aria-controls="RelacionDep"
                aria-selected="false">Relación de Dependencia</a> </li>
            <li class="nav-item"><a class="nav-link disabled" id="SolicitanteTab" runat="server"
                data-toggle="tab" href="#ContentPlaceHolder1_Solicitante" role="tab" aria-controls="Solicitante"
                aria-selected="false">Datos del Solicitante</a> </li>
            <li class="nav-item"><a class="nav-link disabled" id="InstitucionTab" runat="server"
                data-toggle="tab" href="#ContentPlaceHolder1_Institucion" role="tab" aria-controls="Institucion"
                aria-selected="false">Datos de la Insitución</a> </li>
            <li class="nav-item"><a class="nav-link disabled" id="FuncionCargoTab" runat="server"
                data-toggle="tab" href="#ContentPlaceHolder1_FuncionCargo" role="tab" aria-controls="FuncionCargo"
                aria-selected="false">Función y Cargo</a> </li>
            <li class="nav-item"><a class="nav-link disabled" id="FotografiaTab" runat="server"
                data-toggle="tab" href="#ContentPlaceHolder1_Fotografia" role="tab" aria-controls="Fotografia"
                aria-selected="false">Carga de Fotografia</a> </li>
            <li class="nav-item"><a class="nav-link disabled" id="GuardarTab" runat="server"
                data-toggle="tab" href="#ContentPlaceHolder1_Guardar" role="tab" aria-controls="Guardar"
                aria-selected="false">Guardar</a> </li>
        </ul>
    </div>
    <div class="tab-content" id="myTabContent">
        <div class="tab-pane show active" id="TipEmision" role="tabpanel" aria-labelledby="TipEmision-tab"
            runat="server">
            <div class="card shadow mb-4">
                <div class="card-header py-3">
                    <div class="form-group row">
                        <div class="col-sm-4">
                            <h6 class="m-0 font-weight-bold text-primary">
                                <asp:Label ID="lblNroSolicitud" runat="server" CssClass="form-control form-control-sm"
                                    Text="Solicitud Nro. 0000000"></asp:Label>
                                <asp:HiddenField ID="hCodSolicitudTexto" runat="server" />
                            </h6>
                        </div>
                        <div class="col-sm-4">
                            <h6 class="m-0 font-weight-bold text-primary">
                                <asp:Label ID="lblFechaRegistro" CssClass="form-control form-control-sm" runat="server"
                                    Text="Fecha Registro:"></asp:Label>
                            </h6>
                        </div>
                        <div class="col-sm-4">
                            <h6 class="m-0 font-weight-bold text-primary">
                                <span id="ContentPlaceHolder1_lblEstado" class="form-control form-control-sm" style="text-align: center;">
                                    Estado: Registrado</span>
                                <asp:HiddenField ID="hEstadoRegistro" runat="server" />
                            </h6>
                        </div>
                    </div>
                    <asp:HiddenField ID="hCodCarnetIdentidad" runat="server" />
                    <asp:HiddenField ID="hCodRegistroLinea" runat="server" />
                </div>
                <div class="card-body">
                    <asp:Panel ID="tab1" runat="server">
                        <div class="form-group row required">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Tipo de Emisión:</label>
                            <div class="col-sm-4">
                                <asp:DropDownList ID="ddlTipEmision" CssClass="form-control form-control-sm" runat="server"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlTipEmision_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="DIV_DOCUMENTO" runat="server" visible="false" class="form-group row">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Nro. de documento:</label>
                            <div class="col-sm-4">
                                <div class="input-group">
                                    <asp:TextBox ID="txtNroDoc" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                                    <div class="input-group-append">
                                        <asp:LinkButton ID="btnBuscar" CssClass="btn btn-primary" runat="server" OnClick="btnBuscar_Click"> <i class="fas fa-search fa-sm"></i></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                            <div class="col">
                                <div id="Respuesta" runat="server" visible="false" class="form-group row">
                                    <asp:Label ID="lblResuesta" runat="server" CssClass="text-primary" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div id="Div_NUEVO" runat="server" visible="false" class="alert alert-warning">
                            Esta opción genera un nuevo registro de expediente y solicitud de emisión de carné
                            de identidad</div>
                        <div id="Div_DUPLICADO" runat="server" visible="false" class="alert alert-warning">
                            Esta opción genera una nueva solicitud de duplicado de carné de identidad. El documento
                            se emitirá con los mismos datos registrados, misma fecha de vencimiento y una nueva
                            fecha de emisión.</div>
                        <div id="Div_RENOVACION" runat="server" visible="false" class="alert alert-warning">
                            Esta opción genera una nueva solicitud del carné de identidad. El usuario podrá
                            actualizar la informacion para la nueva emisión del carne de identidad.</div>
                        <div id="Div_DUPLICADO_RENOVA" runat="server" visible="false" class="alert alert-danger">
                            En caso no recuerde su número de carnet comunicarse al (01)2043678 / (01)2043686.</div>
                        <hr />
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <asp:LinkButton ID="btnContinuar" CssClass="btn btn-primary btn-icon-split" runat="server"
                                    OnClick="btnContinuar_Click"> <span class="icon text-white-50"><i class="fas fa-long-arrow-alt-right"></i></span><span class="text">Siguiente</span> </asp:LinkButton>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
        <div class="tab-pane" id="RelacionDep" role="tabpanel" aria-labelledby="RelacionDep-tab"
            runat="server">
            <div class="card shadow mb-4">
                <div class="card-header py-3">
                    <div class="form-group row">
                    </div>
                </div>
                <div class="card-body">
                    <asp:Panel ID="tab2" runat="server">
                        <div class="form-group row required">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Relacion Dependencia:</label>
                            <div class="col-sm-4">
                                <asp:DropDownList ID="ddlRelDependencia" CssClass="form-control form-control-sm"
                                    runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlRelDependencia_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="DIV_CARNET_IDENTIDAD" runat="server" visible="false" class="form-group row">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Nro. de Carnet Identidad:</label>
                            <div class="col-sm-4">
                                <div class="input-group">
                                    <asp:TextBox ID="txtCarnet" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                                    <div class="input-group-append">
                                        <asp:LinkButton ID="btnBuscarCarnet" CssClass="btn btn-primary" runat="server" OnClick="btnBuscarCarnet_Click"> <i class="fas fa-search fa-sm"></i></asp:LinkButton>
                                        <asp:HiddenField ID="hCodigoCarnetTitular" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="form-group row">
                            <div class="table-responsive">
                                <div class="dataTables_wrapper dt-bootstrap4">
                                    <div class="col">
                                        <asp:GridView ID="gvRelDep" CssClass="table table-bordered dataTable table-hover"
                                            AutoGenerateColumns="False" OnPreRender="gvRelDep_PreRender" EmptyDataText="[No hay información para mostrar]"
                                            Width="100%" CellSpacing="0" role="grid" aria-describedby="dataTable_info" Style="width: 100%;"
                                            runat="server">
                                            <Columns>
                                                <asp:BoundField DataField="CarneNumero" HeaderText="Número Carnet">
                                                    <HeaderStyle CssClass="tr-style" HorizontalAlign="Left" Width="100px" />
                                                    <ItemStyle CssClass="tr-style" HorizontalAlign="Left" Width="100px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ConFuncionario" HeaderText="Funcionario">
                                                    <HeaderStyle CssClass="tr-style" HorizontalAlign="Left" Width="100px" />
                                                    <ItemStyle CssClass="tr-style" HorizontalAlign="Left" Width="100px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ConCalidadMigratoria" HeaderText="Calidad Migratoria">
                                                    <HeaderStyle CssClass="tr-style" HorizontalAlign="Left" Width="100px" />
                                                    <ItemStyle CssClass="tr-style" HorizontalAlign="Left" Width="100px" />
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <hr />
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <asp:LinkButton ID="btnContinuarSolicitante" CssClass="btn btn-primary btn-icon-split"
                                    runat="server" OnClick="btnContinuarSolicitante_Click"> <span class="icon text-white-50"><i class="fas fa-long-arrow-alt-right"></i></span><span class="text">Siguiente</span> </asp:LinkButton>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
        <div class="tab-pane" id="Solicitante" role="tabpanel" aria-labelledby="Solicitante-tab"
            runat="server">
            <div class="card shadow mb-4">
                <div class="card-header py-3">
                    <div class="form-group row">
                    </div>
                </div>
                <div class="card-body">
                    <asp:Panel ID="tab3" runat="server">
                        <div class="form-group row required">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Primer Apellido:</label>
                            <div class="col-sm-4">
                                <div class="input-group">
                                    <asp:TextBox ID="txtApePat" Style="text-transform: uppercase" CssClass="form-control form-control-sm"
                                        runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Segundo Apellido:</label>
                            <div class="col-sm-4">
                                <div class="input-group">
                                    <asp:TextBox ID="txtApeMat" Style="text-transform: uppercase" CssClass="form-control form-control-sm"
                                        runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group row required">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Nombres:</label>
                            <div class="col-sm-4">
                                <div class="input-group">
                                    <asp:TextBox ID="txtNombres" Style="text-transform: uppercase" CssClass="form-control form-control-sm"
                                        runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group row required">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Fecha de Nacimiento:</label>
                            <div class="col-sm-4">
                                <div class="input-group">
                                    <asp:TextBox ID="txtFecNac" type="date" CssClass="form-control form-control-sm" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group row required">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Sexo:</label>
                            <div class="col-sm-4">
                                <div class="input-group">
                                    <asp:DropDownList ID="ddlSexo" CssClass="form-control form-control-sm" runat="server"
                                        AutoPostBack="true" OnSelectedIndexChanged="seleccionarSexo">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group row required">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Estado Civil:</label>
                            <div class="col-sm-4">
                                <div class="input-group">
                                    <asp:DropDownList ID="ddlEstadoCivil" CssClass="form-control form-control-sm" runat="server"
                                        Enabled="false">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group row required">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Nacionalidad:</label>
                            <div class="col-sm-4">
                                <div class="input-group">
                                    <asp:DropDownList ID="ddlNacionalidad" CssClass="form-control form-control-sm" runat="server"
                                        OnSelectedIndexChanged="obtenerNacionalidadPais" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col">
                                <div class="input-group-append">
                                    <asp:Label ID="lblNacionalidad" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="form-group row required">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Tipo Documento:</label>
                            <div class="col-sm-4">
                                <div class="input-group">
                                    <asp:DropDownList ID="ddlTipDoc" CssClass="form-control form-control-sm" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group row required">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Número de Documento:</label>
                            <div class="col-sm-4">
                                <div class="input-group">
                                    <asp:TextBox ID="txtNroDocumentoSolicitante" Style="text-transform: uppercase" CssClass="form-control form-control-sm"
                                        runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group row required">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Domicilio:</label>
                            <div class="col-sm-4">
                                <div class="input-group">
                                    <asp:TextBox ID="txtDomicilio" Style="text-transform: uppercase" CssClass="form-control form-control-sm"
                                        runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group row required">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Departamento:</label>
                            <div class="col-sm-4">
                                <div class="input-group">
                                    <asp:DropDownList ID="ddlDepartamento" CssClass="form-control form-control-sm" runat="server"
                                        OnSelectedIndexChanged="seleccionarDepartamento" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group row required">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Provincia:</label>
                            <div class="col-sm-4">
                                <div class="input-group">
                                    <asp:DropDownList ID="ddlProvincia" CssClass="form-control form-control-sm" runat="server"
                                        OnSelectedIndexChanged="seleccionarProvincia" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group row required">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Distrito:</label>
                            <div class="col-sm-4">
                                <div class="input-group">
                                    <asp:DropDownList ID="ddlDistrito" CssClass="form-control form-control-sm" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group row required">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Correo Electronico:</label>
                            <div class="col-sm-4">
                                <div class="input-group">
                                    <asp:TextBox ID="txtCorreo" type="email" Style="text-transform: uppercase" CssClass="form-control form-control-sm"
                                        runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group row required">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Teléfono:</label>
                            <div class="col-sm-4">
                                <div class="input-group">
                                    <asp:TextBox ID="txtTelefono" Style="text-transform: uppercase" CssClass="form-control form-control-sm"
                                        runat="server" />
                                </div>
                            </div>
                        </div>
                        <hr />
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <asp:LinkButton ID="btnContinuarIntitucion" CssClass="btn btn-primary btn-icon-split"
                                    runat="server" OnClick="btnContinuarIntitucion_Click"> <span class="icon text-white-50"><i class="fas fa-long-arrow-alt-right"></i></span><span class="text">Siguiente</span> </asp:LinkButton>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
        <div class="tab-pane" id="Institucion" role="tabpanel" aria-labelledby="Institucion-tab"
            runat="server">
            <div class="card shadow mb-4">
                <div class="card-header py-3">
                    <div class="form-group row">
                    </div>
                </div>
                <div class="card-body">
                    <asp:Panel ID="tab4" runat="server">
                        <div class="form-group row required">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Institución:</label>
                            <div class="col-sm-4">
                                <div class="input-group">
                                    <asp:TextBox ID="txtInstitucion" Style="text-transform: uppercase" CssClass="form-control form-control-sm"
                                        runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group row required">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Persona de Contacto:</label>
                            <div class="col-sm-4">
                                <div class="input-group">
                                    <asp:TextBox ID="txtPersonaContacto" Style="text-transform: uppercase" CssClass="form-control form-control-sm"
                                        runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group row required">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Cargo de Contacto:</label>
                            <div class="col-sm-4">
                                <div class="input-group">
                                    <asp:TextBox ID="txtCargoContacto" Style="text-transform: uppercase" CssClass="form-control form-control-sm"
                                        runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group row required">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Correo Electrónico:</label>
                            <div class="col-sm-4">
                                <div class="input-group">
                                    <asp:TextBox ID="txtCorreoContacto" type="email" Style="text-transform: uppercase"
                                        CssClass="form-control form-control-sm" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group row required">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Nro de Telefono:</label>
                            <div class="col-sm-4">
                                <div class="input-group">
                                    <asp:TextBox ID="txtTelefonoContacto" Style="text-transform: uppercase" CssClass="form-control form-control-sm"
                                        runat="server" />
                                </div>
                            </div>
                        </div>
                        <hr />
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <asp:LinkButton ID="btnContinuarFuncionCargo" CssClass="btn btn-primary btn-icon-split"
                                    runat="server" OnClick="btnContinuarFuncionCargo_Click"> <span class="icon text-white-50"><i class="fas fa-long-arrow-alt-right"></i></span><span class="text">Siguiente</span> </asp:LinkButton>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
        <div class="tab-pane" id="FuncionCargo" role="tabpanel" aria-labelledby="FuncionCargo-tab"
            runat="server">
            <div class="card shadow mb-4">
                <div class="card-header py-3">
                    <div class="form-group row">
                    </div>
                </div>
                <div class="card-body">
                    <asp:Panel ID="tab5" runat="server">
                        <div class="form-group row required">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Tipo Institución:</label>
                            <div class="col-sm-4">
                                <asp:DropDownList ID="ddlTipoInstitucion" CssClass="form-control form-control-sm"
                                    AutoPostBack="true" OnSelectedIndexChanged="cargarTipoCargo" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group row required">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Tipo de Cargo:</label>
                            <div class="col-sm-4">
                                <asp:DropDownList ID="ddlCargo" CssClass="form-control form-control-sm" runat="server"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group row required">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Nombre de Cargo:</label>
                            <div class="col-sm-4">
                                <div class="input-group">
                                    <asp:TextBox ID="txtOtroCargo" Style="text-transform: uppercase" CssClass="form-control form-control-sm"
                                        runat="server" />
                                </div>
                            </div>
                        </div>
                        <hr />
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <asp:LinkButton ID="btnContinuarFotografia" CssClass="btn btn-primary btn-icon-split"
                                    runat="server" OnClick="btnContinuarFotografia_Click"> <span class="icon text-white-50"><i class="fas fa-long-arrow-alt-right"></i></span><span class="text">Siguiente</span> </asp:LinkButton>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
        <div class="tab-pane" id="Fotografia" role="tabpanel" aria-labelledby="Fotografia-tab"
            runat="server">
            <div class="card shadow mb-4">
                <div class="card-header py-3">
                    <div class="form-group row">
                    </div>
                </div>
                <div class="card-body">
                    <asp:Panel ID="tab6" runat="server">
                        <div class="form-group row">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Adjuntar Archivo(JPG) - Maximo (1 MB):</label>
                            <div class="col-sm-5">
                                <uc1:cuUploadFile ID="cuUploadFile1" runat="server" />
                                <asp:HiddenField ID="hNombreArchivo" runat="server" />
                                <asp:HiddenField ID="hRuta" runat="server" />
                            </div>
                            <div class="col">
                                <div id="DIV_CARGADO" runat="server" visible="false" class="alert alert-success"
                                    role="alert">
                                    Fotografia Cargada!
                                </div>
                                <div id="DIV_NO_CARGADO" runat="server" visible="false" class="alert alert-danger"
                                    role="alert">
                                    Fotografia no Cargada!
                                </div>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4">
                            </div>
                            <div class="col-sm-5">
                                <div class="alert alert-danger" role="alert">
                                    <h4 class="alert-heading">
                                        Información Importante para subir la fotografia!</h4>
                                    <hr />
                                    <ul class="list-unstyled" style="text-align: left;">
                                        <ul>
                                            <li>Debe ser una foto actual de tamaño pasaporte a color</li>
                                            <li>En la foto el rostro y orejas deben estar descubiertos</li>
                                            <li>En la foto el rostro debe esta con expresión neutral (no sonriente) </li>
                                            <li>No usar lentes</li>
                                            <li>No retocar la foto</li>
                                        </ul>
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <hr />
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <asp:LinkButton ID="btnContinuarGuardar" CssClass="btn btn-primary btn-icon-split"
                                    runat="server" OnClick="btnContinuarGuardar_Click"> <span class="icon text-white-50"><i class="fas fa-long-arrow-alt-right"></i></span><span class="text">Siguiente</span> </asp:LinkButton>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
        <div class="tab-pane" id="Guardar" role="tabpanel" aria-labelledby="Guardar-tab"
            runat="server">
            <div class="card shadow mb-4">
                <div class="card-header py-3">
                    <div class="form-group row">
                        <div class="col-sm-1">
                            <asp:LinkButton ID="btnEditar" CssClass="btn btn-outline-primary btn-icon-split"
                                runat="server" Visible="false" OnClick="btnEditar_Click"> 
                            <span class="icon text-white-50"><i class="fas fa-edit"></i></span>
                            <span class="text">Editar</span> </asp:LinkButton>
                        </div>
                        <div class="col-sm-9">
                        </div>
                        <div class="col-sm-2">
                            <asp:LinkButton ID="btnDescargar" CssClass="btn btn-outline-primary btn-icon-split"
                                runat="server" OnClick="btnDescargar_Click"> 
                            <span class="icon text-white-50"><i class="fas fa-download"></i></span>
                            <span class="text">Descargar Requisitos</span> </asp:LinkButton>
                        </div>
                    </div>
                </div>
                <div class="card-body">
                    <div class="form-group row">
                        <label class="col-sm-4 col-form-label-right-sm">
                            <asp:Label ID="lblTipoEmisionFinal" runat="server" Text="Tipo de Emision:"></asp:Label>
                        </label>
                        <div class="col-sm-4">
                            <div class="input-group">
                                <asp:Label ID="lblTipomisionFinaltxt" runat="server" Style="text-transform: uppercase"
                                    CssClass="form-control form-control-sm" Text=""></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-4 col-form-label-right-sm">
                            <asp:Label ID="lblRelDepFinal" runat="server" Text="Relacion de Dependencia:"></asp:Label>
                        </label>
                        <div class="col-sm-4">
                            <div class="input-group">
                                <asp:Label ID="lblRelDepFinaltxt" runat="server" Style="text-transform: uppercase"
                                    CssClass="form-control form-control-sm" Text=""></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-4 col-form-label-right-sm">
                            <asp:Label ID="lblDatSolicitanteFinal" runat="server" Text="Datos del Solicitante:"></asp:Label>
                        </label>
                        <div class="col-sm-4">
                            <div class="input-group">
                                <asp:Label ID="lblDatSolicitanteFinaltxt" runat="server" Style="text-transform: uppercase"
                                    CssClass="form-control form-control-sm" Text=""></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-4 col-form-label-right-sm">
                            <asp:Label ID="lblDatosInstitucionFinal" runat="server" Text="Datos de Institucion:"></asp:Label>
                        </label>
                        <div class="col-sm-4">
                            <div class="input-group">
                                <asp:Label ID="lblDatosInstitucionFinaltxt" runat="server" Style="text-transform: uppercase"
                                    CssClass="form-control form-control-sm" Text=""></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-4 col-form-label-right-sm">
                            <asp:Label ID="lblFuncionCargoFinal" runat="server" Text="Funcion y Cargo del Solicitante:"></asp:Label>
                        </label>
                        <div class="col-sm-4">
                            <div class="input-group">
                                <asp:Label ID="lblFuncionCargoFinaltxt" runat="server" Style="text-transform: uppercase"
                                    CssClass="form-control form-control-sm" Text=""></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-4 col-form-label-right-sm">
                            <asp:Label ID="lblFotografiaFinal" runat="server" Text="Fotografia:"></asp:Label>
                        </label>
                        <div class="col-sm-4">
                            <div class="input-group">
                                <asp:Label ID="lblFotografiaFinaltxt" runat="server" Style="text-transform: uppercase"
                                    CssClass="form-control form-control-sm" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="col-sm-2">
                            <asp:LinkButton ID="lkbVerImagen" CssClass="btn btn-primary btn-icon-split" runat="server"
                                OnClientClick="window.open('../VerImagenSave.aspx','_blank','width=600,height=750,toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=1','_blank')">
                                <span class="icon text-white-50"><i class="fas fa-chalkboard-teacher"></i></span><span class="text">Ver Imagen</span> </asp:LinkButton>
                        </div>
                    </div>
                    <hr />
                    <hr />
                    <div class="form-group row">
                        <div class="col-sm-12">
                            <asp:LinkButton ID="btnGrabar" CssClass="btn btn-primary btn-icon-split" runat="server"
                                OnClick="btnGrabar_Click"> <span class="icon text-white-50"><i class="fas fa-save"></i></span><span class="text">Grabar</span> </asp:LinkButton>
                            <asp:LinkButton ID="btnVerReporte" Visible="false" CssClass="btn btn-primary btn-icon-split"
                                runat="server" OnClick="btnVerReporte_Click"> <span class="icon text-white-50"><i class="fas fa-chalkboard-teacher"></i></span><span class="text">Ver Reporte</span> </asp:LinkButton>
                            <asp:LinkButton ID="btnEnviarSolicitud" Visible="false" CssClass="btn btn-primary btn-icon-split"
                                runat="server" OnClick="btnEnviarSolicitud_Click"> <span class="icon text-white-50"><i class="fas fas fa-check"></i></span><span class="text">Enviar Solicitud</span> </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="TabName" runat="server" />
    </div>
    <script src='<%= VirtualPathUtility.ToAbsolute("~/Scripts/jquery.min.js")%>' type="text/javascript"></script>
    <script src='<%= VirtualPathUtility.ToAbsolute("~/Scripts/bootstrap.bundle.min.js")%>'
        type="text/javascript"></script>
    <script src='<%= VirtualPathUtility.ToAbsolute("~/Scripts/sb-admin-2.min.js")%>'
        type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var tabName = $("[id*=ContentPlaceHolder1_TabName]").val() != "" ? $("[id*=ContentPlaceHolder1_TabName]").val() : "ContentPlaceHolder1_TipEmision";
            $('#Tabs a[href="#' + tabName + '"]').tab('show');
            $("#Tabs a").click(function () {
                $("[id*=ContentPlaceHolder1_TabName]").val($(this).attr("href").replace("#", ""));
            });
        });

    </script>
    <script type="text/javascript">
        document.onkeydown = function (e) {
            tecla = (document.all) ? e.keyCode : e.which;
            if (tecla == 116) {
                if (confirm("Seguro que quieres refrescar la página, regresaras a la pantalla de generación de código ") == true) {
                    __doPostBack('ACTUALIZA', '1');
                    return false;
                } else {
                    return false;
                }
            }
        }
    </script>
</asp:Content>
