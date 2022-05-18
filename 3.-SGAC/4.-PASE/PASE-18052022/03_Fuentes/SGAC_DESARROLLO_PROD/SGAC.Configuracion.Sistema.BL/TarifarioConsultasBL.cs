using System;
using System.Data;
using SGAC.Configuracion.Sistema.DA;
 
namespace SGAC.Configuracion.Sistema.BL
{
    public class TarifarioConsultasBL
    {
        public DataTable Consultar(int IntSeccionId,
                                   string StrDescripcionCorta,
                                   int IntEstado,
                                   string StrCurrentPage,
                                   int IntPageSize,
                                   ref int IntTotalCount,
                                   ref int IntTotalPages,
                                   bool bDetalle = false)
        {
            DA.TarifarioConsultasDA objDA = new DA.TarifarioConsultasDA();

            try
            {

                return objDA.Consultar(IntSeccionId,
                                       StrDescripcionCorta,
                                       IntEstado,
                                       StrCurrentPage,
                                       IntPageSize,
                                       ref IntTotalCount,
                                       ref IntTotalPages, bDetalle);
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

        public DataTable Obtener(int IntSeccionId, int intNumero, string strLetra,
            string StrDescripcion, int intCurrentPage, int IntPageSize, ref int IntTotalCount, ref int IntTotalPages)
        {
            TarifarioConsultasDA objDA = new TarifarioConsultasDA();
            try
            {

                return objDA.Obtener(IntSeccionId, intNumero, strLetra, StrDescripcion,
                                       intCurrentPage,
                                       IntPageSize,
                                       ref IntTotalCount,
                                       ref IntTotalPages);
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

        public DataTable Obtener(Int16 IntTarifarioNro)
        {
            DA.TarifarioConsultasDA objDA = new DA.TarifarioConsultasDA();

            try
            {

                return objDA.Obtener(IntTarifarioNro);
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

        public int Existe(int IntTarifarioId, string strNumero, string StrLetra, int IntOperacion)
        {
            DA.TarifarioConsultasDA objDA = new DA.TarifarioConsultasDA();

            try
            {
                return objDA.Existe(IntTarifarioId, strNumero, StrLetra, IntOperacion);

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

        public BE.MRE.SI_TARIFARIO ObtenerTarifaPorId(int intTarifaId)
        {
            TarifarioConsultasDA objDA = new TarifarioConsultasDA();
            try
            {
                return objDA.ObtenerTarifaPorId(intTarifaId);
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

        public DataTable ConsultarTarifarioCargaInicial(Int16 intOficinaConsularId)
        {
            DA.TarifarioConsultasDA objDA = new DA.TarifarioConsultasDA();

            try
            {

                return objDA.ConsultarTarifarioCargaInicial(intOficinaConsularId);
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

        public DataTable ConsultarTarifarioTotalCargaInicial()
        {
            DA.TarifarioConsultasDA objDA = new DA.TarifarioConsultasDA();

            try
            {

                return objDA.ConsultarTarifarioTotalCargaInicial();
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
        //-----------------------------------------
    }
}
