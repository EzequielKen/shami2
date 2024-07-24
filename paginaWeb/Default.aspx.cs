using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb
{
    public partial class Default : System.Web.UI.Page
    {
        [Serializable]
        public class MiClaseSerializable
        {
            public int Numero { get; set; }
            public string Texto { get; set; }
            // Otros miembros de la clase...
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}