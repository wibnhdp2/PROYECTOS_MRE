using System;
using System.Collections.Generic;
using System.Data;
using SGAC.Cliente.Colas.DA;

namespace SGAC.Cliente.Colas.BL
{
    public class TicketConsultaBL
    {
        public DataTable TicketConsultaVentanilla(int iOficinaConsularId, int iVentanillaNumero, string cTicketFechaEmision)
        {
            TicketConsultaDA xFun = new TicketConsultaDA();

            try
            {
                return xFun.TicketConsultaVentanilla(iOficinaConsularId, iVentanillaNumero, cTicketFechaEmision);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                xFun = null;
            }
        }

        public DataTable TicketConsultaPromedio(int iOficinaConsularId, int iVentanillaNumero, string cTicketFechaEmision)
        {
            TicketConsultaDA xFun = new TicketConsultaDA();

            try
            {
                return xFun.TicketConsultaPromedio(iOficinaConsularId, iVentanillaNumero, cTicketFechaEmision);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                xFun = null;
            }

        }

        public DataSet TraerDatos(int iOficinaConsularId, string fchConsulta)
        {
            TicketConsultaDA xFun = new TicketConsultaDA();

            try
            {
                return xFun.TraerDatos(iOficinaConsularId, fchConsulta);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                xFun = null;
            }
        }

        public bool Adicionar(ref List<BE.CL_TICKET> lisTicket)
        {
            TicketConsultaDA xFun = new TicketConsultaDA();

            try
            {
                return xFun.Adicionar(ref lisTicket);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                xFun = null;
            }
        }
    }
}
