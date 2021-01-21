﻿using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using SIA_MOVIL.Models;
using System.Net;
using SIA_MOVIL_MODELO.DTO;
using SIA_MOVIL_MODELO;

namespace SIA_MOVIL_API.Controllers
{
    public class LoginController : Controller
    {

        public ActionResult Login()
        {
            return View();
        }

        //[System.Web.Http.HttpPost]
        //public ActionResult IniciarSesion(string Data)
        //{
        //    Response.TrySkipIisCustomErrors = true;
        //    try
        //    {
        //        IRestResponse Req = Comun.ApiPOST(Data, Comun.URL + "Login/", "IniciarSesion");

        //        SIA_MOVIL_MODELO.VM_Usuario SESSION = JsonConvert.DeserializeObject<SIA_MOVIL_MODELO.VM_Usuario>(Req.Content);

        //        if (SESSION.ERROR_ID != 0)
        //        {
        //            return new JsonHttpStatusResult(new SIA_MOVIL_MODELO.Usuario { ERROR_ID = 1, ERROR_DSC = "Usuario no existe, valide datos" }, HttpStatusCode.BadRequest);
        //        }
        //        SESSION.USER_DATA.PASS = "";

        //        Session["UserSession"] = SESSION;
        //        Session["TokenSession"] = SESSION.TOKEN;

        //        Session.Timeout = 480;

        //        SESSION.TOKEN = "";


        //        return new JsonHttpStatusResult(SESSION, Req.StatusCode);
        //    }
        //    catch (Exception e)
        //    {
        //        return new JsonHttpStatusResult(new SIA_MOVIL_MODELO.Usuario { ERROR_ID = 1, ERROR_DSC = "Error solicitud" }, HttpStatusCode.BadRequest);
        //    }
        //}

        [HttpPost]
        public JsonResult SeteaTokenSession(Dictionary<string, object> data)
        {
            DTORespuesta respuesta = new DTORespuesta();
            try
            {
                MSession.RegisterSession(new DTOSessionUsuario() {
                    TokenJWT = data["TOKEN"].ToString()
                });

                Response.StatusCode = (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                respuesta.IsError = true;
                respuesta.Resultado = false;
                respuesta.Mensaje = ex.Message;
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Response.StatusDescription = ex.Message.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("\v", "").Replace("\f", "").ToString();
            }

            return new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = respuesta };
        }

        [HttpGet]
        public JsonResult CerrarSesion()
        {
            DTORespuesta respuesta = new DTORespuesta();
            try
            {
                MSession.FreeSession();
                Response.StatusCode = (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                respuesta.IsError = true;
                respuesta.Resultado = false;
                respuesta.Mensaje = ex.Message;
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Response.StatusDescription = ex.Message.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("\v", "").Replace("\f", "").ToString();
            }

            return new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = respuesta };
        }

        [HttpGet]
        public JsonResult GetClaveEncript()
        {
            DTORespuesta respuesta = new DTORespuesta();
            try
            {
                respuesta.Elemento = System.Web.Configuration.WebConfigurationManager.AppSettings["PrivateKey"];
                Response.StatusCode = (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                respuesta.IsError = true;
                respuesta.Resultado = false;
                respuesta.Mensaje = ex.Message;
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Response.StatusDescription = ex.Message.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("\v", "").Replace("\f", "").ToString();
            }

            return new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = respuesta };
        }

    }
}