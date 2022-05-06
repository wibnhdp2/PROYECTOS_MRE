
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE.Custom
{
    public class CBE_PRESENTANTE
    {

        public Int64 anpr_iActoNotarialPresentanteId { get; set; }
	public Int64 anpr_iActoNotarialId { get; set; }
	public int anpr_sTipoPresentante { get; set; }
    public string anpr_vTipoPresentante_desc { get; set; }
	public string anpr_vPresentanteNombre { get; set; }
	public int anpr_sPresentanteTipoDocumento { get; set; }
    public string anpr_vPresentanteTipoDocumento_desc { get; set; }
	public string anpr_vPresentanteNumeroDocumento { get; set; }
	public int anpr_sPresentanteGenero { get; set; }
	public string anpr_cEtsado { get; set; }
	public int anpr_sUsuario { get; set; }
	public string anpr_vIP { get; set; }
	public string operacion { get; set; }
    public string anpr_sTipoPresentante_desc { get; set; }
    public string anpr_vPresentanteGenero_desc { get; set; }
    }
}
