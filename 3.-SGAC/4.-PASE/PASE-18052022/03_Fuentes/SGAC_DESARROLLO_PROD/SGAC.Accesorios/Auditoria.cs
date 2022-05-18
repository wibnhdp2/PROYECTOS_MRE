using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;

using SGAC.BE;
using System.Diagnostics;

namespace SGAC.Accesorios
{
    public class Auditoria
    {
        private BE.SI_AUDITORIA objAuditoria;

        private enum Propiedad
        {
            Nombre,
            Valor,
        };

        #region NulosC
        /// <summary>
        /// Devuelve comillas simples si un objeto es Nulo
        /// </summary>
        /// <param name="ObjObjeto">Objeto a eveluar</param>
        /// <returns>string</returns>
        private static string NulosC(object ObjObjeto)
        {
            if (ObjObjeto == null)
            {
                return "";
            }
            else
            {
                return ObjObjeto.ToString();
            }
        }
        #endregion NulosN

        #region EntidadObtenerPropiedades
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ObjEntidad"></param>
        /// <param name="TipoPropiedad"></param>
        /// <returns>string</returns>
        private static string EntidadObtenerPropiedades(object ObjEntidad, Propiedad TipoPropiedad)
        {
            string StrCadena = "";
            var ent = ObjEntidad.GetType().GetProperties();

            //Obtengo el objeto anónimo
            var persona = ObjEntidad;
            //Obtengo el tipo del objeto anónimo
            Type tipoPersona = persona.GetType();
            //Obtengo todas las propiedades del objeto anónimo
            PropertyInfo[] propiedades = tipoPersona.GetProperties();
            //Para cada propiedad...

            foreach (PropertyInfo propiedad in propiedades)
            {
                if (propiedad.Name.EndsWith("Creacion"))
                {
                    break;
                }
                else
                {
                    //... creo un objeto con el valor de la propiedad...
                    object valorPropiedadActual = propiedad.GetValue(persona, null);

                    if (TipoPropiedad == Propiedad.Valor)
                    {
                        StrCadena = StrCadena + "|" + NulosC(valorPropiedadActual);
                    }
                    if (TipoPropiedad == Propiedad.Nombre)
                    {
                        StrCadena = StrCadena + "|" + NulosC(propiedad.Name);
                    }
                }
            }
            return StrCadena;
        }
        #endregion EntidadObtenerPropiedades

        public int AuditoriaInsertar(Object pobjBE, int IntFormularioId, int IntTablaId, int IntUsuarioCreacion, int  ClavePrimariaId, 
                                      string strComentario, string StrIpCreacion,  DateTime DtmFechaCreacion,
                                    Enumerador.enmTipoOperacion enmTipoOperacion, BE.Entities Contexto)
        {
            try
            {
                objAuditoria = new SI_AUDITORIA();

                string strCampos = EntidadObtenerPropiedades(pobjBE, Auditoria.Propiedad.Nombre);
                string strValores = EntidadObtenerPropiedades(pobjBE, Auditoria.Propiedad.Valor);

                objAuditoria.audi_sFormularioId = Convert.ToInt16(IntFormularioId);
                objAuditoria.audi_sOperacionTipoId = Convert.ToInt16(enmTipoOperacion);
                objAuditoria.audi_sOperacionResultadoId = 1;
                objAuditoria.audi_sTablaId = Convert.ToInt16(IntTablaId);
                objAuditoria.audi_iClavePrimaria = Convert.ToInt32(ClavePrimariaId);
                objAuditoria.audi_vCampos = strCampos;
                objAuditoria.audi_vValorNuevo = strValores;
                objAuditoria.audi_vHostName = Util.ObtenerHostName();
                objAuditoria.audi_vComentario = strComentario;
                objAuditoria.audi_cEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();
                objAuditoria.audi_sUsuarioCreacion = Convert.ToInt16(IntUsuarioCreacion);
                objAuditoria.audi_vIPCreacion = Util.ObtenerDireccionIP();
                objAuditoria.audi_dFechaCreacion = DtmFechaCreacion;

                Contexto.SI_AUDITORIA.AddObject(objAuditoria);
                Contexto.SaveChanges();
                return 1;
            }
            catch
            {
                return (int)Enumerador.enmResultadoQuery.ERR;
            }
            finally
            {
                objAuditoria = null;
            }
        }




        public static int Insertar(Object objDA, BE.Entities Contexto,
                                Enumerador.enmTipoOperacion enmOperacion,
                                Enumerador.enmFormulario enmFormulario,
                                Enumerador.enmTabla enmTabla,  
                                Enumerador.enmResultadoQuery enmResultado,                  
                                Int64 intClavePrimaria,
                                string strComentario,
                                object intUsuarioCreacion, 
                                Int16 intHoras)
        {
            int intResultado;

            BE.SI_AUDITORIA objAuditoria = new SI_AUDITORIA();

            string strCampos = EntidadObtenerPropiedades(objDA, Auditoria.Propiedad.Nombre);
            string strValores = EntidadObtenerPropiedades(objDA, Auditoria.Propiedad.Valor);

            objAuditoria.audi_sFormularioId = Convert.ToInt16(enmFormulario);
            objAuditoria.audi_sOperacionTipoId = Convert.ToInt16(enmOperacion);
            objAuditoria.audi_sOperacionResultadoId = Convert.ToInt16(enmResultado);
            objAuditoria.audi_sTablaId = Convert.ToInt16(enmTabla);
            objAuditoria.audi_iClavePrimaria = intClavePrimaria;
            objAuditoria.audi_vCampos = strCampos;
            objAuditoria.audi_vValorNuevo = strValores;
            objAuditoria.audi_vHostName = Util.ObtenerHostName();
            objAuditoria.audi_vComentario = strComentario;
            objAuditoria.audi_cEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();
            objAuditoria.audi_sUsuarioCreacion = Convert.ToInt16(intUsuarioCreacion);
            objAuditoria.audi_vIPCreacion = Util.ObtenerDireccionIP();
            objAuditoria.audi_dFechaCreacion = Util.ObtenerFechaActual(intHoras);

            try
            {
                Contexto.SI_AUDITORIA.AddObject(objAuditoria);
                Contexto.SaveChanges();

                intResultado = (int)Enumerador.enmResultadoQuery.OK;
            }
            catch
            {
                intResultado = (int)Enumerador.enmResultadoQuery.ERR;
            }
            finally
            {
                objAuditoria = null;
            }
            return intResultado;
        }

        public static int InsertarError(Exception ex, BE.Entities Contexto,
                                Enumerador.enmTipoOperacion enmOperacion,
                                Enumerador.enmFormulario enmFormulario,
                                Enumerador.enmTabla enmTabla,
                                object intUsuarioCreacion,
                                Int16 intHoras)
        {
            int intResultado;

            BE.SI_AUDITORIA objAuditoria = new SI_AUDITORIA();

            string strCampos = "Error: ";
            string strValores = ex.InnerException.ToString();

            objAuditoria.audi_sFormularioId = Convert.ToInt16(enmFormulario);
            objAuditoria.audi_sOperacionTipoId = Convert.ToInt16(enmOperacion);
            objAuditoria.audi_sOperacionResultadoId = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);
            objAuditoria.audi_sTablaId = Convert.ToInt16(enmTabla);
            objAuditoria.audi_iClavePrimaria = 0;
            objAuditoria.audi_vCampos = strCampos;
            objAuditoria.audi_vValorNuevo = strValores;
            objAuditoria.audi_vHostName = Util.ObtenerHostName();
            objAuditoria.audi_vComentario = ex.Message;
            objAuditoria.audi_cEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();
            objAuditoria.audi_sUsuarioCreacion = Convert.ToInt16(intUsuarioCreacion);
            objAuditoria.audi_vIPCreacion = Util.ObtenerDireccionIP();
            objAuditoria.audi_dFechaCreacion = Util.ObtenerFechaActual(intHoras);

            try
            {
                Contexto.SI_AUDITORIA.AddObject(objAuditoria);
                Contexto.SaveChanges();
                intResultado = (int)Enumerador.enmResultadoQuery.OK;
            }
            catch(Exception e)
            {
                Debug.WriteLine("Error (" + DateTime.Today.ToString("dd/MM hh:mm:ss") + ") => " + e.Message);
                intResultado = (int)Enumerador.enmResultadoQuery.ERR;
            }
            finally
            {
                objAuditoria = null;
            }
            return intResultado;
        }

        
    }
}
