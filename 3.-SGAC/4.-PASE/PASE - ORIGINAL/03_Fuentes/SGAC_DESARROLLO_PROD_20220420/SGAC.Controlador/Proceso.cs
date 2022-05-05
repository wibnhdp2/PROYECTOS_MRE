using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Web.UI.WebControls;
using SGAC.Accesorios;

namespace SGAC.Controlador
{
    public class Proceso
    {
        public int IErrorNumero;
        public string vErrorMensaje;

        public Proceso()
        {
            IErrorNumero = 0;
            vErrorMensaje = string.Empty;
        }

        public object Invocar(ref object[] ObjParametros, string NombreEntidad, Enumerador.enmAccion strNombreAccion,
            Enumerador.enmAplicacion iAplicacion = Enumerador.enmAplicacion.WEB)
        {
            return InvocarGeneral(ref ObjParametros, NombreEntidad, strNombreAccion.ToString(), iAplicacion);
        }

        public object Invocar(ref object[] ObjParametros, string NombreEntidad, string strNombreAccion,
            Enumerador.enmAplicacion iAplicacion = Enumerador.enmAplicacion.WEB)
        {
            return InvocarGeneral(ref ObjParametros, NombreEntidad, strNombreAccion, iAplicacion);
        }

        private object InvocarGeneral(ref object[] ObjParametros, string NombreEntidad, string strNombreAccion,
            Enumerador.enmAplicacion iAplicacion = Enumerador.enmAplicacion.WEB)
        {
            object Result = null;

            var DtResult = new DataTable();
            var TblFuncion = new Tabla();

            var NombreEmsamblado = "";
            var NombreClase = "";
            var NombreEvento = "";

            try
            {
                DtResult = TblFuncion.ObtenerFuncion(strNombreAccion, NombreEntidad);

                if (DtResult.Rows.Count != 0)
                {
                    NombreEmsamblado = DtResult.Rows[0]["func_vNombreEnsamblado"].ToString();
                    NombreClase = DtResult.Rows[0]["func_vNombreClase"].ToString();
                    NombreEvento = DtResult.Rows[0]["func_vNombreFuncion"].ToString();

                    Result = LlamarLibreria(NombreEmsamblado, NombreEvento, NombreClase, ObjParametros, iAplicacion);
                }                
            }
            catch (Exception)
            {
                // SI NO SE HA ENCONTRADO NombreEntidad Y strNombreAccion LA FUNCION DEVOLVER  -1 = No se ha encontrado el La Entidad  o la Accion en la Tabla Funcion
                IErrorNumero = -1;
                vErrorMensaje = "No se ha encontrado el La Entidad o la Acción en la Tabla Función";
                throw new Exception(vErrorMensaje);
            }
            finally
            {
                DtResult = null;
            }
            return Result;
                
        }

        private object LlamarLibreria(string assemblyName, string method, string impInterface, object[] arguments,
            Enumerador.enmAplicacion enmAplicacion = Enumerador.enmAplicacion.WEB)
        {
            object result = null;
            Assembly assembly;

            if (enmAplicacion == Enumerador.enmAplicacion.OTRO)
                assembly = Assembly.LoadFrom(assemblyName);
            else
                assembly =
                    Assembly.LoadFrom((Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "bin\\", assemblyName)));

            try
            {
                // Exploro cada tipo dentro del assembly
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsClass)
                    {
                        //Si no implementa la interfaz que se paso como parametro, salteo esa clase.
                        //if (type.GetInterface(impInterface) == null)
                        if (type.FullName != impInterface)
                        {
                            continue;
                        }
                        // En cambio, si implementa la interfaz creo una instancia del objeto
                        var implementObject = Activator.CreateInstance(type);
                            //Invoco dinamicamente el metodo pasado por parametro

                        result = type.InvokeMember(method, BindingFlags.Default | BindingFlags.InvokeMethod, null,
                            implementObject, arguments);
                        break;
                    }
                }
            }
            catch (Exception excep)
            {
                if (excep.Message.Substring(0, 6) == "Method")
                {
                    // SI NO SE HA ENCONTRADO METODO QUE SE ESTA INVOCANDO LA FUNCION DEVOLVERA
                    IErrorNumero = -2;
                    vErrorMensaje =
                        "No se ha encontrado el metodo especificado o los parámetros del método son los incorrectos.";
                }
                else
                {
                    // Error General
                    IErrorNumero = -1;
                    vErrorMensaje = "Error en la ejecución del sistema.";
                }
                return result;
            }
            return result;
        }

        public object PaginarGridWEB(ref GridView ControlGrid, ref object[] ObjParametros, string NombreEntidad,
            string strNombreAccion, Enumerador.enmAplicacion iAplicacion = Enumerador.enmAplicacion.WEB)
        {
            var DtResult = new DataTable();
            var INumeroRegistros = 0;

            DtResult = (DataTable) InvocarGeneral(ref ObjParametros, NombreEntidad, strNombreAccion, iAplicacion);

            INumeroRegistros = Convert.ToInt32(ObjParametros[2].ToString());
                // RETORNA EL NUMERO DE REGISTROS DE LA COONSULTA
            ControlGrid.AllowPaging = true;
            ControlGrid.PageSize = 10;
            ControlGrid.PageIndex = 5;

            ControlGrid.DataSource = DtResult;
            ControlGrid.DataBind();

            return 0;
        }
        
    }
}