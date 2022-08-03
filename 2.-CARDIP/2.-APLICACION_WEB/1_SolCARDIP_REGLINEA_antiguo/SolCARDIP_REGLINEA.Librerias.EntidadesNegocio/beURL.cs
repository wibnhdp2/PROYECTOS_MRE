using System;
using System.Collections.Generic;

namespace SolCARDIP_REGLINEA.Librerias.EntidadesNegocio
{
    public class beURL
    {
        public string goToURL(int valor)
        {
            string urlValue = "error";
            if (valor == 1) { urlValue = @"..\02_Tipo_Emision\TipoEmision01.aspx"; }
            if (valor == 2) { urlValue = @"..\03_Relacion_Dependencia\RelacionDependencia01.aspx"; }
            if (valor == 3) { urlValue = @"..\04_Datos_Solicitante\DatosSolicitante01.aspx"; }
            if (valor == 4) { urlValue = @"..\05_Datos_Institucion\DatosInstitucion01.aspx"; }
            if (valor == 5) { urlValue = @"..\06_Funcion_Cargo_Solicitante\FuncionCargoSolicitante01.aspx"; }
            if (valor == 6) { urlValue = @"..\07_Carga_Fotografia\CargaFotografia01.aspx"; }
            if (valor == 7) { urlValue = @"..\08_Finalizar\Finalizar01.aspx"; }
            return (urlValue);
        }
    }
}
