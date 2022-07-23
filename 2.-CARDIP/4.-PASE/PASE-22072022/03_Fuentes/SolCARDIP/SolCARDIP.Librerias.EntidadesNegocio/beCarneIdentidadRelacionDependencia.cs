using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolCARDIP.Librerias.EntidadesNegocio
{
    public class beCarneIdentidadRelacionDependencia
    {
        public short TitularDependienteId { get; set; }                                   //CATD_STITULAR_DEPENDIENTE_ID
        public int CarneIdentidadTitId { get; set; }                                    //CATD_SCARNE_IDENTIDAD_TIT_ID
        public int CarneIdentidadDepId { get; set; }                                    //CATD_SCARNE_IDENTIDAD_DEP_ID
        public string Estado { get; set; }                                                //CATD_CESTADO
        public short UsuarioCreacion { get; set; }                                        //CATD_SUSUARIO_CREACION
        public string IpCreacion { get; set; }                                            //CATD_VIP_CREACION
        public DateTime FechaCreacion { get; set; }                                       //CATD_DFECHA_CREACION
        public short UsuarioModificacion { get; set; }                                    //CATD_SUSUARIO_MODIFICACION
        public string IpModificacion { get; set; }                                        //CATD_VIP_MODIFICACION
        public DateTime FechaModificacion { get; set; }                                   //CATD_DFECHA_MODIFICACION

    }
}
