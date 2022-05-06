using SGAC.Registro.Persona.BL;
using System.Data;
using SGAC.Controlador;
using SGAC.Accesorios;
using System;
using System.Linq;
using SGAC.BE;
using System.Reflection;
using MSXML2;
using System.Text.RegularExpressions;


namespace SGAC.WebApp.Accesorios
{
    public class Empresa
    {
        private static string xDat = string.Empty;
        public static DataTable oEmpresa(object[] arrParametro)
        {
            try
            {
                RE_EmpresaFiltro objEn = new RE_EmpresaFiltro();
                DataTable dtEmpresa = new DataTable();

                object[] arrParametros = { arrParametro };

                return dtEmpresa;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Int64 Empresa_Existe(object[] arrParametro)
        {
            try
            {
                RE_EmpresaFiltro objEn = new RE_EmpresaFiltro();                

                //object[] arrParametros = { arrParametro };

                //Proceso p = new Proceso();
                //objEn = (RE_EmpresaFiltro)p.Invocar(ref arrParametros, "EnEmpresa", Enumerador.enmAccion.BUSCAR);

                EmpresaConsultaBL objEmpresaConsultaBL = new EmpresaConsultaBL();

                objEn = objEmpresaConsultaBL.Empresa_Existe(arrParametro);


                return objEn.empr_iEmpresaId;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Validar_Ruc(string n_NroRuc)
        {
            int dig01 = Convert.ToInt32(n_NroRuc.Substring(0, 1)) * 5;
            int dig02 = Convert.ToInt32(n_NroRuc.Substring(1, 1)) * 4;
            int dig03 = Convert.ToInt32(n_NroRuc.Substring(2, 1)) * 3;
            int dig04 = Convert.ToInt32(n_NroRuc.Substring(3, 1)) * 2;
            int dig05 = Convert.ToInt32(n_NroRuc.Substring(4, 1)) * 7;
            int dig06 = Convert.ToInt32(n_NroRuc.Substring(5, 1)) * 6;
            int dig07 = Convert.ToInt32(n_NroRuc.Substring(6, 1)) * 5;
            int dig08 = Convert.ToInt32(n_NroRuc.Substring(7, 1)) * 4;
            int dig09 = Convert.ToInt32(n_NroRuc.Substring(8, 1)) * 3;
            int dig10 = Convert.ToInt32(n_NroRuc.Substring(9, 1)) * 2;
            int dig11 = Convert.ToInt32(n_NroRuc.Substring(10, 1));

            int suma = dig01 + dig02 + dig03 + dig04 + dig05 + dig06 + dig07 + dig08 + dig09 + dig10;
            int residuo = (suma / 11);
            int resta = 11 - (suma - residuo * 11);

            int digChk = 0;

            switch (resta)
            {
                case 10: digChk = 0; break;
                case 11: digChk = 1; break;
                default: digChk = resta; break;
            }

            if (dig11 == digChk) return true; else return false;
        }
    }
}