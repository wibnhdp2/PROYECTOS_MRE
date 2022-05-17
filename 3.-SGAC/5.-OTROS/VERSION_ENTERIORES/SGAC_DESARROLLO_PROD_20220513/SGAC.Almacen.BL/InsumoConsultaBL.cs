using System;
using System.Data;
using SGAC.Almacen.DA;
using System.Transactions;
using SGAC.Accesorios;

namespace SGAC.Almacen.BL
{
    public class InsumoConsultaBL
    {
        public DataTable Consultar(int intOficinaConsularId, int intBodegaTipoId, int intBodegaId, string insu_vMovimientoCod, DateTime datFechaInicio, DateTime datFechaFin,
                                   int intPaginaActual, int intPaginaCantidad, ref int intTotalRegistros, ref int intTotalPaginas,
                                    int intTipoInsumo, string strCodigoFabrica, int intEstado)
        {
            DataTable dtlResult = new DataTable();
            InsumoConsultaDA xFun = new InsumoConsultaDA();

            //bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            Int16 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);
            //Nullable<Int16> iInsumoEstadoId = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {

                    dtlResult = xFun.Consultar(intOficinaConsularId, intBodegaTipoId, intBodegaId, insu_vMovimientoCod, datFechaInicio, datFechaFin,
                                               intPaginaActual, intPaginaCantidad, ref intTotalRegistros, ref intTotalPaginas,
                                               intTipoInsumo, strCodigoFabrica, intEstado);


                    return dtlResult;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex.InnerException);
                }
                finally
                {
                    if (xFun != null)
                        xFun = null;
                }
            }
        }

        public DataTable ConsultarFechaRegistro_por_IdInsumo(int intInsumoId)
        {
            DataTable dtlResult = new DataTable();
            InsumoConsultaDA xFun = new InsumoConsultaDA();

            //bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            Int16 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);
            //Nullable<Int16> iInsumoEstadoId = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {

                    dtlResult = xFun.ConsultarFechaRegistro_por_IdInsumo(intInsumoId);

                    return dtlResult;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex.InnerException);
                }
                finally
                {
                    if (xFun != null)
                        xFun = null;
                }
            }
        }

        //------------------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 04/11/2019
        // Objetivo: Consulta de Boveda
        //------------------------------------------------------------------------
        public DataTable ConsultarBovedas()
        {
            DataTable dtlResult = new DataTable();
            InsumoConsultaDA xFun = new InsumoConsultaDA();

            try
            {
                dtlResult = xFun.ConsultarBovedas();
                return dtlResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                if (xFun != null)
                    xFun = null;
            }
        }

        public DataTable ConsultarHistorico(int intNumInsumo, ref string strMovimientoCodigo)
        {
            DataTable dtlResult = new DataTable();
            InsumoConsultaDA xFun = new InsumoConsultaDA();

            try
            {
                dtlResult = xFun.ConsultarHistorico(intNumInsumo, ref strMovimientoCodigo);
                return dtlResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                if (xFun != null)
                    xFun = null;
            }
        }

        //------------------------------------------------------------------------
        // Autor: Jonatan Silva Cachay
        // Fecha: 17/02/2017
        // Objetivo: Consulta de insumos sin fecha
        //------------------------------------------------------------------------
        public DataTable ConsultarSinFecha(int intOficinaConsularId, int intBodegaTipoId, int intBodegaId, string insu_vMovimientoCod,
                                   int intPaginaActual, int intPaginaCantidad, ref int intTotalRegistros, ref int intTotalPaginas,
                                    int intTipoInsumo, string strCodigoFabrica, int intEstado)
        {
            DataTable dtlResult = new DataTable();
            InsumoConsultaDA xFun = new InsumoConsultaDA();

            //bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            Int16 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);
            //Nullable<Int16> iInsumoEstadoId = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {

                    dtlResult = xFun.ConsultarSinFecha(intOficinaConsularId, intBodegaTipoId, intBodegaId, insu_vMovimientoCod,
                                               intPaginaActual, intPaginaCantidad, ref intTotalRegistros, ref intTotalPaginas,
                                               intTipoInsumo, strCodigoFabrica, intEstado);


                    return dtlResult;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex.InnerException);
                }
                finally
                {
                    if (xFun != null)
                        xFun = null;
                }
            }
        }




        //------------------------------------------------------------------------
        // Autor: Jonatan Silva Cachay
        // Fecha: 17/02/2017
        // Objetivo: Consulta de insumos sin fecha
        //------------------------------------------------------------------------
        public DataTable ConsultarPorRangos(int intOficinaConsularId, string iRangoInicial, string iRangoFinal,
                                   int intPaginaActual,
                                   int intPaginaCantidad,
                                   ref int intTotalRegistros,
                                   ref int intTotalPaginas)
        {
            DataTable dtlResult = new DataTable();
            InsumoConsultaDA xFun = new InsumoConsultaDA();

            //bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            Int16 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);
            //Nullable<Int16> iInsumoEstadoId = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {

                    dtlResult = xFun.ConsultarPorRangos(intOficinaConsularId, iRangoInicial, iRangoFinal,
                                               intPaginaActual, intPaginaCantidad, ref intTotalRegistros, ref intTotalPaginas);


                    return dtlResult;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex.InnerException);
                }
                finally
                {
                    if (xFun != null)
                        xFun = null;
                }
            }
        }


        //------------------------------------------------------------------------
        // Autor: Jonatan Silva Cachay
        // Fecha: 15/10/2018
        // Objetivo: Consulta del ultimo insumo de usuario
        //------------------------------------------------------------------------
        public DataTable ConsultarUltimoInsumoUsuario(Int16 intConsulado,Int16 intUsuario)
        {
            DataTable dtlResult = new DataTable();
            InsumoConsultaDA xFun = new InsumoConsultaDA();

            //bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            Int16 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    dtlResult = xFun.ConsultarUltimoInsumoUsuario(intConsulado, intUsuario);
                    return dtlResult;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex.InnerException);
                }
                finally
                {
                    if (xFun != null)
                        xFun = null;
                }
            }
        }
    }
}