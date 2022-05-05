using System;
using System.Data;
using SGAC.Accesorios;
using SGAC.BE;
using SGAC.Registro.Persona.DA;
using SGAC.DA.MRE;

namespace SGAC.Registro.Persona.BL
{
    public class EmpresaConsultaBL
    {
        public BE.MRE.RE_EMPRESA Obtener(BE.MRE.RE_EMPRESA empresa)
        {
            RE_EMPRESA_DA lEMPRESA_DA = new RE_EMPRESA_DA();
            return lEMPRESA_DA.obtener(empresa);
        }

        public DataSet ConsultarId(Int64 empr_iEmpresaId)
        {
            EmpresaConsultaDA Emp = new EmpresaConsultaDA();

            try
            {
                return Emp.ConsultarId(empr_iEmpresaId);
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                if (Emp != null)
                {
                    Emp = null;
                }
            }
        }

        public RE_EmpresaFiltro Empresa_Existe(object[] arrParametro)
        {
            try
            {
                EmpresaConsultaDA ObjDa = new EmpresaConsultaDA();
                return ObjDa.Empresa_Existe(arrParametro);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}