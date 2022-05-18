using System;
using SGAC.BE.MRE;
using SGAC.Cliente.Colas.DA;
using SGAC.Accesorios;

namespace SGAC.Cliente.Colas.BL
{
    public class VentanillaDetalleMantenimientoBL
    {
        VentanillaDetalleMantenimientoDA objDA;

        public void Insertar(CL_VENTANILLASERVICIO pobjBe, ref bool Error)
        {
            objDA = new VentanillaDetalleMantenimientoDA();

            try
            {
                objDA.Insertar(pobjBe, ref Error);
            }
            catch (Exception ex)
            {
                new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                {
                    audi_vNombreRuta = Util.ObtenerNameForm(),
                    audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                    audi_sTablaId = null,
                    audi_sClavePrimaria = null,
                    audi_sOficinaConsularId = (Int16)pobjBe.OficinaConsultar,
                    audi_vComentario = "",
                    audi_vMensaje = ex.StackTrace.ToString(),
                    audi_vHostName = Util.ObtenerHostName(),
                    audi_sUsuarioCreacion = (Int16)pobjBe.vede_sUsuarioCreacion,
                    audi_vIPCreacion = Util.ObtenerDireccionIP()
                });
                throw ex;
            }
            finally
            {
                objDA = null;
            }
        }

        public void Actualizar(CL_VENTANILLASERVICIO pobjBe, ref bool Error)
        {
            objDA = new VentanillaDetalleMantenimientoDA();

            try
            {
                objDA.Actualizar(pobjBe, ref Error);
            }
            catch (Exception ex)
            {
                new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                {
                    audi_vNombreRuta = Util.ObtenerNameForm(),
                    audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                    audi_sTablaId = null,
                    audi_sClavePrimaria = null,
                    audi_sOficinaConsularId = (Int16)pobjBe.OficinaConsultar,
                    audi_vComentario = "",
                    audi_vMensaje = ex.StackTrace.ToString(),
                    audi_vHostName = Util.ObtenerHostName(),
                    audi_sUsuarioCreacion = (Int16)pobjBe.vede_sUsuarioModificacion,
                    audi_vIPCreacion = Util.ObtenerDireccionIP()
                });

                throw ex;
            }
            finally
            {
                objDA = null;
            }
        }

        public void Eliminar(CL_VENTANILLASERVICIO pobjBe, ref bool Error)
        {
            objDA = new VentanillaDetalleMantenimientoDA();

            try
            {
                objDA.Eliminar(pobjBe, ref Error);
            }
            catch (Exception ex)
            {
                new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                {
                    audi_vNombreRuta = Util.ObtenerNameForm(),
                    audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                    audi_sTablaId = null,
                    audi_sClavePrimaria = null,
                    audi_sOficinaConsularId = (Int16)pobjBe.OficinaConsultar,
                    audi_vComentario = "",
                    audi_vMensaje = ex.StackTrace.ToString(),
                    audi_vHostName = Util.ObtenerHostName(),
                    audi_sUsuarioCreacion = (Int16)pobjBe.vede_sUsuarioModificacion,
                    audi_vIPCreacion = Util.ObtenerDireccionIP()
                });

                throw ex;
            }
            finally
            {
                objDA = null;
            }
        }
    }
}
