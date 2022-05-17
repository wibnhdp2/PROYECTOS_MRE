using System;
using SGAC.BE;
using SGAC.Configuracion.Sistema.DA;

namespace SGAC.Configuracion.Sistema.BL
{
    public class FeriadoMantenimientoBL : EntityCrud<BE.SI_FERIADO>
    {
        private FeriadoMantenimientoDA objDA;

        public override int Insert(ref SI_FERIADO pobjBE)
        {
            try
            {
                objDA = new FeriadoMantenimientoDA();
                return objDA.Insert(ref pobjBE);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                if (objDA != null)
                {
                    objDA = null;
                }
            }            
        }

        public override int Update(SI_FERIADO pobjBE)
        {
            try
            {
                objDA = new FeriadoMantenimientoDA();
                return objDA.Update(pobjBE);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                if (objDA != null)
                {
                    objDA = null;
                }
            }            
        }

        public override int Delete(SI_FERIADO pobjBE)
        {
            try
            {
                objDA = new FeriadoMantenimientoDA();
                return objDA.Delete(pobjBE);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                if (objDA != null)
                {
                    objDA = null;
                }
            }            
        }

        public override int GetOne(ref SI_FERIADO pobjBE)
        {
            try
            {
                objDA = new FeriadoMantenimientoDA();
                return objDA.GetOne(ref pobjBE);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                if (objDA != null)
                {
                    objDA = null;
                }
            }            
        }

        public override System.Collections.Generic.List<SI_FERIADO> GetAll(object pobj)
        {
            try
            {
                objDA = new FeriadoMantenimientoDA();
                return objDA.GetAll(pobj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
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
