using FloralHaven.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FloralHaven.Controllers
{
    public class ProductController : Controller
    {
        // GET: Admin
        FloralHavenDataContext db = new FloralHavenDataContext("Data Source=CongManhPC\\MSSQLSERVER01;Initial Catalog=FloralHaven;Integrated Security=True;TrustServerCertificate=True");
        public ActionResult Index()
        {
            List<HOA> ds = db.HOAs.ToList();
            return View(ds);
        }

        // GET: Admin/Details/5
        public ActionResult Details(string id)
        {
            HOA hoa = db.HOAs.SingleOrDefault(x => x.MAHOA == id);
            ViewBag.mahoa = hoa.MAHOA;
            if (hoa == null)
            {
                return null;
            }
            return View(hoa);
        }

        // GET: Admin/Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.maloai = new SelectList(db.LOAIHOAs.ToList(), "MALOAI", "TENLOAI");
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(HOA hoa, HttpPostedFileBase file)
        {
            ViewBag.maloai = new SelectList(db.LOAIHOAs.ToList(), "MALOAI", "TENLOAI");
            if (file == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            if (ModelState.IsValid)
            {
                var fileName=Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Imgs"), fileName);
                if (System.IO.File.Exists(path))
                {
                    ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                }
                else
                {
                    file.SaveAs(path);
                }
                hoa.IMG = fileName;
                db.HOAs.InsertOnSubmit(hoa);
                db.SubmitChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: Admin/Edit/5
        [HttpGet]
        public ActionResult Edit(string id)
        {
            HOA hoa = db.HOAs.SingleOrDefault(x => x.MAHOA == id);
            ViewBag.maloai = new SelectList(db.LOAIHOAs.ToList(), "MALOAI", "TENLOAI");

            if (hoa == null)
            {
                return null;
            }
            return View(hoa);

        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(HOA hoa, HttpPostedFileBase file)
        {
            ViewBag.maloai = new SelectList(db.LOAIHOAs.ToList(), "MALOAI", "TENLOAI");
           
            if (ModelState.IsValid)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Imgs"), fileName);
                if (System.IO.File.Exists(path))
                {
                    ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                }
                else
                {
                    file.SaveAs(path);
                }
                hoa.IMG = fileName;
                UpdateModel(hoa);
                db.SubmitChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
