using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE
{
    public abstract class EntityCrud<TEntity>
    {
        public abstract int Insert(ref TEntity pobjBE);
        public abstract int Update(TEntity pobjBE);
        public abstract int Delete(TEntity pobjBE);
        public abstract int GetOne(ref TEntity pobjBE);
        public abstract List<TEntity> GetAll(object pobj);
    }
}
