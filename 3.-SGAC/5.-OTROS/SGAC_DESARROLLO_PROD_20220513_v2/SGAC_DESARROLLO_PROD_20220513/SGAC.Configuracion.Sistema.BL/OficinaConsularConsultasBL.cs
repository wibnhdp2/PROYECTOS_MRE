using System;
using System.Data;
using SGAC.Configuracion.Sistema.DA;
 
namespace SGAC.Configuracion.Sistema.BL
{
    public class OficinaConsularConsultasBL
    {
        public DataTable Consultar(int IntCategoriaId,
                                   string StrvNombre,
                                   string StrvContinente,
                                   string StrvPais,
                                   string StrCurrentPage,
                                   int IntPageSize,
                                   ref int IntTotalCount,
                                   ref int IntTotalPages,
                                   string strEstado,
                                   bool bJefatura = false,
                                   bool bElecciones = false)
        {
            DA.OficinaConsularConsultasDA objDA = new DA.OficinaConsularConsultasDA();

            try
            {

                return objDA.Consultar(IntCategoriaId,
                                       StrvNombre,
                                       StrvContinente, 
                                       StrvPais,
                                       StrCurrentPage,
                                       IntPageSize,
                                       ref IntTotalCount,
                                       ref IntTotalPages,
                                       strEstado,
                                       bJefatura,
                                       bElecciones);
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

        public DataTable ObtenerPorId(int iOficinaConsularId)
        {
            DA.OficinaConsularConsultasDA objDA = new DA.OficinaConsularConsultasDA();

            try
            {                
                return objDA.ObtenerPorId(iOficinaConsularId);
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

        public DataTable ObtenerDependientes(int iOficinaConsularId,
                                             string StrCurrentPage,
                                             int IntPageSize,
                                             ref int IntTotalCount,
                                             ref int IntTotalPages)
        {
            DA.OficinaConsularConsultasDA objDA = new DA.OficinaConsularConsultasDA();

            try
            {
                return objDA.ObtenerDependientes(iOficinaConsularId,
                                                 StrCurrentPage,
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

        public int Existe(string strNombre, int IntOffConsularId, int IntOperacion)
        {
            DA.OficinaConsularConsultasDA objDA = new DA.OficinaConsularConsultasDA();

            try
            {
                return objDA.Existe(strNombre, IntOffConsularId, IntOperacion);
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

        // SGAC-PENDIENTE
        public DataTable ListarFuncionarios(object intOficinaConsularId)
        {
            DA.OficinaConsularConsultasDA objDA = new DA.OficinaConsularConsultasDA();

            try
            {
                return objDA.ListarFuncionarios(intOficinaConsularId);
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

        public DataTable ListaCodigoLocal(string strSiglas, string strCodigoLocal)
        {
            DA.OficinaConsularConsultasDA objDA = new DA.OficinaConsularConsultasDA();

            try
            {
                return objDA.ListaCodigoLocal(strSiglas, strCodigoLocal);
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

        public DataTable ConsultarMonedas(Int16 IntOficinaConsular)
        {
            DA.OficinaConsularConsultasDA objDA = new DA.OficinaConsularConsultasDA();

            try
            {

                return objDA.ConsultarMonedas(IntOficinaConsular);
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
        public DataTable ConsultarOficinasConsularesCargaInicial()
        {
            DA.OficinaConsularConsultasDA objDA = new DA.OficinaConsularConsultasDA();

            try
            {

                return objDA.ConsultarOficinasConsularesCargaInicial();
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
        public string OficinaEsActiva(short intOficinaConsularId)
        {
            DA.OficinaConsularConsultasDA objDA = new DA.OficinaConsularConsultasDA();

            try
            {

                return objDA.OficinaEsActiva(intOficinaConsularId);
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
//----------------------------------------------------------
    }
}
