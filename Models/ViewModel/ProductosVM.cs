using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventarioProductos.Models.ViewModel
{
    public class ProductosVM
    {
        public int IdPro { get; set; }
        public int IdSuc { get; set; }
        public string NombrePro { get; set; }
        public string CodBarra { get; set; }
        public double Precio { get; set; }
        public string NombreSuc { get; set; }
        public int Cantidad { get; set; }
        public int IdSucPro { get; set; }

    }
}