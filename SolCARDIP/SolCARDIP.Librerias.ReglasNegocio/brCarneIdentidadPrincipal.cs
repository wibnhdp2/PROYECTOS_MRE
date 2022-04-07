using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Transactions;
using SolCARDIP.Librerias.EntidadesNegocio;
using SolCARDIP.Librerias.AccesoDatos;

namespace SolCARDIP.Librerias.ReglasNegocio
{
    public class brCarneIdentidadPrincipal:brGeneral
    {
        public short adicionar(beCarneIdentidadPrincipal obeCarneIdentidadPrincipal,out string resultado)
        {
            resultado = "";
            short CarneIdentidadId = -1;
            bool exito = false;
            SqlTransaction trx = null;
            using(SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    daPersona odaPersona = new daPersona();
                    long PersonaId = -1;
                    long PersonaIdentId = -1;
                    long ResidenciaId = -1;
                    long PersonaResidenciaId = -1;
                    int PersonaHistorico = -1;
                    int CarneHistorico = -1;
                    int MovCarneIdent = -1;

                    daPersonaIdentificacion odaPersonaIdentificacion = new daPersonaIdentificacion();
                    PersonaIdentId = odaPersonaIdentificacion.validarPersonaIdentificacion(con, trx, obeCarneIdentidadPrincipal.PersonaIdentificacion);
                    if (PersonaIdentId == 0) { exito = true; } else { exito = false; CarneIdentidadId = -2; }
                    if (exito)
                    {
                        PersonaId = odaPersona.adicionar(con, trx, obeCarneIdentidadPrincipal.Persona);
                        if (PersonaId != -1) { 
                            exito = true;
                            resultado = "odaPersona.adicionar - true";
                        } else {
                            exito = false;
                            resultado = "odaPersona.adicionar - false";
                        }
                        
                    }
                    if (exito)
                    {
                        obeCarneIdentidadPrincipal.PersonaIdentificacion.Personaid = PersonaId;
                        PersonaIdentId = odaPersonaIdentificacion.adicionar(con, trx, obeCarneIdentidadPrincipal.PersonaIdentificacion);
                        if (PersonaIdentId != -1) { 
                            exito = true;
                            resultado = "odaPersonaIdentificacion.adicionar - true";
                        } else { 
                            exito = false;
                            resultado = "odaPersonaIdentificacion.adicionar - false";
                        }
                        
                    }
                    if (exito)
                    {
                        daResidencia odaResidencia = new daResidencia();
                        ResidenciaId = odaResidencia.adicionar(con, trx, obeCarneIdentidadPrincipal.Residencia);
                        if (ResidenciaId != -1) { 
                            exito = true;
                            resultado = "odaResidencia.adicionar - true";
                        } else { 
                            exito = false;
                            resultado = "odaResidencia.adicionar - false";
                        }
                    }
                    if (exito)
                    {
                        daPersonaResidencia odaPersonaResidencia = new daPersonaResidencia();
                        obeCarneIdentidadPrincipal.PersonaResidencia.Personaid = PersonaId;
                        obeCarneIdentidadPrincipal.PersonaResidencia.Residenciaid = ResidenciaId;
                        PersonaResidenciaId = odaPersonaResidencia.adicionar(con, trx, obeCarneIdentidadPrincipal.PersonaResidencia);
                        if (PersonaResidenciaId != -1) { 
                            exito = true;
                            resultado = "odaPersonaResidencia.adicionar - true";
                        } else { 
                            exito = false;
                            resultado = "odaPersonaResidencia.adicionar - false";
                        }
                    }
                    if (exito)
                    {
                        daPersonaHistorico odaPersonaHistorico = new daPersonaHistorico();
                        obeCarneIdentidadPrincipal.PersonaHistorico.Personaid = PersonaId;
                        obeCarneIdentidadPrincipal.PersonaHistorico.PersonaIdentificacionId = PersonaIdentId;
                        obeCarneIdentidadPrincipal.PersonaHistorico.PersonaResindenciaId = PersonaResidenciaId;
                        PersonaHistorico = odaPersonaHistorico.adicionar(con, trx, obeCarneIdentidadPrincipal.PersonaHistorico);
                        if (PersonaHistorico != -1) { exito = true; } else { exito = false; }
                    }
                    if (exito)
                    {
                        daCarneidentidad odaCarneidentidad = new daCarneidentidad();
                        obeCarneIdentidadPrincipal.CarneIdentidad.Personaid = PersonaId;
                        CarneIdentidadId = odaCarneidentidad.adicionar(con, trx, obeCarneIdentidadPrincipal.CarneIdentidad);
                        if (CarneIdentidadId != -1) { 
                            exito = true;
                            resultado = "odaCarneidentidad.adicionar - true";
                        } else { 
                            exito = false;
                            resultado = "odaCarneidentidad.adicionar - false";
                        }
                    }
                    if (exito)
                    {
                        daCarneIdentidadHistorico odaCarneIdentidadHistorico = new daCarneIdentidadHistorico();
                        obeCarneIdentidadPrincipal.CarneIdentidadHistorico.CarneIdentidadid = CarneIdentidadId;
                        CarneHistorico = odaCarneIdentidadHistorico.adicionar(con,trx,obeCarneIdentidadPrincipal.CarneIdentidadHistorico);
                        if (CarneHistorico != -1) { 
                            exito = true;
                            resultado = "odaCarneIdentidadHistorico.adicionar - true";
                        } else { 
                            exito = false;
                            resultado = "odaCarneIdentidadHistorico.adicionar - false";
                        }
                    }
                    if (exito)
                    {
                        daMovimientoCarneIdentidad odaMovimientoCarneIdentidad = new daMovimientoCarneIdentidad();
                        obeCarneIdentidadPrincipal.MovimientoCarne.CarneIdentidadid = CarneIdentidadId;
                        MovCarneIdent = odaMovimientoCarneIdentidad.adicionar(con, trx, obeCarneIdentidadPrincipal.MovimientoCarne);
                        if (MovCarneIdent != -1) { exito = true; } else { exito = false; }
                    }

                    if (exito)// ACTUALIZA REGISTRO LINEA
                    {
                        if (obeCarneIdentidadPrincipal.RegistroLinea != null)
                        {
                            if (obeCarneIdentidadPrincipal.RegistroLinea.RegistroLineaId > 0)
                            {
                                daRegistroLinea odaRegistroLinea = new daRegistroLinea();
                                exito = odaRegistroLinea.actualizarEstado(con, trx, obeCarneIdentidadPrincipal.RegistroLinea);
                                resultado = "odaRegistroLinea.actualizarEstado";
                                //inserta EN LA TABLA detalle DECARDIP-LINEA en el estado a aprobado
                                 odaRegistroLinea.registrarDetalleAprobado(con, trx, obeCarneIdentidadPrincipal.RegistroLinea);
                                
                            }
                        }
                    }
                    if (exito)// VINCULACIÓN DE CARNÉ A SOLICITUD
                    {
                        if (obeCarneIdentidadPrincipal.RegistroLinea != null)
                        {
                            if (obeCarneIdentidadPrincipal.RegistroLinea.RegistroLineaId > 0)
                            {
                                obeCarneIdentidadPrincipal.RegistroLinea.CarneIdentidadId = CarneIdentidadId;
                                daRegistroLinea odaRegistroLinea = new daRegistroLinea();
                                exito = odaRegistroLinea.vincularRegLineaCardip(con, trx, obeCarneIdentidadPrincipal.RegistroLinea);
                                resultado = "odaRegistroLinea.vincularRegLineaCardip";
                            }
                        }
                    }
                    if (exito) // SE AGREGO PARA DERIVAR AUTOMATICAMENTE AL USUARIO QUE CREA EL REGISTRO
                    {
                        obeCarneIdentidadPrincipal.CarneIdentidad.CarneIdentidadid = CarneIdentidadId;
                        daCarneidentidad odaCarneidentidad = new daCarneidentidad();
                        exito = odaCarneidentidad.derivar(con, trx, obeCarneIdentidadPrincipal.CarneIdentidad);
                        resultado = "odaCarneidentidad.derivar";
                    }
                    if (exito)
                    {
                        if (obeCarneIdentidadPrincipal.titularTitDep != null)
                        {
                            beCarneIdentidadRelacionDependencia obeCarneIdentidadRelacionDependencia = new beCarneIdentidadRelacionDependencia();
                            obeCarneIdentidadRelacionDependencia.CarneIdentidadTitId = obeCarneIdentidadPrincipal.titularTitDep.CarneIdentidadid;
                            obeCarneIdentidadRelacionDependencia.CarneIdentidadDepId = CarneIdentidadId;
                            obeCarneIdentidadRelacionDependencia.UsuarioCreacion = obeCarneIdentidadPrincipal.titularTitDep.Usuariocreacion;
                            obeCarneIdentidadRelacionDependencia.IpCreacion = obeCarneIdentidadPrincipal.titularTitDep.Ipcreacion;
                            daCarneIdentidadRelacionDependencia odaCarneIdentidadRelacionDependencia = new daCarneIdentidadRelacionDependencia();
                            short idRelDep = odaCarneIdentidadRelacionDependencia.adicionar(con, trx, obeCarneIdentidadRelacionDependencia);
                            if (idRelDep != -1) { 
                                exito = true;
                                resultado = "odaCarneIdentidadRelacionDependencia.adicionar titularTitDep - true";
                            } else {
                                exito = false;
                                resultado = "odaCarneIdentidadRelacionDependencia.adicionar titularTitDep - false";
                            }
                        }
                        else
                        {
                            beCarneIdentidadRelacionDependencia obeCarneIdentidadRelacionDependencia = new beCarneIdentidadRelacionDependencia();
                            obeCarneIdentidadRelacionDependencia.CarneIdentidadTitId = CarneIdentidadId;
                            obeCarneIdentidadRelacionDependencia.CarneIdentidadDepId = CarneIdentidadId;
                            obeCarneIdentidadRelacionDependencia.UsuarioCreacion = obeCarneIdentidadPrincipal.CarneIdentidad.Usuariocreacion;
                            obeCarneIdentidadRelacionDependencia.IpCreacion = obeCarneIdentidadPrincipal.CarneIdentidad.Ipcreacion;
                            daCarneIdentidadRelacionDependencia odaCarneIdentidadRelacionDependencia = new daCarneIdentidadRelacionDependencia();
                            short idRelDep = odaCarneIdentidadRelacionDependencia.adicionar(con, trx, obeCarneIdentidadRelacionDependencia);
                            if (idRelDep != -1) { 
                                exito = true;
                                resultado = "odaCarneIdentidadRelacionDependencia.adicionar - true";
                            } else { 
                                exito = false;
                                resultado = "odaCarneIdentidadRelacionDependencia.adicionar - false";
                            } 
                        }
                    }
                    if (exito) trx.Commit();
                    else
                    {
                        //CarneIdentidadId = -1;
                        trx.Rollback();
                    }
                }
                catch(SqlException exsql)
                {
                    grabarLog(exsql);
                    trx.Rollback();
                }
            }
            return (CarneIdentidadId);
        }

        //public bool actualizar(beCarneIdentidadPrincipal obeCarneIdentidadPrincipal)
        //{
        //    //short CarneIdentidadId = -1;
        //    bool exito = false;
        //    SqlTransaction trx = null;
        //    using (SqlConnection con = new SqlConnection(CadenaConexion))
        //    {
        //        try
        //        {
        //            con.Open();
        //            trx = con.BeginTransaction();
        //            daPersona odaPersona = new daPersona();
        //            long PersonaIdentId = -1;
        //            long ResidenciaId = -1;
        //            long PersonaResidenciaId = -1;
        //            int PersonaHistorico = -1;
        //            int CarneHistorico = -1;
        //            int MovCarneIdent = -1;

        //            //PersonaIdentId = odaPersonaIdentificacion.validarPersonaIdentificacion(con, trx, obeCarneIdentidadPrincipal.PersonaIdentificacion);
        //            //if (PersonaIdentId == 0) { exito = true; } else { exito = false; CarneIdentidadId = -2; }
        //            exito = odaPersona.actualizar(con, trx, obeCarneIdentidadPrincipal.Persona);
        //            //if (exito) { CarneIdentidadId = 1; } else { CarneIdentidadId = -1; }
        //            if (exito)
        //            {
        //                daPersonaIdentificacion odaPersonaIdentificacion = new daPersonaIdentificacion();
        //                PersonaIdentId = odaPersonaIdentificacion.adicionar(con, trx, obeCarneIdentidadPrincipal.PersonaIdentificacion);
        //                if (PersonaIdentId != -1) { exito = true; } else { exito = false; }
        //            }
        //            if (exito)
        //            {
        //                daResidencia odaResidencia = new daResidencia();
        //                ResidenciaId = odaResidencia.adicionar(con, trx, obeCarneIdentidadPrincipal.Residencia);
        //                if (ResidenciaId != -1) { exito = true; } else { exito = false; }
        //            }
        //            if (exito)
        //            {
        //                daPersonaResidencia odaPersonaResidencia = new daPersonaResidencia();
        //                obeCarneIdentidadPrincipal.PersonaResidencia.Residenciaid = ResidenciaId;
        //                PersonaResidenciaId = odaPersonaResidencia.adicionar(con, trx, obeCarneIdentidadPrincipal.PersonaResidencia);
        //                if (PersonaResidenciaId != -1) { exito = true; } else { exito = false; }
        //            }
        //            if (exito)
        //            {
        //                daPersonaHistorico odaPersonaHistorico = new daPersonaHistorico();
        //                obeCarneIdentidadPrincipal.PersonaHistorico.PersonaIdentificacionId = PersonaIdentId;
        //                obeCarneIdentidadPrincipal.PersonaHistorico.PersonaResindenciaId = PersonaResidenciaId;
        //                PersonaHistorico = odaPersonaHistorico.adicionar(con, trx, obeCarneIdentidadPrincipal.PersonaHistorico);
        //                if (PersonaHistorico != -1) { exito = true; } else { exito = false; }
        //            }
        //            if (exito)
        //            {
        //                daCarneidentidad odaCarneidentidad = new daCarneidentidad();
        //                exito = odaCarneidentidad.actualizar(con, trx, obeCarneIdentidadPrincipal.CarneIdentidad);
        //                //if (exito) { CarneIdentidadId = 1; } else { CarneIdentidadId = -1; }
        //            }
        //            if (exito)
        //            {
        //                daCarneIdentidadHistorico odaCarneIdentidadHistorico = new daCarneIdentidadHistorico();
        //                CarneHistorico = odaCarneIdentidadHistorico.adicionar(con, trx, obeCarneIdentidadPrincipal.CarneIdentidadHistorico);
        //                if (CarneHistorico != -1) { exito = true; } else { exito = false; }
        //            }
        //            if (exito)
        //            {
        //                daMovimientoCarneIdentidad odaMovimientoCarneIdentidad = new daMovimientoCarneIdentidad();
        //                MovCarneIdent = odaMovimientoCarneIdentidad.adicionar(con, trx, obeCarneIdentidadPrincipal.MovimientoCarne);
        //                if (MovCarneIdent != -1) { exito = true; } else { exito = false; }
        //            }
        //            if (exito)
        //            {
        //                if (obeCarneIdentidadPrincipal.RegistroPrevio != null)
        //                {
        //                    if (obeCarneIdentidadPrincipal.RegistroPrevio.RegistroPrevioId > 0)
        //                    {
        //                        daRegistroPrevio odaRegistroPrevio = new daRegistroPrevio();
        //                        exito = odaRegistroPrevio.actualizar(con, trx, obeCarneIdentidadPrincipal.RegistroPrevio);
        //                    }
        //                }
        //            }
        //            if (exito) trx.Commit();
        //            else
        //            {
        //                trx.Rollback();
        //            }
        //        }
        //        catch (SqlException exsql)
        //        {
        //            grabarLog(exsql);
        //        }
        //    }
        //    return (exito);
        //}

        public bool actualizar(beCarneIdentidadPrincipal obeCarneIdentidadPrincipal)
        {
            //short CarneIdentidadId = -1;
            bool exito = false;
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    daPersona odaPersona = new daPersona();
                    long PersonaIdentId = -1;
                    long ResidenciaId = -1;
                    long PersonaResidenciaId = -1;
                    int PersonaHistorico = -1;
                    int CarneHistorico = -1;
                    int MovCarneIdent = -1;

                    //PersonaIdentId = odaPersonaIdentificacion.validarPersonaIdentificacion(con, trx, obeCarneIdentidadPrincipal.PersonaIdentificacion);
                    //if (PersonaIdentId == 0) { exito = true; } else { exito = false; CarneIdentidadId = -2; }
                    exito = odaPersona.actualizar(con, trx, obeCarneIdentidadPrincipal.Persona);
                    //if (exito) { CarneIdentidadId = 1; } else { CarneIdentidadId = -1; }
                    if (exito)
                    {
                        daPersonaIdentificacion odaPersonaIdentificacion = new daPersonaIdentificacion();
                        PersonaIdentId = odaPersonaIdentificacion.adicionar(con, trx, obeCarneIdentidadPrincipal.PersonaIdentificacion);
                        if (PersonaIdentId != -1) { exito = true; } else { exito = false; }
                    }
                    if (exito)
                    {
                        daResidencia odaResidencia = new daResidencia();
                        ResidenciaId = odaResidencia.adicionar(con, trx, obeCarneIdentidadPrincipal.Residencia);
                        if (ResidenciaId != -1) { exito = true; } else { exito = false; }
                    }
                    if (exito)
                    {
                        daPersonaResidencia odaPersonaResidencia = new daPersonaResidencia();
                        obeCarneIdentidadPrincipal.PersonaResidencia.Residenciaid = ResidenciaId;
                        PersonaResidenciaId = odaPersonaResidencia.adicionar(con, trx, obeCarneIdentidadPrincipal.PersonaResidencia);
                        if (PersonaResidenciaId != -1) { exito = true; } else { exito = false; }
                    }
                    if (exito)
                    {
                        daPersonaHistorico odaPersonaHistorico = new daPersonaHistorico();
                        obeCarneIdentidadPrincipal.PersonaHistorico.PersonaIdentificacionId = PersonaIdentId;
                        obeCarneIdentidadPrincipal.PersonaHistorico.PersonaResindenciaId = PersonaResidenciaId;
                        PersonaHistorico = odaPersonaHistorico.adicionar(con, trx, obeCarneIdentidadPrincipal.PersonaHistorico);
                        if (PersonaHistorico != -1) { exito = true; } else { exito = false; }
                    }
                    if (exito)
                    {
                        daCarneidentidad odaCarneidentidad = new daCarneidentidad();
                        exito = odaCarneidentidad.actualizar(con, trx, obeCarneIdentidadPrincipal.CarneIdentidad);
                        //if (exito) { CarneIdentidadId = 1; } else { CarneIdentidadId = -1; }
                    }
                    if (exito)
                    {
                        daCarneIdentidadHistorico odaCarneIdentidadHistorico = new daCarneIdentidadHistorico();
                        CarneHistorico = odaCarneIdentidadHistorico.adicionar(con, trx, obeCarneIdentidadPrincipal.CarneIdentidadHistorico);
                        if (CarneHistorico != -1) { exito = true; } else { exito = false; }
                    }
                    if (exito)
                    {
                        daMovimientoCarneIdentidad odaMovimientoCarneIdentidad = new daMovimientoCarneIdentidad();
                        MovCarneIdent = odaMovimientoCarneIdentidad.adicionar(con, trx, obeCarneIdentidadPrincipal.MovimientoCarne);
                        if (MovCarneIdent != -1) { exito = true; } else { exito = false; }
                    }
                    if (exito)// ACTUALIZA REGISTRO LINEA
                    {
                        if (obeCarneIdentidadPrincipal.RegistroLinea != null)
                        {
                            if (obeCarneIdentidadPrincipal.RegistroLinea.RegistroLineaId > 0)
                            {
                                daRegistroLinea odaRegistroLinea = new daRegistroLinea();
                                exito = odaRegistroLinea.actualizarEstado(con, trx, obeCarneIdentidadPrincipal.RegistroLinea);
                                //inserta EN LA TABLA detalle DECARDIP-LINEA en el estado a aprobado
                                odaRegistroLinea.registrarDetalleAprobado(con, trx, obeCarneIdentidadPrincipal.RegistroLinea);
                                /*if (obeCarneIdentidadPrincipal.RegistroLinea.Estado.Equals("OBSERVADO"))
                                {
                                    exito = odaRegistroLinea.registrarDetalleRegistroLinea(con, trx, obeCarneIdentidadPrincipal.RegistroLinea);
                                }*/
                            }
                        }
                    }
                    if (exito)// VINCULACIÓN DE CARNÉ A SOLICITUD
                    {
                        if (obeCarneIdentidadPrincipal.RegistroLinea.RegistroLineaId > 0)
                        {
                            obeCarneIdentidadPrincipal.RegistroLinea.CarneIdentidadId = obeCarneIdentidadPrincipal.CarneIdentidad.CarneIdentidadid;
                            daRegistroLinea odaRegistroLinea = new daRegistroLinea();
                            exito = odaRegistroLinea.vincularRegLineaCardip(con, trx, obeCarneIdentidadPrincipal.RegistroLinea);
                        }
                    }
                    // RELACION DEPENDENCIA
                    if (exito)
                    {
                        if (obeCarneIdentidadPrincipal.RegistroLinea.RegistroLineaId > 0)
                        {
                            if (obeCarneIdentidadPrincipal.RelacionDependencia != null)
                            {
                                if (obeCarneIdentidadPrincipal.RelacionDependencia.CarneIdentidadTitId == 0) { obeCarneIdentidadPrincipal.RelacionDependencia.CarneIdentidadTitId = obeCarneIdentidadPrincipal.CarneIdentidad.CarneIdentidadid; }
                                obeCarneIdentidadPrincipal.RelacionDependencia.CarneIdentidadDepId = obeCarneIdentidadPrincipal.CarneIdentidad.CarneIdentidadid;

                                if (obeCarneIdentidadPrincipal.RelacionDependencia.TitularDependienteId != 0)
                                {
                                    daCarneIdentidadRelacionDependencia odaCarneIdentidadRelacionDependencia = new daCarneIdentidadRelacionDependencia();
                                    exito = odaCarneIdentidadRelacionDependencia.actualizar(con, trx, obeCarneIdentidadPrincipal.RelacionDependencia);
                                }
                                else
                                {
                                    daCarneIdentidadRelacionDependencia odaCarneIdentidadRelacionDependencia = new daCarneIdentidadRelacionDependencia();
                                    short idRelDep = odaCarneIdentidadRelacionDependencia.adicionar(con, trx, obeCarneIdentidadPrincipal.RelacionDependencia);
                                    if (idRelDep != -1) { exito = true; } else { exito = false; }
                                }
                            }
                        }
                    }   
                    //if (exito)
                    //{
                    //    if (obeCarneIdentidadPrincipal.RegistroPrevio != null)
                    //    {
                    //        if (obeCarneIdentidadPrincipal.RegistroPrevio.RegistroPrevioId > 0)
                    //        {
                    //            daRegistroPrevio odaRegistroPrevio = new daRegistroPrevio();
                    //            exito = odaRegistroPrevio.actualizar(con, trx, obeCarneIdentidadPrincipal.RegistroPrevio);
                    //        }
                    //    }
                    //}
                    if (exito) trx.Commit();
                    else
                    {
                        trx.Rollback();
                    }
                }
                catch (SqlException exsql)
                {
                    grabarLog(exsql);
                }
            }
            return (exito);
        }

        public bool GenerarDuplicadoCarnet(beCarneIdentidad parametrosCarneIdentidad, beRegistroLinea parametros,beMovimientoCarneIdentidad parametrosMovimientoCarneIdentidad)
        {
            bool exito = false;
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    daCarneidentidad obj = new daCarneidentidad();
                    obj.GenerarDuplicadoCarnet(con, trx, parametrosCarneIdentidad, parametros);
                    daMovimientoCarneIdentidad objMov = new daMovimientoCarneIdentidad();
                    objMov.adicionar(con, trx, parametrosMovimientoCarneIdentidad);
                    trx.Commit();
                    con.Close();
                }
                catch (SqlException exsql)
                {
                    trx.Rollback();
                    con.Close();
                    grabarLog(exsql);
                }
            }
            return exito;
        }

        public bool ConsultarExistenciaCarnetPorNombre(beCarneIdentidad parametrosCarneIdentidad)
        {
            bool exito = false;
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    daCarneidentidad obj = new daCarneidentidad();
                    exito = obj.ConsultarExistenciaCarnetPorNombre(con, trx, parametrosCarneIdentidad);
                    trx.Commit();
                    con.Close();
                }
                catch (SqlException exsql)
                {
                    trx.Rollback();
                    con.Close();
                    grabarLog(exsql);
                }
            }
            return exito;
        }
        public short actualizarRegistroCompleto(beCarneIdentidadPrincipal obeCarneIdentidadPrincipal)
        {
            short CarneIdentidadId = -1;
            bool exito = false;
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    daPersona odaPersona = new daPersona();
                    long PersonaId = -1;
                    long PersonaIdentId = -1;
                    long ResidenciaId = -1;
                    long PersonaResidenciaId = -1;
                    int PersonaHistorico = -1;
                    int CarneHistorico = -1;
                    int MovCarneIdent = -1;
                    #region Persona
                    daPersonaIdentificacion odaPersonaIdentificacion = new daPersonaIdentificacion();
                    PersonaIdentId = odaPersonaIdentificacion.validarPersonaIdentificacion(con, trx, obeCarneIdentidadPrincipal.PersonaIdentificacion);
                    if (PersonaIdentId == 0) { exito = true; } else { exito = false; CarneIdentidadId = -2; }
                    if (exito)
                    {
                        PersonaId = odaPersona.adicionar(con, trx, obeCarneIdentidadPrincipal.Persona);
                        if (PersonaId != -1) { exito = true; } else { exito = false; }
                    }
                    if (exito)
                    {
                        obeCarneIdentidadPrincipal.PersonaIdentificacion.Personaid = PersonaId;
                        PersonaIdentId = odaPersonaIdentificacion.adicionar(con, trx, obeCarneIdentidadPrincipal.PersonaIdentificacion);
                        if (PersonaIdentId != -1) { exito = true; } else { exito = false; }
                    }
                    if (exito)
                    {
                        daResidencia odaResidencia = new daResidencia();
                        ResidenciaId = odaResidencia.adicionar(con, trx, obeCarneIdentidadPrincipal.Residencia);
                        if (ResidenciaId != -1) { exito = true; } else { exito = false; }
                    }
                    if (exito)
                    {
                        daPersonaResidencia odaPersonaResidencia = new daPersonaResidencia();
                        obeCarneIdentidadPrincipal.PersonaResidencia.Personaid = PersonaId;
                        obeCarneIdentidadPrincipal.PersonaResidencia.Residenciaid = ResidenciaId;
                        PersonaResidenciaId = odaPersonaResidencia.adicionar(con, trx, obeCarneIdentidadPrincipal.PersonaResidencia);
                        if (PersonaResidenciaId != -1) { exito = true; } else { exito = false; }
                    }
                    if (exito)
                    {
                        daPersonaHistorico odaPersonaHistorico = new daPersonaHistorico();
                        obeCarneIdentidadPrincipal.PersonaHistorico.Personaid = PersonaId;
                        obeCarneIdentidadPrincipal.PersonaHistorico.PersonaIdentificacionId = PersonaIdentId;
                        obeCarneIdentidadPrincipal.PersonaHistorico.PersonaResindenciaId = PersonaResidenciaId;
                        PersonaHistorico = odaPersonaHistorico.adicionar(con, trx, obeCarneIdentidadPrincipal.PersonaHistorico);
                        if (PersonaHistorico != -1) { exito = true; } else { exito = false; }
                    }
                    #endregion
                    if (exito)
                    {
                        daCarneidentidad odaCarneidentidad = new daCarneidentidad();
                        obeCarneIdentidadPrincipal.CarneIdentidad.Personaid = PersonaId;
                        exito = odaCarneidentidad.actualizar(con, trx, obeCarneIdentidadPrincipal.CarneIdentidad);
                        if (exito) { CarneIdentidadId = 1; } else { CarneIdentidadId = -1; }
                    }
                    if (exito)
                    {
                        daCarneIdentidadHistorico odaCarneIdentidadHistorico = new daCarneIdentidadHistorico();
                        CarneHistorico = odaCarneIdentidadHistorico.adicionar(con, trx, obeCarneIdentidadPrincipal.CarneIdentidadHistorico);
                        if (CarneHistorico != -1) { exito = true; } else { exito = false; }
                        if (exito) { CarneIdentidadId = 1; } else { CarneIdentidadId = -1; }
                    }
                    if (exito)
                    {
                        daMovimientoCarneIdentidad odaMovimientoCarneIdentidad = new daMovimientoCarneIdentidad();
                        MovCarneIdent = odaMovimientoCarneIdentidad.adicionar(con, trx, obeCarneIdentidadPrincipal.MovimientoCarne);
                        if (MovCarneIdent != -1) { exito = true; } else { exito = false; }
                        if (exito) { CarneIdentidadId = 1; } else { CarneIdentidadId = -1; }
                    }
                    if (exito)
                    {
                        daRegistroPrevio odaRegistroPrevio = new daRegistroPrevio();
                        exito = odaRegistroPrevio.actualizar(con, trx, obeCarneIdentidadPrincipal.RegistroPrevio);
                        if (exito) { CarneIdentidadId = 1; } else { CarneIdentidadId = -1; }
                    }
                    if (exito) trx.Commit();
                    else
                    {
                        trx.Rollback();
                    }
                }
                catch (SqlException exsql)
                {
                    grabarLog(exsql);
                }
            }
            return (CarneIdentidadId);
        }

        public bool actualizarEstado(beCarneIdentidadPrincipal obeCarneIdentidadPrincipal)
        {
            bool exito = false;
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    string Fecha = "01/01/0001";
                    DateTime FechaNull = DateTime.Parse(Fecha);
                    int MovCarneIdent = -1;
                    int CarneHistorico = -1;
                    con.Open();
                    trx = con.BeginTransaction();
                    daCarneidentidad odaCarneidentidad = new daCarneidentidad();
                    exito = odaCarneidentidad.actualizarEstado(con, trx, obeCarneIdentidadPrincipal.CarneIdentidad);
                    if (exito)
                    {
                        if (obeCarneIdentidadPrincipal.CarneIdentidadHistorico != null)
                        {
                            if (obeCarneIdentidadPrincipal.CarneIdentidadHistorico.FechaEmision != FechaNull)
                            {
                                daCarneIdentidadHistorico odaCarneIdentidadHistorico = new daCarneIdentidadHistorico();
                                CarneHistorico = odaCarneIdentidadHistorico.adicionar(con, trx, obeCarneIdentidadPrincipal.CarneIdentidadHistorico);
                                if (CarneHistorico != -1) { exito = true; } else { exito = false; }
                            }
                        }
                    }
                    if (exito)
                    {
                        daMovimientoCarneIdentidad odaMovimientoCarneIdentidad = new daMovimientoCarneIdentidad();
                        MovCarneIdent = odaMovimientoCarneIdentidad.adicionar(con, trx, obeCarneIdentidadPrincipal.MovimientoCarne);
                        if (MovCarneIdent != -1) { exito = true; } else { exito = false; }
                    }
                    if (exito) trx.Commit();
                    else
                    {
                        trx.Rollback();
                    }
                    
                }
                catch (SqlException exsql)
                {
                    grabarLog(exsql);
                }
            }
            return (exito);
        }
        public bool registrarDetalleRegistroLineaAtendido(int CarneIdentidadid, string mensaje)
        {
            bool exito = false;
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    daRegistroLinea odaCarneidentidad = new daRegistroLinea();
                    exito = odaCarneidentidad.registrarDetalleRegistroLineaAtendido(con, CarneIdentidadid, mensaje);
                    
                }
                catch (SqlException exsql)
                {
                    grabarLog(exsql);
                }
            }
            return (exito);
        }
        
        public bool actualizarEstadoObservado(beCarneIdentidadPrincipal obeCarneIdentidadPrincipal)
        {
            bool exito = false;
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    string Fecha = "01/01/0001";
                    DateTime FechaNull = DateTime.Parse(Fecha);
                    int MovCarneIdent = -1;
                    con.Open();
                    trx = con.BeginTransaction();
                    daCarneidentidad odaCarneidentidad = new daCarneidentidad();
                    exito = odaCarneidentidad.actualizarEstadoObservado(con, trx, obeCarneIdentidadPrincipal.CarneIdentidad);
                    /*if (exito)
                    {
                        daMovimientoCarneIdentidad odaMovimientoCarneIdentidad = new daMovimientoCarneIdentidad();
                        MovCarneIdent = odaMovimientoCarneIdentidad.adicionar(con, trx, obeCarneIdentidadPrincipal.MovimientoCarne);
                        if (MovCarneIdent != -1) { exito = true; } else { exito = false; }
                    }*/
                    if (exito) trx.Commit();
                    else
                    {
                        trx.Rollback();
                    }
                }
                catch (SqlException exsql)
                {
                    grabarLog(exsql);
                }
            }
            return (exito);
        }
        public beCarneIdentidad obtenerCorreoCiudadano(beCarneIdentidadPrincipal obeCarneIdentidadPrincipal)
        {
            beCarneIdentidad be =null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    daCarneidentidad odaCarneidentidad = new daCarneidentidad();
                    be = odaCarneidentidad.obtenerCorreoCiudadano(con,  obeCarneIdentidadPrincipal.CarneIdentidad);
                }
                catch (SqlException exsql)
                {
                    grabarLog(exsql);
                }
            }
            return be;
        }
        
        public string obtenerMensajeEstado(beCarneIdentidadPrincipal obeCarneIdentidadPrincipal)
        {
            string mensaje = "";
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    daCarneidentidad odaCarneidentidad = new daCarneidentidad();
                    mensaje = odaCarneidentidad.obtenerMesajeEstado(con,  obeCarneIdentidadPrincipal.CarneIdentidad);
                }
                catch (SqlException exsql)
                {
                    grabarLog(exsql);
                }
            }
            return mensaje;
        }

        public beCarneIdentidadPrincipal consultar(beCarneIdentidad parametrosCarneIdentidad)
        {
            beCarneIdentidadPrincipal obeCarneIdentidadPrincipal = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    daCarneidentidad daCarneidentidad = new daCarneidentidad();
                    obeCarneIdentidadPrincipal = daCarneidentidad.consultar(con, parametrosCarneIdentidad);
                }
                catch (Exception ex)
                {
                    grabarLog(ex);
                }
            }
            return (obeCarneIdentidadPrincipal);
        }

        public beCarneIdentidad consultarxId(short carneID)
        {
            beCarneIdentidad obeCarneIdentidad = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    daCarneidentidad daCarneidentidad = new daCarneidentidad();
                    obeCarneIdentidad = daCarneidentidad.consultarxId(con, carneID);
                }
                catch (Exception ex)
                {
                    grabarLog(ex);
                }
            }
            return (obeCarneIdentidad);
        }

        public beCarneIdentidadPrincipal consultarPrivilegios(beCarneIdentidad parametrosCarneIdentidad)
        {
            beCarneIdentidadPrincipal obeCarneIdentidadPrincipal = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    daCarneidentidad daCarneidentidad = new daCarneidentidad();
                    obeCarneIdentidadPrincipal = daCarneidentidad.consultarPrivilegios(con, parametrosCarneIdentidad);
                }
                catch (Exception ex)
                {
                    grabarLog(ex);
                }
            }
            return (obeCarneIdentidadPrincipal);
        }

        public beCarneIdentidadPrincipal consultarRegistroEdicion(beCarneIdentidadPrincipal parametrosCarneIdentidad)
        {
            beCarneIdentidadPrincipal obeCarneIdentidadPrincipal = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    daCarneidentidad odaCarneidentidad = new daCarneidentidad();
                    obeCarneIdentidadPrincipal = odaCarneidentidad.consultarRegistroEdicion(con, parametrosCarneIdentidad.CarneIdentidad);
                    if (obeCarneIdentidadPrincipal.CarneIdentidad == null)
                    {
                        daRegistroPrevio odaRegistroPrevio = new daRegistroPrevio();
                        obeCarneIdentidadPrincipal.RegistroPrevio = odaRegistroPrevio.consultarRegistroEdicion(con, parametrosCarneIdentidad.RegistroPrevio);
                        if (obeCarneIdentidadPrincipal.RelacionDependencia != null)
                        {
                            short titId = obeCarneIdentidadPrincipal.RelacionDependencia.CarneIdentidadTitId;
                            short depId = obeCarneIdentidadPrincipal.RelacionDependencia.CarneIdentidadDepId;
                            if (titId != depId)
                            {
                                beCarneIdentidad obeCarneIdentidad = new beCarneIdentidad();
                                obeCarneIdentidad = odaCarneidentidad.consultarxId(con, titId);
                                obeCarneIdentidadPrincipal.RelacionDependenciaEdicion = obeCarneIdentidad;
                            }
                        }
                    }
                    else
                    {
                        if (obeCarneIdentidadPrincipal.RelacionDependencia != null)
                        {
                            short titId = obeCarneIdentidadPrincipal.RelacionDependencia.CarneIdentidadTitId;
                            short depId = obeCarneIdentidadPrincipal.RelacionDependencia.CarneIdentidadDepId;
                            if (titId != depId)
                            {
                                beCarneIdentidad obeCarneIdentidad = new beCarneIdentidad();
                                obeCarneIdentidad = odaCarneidentidad.consultarxId(con, titId);
                                obeCarneIdentidadPrincipal.RelacionDependenciaEdicion = obeCarneIdentidad;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    grabarLog(ex);
                }
            }
            return (obeCarneIdentidadPrincipal);
        }

        public string obtenerIdent(short parametro)
        {
            string Ident = "error";
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    daCarneidentidad daCarneidentidad = new daCarneidentidad();
                    Ident = daCarneidentidad.obtenerIdent(con, parametro);
                }
                catch (Exception ex)
                {
                    grabarLog(ex);
                }
            }
            return (Ident);
        }

        public bool derivar(beCarneIdentidadPrincipal obeCarneIdentidadPrincipal)
        {
            bool exito = false;
            int MovCarneIdent = -1;
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    daCarneidentidad odaCarneidentidad = new daCarneidentidad();
                    exito = odaCarneidentidad.derivar(con, trx, obeCarneIdentidadPrincipal.CarneIdentidad);
                    if (exito)
                    {
                        daMovimientoCarneIdentidad odaMovimientoCarneIdentidad = new daMovimientoCarneIdentidad();
                        MovCarneIdent = odaMovimientoCarneIdentidad.adicionar(con, trx, obeCarneIdentidadPrincipal.MovimientoCarne);
                        if (MovCarneIdent != -1) { exito = true; } else { exito = false; }
                    }
                    if (exito) trx.Commit();
                    else
                    {
                        trx.Rollback();
                    }
                }
                catch (SqlException exsql)
                {
                    grabarLog(exsql);
                }
            }
            return (exito);
        }

        public bool derivarRenovacion(beCarneIdentidadPrincipal obeCarneIdentidadPrincipal)
        {
            bool exito = false;
            int MovCarneIdent = -1;
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    daCarneidentidad odaCarneidentidad = new daCarneidentidad();
                    exito = odaCarneidentidad.derivarRenovacion(con, trx, obeCarneIdentidadPrincipal.CarneIdentidad);
                    if (exito)
                    {
                        daMovimientoCarneIdentidad odaMovimientoCarneIdentidad = new daMovimientoCarneIdentidad();
                        MovCarneIdent = odaMovimientoCarneIdentidad.adicionar(con, trx, obeCarneIdentidadPrincipal.MovimientoCarne);
                        if (MovCarneIdent != -1) { exito = true; } else { exito = false; }
                    }
                    if (exito) trx.Commit();
                    else
                    {
                        trx.Rollback();
                    }
                }
                catch (SqlException exsql)
                {
                    grabarLog(exsql);
                }
            }
            return (exito);
        }
        public bool subsanarObservacion(beRegistroLinea obeRegistroLinea)
        {
            bool exito = false;
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    daCarneidentidad odaCarneidentidad = new daCarneidentidad();
                    daRegistroLinea odaRegistroLinea = new daRegistroLinea();
                    //-----actualiza el estado observado a estado previo Enviado
                    exito = odaRegistroLinea.registrarDetalleRegistroLinea(con, trx, obeRegistroLinea);

                    if (exito) { trx.Commit(); }
                    else
                    {
                        trx.Rollback();
                    }
                }
                catch (SqlException exsql)
                {
                    grabarLog(exsql);
                }
            }
            return (exito);
        }

    }
}
