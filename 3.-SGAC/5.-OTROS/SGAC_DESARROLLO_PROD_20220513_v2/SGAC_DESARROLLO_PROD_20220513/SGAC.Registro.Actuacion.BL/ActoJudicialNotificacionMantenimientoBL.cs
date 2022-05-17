using System;
using System.Configuration;
using SGAC.Accesorios;
using System.Collections.Generic;
using SGAC.Registro.Actuacion.DA;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using SGAC.BE.MRE;

namespace SGAC.Registro.Actuacion.BL
{
    public class ActoJudicialNotificacionMantenimientoBL
    {

        private string strMensajeError;
        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public int Insertar(List<SGAC.BE.MRE.RE_ACTOJUDICIALNOTIFICACION> NOTIFICACIONES_LISTA)
        {
            SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIALNOTIFICACION objDA = new SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIALNOTIFICACION();
            int intFila = 0;
            int intResultado = 0;

            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };


            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {

                    for (intFila = 0; intFila <= NOTIFICACIONES_LISTA.Count - 1; intFila++)
                    {
                        SGAC.BE.MRE.RE_ACTOJUDICIALNOTIFICACION Notificacion = new SGAC.BE.MRE.RE_ACTOJUDICIALNOTIFICACION();

                        if (NOTIFICACIONES_LISTA[intFila].ajno_iActoJudicialNotificacionId < 0)
                        {
                            Notificacion = objDA.Insertar(NOTIFICACIONES_LISTA[intFila]);
                            
                        }
                        else
                        {
                            Notificacion = objDA.Actualizar(NOTIFICACIONES_LISTA[intFila]);
                            
                        }

                        ValidacionError(objDA.strError, Notificacion.OficinaConsultar, Notificacion.ajno_sUsuarioCreacion);

                        if (Notificacion.Error == true)
                        {
                            throw new DataException();
                        }
                        
                    }

                    

                    intResultado = 1;
                    scope.Complete();

                }
                catch (Exception ex)
                {
                    intResultado = 0;
                    Transaction.Current.Rollback();
                    throw new Exception(ex.Message, ex.InnerException);
                }
                finally
                {
                    
                    scope.Dispose();
                }

            }
            return intResultado;
        }
       

        public void ValidacionError(string mensaje, Int16 sOficinaConsular, Int16 sUsuario)
        {

            if (!string.IsNullOrEmpty(mensaje))
            {


                new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SI_AUDITORIA
                {
                    audi_vNombreRuta = Util.ObtenerNameForm(),
                    audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                    audi_sTablaId = null,
                    audi_sClavePrimaria = null,
                    audi_sOficinaConsularId = sOficinaConsular,
                    audi_vComentario = "",
                    audi_vMensaje = mensaje,
                    audi_vHostName = Util.ObtenerHostName(),
                    audi_sUsuarioCreacion = sUsuario,
                    audi_vIPCreacion = Util.ObtenerDireccionIP()
                });

            }
        }
    }
}