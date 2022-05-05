using System;
using System.Data;
using SGAC.Accesorios;

namespace SGAC.Registro.Persona.BL
{
    public class FuncionarioConsultaBL
    {
        public DataTable Funcionario_Consultar(int sOficinaConsularId, int IfuncionarioId)
        {
            DA.FuncionarioConsultaDA objDA = new DA.FuncionarioConsultaDA();

            try
            {
                return objDA.Funcionario_Consultar(sOficinaConsularId, IfuncionarioId);
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                if (objDA != null)
                {
                    objDA = null;
                }
            }
        }

        public DataTable Funcionario_MRE(string vOficinaConsularId)
        {
            DA.FuncionarioConsultaDA objDA = new DA.FuncionarioConsultaDA();

            try
            {
                return objDA.Funcionario_MRE(vOficinaConsularId);
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                if (objDA != null)
                {
                    objDA = null;
                }
            }
        }

        public DataTable Funcionario_Listar(string vNroDocumento,
                                            string vPrimerApellido,
                                            string vSegundoApellido,
                                            string vNombre,
                                            string StrCurrentPage,
                                            int IntPageSize,
                                            ref int IntTotalCount,
                                            ref int IntTotalPages)
        {
            DA.FuncionarioConsultaDA objDA = new DA.FuncionarioConsultaDA();
            try
            {
                return objDA.Funcionario_Listar(vNroDocumento,
                                                vPrimerApellido,
                                                vSegundoApellido,
                                                vNombre,
                                                StrCurrentPage,
                                                IntPageSize,
                                                ref IntTotalCount,
                                                ref IntTotalPages);
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                if (objDA != null)
                {
                    objDA = null;
                }
            }            
        }
    }
}