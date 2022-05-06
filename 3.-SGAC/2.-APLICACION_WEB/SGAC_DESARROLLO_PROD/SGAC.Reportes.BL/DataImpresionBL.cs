using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SGAC.Reportes.DA;
using System.Transactions;
namespace SGAC.Reportes.BL
{
    public class DataImpresionBL
    {

        DataImpresionDA ObjDataImpresion = new DataImpresionDA();

        public DataTable ObtenerDataDeImpresion(Int64 iActuacionID, int sTipoDocImpresion)
        {
            return ObjDataImpresion.ObtenerDataDeImpresion(iActuacionID, sTipoDocImpresion);
        }

        public void RegistrarDataImpresion(Int64 ximp_iActuacionID, Int16 ximp_vTipoDocImpresion, Int16 ximp_sUsuarioCreacion, string ximp_vIPCreacion)
        {

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 2, 0);

            DataImpresionDA objDA = new DataImpresionDA();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                try
                {
                    objDA.RegistrarDataImpresion(ximp_iActuacionID, ximp_vTipoDocImpresion, ximp_sUsuarioCreacion, ximp_vIPCreacion);
                    scope.Complete();
                }
                catch{
                    Transaction.Current.Rollback();
                }
            }
        }
    }
}
