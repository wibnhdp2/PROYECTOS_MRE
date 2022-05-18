using System;
using SGAC.Almacen.DA;
using SGAC.BE.MRE;
using SGAC.Accesorios;

namespace SGAC.Almacen.BL
{
    public class PedidoHistoricoMantenimientoBL
    {
        private PedidoHistoricoMantenimientoDA objDA;

        public int PedidoHistoricoAdicionar(BE.AL_PEDIDOHISTORICO objBE, int intOficinaConsularId)
        {
            objDA = new PedidoHistoricoMantenimientoDA();

            try
            {
                return objDA.PedidoHistoricoAdicionar(objBE, intOficinaConsularId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                ValidacionError(objDA.strError, objBE.OficinaConsularId, objBE.pehi_sUsuarioCreacion);

                if (objDA != null)
                    objDA = null;
            }
        }

        public int PedidoHistoricoActualizar(BE.AL_PEDIDOHISTORICO objBE, int intOficinaConsularId)
        {
            objDA = new PedidoHistoricoMantenimientoDA();

            try
            {
                return objDA.PedidoHistoricoActualizar(objBE, intOficinaConsularId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                ValidacionError(objDA.strError, objBE.OficinaConsularId, objBE.pehi_sUsuarioCreacion);

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