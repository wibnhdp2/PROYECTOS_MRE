using System;
using System.Transactions;
using SGAC.Accesorios;

namespace SGAC.Registro.Persona.BL
{
    public class PersonaResidenciaMantenimientoBL
    {
        public string strError = string.Empty;
        /**************************************************************************************************/
        /************MANEJO DE LAS TABLAS RESIDENCIA Y PERSONA RESIDENCIA X REGISTRO***********************/
        /**************************************************************************************************/

        public int Insertar(BE.RE_RESIDENCIA ObjResBE,
                            BE.RE_PERSONARESIDENCIA ObjPersResBE,
                            int IntOficinaConsularId)
        {
            DA.PersonaResidenciaMantenimientoDA objDA = new DA.PersonaResidenciaMantenimientoDA();

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 2, 0);

            TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options);

            try
            {
                return objDA.Insertar(ObjResBE,
                                      ObjPersResBE,
                                      IntOficinaConsularId);
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                scope.Complete();
                scope.Dispose();

                if (objDA != null)
                {
                    objDA = null;
                }
            }
        }

        public int Actualizar(BE.RE_RESIDENCIA ObjResBE,
                              BE.RE_PERSONARESIDENCIA ObjPersResBE,
                              int IntOficinaConsularId)
        {
            DA.PersonaResidenciaMantenimientoDA objDA = new DA.PersonaResidenciaMantenimientoDA();

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 2, 0);

            TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options);

            try
            {
                return objDA.Actualizar(ObjResBE,
                                        ObjPersResBE,
                                        IntOficinaConsularId);
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                scope.Complete();
                scope.Dispose();

                if (objDA != null)
                {
                    objDA = null;
                }
            }
        }

        public int Eliminar(BE.RE_RESIDENCIA ObjResBE,
                            BE.RE_PERSONARESIDENCIA ObjPersResBE,
                            int IntOficinaConsularId)
        {
            DA.PersonaResidenciaMantenimientoDA objDA = new DA.PersonaResidenciaMantenimientoDA();

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 2, 0);

            TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options);

            try
            {
                return objDA.Eliminar(ObjResBE,
                                      ObjPersResBE,
                                      IntOficinaConsularId);
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                scope.Complete();
                scope.Dispose();

                if (objDA != null)
                {
                    objDA = null;
                }
            }
        }
    }
}