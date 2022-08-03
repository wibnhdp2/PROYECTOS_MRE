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
                aria-selected="false">Datos de la Institución</a> </li>
            <li class="nav-item"><a class="nav-link disabled" id="FuncionCargoTab" runat="server"
                data-toggle="tab" href="#ContentPlaceHolder1_FuncionCargo" role="tab" aria-controls="FuncionCargo"
                aria-selected="false">Función y Cargo</a> </li>
            <li class="nav-item"><a class="nav-link disabled" id="FotografiaTab" runat="server"
                data-toggle="tab" href="#ContentPlaceHolder1_Fotografia" role="tab" aria-controls="Fotografia"
                aria-selected="false">Fotografía</a> </li>
            <li class="nav-item"><a class="nav-link disabled" id="FirmaTab" runat="server"
                data-toggle="tab" href="#ContentPlaceHolder1_Firma" role="tab" aria-controls="Firma"
                aria-selected="false">Firma</a> </li>
            <li class="nav-item"><a class="nav-link disabled" id="PasaporteTab" runat="server"
                data-toggle="tab" href="#ContentPlaceHolder1_Pasaporte" role="tab" aria-controls="Pasaporte"
                aria-selected="false">Pasaporte</a> </li>
            <li class="nav-item"><a class="nav-link disabled" id="DenunciaTab" runat="server"
                data-toggle="tab" href="#ContentPlaceHolder1_Denuncia" role="tab" aria-controls="Denuncia"
                aria-selected="false">Denuncia</a> </li>
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
                            <div class="col-sm-4">
                            <asp:LinkButton ID="btnDescargar" CssClass="btn btn-outline-primary btn-icon-split" Visible="false"
                                runat="server" OnClick="btnDescargar_Click"> 
                            <span class="icon text-white-50"><i class="fas fa-download"></i></span>
                            <span class="text">Descargar Requisitos</span> </asp:LinkButton>
                        </div>
                        </div>
                        <div id="DIV_MOTIVO" runat="server" visible="false" class="form-group row required">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Motivo:</label>
                            <div class="col-sm-4">
                                <div class="input-group">
                                    <asp:DropDownList ID="ddlMotivo" CssClass="form-control form-control-sm"
                                    runat="server">
                                        <asp:ListItem> PERDIDA O ROBO</asp:ListItem>
                                        <asp:ListItem> DETERIORO</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div id="DIV_DOCUMENTO" runat="server" visible="false" class="form-group row required">
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
                            En caso no recuerde su número de carnet comunicarse al (01)2043678 / (01)2043303.</div>
                        <hr />
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <asp:LinkButton ID="btnContinuar" CssClass="btn btn-primary btn-icon-split" runat="server"
                                    OnClick="btnContinuar_Click"> <span class="icon text-white-50"><i class="fas fa-long-arrow-alt-right"></i></span><span class="text">Siguiente</span> </asp:LinkButton>
                                <asp:LinkButton ID="btnCancelar1" CssClass="btn btn-outline-secondary btn-icon-split" runat="server" OnClick="btnCancelar_click"> <span class="icon text-white-50"><i class="fas fa-ban"></i></span><span class="text">Cancelar</span> </asp:LinkButton>
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
                        <div id="DIV_CARNET_IDENTIDAD" runat="server" visible="false" class="form-group ">
                            <div class="row mt-3" id="div_row_titular" runat="server">
                                <label class="col-sm-4 col-form-label-right-sm">
                                Titular:</label>
                                <div id="DIV_TITULAR_NOMBRES" class="col-sm-4 text-left mt-2" runat="server"></div>
                            </div>

                            <div class="row">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Nro. de Carnet Identidad:</label>
                            <div class="col-sm-4">
                                <div class="input-group">
                                    <asp:TextBox ID="txtCarnet" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                                    <div class="input-group-append">
                                        <asp:LinkButton ID="btnBuscarCarnet" CssClass="btn btn-primary" runat="server" OnClick="btnBuscarCarnet_Click"> <i class="fas fa-search fa-sm"></i></asp:LinkButton>
                                        <asp:HiddenField ID="hCodigoCarnetTitular" runat="server" />
                                        <asp:HiddenField ID="hApePatTitular" runat="server" />
                                        <asp:HiddenField ID="hApeMatTitular" runat="server" />
                                        <asp:HiddenField ID="hNomTitular" runat="server" />
                                        <asp:HiddenField ID="hCalidadMigratoriaTitular" runat="server" />
                                    </div>
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
                                <asp:LinkButton ID="LinkButton1" CssClass="btn btn-outline-secondary btn-icon-split" runat="server" OnClick="btnCancelar_click"> <span class="icon text-white-50"><i class="fas fa-ban"></i></span><span class="text">Cancelar</span> </asp:LinkButton>
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
                                    <asp:DropDownList ID="ddlTipDoc" CssClass="form-control form-control-sm" runat="server" OnSelectedIndexChanged="obtenerTipoDoc" AutoPostBack="true">
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
                                Correo Electrónico:</label>
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
                                    <asp:TextBox ID="txtTelefono" CssClass="form-control form-control-sm" MaxLength="15" 
                                        runat="server" />
                                </div>
                            </div>
                        </div>
                        
                        <div class="form-group row" id="divctipovisa" runat="server">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Tipo Visa:</label>
                            <div class="col-sm-4">
                                <div class="input-group">
                                    <asp:TextBox ID="txtTipoVisaId" Style="text-transform: uppercase" CssClass="form-control form-control-sm d-none"
                                        runat="server" />
                                    <asp:TextBox ID="txtTipoVisa" Style="text-transform: uppercase" CssClass="form-control form-control-sm"
                                        runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group row" id="divcvisa" runat="server">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Visa:</label>
                            <div class="col-sm-4">
                                <div class="input-group">
                                    <asp:TextBox ID="txtVisaId" Style="text-transform: uppercase" CssClass="form-control form-control-sm d-none"
                                        runat="server" />
                                    <asp:TextBox ID="txtVisa" Style="text-transform: uppercase" CssClass="form-control form-control-sm"
                                        runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group row" id="divctitular" runat="server">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Titular/Familia :</label>
                            <div class="col-sm-4">
                                <div class="input-group">
                                    <asp:TextBox ID="txtTitularFamiliarId" Style="text-transform: uppercase" CssClass="form-control form-control-sm d-none"
                                        runat="server" />
                                    <asp:TextBox ID="txtTitularFamiliar" Style="text-transform: uppercase" CssClass="form-control form-control-sm"
                                        runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group row" id="divcpermanencia" runat="server">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Tiempo Permanencia(días) :</label>
                            <div class="col-sm-4">
                                <div class="input-group">
                                    <asp:TextBox ID="txtTiempoPermanencia" Style="text-transform: uppercase" CssClass="form-control form-control-sm"
                                        runat="server" />
                                </div>
                            </div>
                        </div>
                        <hr />
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <asp:LinkButton ID="btnContinuarIntitucion" CssClass="btn btn-primary btn-icon-split" OnClientClick="return ValidarDatosSolicitante();"
                                    runat="server" OnClick="btnContinuarIntitucion_Click"> <span class="icon text-white-50">
                                        <i class="fas fa-long-arrow-alt-right"></i></span><span class="text">Siguiente</span> 
                                </asp:LinkButton>
                                <asp:LinkButton ID="LinkButton2" CssClass="btn btn-outline-secondary btn-icon-split" runat="server" OnClick="btnCancelar_click"> <span class="icon text-white-50"><i class="fas fa-ban"></i></span><span class="text">Cancelar</span> </asp:LinkButton>
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
                                Categoría:</label>
                            <div class="col-sm-4">
                                <div class="input-group">
                                    <asp:DropDownList ID="ddlCategoriaInstitucion" CssClass="form-control form-control-sm"
                                    AutoPostBack="true" OnSelectedIndexChanged="seleccionarCategoriaOficina" runat="server">
                                </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group row required">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Institución:</label>
                            <div class="col-sm-4">
                                <div class="input-group">
                                    <asp:DropDownList ID="ddlInstitucion" CssClass="form-control form-control-sm" runat="server">
                                </asp:DropDownList>
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
                                Nro de Teléfono:</label>
                            <div class="col-sm-4">
                                <div class="input-group">
                                    <asp:TextBox ID="txtTelefonoContacto" Style="text-transform: uppercase" CssClass="form-control form-control-sm" MaxLength="15"
                                        runat="server" />
                                </div>
                            </div>
                        </div>
                        
                        <hr />
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <asp:LinkButton ID="btnContinuarFuncionCargo" CssClass="btn btn-primary btn-icon-split" OnClientClick="return ValidarDatosInstitucion();"
                                    runat="server" OnClick="btnContinuarFuncionCargo_Click"> <span class="icon text-white-50"><i class="fas fa-long-arrow-alt-right"></i></span><span class="text">Siguiente</span> </asp:LinkButton>
                                <asp:LinkButton ID="LinkButton3" CssClass="btn btn-outline-secondary btn-icon-split" runat="server" OnClick="btnCancelar_click"> <span class="icon text-white-50"><i class="fas fa-ban"></i></span><span class="text">Cancelar</span> </asp:LinkButton>
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
                                Tipo Cargo:</label>
                            <div class="col-sm-4">
                                <asp:DropDownList ID="ddlTipoCargo" CssClass="form-control form-control-sm"
                                    AutoPostBack="true" OnSelectedIndexChanged="SeleccionarCargo" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group row required">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Cargo:</label>
                            <div class="col-sm-4">
                                <asp:DropDownList ID="ddlCargo" CssClass="form-control form-control-sm" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <hr />
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <asp:LinkButton ID="btnContinuarFotografia" CssClass="btn btn-primary btn-icon-split"
                                    runat="server" OnClick="btnContinuarFotografia_Click"> <span class="icon text-white-50"><i class="fas fa-long-arrow-alt-right"></i></span><span class="text">Siguiente</span> </asp:LinkButton>
                                <asp:LinkButton ID="LinkButton4" CssClass="btn btn-outline-secondary btn-icon-split" runat="server" OnClick="btnCancelar_click"> <span class="icon text-white-50"><i class="fas fa-ban"></i></span><span class="text">Cancelar</span> </asp:LinkButton>
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
                                Adjuntar Archivo(JPG) - Máximo (1 MB):</label>
                            <div class="col-sm-5">
                                <uc1:cuUploadFile ID="cuUploadFile1" runat="server"   />
                                <asp:HiddenField ID="hNombreArchivo" runat="server" />
                                <asp:HiddenField ID="hRuta" runat="server" />
                                <asp:HiddenField ID="hFileNameFinal" runat="server" />
                            </div>
                            <div class="col">
                                <div id="DIV_CARGADO" runat="server" visible="false" class="alert alert-success"
                                    role="alert">
                                    ¡Fotografía Cargada!
                                </div>
                                <div id="DIV_NO_CARGADO" runat="server" visible="false" class="alert alert-danger"
                                    role="alert">
                                    ¡Fotografía no Cargada!
                                </div>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4">
                            </div>
                            <div class="col-sm-5">
                                <div class="alert alert-danger" role="alert">
                                    <h4 class="alert-heading">
                                        ¡Información Importante para subir la fotografía!</h4>
                                    <hr />
                                    <ul class="list-unstyled" style="text-align: left;">
                                        <ul>
                                            <li>Debe ser una foto actual de tamaño pasaporte a color</li>
                                            <li>En la foto el rostro y orejas deben estar descubiertos</li>
                                            <li>En la foto el rostro debe estar con expresión neutral (no sonriente) </li>
                                            <li>No usar lentes</li>
                                            <li>No retocar la foto</li>
                                        </ul>
                                    </ul>
                                </div>
                            </div>
                            <div class="col-sm-3">      
                                <asp:Image ID="imgFoto" runat="server" style="width:150px;" /><br/>
                                <asp:Button ID="btnEliminarFoto" runat="server"  style="width:100px" OnClick="btnEliminarFoto_Click" class="btn btn-sm btn-outline-secondary mt-2 " Text="Eliminar" />
                                   
                            </div>
                        </div>
                        <hr />
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <asp:LinkButton ID="btnContinuarFirma" CssClass="btn btn-primary btn-icon-split"
                                    runat="server" OnClick="btnContinuarFirma_Click"> <span class="icon text-white-50"><i class="fas fa-long-arrow-alt-right"></i></span><span class="text">Siguiente</span> 
                                </asp:LinkButton>
                                <asp:LinkButton ID="LinkButton5" CssClass="btn btn-outline-secondary btn-icon-split" runat="server" OnClick="btnCancelar_click"> <span class="icon text-white-50"><i class="fas fa-ban"></i></span><span class="text">Cancelar</span> </asp:LinkButton>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>

        <div class="tab-pane" id="Firma" role="tabpanel" aria-labelledby="Fotografia-tab"
            runat="server">
            <div class="card shadow mb-4">
                <div class="card-header py-3">
                    <div class="form-group row">
                    </div>
                </div>
                <div class="card-body">
                    <asp:Panel ID="tab7" runat="server">
                        <div class="form-group row">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Adjuntar Archivo(JPG) - Máximo (1 MB):</label>
                            <div class="col-sm-5">
                                <uc1:cuUploadFile ID="cuUploadFile2" runat="server" />
                                <asp:HiddenField ID="hNombreArchivoFirma" runat="server" />
                                <asp:HiddenField ID="hRutaFirma" runat="server" />
                                <asp:HiddenField ID="hFirmaFileNameFinal" runat="server" />
                            </div>
                            <div class="col">
                                <div id="DIV_CARGADO_FIRMA" runat="server" visible="false" class="alert alert-success"
                                    role="alert">
                                    ¡Firma Cargada!
                                </div>
                                <div id="DIV_NO_CARGADO_FIRMA" runat="server" visible="false" class="alert alert-danger"
                                    role="alert">
                                    ¡Firma no Cargada!
                                </div>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4">
                            </div>
                            <div class="col-sm-5">
                                <div class="alert alert-danger" role="alert">
                                    <h4 class="alert-heading">
                                        ¡Información Importante para subir la firma!</h4>
                                    <hr />
                                    <ul class="list-unstyled" style="text-align: left;">
                                        <ul>
                                            <li>Debe ser una imagen legible</li>
                                            <li>La imagen de la firma debe ser nítida</li>
                                            <li>No retocar la imagen</li>
                                        </ul>
                                    </ul>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <asp:Image ID="imgFirma" runat="server" style="width:150px;" />
                                <br/>
                                <asp:Button ID="btnEliminarFirma" runat="server"  style="width:100px" OnClick="btnEliminarFirma_Click" class="btn btn-sm btn-outline-secondary mt-2 " Text="Eliminar"/>
                                    
                            </div>
                        </div>
                        <hr />
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <asp:LinkButton ID="btnContinuarGuardarFirma" CssClass="btn btn-primary btn-icon-split" CommandArgument="Firma"
                                    runat="server" OnClick="btnContinuarGuardar_Click"> <span class="icon text-white-50"><i class="fas fa-long-arrow-alt-right"></i></span><span class="text">Siguiente</span> </asp:LinkButton>
                                <asp:LinkButton ID="LinkButton6" CssClass="btn btn-outline-secondary btn-icon-split" runat="server" OnClick="btnCancelar_click"> <span class="icon text-white-50"><i class="fas fa-ban"></i></span><span class="text">Cancelar</span> </asp:LinkButton>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>

        <div class="tab-pane" id="Pasaporte" role="tabpanel" aria-labelledby="Fotografia-tab"
            runat="server">
            <div class="card shadow mb-4">
                <div class="card-header py-3">
                    <div class="form-group row">
                    </div>
                </div>
                <div class="card-body">
                    <asp:Panel ID="tab8" runat="server">
                        <div class="form-group row">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Adjuntar Archivo(JPG) - Máximo (1 MB):</label>
                            <div class="col-sm-5">
                                <uc1:cuUploadFile ID="cuUploadFile3" runat="server" />
                                <asp:HiddenField ID="hNombreArchivoPasaporte" runat="server" />
                                <asp:HiddenField ID="hRutaPasaporte" runat="server" />
                                <asp:HiddenField ID="hPasaporteFileNameFinal" runat="server" />
                            </div>
                            <div class="col">
                                <div id="DIV_CARGADO_PASAPORTE" runat="server" visible="false" class="alert alert-success"
                                    role="alert">
                                    ¡Pasaporte Cargado!
                                </div>
                                <div id="DIV_NO_CARGADO_PASAPORTE" runat="server" visible="false" class="alert alert-danger"
                                    role="alert">
                                    ¡Pasaporte no Cargado!
                                </div>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4">
                            </div>
                            <div class="col-sm-5">
                                <div class="alert alert-danger" role="alert">
                                    <h4 class="alert-heading">
                                        ¡Información Importante para subir la imagen del Pasaporte!</h4>
                                    <hr />
                                    <ul class="list-unstyled" style="text-align: left;">
                                        <ul>
                                            <li>Se debe adjuntar una imagen de la copia del pasaporte que muestra los datos biográficos </li>
                                            <li> Debe ser una imagen legible</li>
                                            <li>Se deben visualizar los datos biográficos de forma nítida</li>
                                            <li>No retocar la imagen</li>
                                        </ul>
                                    </ul>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <asp:Image ID="imgPasaporte" runat="server" style="width:150px;" />
                                <br/>
                                <asp:Button ID="btnEliminarPasaporte" runat="server"  style="width:100px" OnClick="btnEliminarPasaporte_Click" class="btn btn-sm btn-outline-secondary mt-2 " Text="Eliminar"/>
                            </div>
                        </div>
                        <hr />
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <asp:LinkButton ID="btnContinuarGuardarPasaporte" CssClass="btn btn-primary btn-icon-split" CommandArgument="Pasaporte"
                                    runat="server" OnClick="btnContinuarGuardar_Click"> 
                                    <span class="icon text-white-50"><i class="fas fa-long-arrow-alt-right"></i></span>
                                    <span class="text">Siguiente</span>
                                </asp:LinkButton>
                                <asp:LinkButton ID="LinkButton7" CssClass="btn btn-outline-secondary btn-icon-split" runat="server" OnClick="btnCancelar_click"> 
                                    <span class="icon text-white-50"><i class="fas fa-ban"></i></span><span class="text">Cancelar</span>
                                </asp:LinkButton>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>

        <div class="tab-pane" id="Denuncia" role="tabpanel" aria-labelledby="Fotografia-tab"
            runat="server">
            <div class="card shadow mb-4">
                <div class="card-header py-3">
                    <div class="form-group row">
                    </div>
                </div>
                <div class="card-body">
                    <asp:Panel ID="tab9" runat="server">
                        <div class="form-group row">
                            <label class="col-sm-4 col-form-label-right-sm">
                                Adjuntar Archivo(PDF) - Máximo (1 MB):</label>
                            <div class="col-sm-5">
                                <uc1:cuUploadFile ID="cuUploadFile4" runat="server" />
                                <asp:HiddenField ID="hNombreArchivoDenuncia" runat="server" />
                                <asp:HiddenField ID="hRutaDenuncia" runat="server" />
                            </div>
                            <div class="col">
                                <div id="DIV_CARGADO_DENUNCIA" runat="server" visible="false" class="alert alert-success"
                                    role="alert">
                                    ¡Denuncia Cargada!
                                </div>
                                <div id="DIV_NO_CARGADO_DENUNCIA" runat="server" visible="false" class="alert alert-danger"
                                    role="alert">
                                    ¡Denuncia no Cargada!
                                </div>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4">
                            </div>
                            <div class="col-sm-5">
                                <div class="alert alert-danger" role="alert">
                                    <h4 class="alert-heading">
                                        ¡Información Importante para subir la Denuncia!</h4>
                                    <hr />
                                    <ul class="list-unstyled" style="text-align: left;">
                                        <ul>
                                            <li>Se debe adjuntar la denuncia en formato PDF </li>
                                            <li> La información del archivo PDF debe ser legible</li>
                                        </ul>
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <hr />
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <asp:LinkButton ID="btnContinuarGuardarDenuncia" CssClass="btn btn-primary btn-icon-split" CommandArgument="Denuncia" 
                                    runat="server" OnClick="btnContinuarGuardar_Click"> <span class="icon text-white-50"><i class="fas fa-long-arrow-alt-right"></i></span><span class="text">Siguiente</span> </asp:LinkButton>
                                <asp:LinkButton ID="LinkButton8" CssClass="btn btn-outline-secondary btn-icon-split" runat="server" OnClick="btnCancelar_click"> <span class="icon text-white-50"><i class="fas fa-ban"></i></span><span class="text">Cancelar</span> </asp:LinkButton>
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
                                runat="server" Visible="false" OnClick="btnEditar_Click"> <span class="icon text-white-50"><i class="fas fa-edit"></i></span><span class="text">Editar</span> </asp:LinkButton>
                        </div>
                        <div class="col-sm-9">
                        </div>
                        <%--<div class="col-sm-2">
                            <asp:LinkButton ID="btnDescargar" CssClass="btn btn-outline-primary btn-icon-split"
                                runat="server" OnClick="btnDescargar_Click"> 
                            <span class="icon text-white-50"><i class="fas fa-download"></i></span>
                            <span class="text">Descargar Requisitos</span> </asp:LinkButton>
                        </div>--%>
                    </div>
                </div>
                <div class="card-body">
                    <div class="form-group row">
                        <label class="col-sm-4 col-form-label-right-sm">
                            <asp:Label ID="lblNumeroSolicitud" runat="server" Text="Número Solicitud:"></asp:Label>
                        </label>
                        <div class="col-sm-4">
                            <div class="input-group">
                                <asp:Label ID="lblNumeroSolicitudtxt" runat="server" Style="text-transform: uppercase"
                                    CssClass="form-control form-control-sm" Text=""></asp:Label>
                            </div>
                        </div>
                    </div>
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
                            <asp:Label ID="lblDatosInstitucionFinal" runat="server" Text="Datos de Institución:"></asp:Label>
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
                            <asp:LinkButton ID="lkbVerImagen" CssClass="btn btn-primary btn-sm btn-icon-split" runat="server"
                                OnClientClick="window.open('../VerImagenSave.aspx?imagen=foto','_blank','width=600,height=750,toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=1','_blank')"> <span class="icon text-white-50"><i class="fas fa-chalkboard-teacher"></i></span><span class="text">Ver Fotografía</span> </asp:LinkButton>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-4 col-form-label-right-sm">
                            <asp:Label ID="lblFirmaFinal" runat="server" Text="Firma:"></asp:Label>
                        </label>
                        <div class="col-sm-4">
                            <div class="input-group">
                                <asp:Label ID="lblFirmaFinaltxt" runat="server" Style="text-transform: uppercase"
                                    CssClass="form-control form-control-sm" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="col-sm-2">
                            <asp:LinkButton ID="lkbVerFirma" CssClass="btn btn-primary btn-sm btn-icon-split" runat="server"
                                OnClientClick="window.open('../VerImagenSave.aspx?imagen=firma','_blank','width=600,height=750,toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=1','_blank')"> <span class="icon text-white-50"><i class="fas fa-chalkboard-teacher"></i></span><span class="text">Ver Firma</span> </asp:LinkButton>
                        </div>
                    </div>

                    <div class="form-group row">
                        <label class="col-sm-4 col-form-label-right-sm">
                            <asp:Label ID="lblPasaporteFinal" runat="server" Text="Pasaporte:"></asp:Label>
                        </label>
                        <div class="col-sm-4">
                            <div class="input-group">
                                <asp:Label ID="lblPasaporteFinaltxt" runat="server" Style="text-transform: uppercase"
                                    CssClass="form-control form-control-sm" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="col-sm-2">
                            <asp:LinkButton ID="lkbVerPasaporte" CssClass="btn btn-primary btn-sm btn-icon-split" runat="server"
                                OnClientClick="window.open('../VerImagenSave.aspx?imagen=pasaporte','_blank','width=600,height=750,toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=1','_blank')"> <span class="icon text-white-50"><i class="fas fa-chalkboard-teacher"></i></span><span class="text">Ver Pasaporte</span> </asp:LinkButton>
                        </div>
                    </div>

                    <div class="form-group row">
                        <label class="col-sm-4 col-form-label-right-sm">
                            <asp:Label ID="lblDenunciaFinal" runat="server" Text="Denuncia:"></asp:Label>
                        </label>
                        <div class="col-sm-4">
                            <div class="input-group">
                                <asp:Label ID="lblDenunciaFinaltxt" runat="server" Style="text-transform: uppercase"
                                    CssClass="form-control form-control-sm" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="col-sm-2">
                            <asp:LinkButton ID="lkbVerDenuncia" CssClass="btn btn-primary btn-sm btn-icon-split" runat="server"
                                OnClientClick="window.open('../PDF.aspx?imagen=denuncia','_blank','width=600,height=750,toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=1','_blank')"> <span class="icon text-white-50"><i class="fas fa-chalkboard-teacher"></i></span><span class="text">Ver Denuncia</span> </asp:LinkButton>
                        </div>
                    </div>
                    <hr />
                    <hr />
                    <div class="form-group row">
                        <div class="col-sm-12">
                            <asp:LinkButton ID="btnGrabar" CssClass="btn btn-primary btn-icon-split" runat="server"
                                OnClick="btnGrabar_Click"> <span class="icon text-white-50"><i class="fas fa-save"></i></span><span class="text">Grabar</span> </asp:LinkButton>
                            <asp:LinkButton ID="LinkButton9" CssClass="btn btn-outline-secondary btn-icon-split" runat="server" OnClick="btnCancelar_click"> 
                                <span class="icon text-white-50"><i class="fas fa-ban"></i></span><span class="text">Finalizar</span> </asp:LinkButton>
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
        <asp:HiddenField ID="hRutaDocumentoAnexo" runat="server" />
    </div>
    
    <%--<div id="modalMensaje" class="modal">
            <div class="modal-windowlarge">
                <div class="modal-titulo">
                    <asp:ImageButton ID="ImageButton2" CssClass="close" ImageUrl="~/Imagenes/Iconos/Delete.png"
                        OnClientClick="cerrarPopupMensaje(); return false" runat="server" />
                    <span>SE GRABÓ LA SOLICITUD CORRECTAMENTE</span>
                </div>
                <div class="modal-cuerpo">
                    <asp:Label ID="lblMensaje" Font-Bold="true" runat="server" Text=""></asp:Label>
                </div>
                <div class="modal-pie" style="padding-top:10px;">
                    <asp:LinkButton ID="lkbEnviarPopup" CssClass="btn btn-primary btn-icon-split"
                                runat="server" OnClick="btnEnviarSolicitud_Click"> <span class="icon text-white-50"><i class="fas fas fa-check"></i></span><span class="text">Enviar Solicitud</span> </asp:LinkButton>
                </div>
            </div>
        </div>--%>


    <div class="modal fade" id="modalMensaje" tabindex="-1" role="dialog" data-backdrop="static"
        aria-hidden="true">
        <div id="imprimir" class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">SE GRABÓ LA SOLICITUD CORRECTAMENTE
                    </h5>
                    <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">×</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="card-body">
                        <asp:Label ID="lblMensaje" Font-Bold="true" runat="server" Text=""></asp:Label>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lkbEnviarPopup" CssClass="btn btn-primary btn-icon-split"
                        runat="server" OnClick="btnEnviarSolicitud_Click"> <span class="icon text-white-50"><i class="fas fas fa-check"></i></span><span class="text">Enviar Solicitud</span> </asp:LinkButton>
                </div>
            </div>
        </div>
    </div>

        <div class="modal fade" id="modalMensajeEnviar" tabindex="-1" role="dialog" data-backdrop="static"
                aria-hidden="true">
                <div id="imprimir2" class="modal-dialog modal-lg" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="H1">SE ENVIÓ LA SOLICITUD CORRECTAMENTE
                            </h5>
                            <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">×</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <div class="card-body">
                                <asp:Label ID="lblMensajeEnviar" Font-Bold="true" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button id="btnAceptar" class="btn btn-primary" type="button" data-dismiss="modal">
                                Aceptar</button>
                        </div>
                    </div>
                </div>
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


            var image = document.getElementById("ContentPlaceHolder1_cuUploadFile1_fileupload");
            image.setAttribute("accept", "image/jpeg");

            var image2 = document.getElementById("ContentPlaceHolder1_cuUploadFile2_fileupload");
            image2.setAttribute("accept", "image/jpeg");

            var image3 = document.getElementById("ContentPlaceHolder1_cuUploadFile3_fileupload");
            image3.setAttribute("accept", "image/jpeg");

            document.getElementById("ContentPlaceHolder1_cuUploadFile1_fileupload").onchange = evt => {
                const [file] = document.getElementById("ContentPlaceHolder1_cuUploadFile1_fileupload").files
                
                if (validarExtension(file.type)) {
                    alert("Archivo no válido, sólo cargar imagenes con extesión JPG");
                    $('#ContentPlaceHolder1_cuUploadFile1_fileupload').val('');
                    $('#ContentPlaceHolder1_cuUploadFile1_fileupload').next('label').html('Seleccione su Archivo');
                    return;
                } else if (validarPeso(file.size)) {
                    alert("Archivo no válido, sólo adjuntar archivos menores o igual a 1MB");
                    $('#ContentPlaceHolder1_cuUploadFile1_fileupload').val('');
                    $('#ContentPlaceHolder1_cuUploadFile1_fileupload').next('label').html('Seleccione su Archivo');
                    return;
                }

                if (file) {
                    document.getElementById("ContentPlaceHolder1_imgFoto").src = URL.createObjectURL(file)
                    $("#ContentPlaceHolder1_btnEliminarFoto").removeClass("d-none");
                    
                } else {
                    document.getElementById("ContentPlaceHolder1_imgFoto").src = "";
                    $("#ContentPlaceHolder1_btnEliminarFoto").addClass("d-none");
                }
            }

            document.getElementById("ContentPlaceHolder1_cuUploadFile2_fileupload").onchange = evt => {
                const [file] = document.getElementById("ContentPlaceHolder1_cuUploadFile2_fileupload").files

                if (validarExtension(file.type)) {
                    alert("Archivo no válido, sólo cargar imagenes con extesión JPG");
                    $('#ContentPlaceHolder1_cuUploadFile2_fileupload').val('');
                    $('#ContentPlaceHolder1_cuUploadFile2_fileupload').next('label').html('Seleccione su Archivo');
                    return;
                } else if (validarPeso(file.size)) {
                    alert("Archivo no válido, sólo adjuntar archivos menores o igual a 1MB");
                    $('#ContentPlaceHolder1_cuUploadFile2_fileupload').val('');
                    $('#ContentPlaceHolder1_cuUploadFile2_fileupload').next('label').html('Seleccione su Archivo');
                    return;
                }

                if (file) {
                    document.getElementById("ContentPlaceHolder1_imgFirma").src = URL.createObjectURL(file)
                    $("#ContentPlaceHolder1_btnEliminarFirma").removeClass("d-none");
                    
                } else {
                    document.getElementById("ContentPlaceHolder1_imgFirma").src = "";
                    $("#ContentPlaceHolder1_btnEliminarFirma").addClass("d-none");
                }
            }

            document.getElementById("ContentPlaceHolder1_cuUploadFile3_fileupload").onchange = evt => {
                const [file] = document.getElementById("ContentPlaceHolder1_cuUploadFile3_fileupload").files

                if (validarExtension(file.type)) {
                    alert("Archivo no válido, sólo cargar imagenes con extesión JPG");
                    $('#ContentPlaceHolder1_cuUploadFile3_fileupload').val('');
                    $('#ContentPlaceHolder1_cuUploadFile3_fileupload').next('label').html('Seleccione su Archivo');
                    return;
                } else if (validarPeso(file.size)) {
                    alert("Archivo no válido, sólo adjuntar archivos menores o igual a 1MB");
                    $('#ContentPlaceHolder1_cuUploadFile3_fileupload').val('');
                    $('#ContentPlaceHolder1_cuUploadFile3_fileupload').next('label').html('Seleccione su Archivo');
                    return;
                }

                if (file) {
                    document.getElementById("ContentPlaceHolder1_imgPasaporte").src = URL.createObjectURL(file)
                    $("#ContentPlaceHolder1_btnEliminarPasaporte").removeClass("d-none");
                    
                } else {
                    document.getElementById("ContentPlaceHolder1_imgFirma").src = "";
                    $("#ContentPlaceHolder1_btnEliminarPasaporte").addClass("d-none");
                }
            }

            $("#ContentPlaceHolder1_btnEliminarFoto").addClass("d-none");
            $("#ContentPlaceHolder1_btnEliminarFirma").addClass("d-none");
            $("#ContentPlaceHolder1_btnEliminarPasaporte").addClass("d-none");
        });

        $('#ContentPlaceHolder1_btnContinuarFirma').click(function(){
            ValidarCarga('1');
        });

        $('#ContentPlaceHolder1_btnContinuarGuardarFirma').click(function(){
            ValidarCarga('2');
        });

        $('#ContentPlaceHolder1_btnContinuarGuardarPasaporte').click(function(){
            ValidarCarga('3');
        });

        function ValidarCarga(control) {                            
            var image = $('#ContentPlaceHolder1_cuUploadFile'+control+'_fileupload').val();
            if (image.length <= 0) {
                alert('Debe seleccionar una imagen');                
            }
        }

        function validarExtension(tipoFile) {
            var esImg = false;
            var tipo = tipoFile.split('/');
            if (tipo[1] != 'jpeg') {
                esImg = true;
            }
            return esImg;
        }

        function validarPeso(size) {
            var esPesado = false;
            if (size > 1000000) {
                esPesado = true;
            }
            return esPesado;
        }        

    </script>



    <script type="text/javascript">
        document.onkeydown = function (e) {
            tecla = (document.all) ? e.keyCode : e.which;
            if (tecla == 116) {
                if (confirm("¿Seguro que quieres refrescar la página? regresaras a la pantalla de generación de código ") == true) {
                    __doPostBack('ACTUALIZA', '1');
                    return false;
                } else {
                    return false;
                }
            }
        }


        //Valida Telefono Solicitante
        $('#<%=txtTelefono.ClientID %>').keyup(function () {
            var txtTel = $('#<%=txtTelefono.ClientID %>').val().trim();
            $('#<%=txtTelefono.ClientID %>').val(convertirNumero(txtTel));
        });

        $('#<%=txtTelefono.ClientID %>').keydown(function () {
            var txtTel = $('#<%=txtTelefono.ClientID %>').val().trim();
            $('#<%=txtTelefono.ClientID %>').val(convertirNumero(txtTel));
        });

        $('#<%=txtTelefono.ClientID %>').keypress(function () {
            var txtTel = $('#<%=txtTelefono.ClientID %>').val().trim();
            $('#<%=txtTelefono.ClientID %>').val(convertirNumero(txtTel));
        });

        $('#<%=txtTelefono.ClientID %>').change(function () {
            var txtTel = $('#<%=txtTelefono.ClientID %>').val().trim();
            $('#<%=txtTelefono.ClientID %>').val(convertirNumero(txtTel));            
        });


        //Valida Telefono Contacto
        $('#<%=txtTelefonoContacto.ClientID %>').keyup(function () {
            var txtTel = $('#<%=txtTelefonoContacto.ClientID %>').val().trim();
            $('#<%=txtTelefonoContacto.ClientID %>').val(convertirNumero(txtTel));
        });

        $('#<%=txtTelefonoContacto.ClientID %>').keydown(function () {
            var txtTel = $('#<%=txtTelefonoContacto.ClientID %>').val().trim();
            $('#<%=txtTelefonoContacto.ClientID %>').val(convertirNumero(txtTel));
        });

        $('#<%=txtTelefonoContacto.ClientID %>').keypress(function () {
            var txtTel = $('#<%=txtTelefonoContacto.ClientID %>').val().trim();
            $('#<%=txtTelefonoContacto.ClientID %>').val(convertirNumero(txtTel));
        });

        $('#<%=txtTelefonoContacto.ClientID %>').change(function () {
            var txtTel = $('#<%=txtTelefonoContacto.ClientID %>').val().trim();
            $('#<%=txtTelefonoContacto.ClientID %>').val(convertirNumero(txtTel));
        });

        function convertirNumero(valor) {            
            var nuevoNumero = "";
            if (!isNaN(parseInt(valor))) {
                nuevoNumero = parseInt(valor);
            }
            return nuevoNumero;
        }

        function validarTexto(valor) {
            var alfabeto = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'ñ', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'Ñ', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', ' ',
                'ä', 'ë', 'ï', 'ö', 'ü', 'Ä', 'Ë', 'Ï', 'Ö', 'Ü', 'á', 'é', 'í', 'ó', 'ú', 'Á', 'É', 'Í', 'Ó', 'Ú'
            ]

            var valido = false;
            valor = valor.toUpperCase();
            x = 0;
            for (var i = 0; i < valor.length; i++) {                
                for (var j = 0; j < alfabeto.length; j++) {
                    var abc = alfabeto[j];
                    var chr = valor.substr(i, 1);
                    if (chr == abc) {
                        x++;                        
                    }
                }
            }

            if (valor.length==x){
                valido = true;
            }

            return valido;
        }

        function valdarDire(valor) {
            
            var chrSpecial = ['@', '<', '>', '=', '*', '+', '_', '$', '#', '&', '%', '|', '.', ',', ':', ';', '¿', '?', '{', '}', '[', ']', '(', ')', '/', '\\', '""', '!', '¡'];
            var valido = true;
            valor = valor.toUpperCase();
            x = 0;
            for (var i = 0; i < valor.length; i++) {
                for (var j = 0; j < chrSpecial.length; j++) {
                    var chrS = chrSpecial[j];
                    var chr = valor.substr(i, 1);
                    if (chr == chrS) {
                        x++;
                        break;
                    }
                }
            }

            if (x>0) {
                valido = false;
            }

            return valido;
        }

        function ValidarDatosSolicitante() {
            
            var txtApePat = $('#ContentPlaceHolder1_txtApePat').val().trim();
            var txtNombres = $('#ContentPlaceHolder1_txtNombres').val().trim();
            var txtFecNac = $('#ContentPlaceHolder1_txtFecNac').val().trim();
            var ddlSexo = $('#ContentPlaceHolder1_ddlSexo').val().trim();
            var ddlEstadoCivil = $('#ContentPlaceHolder1_ddlEstadoCivil').val().trim();
            var ddlNacionalidad = $('#ContentPlaceHolder1_ddlNacionalidad').val().trim();
            var ddlTipDoc = $('#ContentPlaceHolder1_ddlTipDoc').val().trim();
            var txtNroDocumentoSolicitante = $('#ContentPlaceHolder1_txtNroDocumentoSolicitante').val().trim();
            var txtDomicilio = $('#ContentPlaceHolder1_txtDomicilio').val().trim();
            var ddlDepartamento = $('#ContentPlaceHolder1_ddlDepartamento').val().trim();
            var ddlProvincia = $('#ContentPlaceHolder1_ddlProvincia').val().trim();
            var ddlDistrito = $('#ContentPlaceHolder1_ddlDistrito').val().trim();
            var txtCorreo = $('#ContentPlaceHolder1_txtCorreo').val().trim();
            var txtTelefono = $('#ContentPlaceHolder1_txtTelefono').val().trim();
            
            
            var resultado = "";

            if (txtApePat.length == 0) {
                resultado = "Apellido Paterno";

            } else if (txtNombres.length == 0) {
                resultado = "Nombre";

            } else if (txtFecNac.length == 0) {
                resultado = "Fecha de Nacimiento";

            } else if (ddlSexo == 0) {
                resultado = "Sexo";

            } else if (ddlEstadoCivil == 0) {
                resultado = "Estado Civil";

            } else if (ddlNacionalidad == 0) {
                resultado = "Nacionalidad";

            } else if (ddlTipDoc == 0) {
                resultado = "Tipo Documento";

            } else if (txtNroDocumentoSolicitante.length == 0) {
                resultado = "Nro Documento";

            } else if (txtDomicilio.length == 0) {
                resultado = "Domicilio";
            } else if (txtDomicilio.length > 0) {
                resultado = "";
                if (!valdarDire(txtDomicilio)) {
                    alert("Domicilio no es válido, debe insgresar sólo texto")
                } else {
                    if(ddlDepartamento == 0) {
                    resultado = "Departamento";

                    } else if (ddlProvincia == 0) {
                        resultado = "Provincia";

                    } else if (ddlDistrito == 0) {
                        resultado = "Distrito";

                    } else if (txtCorreo.length == 0) {
                        resultado = "Correo Electrónico";

                    } else if (txtCorreo.length > 0) {
                        var regex = /[\w-\.]{2,}@([\w-]{2,}\.)*([\w-]{2,}\.)[\w-]{2,4}/;
                        if (!regex.test($('#<%= txtCorreo.ClientID %>').val().trim())) {
                            resultado = "";
                            alert("Correo Electrónico ingreasdo no es válido, Vuelva a ingresar");
                        } else {
                            if (txtTelefono.length == 0) {
                                resultado = "Teléfono";
                            } else {
                                resultado = "";
                                if (txtTelefono.length <= 6) {
                                    alert("Número de teléfono debe ser mayor de 6 dígitos, Vuelva a ingresar");
                                }
                            }
                        }

                    }
                }
                
            } 

            if (resultado.length>0) {
                alert('El campo "' + resultado + '" es obligatorios, Ingrese o Elija un dato por favor');
            }          

            return;
        }

        function ValidarDatosInstitucion() {
                        
            var ddlCategoriaInstitucion = $('#ContentPlaceHolder1_ddlCategoriaInstitucion').val().trim();
            var ddlInstitucion = $('#ContentPlaceHolder1_ddlInstitucion').val().trim();
            var txtPersonaContacto = $('#ContentPlaceHolder1_txtPersonaContacto').val().trim();
            var txtCargoContacto = $('#ContentPlaceHolder1_txtCargoContacto').val().trim();
            var txtCorreoContacto = $('#ContentPlaceHolder1_txtCorreoContacto').val().trim();
            var txtTelefonoContacto = $('#ContentPlaceHolder1_txtTelefonoContacto').val().trim();
            
            var resultado = "";
            
            if (ddlCategoriaInstitucion == 0) {
                resultado = "Categoría";
            } else if (ddlInstitucion == 0) {
                resultado = "Institución";
            } else if (txtPersonaContacto.length <= 0) {
                resultado = "Persona de Contacto";
            } else if (txtPersonaContacto.length > 0) {
                resultado = '';
                if (!validarTexto(txtPersonaContacto)) {
                    alert("Nombre ingresado sólo puede contener texto");
                } else {

                    if (txtCargoContacto.length <= 0) {
                        resultado = "Cargo de Contacto";

                    } else if (txtCargoContacto.length > 0) {
                        resultado = "";
                        if (!validarTexto(txtCargoContacto)) {
                            alert("Nombre del Contacto ingresado sólo puede contener texto");
                        } else {

                            if (txtCorreoContacto.length <= 0) {
                                resultado = "Correo Electrónico";

                            } else if (txtCorreoContacto.length > 0) {
                                var regex = /[\w-\.]{2,}@([\w-]{2,}\.)*([\w-]{2,}\.)[\w-]{2,4}/;
                                if (!regex.test(txtCorreoContacto)) {
                                    resultado = "";
                                    alert("Correo Electrónico ingreasdo no es válido, Vuelva a ingresar");
                                } else if (txtTelefonoContacto.length == 0) {
                                    resultado = "Nro de Teléfono";
                                } else {
                                    resultado = "";
                                    if (txtTelefonoContacto.length <= 6) {
                                        alert("Número de teléfono debe ser mayor de 6 dígitos, Vuelva a ingresar");
                                    }
                                }

                            }

                        }
                    }

                }
            } 

            if (resultado.length > 0) {
                alert('El campo "' + resultado + '" es obligatorios, Ingrese o Elija un dato por favor');
            }

            return;

        }
               

    </script>
</asp:Content>
