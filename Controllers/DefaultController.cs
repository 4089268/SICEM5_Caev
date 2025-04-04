using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Aspose.Pdf.Devices;
using Aspose.Pdf;
using Microsoft.AspNetCore.Mvc;
using Sicem_Blazor.Services;
using System.Data.SqlClient;
using Sicem_Blazor.Models;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Sicem_Blazor.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class DefaultController : Controller {

        private readonly SicemService sicemService;
        private readonly IConfiguration configuration;
        private readonly ILogger<DefaultController> logger;

        public DefaultController(IConfiguration c, SicemService s, ILogger<DefaultController> logger) {
            this.configuration = c;
            this.sicemService = s;
            this.logger = logger;
        }


        [HttpGet]
        [Route("/api/Documento/{idOficina}/{idImagen}")]
        public IActionResult ConsultaGral_Imagen(int idOficina, int idImagen, [FromQuery] int W, [FromQuery] int H) {
            var sicemDocument = new ConsultaGral_Documentos();
            var TipoDatos = "";

            /****** Obtener Enlace de la base de datos ******/
            var xEnlace = sicemService.ObtenerEnlaces(idOficina).First();


            /****** Obtener los datos de la Base de Datos ******/
            try {
                string xQuery1 = String.Format("Select top 1 id_imagen, imagen, archivo, file_extension, documento From [{1}Media].[Global].[Opr_Imagenes] " +
                    "Where id_imagen = '{0}'", idImagen, xEnlace.BaseDatos);

                using(var xConnecton = new SqlConnection(xEnlace.StringConection)) {
                    xConnecton.Open();
                    var xCommand = new SqlCommand();
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = xQuery1;

                    using(SqlDataReader xReader = xCommand.ExecuteReader()) {

                        if(xReader.Read()) {

                            sicemDocument.Archivo = xReader["archivo"].ToString();
                            sicemDocument.Extencion = xReader["file_extension"].ToString();

                            if(sicemDocument.Extencion.ToLower() == ".pdf") {
                                try {
                                    sicemDocument.Documento = (byte[])xReader.GetValue("documento");
                                } catch(Exception) {
                                    sicemDocument.Documento = null;
                                }
                            }
                            else {
                                try {
                                    sicemDocument.Documento = (byte[])xReader.GetValue("imagen");
                                } catch(Exception) {
                                    sicemDocument.Documento = null;
                                }
                            }

                            /****** Comprobar la extencion del documento para regresarlo correctamente ******/
                            var tmpExtencion = sicemDocument.Extencion.Split(".").Last<string>().ToLower();
                            switch(tmpExtencion) {
                                case "pdf":
                                    TipoDatos = "application/pdf";
                                    break;
                                case "jpg":
                                    TipoDatos = "image/jpg";
                                    break;
                                case "jpeg":
                                    TipoDatos = "image/jpeg";
                                    break;
                                case "png":
                                    TipoDatos = "image/png";
                                    break;
                                case "gif":
                                    TipoDatos = "image/gif";
                                    break;
                                default:
                                    TipoDatos = "image/jpg";
                                    break;
                            }

                        }
                    }
                }

            } catch(Exception e) {
                return BadRequest(e.Message + Environment.NewLine + e.StackTrace);
            }


            /****** Comprobar si hay parametros para reducir la imagen ******/
            if(W > 0 && H > 0) {


                // ******* Convertir el pdf a imagen y redimencionar
                if(TipoDatos == "application/pdf") {

                    using(MemoryStream pdfStream = new MemoryStream(sicemDocument.Documento)) {
                        Document pdfDocument = new Document(pdfStream);
                        using(MemoryStream imageStream = new MemoryStream()) {
                            Resolution resolution = new Resolution(300);
                            JpegDevice JpegDevice = new JpegDevice(W, H, resolution);
                            JpegDevice.Process(pdfDocument.Pages[1], imageStream);
                            sicemDocument.Documento = imageStream.ToArray();
                            TipoDatos = "image/jpeg";
                        }
                    }

                }
                // ******* Redimensionar la imagen
                else {
                    SixLabors.ImageSharp.Image originalImage = SixLabors.ImageSharp.Image.Load(sicemDocument.Documento);
                    originalImage.Mutate(x => x.Resize(W, H));
                    using(MemoryStream m = new MemoryStream()) {
                        originalImage.SaveAsJpeg(m);
                        sicemDocument.Documento = m.ToArray();
                        TipoDatos = "image/jpeg";
                    }
                }

            }

            /****** Regresar la imagen ******/
            Console.WriteLine($">> Tipo: {TipoDatos}");

            return File(sicemDocument.Documento, TipoDatos);

        }

        [HttpGet]
        [Route("/api/ListaImagenes/{idOficina}/{idCuenta}")]
        public IActionResult ObtenerListaImagenes(int idOficina, int idCuenta) {
            
            /****** Obtener Enlace de la base de datos ******/
            var xEnlace = sicemService.ObtenerEnlaces(idOficina).First();

            var imagenes = new List<int>();
            /****** Obtener los datos de la Base de Datos ******/
            try {
                string xQuery1 = String.Format(" Select i.id_imagen " +
                    "FROM [{1}Media].[Global].[Opr_Imagenes] i "+ 
                    "Inner Join [{1}].Padron.Cat_Padron p on i.id_tabla = p.id_padron " +
                    "Where p.id_cuenta = {0}", idCuenta, xEnlace.BaseDatos);

                using(var xConnecton = new SqlConnection(xEnlace.StringConection)) {
                    xConnecton.Open();
                    var xCommand = new SqlCommand();
                    xCommand.Connection = xConnecton;
                    xCommand.CommandText = xQuery1;
                    using(SqlDataReader xReader = xCommand.ExecuteReader()) {
                        while(xReader.Read()) {
                            if(int.TryParse(xReader["id_imagen"].ToString(), out int tmpid)){
                                imagenes.Add(tmpid);
                            }
                        }
                    }
                }

            } catch(Exception e) {
                return BadRequest(e.Message + Environment.NewLine + e.StackTrace);
            }
            return Ok(imagenes.ToArray());
        }



        [HttpGet]
        [Route("/api/Download/{guid}")]
        public IActionResult DescargarArchivo(string guid)
        {
            this.logger.LogInformation("Descargando archivo con GUID: " + guid);
            var _tmpFolder = configuration.GetValue<string>("TempFolder");
            var _dirInfo = new DirectoryInfo($"{_tmpFolder}{guid}");

            if(!_dirInfo.Exists){
                return NoContent();
            }

            var _firstFile = _dirInfo.GetFiles().FirstOrDefault();
            if(_firstFile == null){
                return NoContent();
            }

            var _nameFiles = _dirInfo.GetFiles().Select<FileInfo,string>(item => item.Name).ToList();
            foreach(var n in _nameFiles){
                Console.WriteLine(n);
            }

            // .xls => application/vnd.ms-excel;
            // .xlsx => application/vnd.openxmlformats-officedocument.spreadsheetml.sheet


            var fileBytes = System.IO.File.ReadAllBytes(_firstFile.FullName);
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            
        }
    }
}
