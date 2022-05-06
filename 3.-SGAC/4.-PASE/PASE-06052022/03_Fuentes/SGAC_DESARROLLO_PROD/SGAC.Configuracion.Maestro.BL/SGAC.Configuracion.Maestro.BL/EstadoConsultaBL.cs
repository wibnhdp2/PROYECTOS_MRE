using System;
using System.Data;
using SGAC.Accesorios;
using SGAC.Configuracion.Maestro.DA;

namespace SGAC.Configuracion.Maestro.BL
{
    public class EstadoConsultaBL
    {
        public DataTable ConsultaGrupo(Enumerador.enmEstadoGrupo EstadoGrupo)
        {
            DataTable DtResult = new DataTable();
            EstadoConsultaDA Estado = new EstadoConsultaDA();

            DtResult = Estado.ConsultaGrupo(EstadoGrupo);
            return DtResult;
        }

        public DataTable ConsultaGrupo(string EstadoGrupo)
        {
            DataTable DtResult = new DataTable();
            EstadoConsultaDA Estado = new EstadoConsultaDA();

            DtResult = Estado.ConsultaGrupo(EstadoGrupo);
            return DtResult;
        }

        public DataTable ConsultaGrupoMRE(ref Int16 intCantidadPaginas, Int16 intEstadoId = 0, string strDesCorta = "", string strEstado = "A",
            Int16 intPageSize = 10000, Int16 intNumeroPagina = 1, string strContar = "N", string strNombreGrupo = "")
        {
            DataTable DtResult = new DataTable();
            EstadoConsultaDA Estado = new EstadoConsultaDA();

            DtResult = Estado.ConsultaGrupoMRE(ref intCantidadPaginas, intEstadoId, strDesCorta, strEstado,
               intPageSize, intNumeroPagina, strContar, strNombreGrupo);
            return DtResult;
        }
    }
}
