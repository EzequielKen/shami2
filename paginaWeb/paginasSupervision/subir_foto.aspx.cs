using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasSupervision
{
    public partial class subir_foto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Verificar si hay un ID pasado a través de QueryString
                if (Request.QueryString["id"] != null)
                {
                    hiddenId.Value = Request.QueryString["id"];
                }
            }
        }
        protected void btnSubirFoto_Click(object sender, EventArgs e)
        {
            if (fileUploadFoto.HasFile)
            {
                try
                {
                    string id = hiddenId.Value;
                    string fileName = $"{id}{Path.GetExtension(fileUploadFoto.FileName)}";
                    string folderPath;
                    if ("1" == ConfigurationManager.AppSettings["desarrollo"])
                    {
                        folderPath = @ConfigurationManager.AppSettings["folderPath_desarrollo"];
                    }
                    else
                    {
                        folderPath = @ConfigurationManager.AppSettings["folderPath"];
                    }

                    // Crea la carpeta si no existe
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    // Guarda el archivo en el servidor
                    string filePath = Path.Combine(folderPath, fileName);
                    fileUploadFoto.SaveAs(filePath);

                    lblMensaje.ForeColor = System.Drawing.Color.Green;
                    lblMensaje.Text = "Foto subida exitosamente!";

                    // Redirigir de vuelta a la página principal después de la subida
                    string returnUrl = Request.QueryString["returnUrl"];
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        Response.Redirect(returnUrl);
                    }
                }
                catch (Exception ex)
                {
                    lblMensaje.ForeColor = System.Drawing.Color.Red;
                    lblMensaje.Text = $"Error al subir la foto: {ex.Message}";
                }
            }
            else
            {
                lblMensaje.ForeColor = System.Drawing.Color.Red;
                lblMensaje.Text = "Por favor selecciona una foto.";
            }
        }
    }
}