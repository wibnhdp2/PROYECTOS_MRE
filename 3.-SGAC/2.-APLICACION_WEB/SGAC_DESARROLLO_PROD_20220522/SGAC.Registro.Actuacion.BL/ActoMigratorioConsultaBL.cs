using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGAC.BE.MRE.Custom;
using System.Data;
using SGAC.Registro.Actuacion.DA;
using SGAC.Accesorios;

namespace SGAC.Registro.Actuacion.BL
{
    using SGAC.BE.MRE;
    using SGAC.DA.MRE.ACTOMIGRATORIO;

    public class ActoMigratorioConsultaBL
    {
        public Int64 Consultar_Correlativo(Int64 acmi_sTipoDocumentoMigratorioId)
        {
            return new Actuacion.DA.ActoMigratorioConsultaDA().Consultar_Correlativo(acmi_sTipoDocumentoMigratorioId);

        }

        public Int64 Consultar_Correlativo_Tipo_Doc_Migratorio(Int64 exp_sTipoDocMigId)
        {
            return new Actuacion.DA.ActoMigratorioConsultaDA().Consultar_Correlativo_Tipo_Doc_Migratorio(exp_sTipoDocMigId);
        }

        public CBE_MIGRATORIO Consultar_Acto_Migratorio(long pers_iPersonaId, long acmi_iActuacionDetalleId)
        {
            CBE_MIGRATORIO obj_Resultado = new CBE_MIGRATORIO();
            try
            {
                obj_Resultado.ACTO = new SGAC.Registro.Actuacion.DA.ActoMigratorioConsultaDA().Consultar_Acto_Migratorio(acmi_iActuacionDetalleId);
                obj_Resultado.PERSONA = new SGAC.Registro.Actuacion.DA.ActoMigratorioConsultaDA().Consultar_Persona(pers_iPersonaId);
                /*SI en caso se desea agregar mas cosas*/
                obj_Resultado.FORMATO = new SGAC.Registro.Actuacion.DA.ActoMigratorioConsultaDA().Consultar_Acto_Migratorio_Formato(obj_Resultado.ACTO.acmi_iActoMigratorioId);
            }
            catch (Exception ex)
            {
                obj_Resultado = null;
                obj_Resultado.message = ex.Message;
            }
            return obj_Resultado;
        }

        

        public DataTable FormatoMigratorio(Int64 iActuacionDetalleId, Int64 iActoMigratorioId)
        {
            DA.ActoMigratorioConsultaDA objDA = new DA.ActoMigratorioConsultaDA();

            try
            {
                return objDA.FormatoMigratorio(iActuacionDetalleId, iActoMigratorioId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                if (objDA != null)
                {
                    objDA = null;
                }
            }
        }

        public DataTable FormatoMigratorio_Lamina(Int64 iActuacionDetalleId, Int64 iActoMigratorioId)
        {
            DA.ActoMigratorioConsultaDA objDA = new DA.ActoMigratorioConsultaDA();

            try
            {
                return objDA.FormatoMigratorio_Lamina(iActuacionDetalleId, iActoMigratorioId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                if (objDA != null)
                {
                    objDA = null;
                }
            }
        }

        public DataTable FormatoMigratorio_Baja(Int64 iActuacionDetalleId, Int64 iActoMigratorioId, Int64 iActoMigratorioHistorico)
        {
            DA.ActoMigratorioConsultaDA objDA = new DA.ActoMigratorioConsultaDA();

            try
            {
                return objDA.FormatoMigratorio_Baja(iActuacionDetalleId, iActoMigratorioId, iActoMigratorioHistorico);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                if (objDA != null)
                {
                    objDA = null;
                }
            }
        }

        public DataTable FormatoMigratorio_Anulado(Int64 iActuacionDetalleId, Int64 iActoMigratorioId, Int64 iActoMigratorioHistorico)
        {
            DA.ActoMigratorioConsultaDA objDA = new DA.ActoMigratorioConsultaDA();

            try
            {
                return objDA.FormatoMigratorio_Anulado(iActuacionDetalleId, iActoMigratorioId, iActoMigratorioHistorico);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                if (objDA != null)
                {
                    objDA = null;
                }
            }
        }
        public DataTable ConsultarDocumentosMigratorios(
            BE.MRE.RE_ACTOMIGRATORIO objActoMigratorio, BE.MRE.RE_PERSONA objPersona,
            int? intOficinaConsularId, int intPaginaActual, int intPaginaCantidad,
            ref int intTotalRegistros, ref int intTotalPaginas)
        {
            ActoMigratorioConsultaDA objDA;
            try
            {
                objDA = new ActoMigratorioConsultaDA();
                return objDA.ConsultarDocumentosMigratorios(objActoMigratorio, objPersona, intOficinaConsularId,
                    intPaginaActual, intPaginaCantidad, ref intTotalRegistros, ref intTotalPaginas);
            }
            catch (DataException ex)
            {
                throw ex;
            }
        }

        public DataTable Consultar_Detalle_Bajas(long acmi_iActoMigratorioId)
        {
            try
            {
                return new ActoMigratorioConsultaDA().Consultar_Detalle_Bajas(acmi_iActoMigratorioId);
            }
            catch (DataException ex)
            {
                throw ex;
            }
        }

        public string ExisteNumeroDocumento(BE.MRE.SI_EXPEDIENTE objExpediente)
        {
            RE_ACTOMIGRATORIO_DA oRE_ACTOMIGRATORIO_DA = new RE_ACTOMIGRATORIO_DA();
            return oRE_ACTOMIGRATORIO_DA.ExisteNumeroExpediente(objExpediente);
        }
    }
}
