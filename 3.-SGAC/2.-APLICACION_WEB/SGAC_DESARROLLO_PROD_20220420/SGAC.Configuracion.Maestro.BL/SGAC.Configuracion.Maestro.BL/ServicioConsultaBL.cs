using System.Data;
using SGAC.Configuracion.Maestro.DA;

namespace SGAC.Configuracion.Maestro.BL
{
	public class ServicioConsultaBL
	{
        public DataTable Consulta(int intTipo)
        {
            DataTable dtResult = new DataTable();
            ServicioConsultaDA Funcion = new ServicioConsultaDA();

            dtResult = Funcion.Consulta(intTipo);
            return dtResult;
        }

        public DataTable Consulta()
        {
            DataTable dtResult = new DataTable();
            ServicioConsultaDA Funcion = new ServicioConsultaDA();

            dtResult = Funcion.Consulta();
            return dtResult;
        }

        public DataTable ConsultarGeneral(string strConsulta)
        {
            DataTable dtResult = new DataTable();
            ServicioConsultaDA Funcion = new ServicioConsultaDA();

            dtResult = Funcion.ConsultarGeneral(strConsulta);
            return dtResult;
        }
	}
}
