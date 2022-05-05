using System;
using System.Data;
using SGAC.BE;
using SGAC.Registro.Actuacion.DA;

namespace SGAC.Registro.Actuacion.BL
{
    public class ActoJudicialConsultaBL
    {
        public DataTable Consultar_Expediente(RE_ExpedienteJudicial objEn)
        {
            //DA.ActoJudicialConsultaDA objDA = new DA.ActoJudicialConsultaDA();
            SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIAL objDA = new SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIAL();

            try
            {
                return objDA.Consultar_Expediente(objEn);
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

        public DataTable consultar_Exp_Participante(Int64 iActoJudicialId,Int64 iActoJudicialParticipanteId)
        {
            //DA.ActoJudicialConsultaDA objDA = new DA.ActoJudicialConsultaDA();
            SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIAL objDA = new SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIAL();
            try
            {
                return objDA.consultar_Exp_Participante(iActoJudicialId, iActoJudicialParticipanteId);
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

        public DataTable Consultar_Expediente_Historico(Int64 iActoJudicialId)
        {
            //DA.ActoJudicialConsultaDA objDA = new DA.ActoJudicialConsultaDA();
            SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIAL objDA = new SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIAL();
            try
            {
                return objDA.Consultar_Expediente_Historico(iActoJudicialId);
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

        public DataTable ConsultarExpedientePorPersona(long lngPersonaId, long lngEmpresaId, DateTime datFechaInicio, DateTime datFechaFin,
            int intPaginaActual, int intPaginaCantidad,ref int intTotalRegistros,ref int intTotalPaginas)
        {
            //ActoJudicialConsultaDA objDA = new ActoJudicialConsultaDA();
            SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIAL objDA = new SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIAL();

            return objDA.ConsultarExpedientePorPersona(lngPersonaId, lngEmpresaId, datFechaInicio, datFechaFin,
                intPaginaActual, intPaginaCantidad,ref intTotalRegistros,ref intTotalPaginas);
        }

        public BE.RE_ACTOJUDICIAL Obtener(Int64 iActoJudicialId)
        {
            //DA.ActoJudicialConsultaDA objDA = new DA.ActoJudicialConsultaDA();
            SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIAL objDA = new SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIAL();
            try
            {
                return objDA.Obtener(iActoJudicialId);
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

        public DataTable Obtenertarifas()
        {
            //DA.ActoJudicialConsultaDA objDA = new DA.ActoJudicialConsultaDA();
            SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIAL objDA = new SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIAL();
            try
            {
                return objDA.Obtenertarifas();
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


        public DataTable SaberSiCerramos(Int64 ajpa_iActoJudicialId)
        {
            //DA.ActoJudicialConsultaDA objDA = new DA.ActoJudicialConsultaDA();
            SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIAL objDA = new SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIAL();
            try
            {
                return objDA.SaberSiCerramos(ajpa_iActoJudicialId);
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
        //------------------------------------------------
        //Fecha: 04/01/2017
        //Autor: Jonatan Silva Cachay
        //Objetivo: Para la impresión
        //------------------------------------------------
        public DataTable Reporte_Expediente(RE_ExpedienteJudicial objEn)
        {
            //DA.ActoJudicialConsultaDA objDA = new DA.ActoJudicialConsultaDA();
            SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIAL objDA = new SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIAL();

            try
            {
                return objDA.Reporte_Expediente(objEn);
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
    }
}