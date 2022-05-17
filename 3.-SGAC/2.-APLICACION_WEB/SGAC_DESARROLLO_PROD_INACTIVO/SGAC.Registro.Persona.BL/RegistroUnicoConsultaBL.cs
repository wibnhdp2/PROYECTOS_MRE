
using SGAC.Registro.Persona.DA;
namespace SGAC.Registro.Persona.BL
{
    public class RegistroUnicoConsultaBL
    {
        internal BE.MRE.RE_REGISTROUNICO Obtener(BE.MRE.RE_REGISTROUNICO REGISTROUNICO)
        {
            RegistroUnicoConsultaDA objDA = new RegistroUnicoConsultaDA();
            return objDA.Obtener(REGISTROUNICO);
        }
    }
}
