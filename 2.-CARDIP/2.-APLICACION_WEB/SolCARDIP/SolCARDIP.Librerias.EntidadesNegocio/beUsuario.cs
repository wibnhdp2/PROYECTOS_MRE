using System;
namespace SolCARDIP.Librerias.EntidadesNegocio
{
	public class beUsuario
	{
		public short Usuarioid { get; set; }                                              //USUA_SUSUARIOID
		public short Empresaid { get; set; }                                              //USUA_SEMPRESAID
		public string Alias { get; set; }                                                 //USUA_VALIAS
		public string Apellidopaterno { get; set; }                                       //USUA_VAPELLIDOPATERNO
		public string Apellidomaterno { get; set; }                                       //USUA_VAPELLIDOMATERNO
		public string Nombres { get; set; }                                               //USUA_VNOMBRES
		public short Personatipoid { get; set; }                                          //USUA_SPERSONATIPOID
		public short Documentotipoid { get; set; }                                        //USUA_SDOCUMENTOTIPOID
		public string Documentonumero { get; set; }                                       //USUA_VDOCUMENTONUMERO
		public string Correoelectronico { get; set; }                                     //USUA_VCORREOELECTRONICO
		public string Telefono { get; set; }                                              //USUA_VTELEFONO
		public string Direccion { get; set; }                                             //USUA_VDIRECCION
		public short Referenciaid { get; set; }                                           //USUA_SREFERENCIAID
		public DateTime Fechacaducidad { get; set; }                                      //USUA_DFECHACADUCIDAD
		public string Direccionip { get; set; }                                           //USUA_VDIRECCIONIP
		public bool Sesionactiva { get; set; }                                            //USUA_BSESIONACTIVA
		public bool BloqueoActiva { get; set; }                                           //USUA_BBLOQUEOACTIVA
		public bool Notificaremesa { get; set; }                                          //USUA_BNOTIFICAREMESA
		public string Estado { get; set; }                                                //USUA_CESTADO
		public short Usuariocreacion { get; set; }                                        //USUA_SUSUARIOCREACION
		public string Ipcreacion { get; set; }                                            //USUA_VIPCREACION
		public DateTime Fechacreacion { get; set; }                                       //USUA_DFECHACREACION
		public short Usuariomodificacion { get; set; }                                    //USUA_SUSUARIOMODIFICACION
		public string Ipmodificacion { get; set; }                                        //USUA_VIPMODIFICACION
		public DateTime Fechamodificacion { get; set; }                                   //USUA_DFECHAMODIFICACION

        //CONSULTA
        public string NombreCompleto { get; set; }
        public short idOficinaConsular { get; set; }
        public string NombreOficinaConsular { get; set; }
        public string Rol { get; set; }
        public string TipoDocIdentidad { get; set; }
        public short UsuarioRolId { get; set; }
        public short Rol_Id { get; set; }
    }
}
