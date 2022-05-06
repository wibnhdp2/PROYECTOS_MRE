using System;
using SGAC.Almacen.DA;
using SGAC.BE.MRE;
using SGAC.Accesorios;

namespace SGAC.Almacen.BL
{
    public class MovimientoDetalleMantenimientoBL
    {
        private MovimientoDetalleMantenimientoDA objDA;

        public int MovimientoDetalleAdicionar(BE.AL_MOVIMIENTODETALLE objBE)
        {
            objDA = new MovimientoDetalleMantenimientoDA();

            try
            {
                return objDA.MovimientoDetalleAdicionar(objBE);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                ValidacionError(objDA.strError, objBE.OficinaConsularId, objBE.mode_sUsuarioCreacion);

                if (objDA != null)
                    objDA = null;
            }
        }

        public int MovimientoDetalleActualizar(BE.AL_MOVIMIENTODETALLE objBE)
        {
            objDA = new MovimientoDetalleMantenimientoDA();

            try
            {
                return objDA.MovimientoDetalleActualizar(objBE);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                ValidacionError(objDA.strError, objBE.OficinaConsularId, objBE.mode_sUsuarioCreacion);

                if (objDA != null)
                    objDA = null;
            }
        }

        public int MovimientoDetalleEliminar(BE.AL_MOVIMIENTODETALLE objBE)
        {
            objDA = new MovimientoDetalleMantenimientoDA();

            try
            {
                return objDA.MovimientoDetalleEliminar(objBE);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                ValidacionError(objDA.strError, objBE.OficinaConsularId, objBE.mode_sUsuarioCreacion);

                if (objDA != null)
                    objDA = null;
            }
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