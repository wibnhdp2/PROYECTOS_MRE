using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SGAC.BE.MRE
{
    public class RE_ASISTENCIA : BaseBussinesObject
    {
	    public long asis_iAsistenciaId { get; set; }
	    public long asis_iPersonaId { get; set; }
	    public short asis_sTipAsistencia  { get; set; }
        public DateTime asis_dFecServicio { get; set; }
	    public string asis_vNroCaso { get; set; }
	    public string asis_vHoraInicio  { get; set; }
	    public string asis_vHoraFin  { get; set; }
	    public short asis_sTipoServId  { get; set; }
	    public short asis_sOtrosServiciosId  { get; set; }
	    public string asis_cUbigeo  { get; set; }
	    public short asis_sOficinaConsularOrigenId  { get; set; }
	    public short asis_sOficinaConsularId { get; set; }
        public short asis_sCircunscripcionId { get; set; }
	    public int asis_IFuncionarioId  { get; set; }
	    public string asis_vDirURL  { get; set; }
	    public short asis_sMonedaId  { get; set; }
	    public Nullable<double> asis_FMontoServ  { get; set; }
	    public string asis_vObservaciones  { get; set; }
	    public string asis_vNombreArchivo  { get; set; }
	    public short asis_sCubrioPAH  { get; set; }
	    public short asis_sTipoAyudaPah  { get; set; }
	    public short asis_sNumeroBeneficiario  { get; set; }
	    public short asis_sEstado  { get; set; }
	    public string asis_cEstado  { get; set; }
	    public short asis_sUsuarioCreacion  { get; set; }
	    public string asis_vIPCreacion  { get; set; }
	    public DateTime asis_dFechaCreacion  { get; set; }
	    public Nullable<short> asis_sUsuarioModificacion  { get; set; }
	    public string asis_vIPModificacion  { get; set; }
	    public Nullable<DateTime> asis_dFechaModificacion  { get; set; }
    }
}
