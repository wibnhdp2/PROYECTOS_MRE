using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;
using SGAC.BE;
using SGAC.Accesorios;
using System.Collections;

namespace SGAC.Configuracion.Maestro.DA.Modulo
{
    class Funciones
    {
        //bool GrabarAuditoria(object pobjBE)
        //{
        //    SI_AUDITORIA objAuditoria;
        //    string strComentario = string.Empty;

        //    string strCampos = "banc_sBancoId|banc_vDescripcionCorta|banc_vDescripcionLarga|banc_cEstado|banc_sUsuarioCreacion|banc_vIPCreacion|banc_dFechaCreacion|banc_sUsuarioModificacion|banc_vIPModificacion|banc_dFechaModificacion";
        //    string strValores = pobjBE.banc_sBancoId + "|" + pobjBE.banc_vDescripcionCorta + "|" + pobjBE.banc_vDescripcionLarga + "|" + pobjBE.banc_cEstado + "|" + pobjBE.banc_sUsuarioCreacion + "|" + pobjBE.banc_vIPCreacion + "|" + pobjBE.banc_dFechaCreacion + "|" + pobjBE.banc_sUsuarioModificacion + "|" + pobjBE.banc_vIPModificacion + "|" + pobjBE.banc_dFechaModificacion;

        //    objAuditoria = new SI_AUDITORIA();
        //    objAuditoria.audi_IFormularioId = (int)Enumerador.enmFormulario.PERFILES_DE_ATENCION;
        //    objAuditoria.audi_IOperacionTipoId = (int)Enumerador.enmTipoOperacion.REGISTRO;
        //    objAuditoria.audi_ITablaId = (int)Enumerador.enmTabla.MA_BANCO;
        //    objAuditoria.audi_IOperacionResultadoId = 1;
        //    objAuditoria.audi_iTablaValorId = 0;
        //    objAuditoria.audi_vCamposNombre = strCampos;
        //    objAuditoria.audi_vCamposValor = strValores;
        //    objAuditoria.audi_vComentario = strComentario;
        //    objAuditoria.audi_vHostName = Util.ObtenerHostName();
        //    objAuditoria.audi_vDireccionIP = pobjBE.peat_vCreaDireccionIP;
        //    objAuditoria.audi_cActivo = ((char)Enumerador.enmEstado.ACTIVO).ToString();
        //    objAuditoria.audi_dCreaFecha = DateTime.Today;
        //    objAuditoria.audi_ICreaUsuarioId = Convert.ToInt32(pobjBE.peat_ICreaUsuarioId);
        //    context.SI_AUDITORIA.AddObject(objAuditoria);
        //    context.SaveChanges();
        //}
    }
}
