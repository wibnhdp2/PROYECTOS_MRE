using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolCARDIP.Librerias.EntidadesNegocio
{
    public class beSolicitante
    {
        public short SolicitanteId { get; set; }                                          //SOLI_SSOLICITANTE_ID
        public string PrimerApellido { get; set; }                                        //SOLI_VPRIMER_APELLIDO
        public string SegundoApellido { get; set; }                                       //SOLI_VSEGUNDO_APELLIDO
        public string Nombres { get; set; }                                               //SOLI_VNOMBRES
        public short TipoDocumentoIdentidadId { get; set; }                               //SOLI_STIPO_DOCUMENTO_IDENTIDAD_ID
        public string NumeroDocumentoIdentidad { get; set; }                              //SOLI_VNUMERO_DOCUMENTO_IDENTIDAD
        public string Telefono { get; set; }                                              //SOLI_VTELEFONO
        public string Estado { get; set; }                                                //SOLI_CESTADO
        public short Usuariocreacion { get; set; }                                        //SOLI_SUSUARIOCREACION
        public string Ipcreacion { get; set; }                                            //SOLI_VIPCREACION
        public DateTime Fechacreacion { get; set; }                                       //SOLI_DFECHACREACION
        public short Usuariomodificacion { get; set; }                                    //SOLI_SUSUARIOMODIFICACION
        public string Ipmodificacion { get; set; }                                        //SOLI_VIPMODIFICACION
        public DateTime Fechamodificacion { get; set; }                                   //SOLI_DFECHAMODIFICACION
        // CONSULTA
        public string ConTipoDocIdent { get; set; }
        public string ConNombreCompleto { get; set; }
    }
}
