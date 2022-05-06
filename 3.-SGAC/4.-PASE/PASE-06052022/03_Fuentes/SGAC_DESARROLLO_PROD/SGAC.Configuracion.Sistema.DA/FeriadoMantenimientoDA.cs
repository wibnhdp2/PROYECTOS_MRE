using System;
using System.Collections.Generic;
using SGAC.BE;
using System.Collections;

namespace SGAC.Configuracion.Sistema.DA
{
    public class FeriadoMantenimientoDA : EntityCrud<BE.SI_FERIADO>
    {
        private BE.SI_FERIADO objDA;

        public override int Insert(ref SI_FERIADO pobjBe)
        {
            throw new NotImplementedException();
        }

        public override int Update(SI_FERIADO pobjBe)
        {
            throw new NotImplementedException();
        }

        public override int Delete(SI_FERIADO pobjBe)
        {
            throw new NotImplementedException();
        }

        public override int GetOne(ref SI_FERIADO pobjBe)
        {
            throw new NotImplementedException();
        }

        public override List<SI_FERIADO> GetAll(object pobj)
        {
            throw new NotImplementedException();
        }

        ~FeriadoMantenimientoDA()
        {
            GC.Collect(); 
        }
    }
}
