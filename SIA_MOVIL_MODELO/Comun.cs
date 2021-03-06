﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIA_MOVIL_MODELO
{

    public class Config
    {
        public int _TIEMPO_EXPIRA_SESION = 120;
        public string _VERSION = string.Empty;
        public string _STR_CON = string.Empty;
        public string _SOURCE = string.Empty;
        public string _USUARIO = string.Empty;
        public string _PASS = string.Empty;
        public bool _TESTING = false;

        public string _KEY = string.Empty;
        public string _PACKAGE = string.Empty;
    }

    public class Comun
    {
        public static object VALIDA_NULO(object ENTRADA)
        {


            if (ENTRADA == null || ENTRADA.ToString() == "null" || ENTRADA.ToString() == string.Empty)
            {

                return DBNull.Value;

            }
            else
            {

                return ENTRADA.ToString().ToUpper();

            }



        }

        public static string ESCRIBE_CONFIG(string ENTRADA)
        {
            try
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"Configuracion\config.json", ENTRADA);
                return "OK";
            }
            catch (Exception e)
            {
                return e.Message;

            }


        }

        public static Config RETORNA_CONFIG()
        {
            try
            {
                string JSON = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"Configuracion\config.json");
                return JsonConvert.DeserializeObject<Config>(JSON);
            }
            catch (Exception e)
            {
                return new Config();

            }


        }

        public static int _TIEMPO_EXPIRA_SESION()
        {
            try
            {
                Config JSON = RETORNA_CONFIG();
                return JSON._TIEMPO_EXPIRA_SESION;
            }
            catch (Exception e)
            {
                return 120;

            }


        }

        public static string _VERSION()
        {
            try
            {
                Config JSON = RETORNA_CONFIG();
                return JSON._VERSION;
            }
            catch (Exception e)
            {
                return "";

            }

        }

        
        public static string _PACKAGE()
        {
            try
            {
                Config JSON = RETORNA_CONFIG();
                return JSON._PACKAGE;
            }
            catch (Exception e)
            {
                return "";

            }

        }

        public static string _STR_CON()
        {
            try
            {
                Config JSON = RETORNA_CONFIG();
                return JSON._STR_CON;
            }
            catch (Exception e)
            {
                return "";

            }

        }

        public static string _USUARIO()
        {
            try
            {
                Config JSON = RETORNA_CONFIG();
                return JSON._USUARIO;
            }
            catch (Exception e)
            {
                return "";

            }

        }

        public static string _PASS()
        {
            try
            {
                Config JSON = RETORNA_CONFIG();
                return JSON._PASS;
            }
            catch (Exception e)
            {
                return "";

            }

        }

        public static bool _TESTING()
        {
            try
            {
                Config JSON = RETORNA_CONFIG();
                return JSON._TESTING;
            }
            catch (Exception e)
            {
                return false;

            }

        }

        public static string _SOURCE()
        {
            try
            {
                Config JSON = RETORNA_CONFIG();
                return JSON._SOURCE;
            }
            catch (Exception e)
            {
                return "";

            }

        }


        public static string _STR_CON_LOGIN(string user, string pass)
        {
            try
            {
                Config JSON = RETORNA_CONFIG();
                String source = JSON._SOURCE;
                return "Data Source="+ source + ";User Id="+ user + ";Password=" + pass + ";Pooling=no;Pooling=false;Connection Lifetime=60;";  
            }
            catch (Exception e)
            {
                return "";

            }

        }

    }

}
