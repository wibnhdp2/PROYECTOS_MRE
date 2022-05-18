using System;
using System.Data; 
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace SGAC.WebApp.Accesorios
{
    public class SysTables
    { 
        public static DataRow TraerSysRow(String str_pAplicacion, String str_Tabla, String str_Filtro)
        {
            DataSet ads_XML = new DataSet();
            DataTable adt_XML;

            string xmlFilePath = System.Configuration.ConfigurationManager.AppSettings["xmlFilePath"];
            ads_XML.ReadXml(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, String.Format("Accesorios\\SysTablesXML\\SysTables.xml", str_pAplicacion)));
            adt_XML = ads_XML.Tables[str_Tabla];

            return adt_XML.Select(str_Filtro)[0];
        }

        public static DataTable TraerSysTable(String str_pAplicacion, String str_Tabla)
        {
            DataSet ads_XML = new DataSet();
            DataTable adt_XML;

            string xmlFilePath = System.Configuration.ConfigurationManager.AppSettings["xmlFilePath"];
            ads_XML.ReadXml(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, String.Format("Accesorios\\SysTablesXML\\SysTables.xml", str_pAplicacion)));           
            adt_XML = ads_XML.Tables[str_Tabla];

            return adt_XML;
        }
    }
}