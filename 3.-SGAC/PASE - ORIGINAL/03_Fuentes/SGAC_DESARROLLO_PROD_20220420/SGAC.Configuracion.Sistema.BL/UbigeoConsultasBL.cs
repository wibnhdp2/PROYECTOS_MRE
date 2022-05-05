using System;
using System.Collections.Generic;
using System.Data;
using SGAC.Configuracion.Sistema.DA;
using SGAC.Accesorios;
 
namespace SGAC.Configuracion.Sistema.BL
{
    public class UbigeoConsultasBL
    {
        public DataTable Consultar(string StrDepartamento,
                                   string StrProvincia,
                                   string StrDistrito,
                                   string StrCurrentPage,
                                   int IntPageSize,
                                   ref int IntTotalCount,
                                   ref int IntTotalPages,
                                   string StrEstado)
        {
            DA.UbigeoConsultasDA objDA = new DA.UbigeoConsultasDA();

            try
            {

                return objDA.Consultar(StrDepartamento,
                                       StrProvincia,
                                       StrDistrito,
                                       StrCurrentPage,
                                       IntPageSize,
                                       ref IntTotalCount,
                                       ref IntTotalPages,
                                       StrEstado);
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

        public DataTable Consultar(
            string strCodigoUbigeo01, string strCodigoUbigeo02, string strCodigoUbigeo03)
        {
            UbigeoConsultasDA objDA = new UbigeoConsultasDA();

            try
            {

                return objDA.Consultar(strCodigoUbigeo01,
                                       strCodigoUbigeo02,
                                       strCodigoUbigeo03);
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

        public int Existe(string StrCodigo, int IntOperacion)
        {
            DA.UbigeoConsultasDA objDA = new DA.UbigeoConsultasDA();

            try
            {
                return objDA.Existe(StrCodigo, IntOperacion);

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

        public DataTable ObtenerUbigeo(string cUbigeo)
        {
            DA.UbigeoConsultasDA objDA = new DA.UbigeoConsultasDA();

            try
            {                
                return objDA.ObtenerUbigeo(cUbigeo);
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

        public DataTable Consultar_Pais()
        {
            DA.UbigeoConsultasDA objDA = new DA.UbigeoConsultasDA();

            try
            {
                return objDA.Consultar_Pais();
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

        //--------------------------------------------------------------
        //Fecha: 31/03/2017
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Consultar La Tabla Ubigeo y asignar a un objeto List
        //--------------------------------------------------------------

        public List<listaUbigeo> ObtenerUbigeo(string strCodigoUbigeo01, string strCodigoUbigeo02, string strCodigoUbigeo03,
           string strValor, string strDescripcion, bool bolAgregarItemAdicional, string pstrItemText, string pstrItemValue)
        {
            DA.UbigeoConsultasDA objDA = new DA.UbigeoConsultasDA();

            try
            {
                return objDA.ObtenerUbigeo(strCodigoUbigeo01, strCodigoUbigeo02, strCodigoUbigeo03,
                    strValor, strDescripcion, bolAgregarItemAdicional, pstrItemText, pstrItemValue);
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

        public beUbigeoListas obtenerUbiGeo()
        {
            DA.UbigeoConsultasDA objDA = new DA.UbigeoConsultasDA();

            try
            {
                return objDA.obtenerUbiGeo();
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
