﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;

namespace SIA_MOVIL_MODELO
{
    public class Metodos
    {
        public static void IniciaSesion(VM_Usuario DATA)
        {
            try
            {
                DATA.USER_DATA.USER = DATA.USER_DATA.USER.ToUpper();


                DataTable DT = new DataTable();

                OracleConnection CON = new OracleConnection(Comun._STR_CON());
                OracleCommand CMD = new OracleCommand();
                CMD.Connection = CON;
                CMD.CommandText = Comun._PACKAGE() + "SP_VALIDA_USUARIO";
                CMD.CommandType = CommandType.StoredProcedure;

                CMD.Parameters.Add("VV_USUARIO", OracleType.VarChar, 50).Value = DATA.USER_DATA.USER;
                CMD.Parameters.Add("VV_PASSWORD", OracleType.VarChar, 50).Value = DATA.USER_DATA.PASS;


                OracleParameter PC_DSC_ERROR = new OracleParameter();
                PC_DSC_ERROR.ParameterName = "VV_MENSAJE";
                PC_DSC_ERROR.Direction = ParameterDirection.Output;
                PC_DSC_ERROR.OracleType = OracleType.VarChar;
                PC_DSC_ERROR.Size = 2000;

                CMD.Parameters.Add(PC_DSC_ERROR);

                CMD.Parameters.Add("IO_CURSOR", OracleType.Cursor).Direction = ParameterDirection.Output;

                OracleDataAdapter DA = new OracleDataAdapter(CMD);

                DA.Fill(DT);

                if (DT.Rows.Count > 0)
                {
                    foreach (DataRow ITEM in DT.Rows)
                    {

                        DATA.USER_DATA.NOMBRE = ITEM["USUA_NOMBRE"].ToString();
                        DATA.USER_DATA.CORREO = ITEM["USUA_EMAIL"].ToString();
                        DATA.USER_DATA.TELEFONO = ITEM["USUA_TELEFONO"].ToString();
                    }
                    //DATA.TOKEN = GenesysJWT.JWT.GeneraToken(DATA);
                    DATA.TOKEN = GenesysJWT.JWT.GenerateTokenJwt(DATA.USER_DATA.NOMBRE);
                }
                else
                {
                    DATA.ERROR_ID = 1;
                    DATA.ERROR_DSC = "Usuario no registrado";
                }
            }
            catch (Exception ex)
            {
                DATA.ERROR_ID = 1;
                DATA.ERROR_DSC = String.Format("Error al iniciar sesion: {0}", ex.Message);
            }
            DATA.USER_DATA.PASS = string.Empty;
        }

        //public static void GeneraMenu(Menu MENU)
        //{
        //    try
        //    {

        //        DataTable DT = new DataTable();

        //        OracleConnection CON = new OracleConnection(Comun._STR_CON());
        //        OracleCommand CMD = new OracleCommand();
        //        CMD.Connection = CON;
        //        CMD.CommandText = Comun._PACKAGE() + "SP_MENU";
        //        CMD.CommandType = CommandType.StoredProcedure;

        //        CMD.Parameters.Add("PC_USUARIO", OracleType.VarChar, 50).Value = MENU.USER.USER;


        //        CMD.Parameters.Add("P_CURSOR", OracleType.Cursor).Direction = ParameterDirection.Output;

        //        OracleDataAdapter DA = new OracleDataAdapter(CMD);



        //        DA.Fill(DT);

        //        if (DT.Rows.Count > 0)
        //        {
        //            foreach (DataRow ITEM in DT.Rows)
        //            {

        //                MenuBody ROW = new MenuBody();

        //                ROW.ID = ITEM["ID"].ToString();
        //                ROW.DSC = ITEM["DSC"].ToString();
        //                ROW.PADREID = ITEM["PADREID"].ToString();
        //                ROW.ICONO = ITEM["ICONO"].ToString();
        //                ROW.URL = ITEM["URL"].ToString();
        //                ROW.NIVEL = ITEM["NIVEL"].ToString();


        //                MENU.MENU.Add(ROW);

        //            }

        //        }



        //    }
        //    catch (Exception EX)
        //    {

        //        MENU.ERROR_ID = 1;
        //        MENU.ERROR_DSC = "Error al generar menu";
        //        MENU.ERROR_EX = "BaseLogin GeneraMenu: " + EX.Message;

        //    }


        //}

        public static bool ValidaCaptcha(string token)
        {
            if (Comun._TESTING())
            {
                return true;
            }

            return true;

            //var client = new RestClient("https://www.google.com/recaptcha/api/siteverify?secret=6LdNfqwZAAAAAMqskSRbeiS3maaulxctCtmJZx1W&response=" + token);
            //client.Timeout = -1;
            //var request = new RestRequest(Method.POST);
            //IRestResponse response = client.Execute(request);
            //Console.WriteLine(response.Content);

            //dynamic obj = JsonConvert.DeserializeObject<dynamic>(response.Content);


            //return obj.success;

        }

        public static DTORespuesta ConsultaEstaciones(string usuario)
        {
            DTORespuesta respuesta = new DTORespuesta();
            try
            {
                List<Dictionary<string, object>> estaciones = new List<Dictionary<string, object>>();
                DataTable DT = new DataTable();

                OracleConnection CON = new OracleConnection(Comun._STR_CON());
                OracleCommand CMD = new OracleCommand();
                CMD.Connection = CON;
                CMD.CommandText = Comun._PACKAGE() + "SP_LISTA_ESTACIONES";
                CMD.CommandType = CommandType.StoredProcedure;

                CMD.Parameters.Add("VV_PLANTA", OracleType.VarChar, 50).Value = "TOT";
                CMD.Parameters.Add("VN_ESTACION", OracleType.Number).Value = 0;
                CMD.Parameters.Add("VV_USUARIO", OracleType.VarChar, 50).Value = usuario;
                CMD.Parameters.Add("IO_CURSOR", OracleType.Cursor).Direction = ParameterDirection.Output;

                OracleDataAdapter DA = new OracleDataAdapter(CMD);

                DA.Fill(DT);

                if (DT.Rows.Count > 0)
                {
                    foreach (DataRow elem in DT.Rows)
                    {
                        estaciones.Add(new Dictionary<string, object>() {
                            { "PLANTA_ID", elem["planta_Id"].ToString() },
                            { "PLANTA_DSC", elem["planta_Dsc"].ToString() },
                            { "ESTACION_ID", elem["estacion_Id"].ToString() },
                            { "ESTACION_DSC", elem["estacion_Dsc"].ToString() },
                            { "ESTACION_FECHA", elem["estacion_Fecha"].ToString() },
                            { "ESTACION_ESTADO", elem["estacion_Estado"].ToString() },
                            { "TRS", elem["TRS"].ToString() },
                            { "DIRV", elem["DIRV"].ToString() },
                            { "COMUNIDAD", elem["comunidad"].ToString() }
                        });
                    }
                }

                respuesta.Elemento = estaciones;
            }
            catch (Exception ex)
            {
                respuesta.IsError = true;
                respuesta.Resultado = false;
                respuesta.Mensaje = String.Format("Error al al consultar los datos: {0}", ex.Message);
            }

            return respuesta;
        }

        public static DTORespuesta ConsultaGraficoEstacion(string planta, int estacion, string fecha_inicio, int rango, string usuario)
        {
            DTORespuesta respuesta = new DTORespuesta();
            try
            {
                List<Dictionary<string, object>> estaciones = new List<Dictionary<string, object>>();
                DataTable DT = new DataTable();

                OracleConnection CON = new OracleConnection(Comun._STR_CON());
                OracleCommand CMD = new OracleCommand();
                CMD.Connection = CON;
                CMD.CommandText = Comun._PACKAGE() + "SP_GRAF_COMP_VARIABLES";
                CMD.CommandType = CommandType.StoredProcedure;

                CMD.Parameters.Add("VV_PLANTA", OracleType.VarChar, 50).Value = planta;
                CMD.Parameters.Add("VN_VARIABLE", OracleType.Number).Value = 0;
                CMD.Parameters.Add("VN_ESTACION", OracleType.Number).Value = estacion;
                CMD.Parameters.Add("VV_FechaInicio", OracleType.VarChar, 50).Value = fecha_inicio;
                CMD.Parameters.Add("VN_RANGO", OracleType.Number).Value = rango;
                CMD.Parameters.Add("VV_USUARIO", OracleType.VarChar, 50).Value = usuario;
                CMD.Parameters.Add("IO_CURSOR", OracleType.Cursor).Direction = ParameterDirection.Output;

                OracleDataAdapter DA = new OracleDataAdapter(CMD);

                DA.Fill(DT);

                if (DT.Rows.Count > 0)
                {
                    foreach (DataRow elem in DT.Rows)
                    {
                        estaciones.Add(new Dictionary<string, object>() {
                            { "VARIABLE", elem["variable_dsc"].ToString() },
                            { "FECHA", elem["FECHA"].ToString() },
                            { "VALOR", elem["VALOR"].ToString() },
                            { "ESTADO", elem["ESTADO"].ToString() }
                        });
                    }
                }

                respuesta.Elemento = estaciones;
            }
            catch (Exception ex)
            {
                respuesta.IsError = true;
                respuesta.Resultado = false;
                respuesta.Mensaje = String.Format("Error al al consultar los datos: {0}", ex.Message);
            }

            return respuesta;
        }

        public static DTORespuesta ConsultaTablaEstacion(string planta, int estacion, string fecha_inicio, int rango, string usuario)
        {
            DTORespuesta respuesta = new DTORespuesta();
            try
            {
                List<Dictionary<string, object>> estaciones = new List<Dictionary<string, object>>();
                DataTable DT = new DataTable();

                OracleConnection CON = new OracleConnection(Comun._STR_CON());
                OracleCommand CMD = new OracleCommand();
                CMD.Connection = CON;
                CMD.CommandText = Comun._PACKAGE() + "SP_TABLA_COMP_VARIABLES";
                CMD.CommandType = CommandType.StoredProcedure;

                CMD.Parameters.Add("VV_PLANTA", OracleType.VarChar, 50).Value = planta;
                CMD.Parameters.Add("VN_VARIABLE", OracleType.Number).Value = 0;
                CMD.Parameters.Add("VN_ESTACION", OracleType.Number).Value = estacion;
                CMD.Parameters.Add("VV_FechaInicio", OracleType.VarChar, 50).Value = fecha_inicio;
                CMD.Parameters.Add("VN_RANGO", OracleType.Number).Value = rango;
                CMD.Parameters.Add("VV_USUARIO", OracleType.VarChar, 50).Value = usuario;
                CMD.Parameters.Add("IO_CURSOR", OracleType.Cursor).Direction = ParameterDirection.Output;

                OracleDataAdapter DA = new OracleDataAdapter(CMD);

                DA.Fill(DT);

                if (DT.Rows.Count > 0)
                {
                    foreach (DataRow elem in DT.Rows)
                    {
                        estaciones.Add(new Dictionary<string, object>() {
                            { "PLANTA_ID", elem["planta_Id"].ToString() },
                            { "VARIABLE_ID", elem["variable_id"].ToString() },
                            { "VARIABLE_DSC", elem["variable_dsc"].ToString() },
                            { "ESTACION_DSC", elem["estacion_Dsc"].ToString() },
                            { "ESTACION_FECHA", elem["estacion_Fecha"].ToString() },
                            { "CLASE", elem["clase"].ToString() },
                            { "UNIDAD", elem["unidad"].ToString() },
                            { "VALOR", elem["VALOR"].ToString() }
                        });
                    }
                }

                respuesta.Elemento = estaciones;
            }
            catch (Exception ex)
            {
                respuesta.IsError = true;
                respuesta.Resultado = false;
                respuesta.Mensaje = String.Format("Error al al consultar los datos: {0}", ex.Message);
            }

            return respuesta;
        }

        public static DTORespuesta ConsultaDetalleVariable(string planta, int variable, int estacion, string fecha_inicio, int rango, string usuario)
        {
            DTORespuesta respuesta = new DTORespuesta();
            try
            {
                List<Dictionary<string, object>> estaciones = new List<Dictionary<string, object>>();
                DataTable DT = new DataTable();

                OracleConnection CON = new OracleConnection(Comun._STR_CON());
                OracleCommand CMD = new OracleCommand();
                CMD.Connection = CON;
                CMD.CommandText = Comun._PACKAGE() + "SP_TABLA_COMP_VARIABLES_DET";
                CMD.CommandType = CommandType.StoredProcedure;

                CMD.Parameters.Add("VV_PLANTA", OracleType.VarChar, 50).Value = planta;
                CMD.Parameters.Add("VN_VARIABLE", OracleType.Number).Value = variable;
                CMD.Parameters.Add("VN_ESTACION", OracleType.Number).Value = estacion;
                CMD.Parameters.Add("VV_FechaInicio", OracleType.VarChar, 50).Value = fecha_inicio;
                CMD.Parameters.Add("VN_RANGO", OracleType.Number).Value = rango;
                CMD.Parameters.Add("VV_USUARIO", OracleType.VarChar, 50).Value = usuario;
                CMD.Parameters.Add("IO_CURSOR", OracleType.Cursor).Direction = ParameterDirection.Output;

                OracleDataAdapter DA = new OracleDataAdapter(CMD);

                DA.Fill(DT);

                if (DT.Rows.Count > 0)
                {
                    foreach (DataRow elem in DT.Rows)
                    {
                        estaciones.Add(new Dictionary<string, object>() {
                            { "FECHA", elem["FECHA"].ToString() },
                            { "VALOR", elem["VALOR"].ToString() },
                            { "ESTADO", elem["ESTADO"].ToString() }
                        });
                    }
                }

                respuesta.Elemento = estaciones;
            }
            catch (Exception ex)
            {
                respuesta.IsError = true;
                respuesta.Resultado = false;
                respuesta.Mensaje = String.Format("Error al al consultar los datos: {0}", ex.Message);
            }

            return respuesta;
        }
    }

}