using System;
using System.Web.Security;
using System.Collections.Generic;
using System.Text;
using System.DirectoryServices;
using System.Runtime.InteropServices;
using System.DirectoryServices.AccountManagement;
using SGAC.Accesorios;
using System.Configuration;

namespace SGAC.Accesorios
{
    public class DirectorioActivo
    {
        private static DirectoryEntry entry;

        public static bool Autenticar(string userName, string password)
        {            
            bool authentic = false;

            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["DominioActivo"]))
            {
                if (ConfigurationManager.AppSettings["DominioActivo"] != "1")
                {
                    authentic = true;
                }
            }

            if (!authentic)
            {
                try
                {
                    string _path = ConfigurationManager.AppSettings["DominioRuta"];
                    string domain = ConfigurationManager.AppSettings["DominioNombre"];
                    String domainAndUsername = domain + @"\" + userName;
                    entry = new DirectoryEntry(_path, domainAndUsername, password);

                    Object obj = entry.NativeObject;

                    DirectorySearcher search = new DirectorySearcher(entry);
                    search.Filter = "(SAMAccountName=" + userName + ")";
                    search.PropertiesToLoad.Add("cn");
                    SearchResult result = search.FindOne();

                    if (result == null)
                        authentic = false;
                    else
                    {
                        _path = result.Path;
                        authentic = true;
                    }
                }
                catch (COMException ex)
                {
                    ex.Message.ToString();
                }
            }
            /////////BSOLO PARA PRUEBAS ---BORRAR --23/09/2016
            //authentic = true;
            //////////////////////////////
            return authentic;
        }

        public static bool ExisteUsuario(string strAlias)
        {
            bool bolExiste = false;

            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["DominioActivo"]))
            {
                if (ConfigurationManager.AppSettings["DominioActivo"] != "1")
                {
                    bolExiste = true;
                }
            }

            if (!bolExiste)
            {
                string _path = ConfigurationManager.AppSettings["DominioRuta"];
                string domain = ConfigurationManager.AppSettings["DominioNombre"];
                String domainAndUsername = domain + @"\" + strAlias;

                Object obj = entry.NativeObject;

                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(SAMAccountName=" + strAlias + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();

                if (result == null)
                    bolExiste = false;
                else
                {
                    _path = result.Path;
                    bolExiste = true;
                }
            }
            return bolExiste;
        }
    }
}
