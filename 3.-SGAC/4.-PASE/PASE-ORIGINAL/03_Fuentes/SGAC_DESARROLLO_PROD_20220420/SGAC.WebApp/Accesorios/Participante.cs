using System;
using System.Collections.Generic;
using System.Data;
using SGAC.Controlador;
using SGAC.Accesorios;

namespace SGAC.WebApp.Accesorios
{
    public class Participante
    {

        private void CrearTablaParticipante()
        {

        }

        private void AdicionarParticipanteATabla(DataTable dtParticipante)
        {

        }

        // Solo es para actuacion civil y militar
        private DataTable ObtenerParticipante(long lngActuacionDetalleId)
        {
            DataTable dtParticipantes = new DataTable();

            object[] arrParametros = { lngActuacionDetalleId }; 

            Proceso p = new Proceso();
            dtParticipantes = (DataTable)p.Invocar(ref arrParametros, "RE_PARTICIPANTE", Enumerador.enmAccion.BUSCAR);

            return dtParticipantes;
        }

        public List<BE.RE_PARTICIPANTE> ObtenerListaParticipantes(
             Enumerador.enmTipoActuacionParticipante enmTipoActuacion,
            long lngActuacionId, 
            int intUsuarioId, int intOficinaConsularId, 
            DataTable dtParticipante)
        {
            List<BE.RE_PARTICIPANTE> lstParticipantes = new List<BE.RE_PARTICIPANTE>();
            BE.RE_PARTICIPANTE objParticipante = new BE.RE_PARTICIPANTE();            

            if (dtParticipante.Rows.Count > 0)
            {                
                foreach (DataRow drParticipante in dtParticipante.Rows)
                {
                    objParticipante = new BE.RE_PARTICIPANTE();
                    switch (enmTipoActuacion)
                    {
                        case Enumerador.enmTipoActuacionParticipante.CIVIL:
                            #region Participante Civil
                            objParticipante.sTipoActuacionId = (int)Enumerador.enmTipoActuacionParticipante.CIVIL;
                            objParticipante.iParticipanteId = Convert.ToInt64(drParticipante["iActuacionParticipanteId"]);
                            objParticipante.iActuacionDetId = lngActuacionId;

                            objParticipante.sTipoParticipanteId = Convert.ToInt16(drParticipante["sTipoParticipanteId"]);
                            objParticipante.sTipoDatoId = Convert.ToInt16(drParticipante["sTipoDatoId"]);
                            objParticipante.sTipoVinculoId = Convert.ToInt16(drParticipante["sTipoVinculoId"]);

                            objParticipante.iPersonaId = Convert.ToInt64(drParticipante["iPersonaId"]);

                            if (objParticipante.iPersonaId == 0)
                            {
                                objParticipante.sTipoPersonaId = (int)Enumerador.enmTipoPersona.NATURAL;
                                objParticipante.sTipoDocumentoId = Convert.ToInt16(drParticipante["sDocumentoTipoId"]);
                                objParticipante.vNumeroDocumento = drParticipante["vDocumentoNumero"].ToString();
                                objParticipante.sNacionalidadId = Convert.ToInt16(drParticipante["sNacionalidadId"]);
                                objParticipante.vNombres = drParticipante["vNombres"].ToString();
                                objParticipante.vPrimerApellido = drParticipante["vNombres"].ToString();
                                objParticipante.vSegundoApellido = drParticipante["vApellidoPaterno"].ToString();
                                objParticipante.vDireccion = drParticipante["vResidenciaDireccion"].ToString();
                                objParticipante.vUbigeo = drParticipante["cResidenciaUbigeo"].ToString();
                                objParticipante.ICentroPobladoId = Convert.ToInt32(drParticipante["ICentroPobladoId"]);
                            }

                            objParticipante.cEstado = drParticipante["cEstado"].ToString();
                            objParticipante.sUsuarioCreacion = Convert.ToInt16(intUsuarioId);
                            objParticipante.vIPCreacion = Util.ObtenerDireccionIP();
                            objParticipante.sUsuarioModificacion = Convert.ToInt16(intUsuarioId);
                            objParticipante.vIPModificacion = Util.ObtenerDireccionIP();
                            objParticipante.sOficinaConsularId = Convert.ToInt16(intOficinaConsularId);
                            objParticipante.vHostname = Util.ObtenerHostName();
                            lstParticipantes.Add(objParticipante);
                            #endregion
                            break;
                        case Enumerador.enmTipoActuacionParticipante.MILITAR:
                            #region Participante Militar
                            objParticipante.sTipoActuacionId = (int)Enumerador.enmTipoActuacionParticipante.MILITAR;
                            objParticipante.iParticipanteId = Convert.ToInt64(drParticipante["iActuacionParticipanteId"]);
                            objParticipante.iActuacionDetId = lngActuacionId;

                            objParticipante.sTipoParticipanteId = Convert.ToInt16(drParticipante["sTipoParticipanteId"]);
                            objParticipante.sTipoDatoId = Convert.ToInt16(drParticipante["sTipoDatoId"]);
                            objParticipante.sTipoVinculoId = Convert.ToInt16(drParticipante["sTipoVinculoId"]);

                            objParticipante.iPersonaId = Convert.ToInt64(drParticipante["iPersonaId"]);
                            objParticipante.sTipoPersonaId = (Int16)Enumerador.enmTipoPersona.NATURAL;
                            if (objParticipante.iPersonaId == 0)
                            {                                
                                objParticipante.sTipoDocumentoId = Convert.ToInt16(drParticipante["sDocumentoTipoId"]);
                                objParticipante.vNumeroDocumento = drParticipante["vDocumentoNumero"].ToString();
                                objParticipante.sNacionalidadId = Convert.ToInt16(drParticipante["sNacionalidadId"]);
                                objParticipante.vNombres = drParticipante["vNombres"].ToString();
                                objParticipante.vPrimerApellido = drParticipante["vNombres"].ToString();
                                objParticipante.vSegundoApellido = drParticipante["vApellidoPaterno"].ToString();
                                objParticipante.vDireccion = drParticipante["vResidenciaDireccion"].ToString();
                                objParticipante.vUbigeo = drParticipante["cResidenciaUbigeo"].ToString();
                                objParticipante.ICentroPobladoId = 0;
                                if (drParticipante["ICentroPobladoId"] != null)
                                    if (drParticipante["ICentroPobladoId"].ToString() != string.Empty)
                                        objParticipante.ICentroPobladoId = Convert.ToInt32(drParticipante["ICentroPobladoId"]);                                    
                            }

                            objParticipante.cEstado = drParticipante["cEstado"].ToString();
                            objParticipante.sUsuarioCreacion = Convert.ToInt16(intUsuarioId);
                            objParticipante.vIPCreacion = Util.ObtenerDireccionIP();
                            objParticipante.sUsuarioModificacion = Convert.ToInt16(intUsuarioId);
                            objParticipante.vIPModificacion = Util.ObtenerDireccionIP();
                            objParticipante.sOficinaConsularId = Convert.ToInt16(intOficinaConsularId);
                            objParticipante.vHostname = Util.ObtenerHostName();
                            lstParticipantes.Add(objParticipante);
                            #endregion
                            break;
                        case Enumerador.enmTipoActuacionParticipante.JUDICIAL:
                            break;
                        case Enumerador.enmTipoActuacionParticipante.NOTARIAL:
                            break;
                        default:
                            break;
                    }
                }
            }
            return lstParticipantes;
        }

    }
}