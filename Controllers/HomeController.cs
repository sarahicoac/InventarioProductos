using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InventarioProductos.Models;
using InventarioProductos.Models.ViewModel;

namespace InventarioProductos.Controllers
{
    public class HomeController : Controller {


        public string draw = "";
        public string start = "";
        public string length = "";
        public string sortColumn = "";
        public string sortColumnDir = "";
        public string searchValue = "";
        public int pageSize, skip, recordsTotal;
        public int idG=0;
        public ActionResult Index()
        {
            List<SucursalVM> listaS = new List<SucursalVM>();
            using (InventarioEntities11 db = new InventarioEntities11())
            {
                listaS = (from s in db.TSucursal
                          select new SucursalVM
                          {
                              IdSuc = s.IdSuc,
                              NombreSuc = s.NombreSuc
                          }).ToList();
            }
            int c = listaS.Count();
            return View(listaS);
        }

        [HttpPost]
        public ActionResult Prod(int vIdS)
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

            List<ProductosVM> listaP = new List<ProductosVM>();
            using (InventarioEntities11 db = new InventarioEntities11())
            {
                listaP = (from sp in db.Suc_Pro
                          join p in db.TProductos on sp.IdPro equals p.IdPro
                          join s in db.TSucursal on sp.IdSuc equals s.IdSuc
                          where p.NombrePro.Contains(searchValue) && sp.IdSuc.Equals(vIdS)
                          select new ProductosVM
                          {
                              IdPro = p.IdPro,
                              IdSuc = s.IdSuc,
                              NombrePro = p.NombrePro,
                              CodBarra = p.CodBarra,
                              Precio = (double)p.Precio,
                              NombreSuc = s.NombreSuc,
                              Cantidad = (int)sp.Cantidad
                          }).ToList();
            }

            recordsTotal = listaP.Count();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = listaP });

        }

        public ActionResult ProdG()
        {
            
            List<ProductosVM> listaP = new List<ProductosVM>();
            using (InventarioEntities11 db = new InventarioEntities11())
            {
                listaP = (from p in db.TProductos 
                          select new ProductosVM
                          {
                              IdPro = p.IdPro,
                              NombrePro = p.NombrePro,
                              CodBarra = p.CodBarra,
                              Precio = (double)p.Precio
                          }).ToList();
            }

            
            return Json(new { data = listaP });

        }
     
        [HttpGet]
        public void Id(int id)
        {
            idG=id;
        }

        [HttpPost]
        public ActionResult InsertSuc(string nombreSuc)
        {
           
            try
            {
                
                using (InventarioEntities11 db=new InventarioEntities11())
                {
                    var SucT = new TSucursal();
                    SucT.NombreSuc = nombreSuc;
                    db.TSucursal.Add(SucT);
                    db.SaveChanges();
                    
                }
                return Redirect("Index/");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [HttpPost]
        public ActionResult InsertPro(string nombrePro, string codBarra, decimal precio)
        {

            try
            {

                using (InventarioEntities11 db = new InventarioEntities11())
                {
                    var proT = new TProductos();
                    proT.NombrePro = nombrePro;
                    proT.CodBarra = codBarra;
                    proT.Precio = precio;
                    db.TProductos.Add(proT);
                    db.SaveChanges();

                }
                return Redirect("Index/");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [HttpPost]
        public ActionResult EditSucPro(int cantidad, int hiddenSuc, int vIdP)
        {
            int cant = 0;
            int vIdS = hiddenSuc;
            try
            {
                List<ProductosVM> listaP = new List<ProductosVM>();

                using (InventarioEntities11 db = new InventarioEntities11())
                {
                    listaP = (from sp in db.Suc_Pro
                              join p in db.TProductos on sp.IdPro equals p.IdPro
                              join s in db.TSucursal on sp.IdSuc equals s.IdSuc
                              where p.IdPro.Equals(vIdP) && sp.IdSuc.Equals(vIdS)
                              select new ProductosVM
                              {
                                  IdSucPro = sp.IdSucPro,
                                  IdPro = p.IdPro,
                                  IdSuc = s.IdSuc,
                                  NombrePro = p.NombrePro,
                                  CodBarra = p.CodBarra,
                                  Precio = (double)p.Precio,
                                  NombreSuc = s.NombreSuc,
                                  Cantidad = (int)sp.Cantidad
                              }).ToList();
                }

                int c = listaP.Count();
                if (c > 0)
                {
                    int idSP=0;
                    foreach(var i in listaP)
                    {
                        idSP = i.IdSucPro;
                        cant = i.Cantidad;
                    }
                    cant=cant + cantidad;
                    using (InventarioEntities11 db2 = new InventarioEntities11())
                    {
                        var SucT = db2.Suc_Pro.Find(idSP);
                        SucT.Cantidad = cant;
                        db2.Entry(SucT).State =System.Data.Entity.EntityState.Modified;
                        db2.SaveChanges();

                    }

                }
                else {
                    using (InventarioEntities11 db3 = new InventarioEntities11())
                    {
                        var SucT = new Suc_Pro();
                        SucT.IdPro = vIdP;
                        SucT.IdPro = vIdS;
                        SucT.Cantidad = cantidad;
                        db3.Suc_Pro.Add(SucT);
                        db3.SaveChanges();

                    }
                }
                return Redirect("Index/");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}