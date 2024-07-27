using FloralHaven.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FloralHaven.Controllers
{
    public class AdminController : Controller
    {
        FloralHavenDataContext db = new FloralHavenDataContext("Data Source=CongManhPC\\MSSQLSERVER01;Initial Catalog=FloralHaven;Integrated Security=True;TrustServerCertificate=True");

        // GET: Admin
        public ActionResult Index()
        {
            List<NHANVIEN> ds = db.NHANVIENs.ToList();
            return View(ds);
        }

        // GET: Admin/Details/5
        public ActionResult Details(string id)
        {
            NHANVIEN nv = db.NHANVIENs.SingleOrDefault(x => x.MANV == id);
            ViewBag.manv = nv.MANV;
            if (nv == null)
            {
                return null;
            }
            return View(nv);
        }

        // GET: Admin/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(NHANVIEN nv)
        {
            db.NHANVIENs.InsertOnSubmit(nv);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        // GET: Admin/Edit/5
        [HttpGet]
        public ActionResult Edit(string id)
        {
            NHANVIEN nv = db.NHANVIENs.SingleOrDefault(x => x.MANV == id);
            if (nv == null)
            {
                return null;
            }
            return View(nv);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(NHANVIEN nv, HttpPostedFileBase file)
        {
            var nvUpdate = db.NHANVIENs.SingleOrDefault(x => x.MANV == nv.MANV);
            //if (nvUpdate != null)
            //{
            //    if (ModelState.IsValid && file != null)
            //    {
            //        var fileName = Path.GetFileName(file.FileName);
            //        var path = Path.Combine(Server.MapPath("~/Imgs"), fileName);
            //        if (System.IO.File.Exists(path))
            //        {
            //            ViewBag.Thongbao = "Hình ảnh đã tồn tại";
            //        }
            //        else
            //        {
            //            file.SaveAs(path);
            //        }
            //        nvUpdate.IMG = fileName;
            //    }
            //}
            UpdateModel(nvUpdate);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
        // GET: Admin/Delete/5
        [HttpGet]
        public ActionResult Delete(string id)
        {
            NHANVIEN nv = db.NHANVIENs.SingleOrDefault(x => x.MANV == id);
            if (nv == null)
            {
                return null;
            }
            return View(nv);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(string id, FormCollection collection)
        {
            NHANVIEN nv = db.NHANVIENs.SingleOrDefault(x => x.MANV == id);
            if (nv == null)
            {
                return null;
            }
            db.NHANVIENs.DeleteOnSubmit(nv);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
    }
}
