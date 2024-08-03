using FloralHaven.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FloralHaven.Models;
using System.Globalization;
using System.Text;
namespace FloralHaven.Controllers
{

    public class AdminController : Controller
    {
        private FloralHavenDataContext _db = FloralHavenDBContextConfig.GetFloralHavenDataContext();
        string _imgPrefix = "https://congmanh270504.github.io/Db-FloralHaven/";
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            ViewBag.Categories = _db.CATEGORies.ToList();
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(PRODUCT prd, FormCollection collection)
        {
            ViewBag.Categories = _db.CATEGORies.ToList();
            if (ModelState.IsValid)
            {
                _db.PRODUCTs.InsertOnSubmit(prd);
                _db.SubmitChanges();
                return RedirectToAction("Index", "Product", null);
            }
            else
            {
                return View(prd);
            }
        }
        public ActionResult CreateNewCategoryItem()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateNewCategoryItem(CATEGORY category)
        {
            if (category.name == null)
            {
                ViewBag.Error = "Category name is required";
                return View(category);
            }
            category.id = RemoveDiacritics(category.name).ToLower().Replace(" ", "-");
            if (ModelState.IsValid)
            {
                _db.CATEGORies.InsertOnSubmit(category);
                _db.SubmitChanges();
                return RedirectToAction("Index", "Product", null);
            }
            else
            {
                return View(category);
            }
        }
        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        [Route("Product/{handle}")]
        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            PRODUCT item = _db.PRODUCTs.FirstOrDefault(t => t.id == id);
            if (item == null)
            {
                return HttpNotFound();

            }
            string _imgPath = _imgPrefix + item.handle + "/";
            var productImages = _db.IMAGEs.Where(image => image.productid == item.id).Select(image => _imgPath + image.path).ToList();

            string CategoryName = (_db.CATEGORies.FirstOrDefault(category => category.id == item.categoryid)?.name) ?? "";
            ProductViewModel productViewModel = new ProductViewModel(item.id, item.title, item.handle, item.instock, item.price, item.saleprice, productImages, item.categoryid, CategoryName, item.description, item.sku);
            return View(productViewModel);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(int id, FormCollection collection)
        {
            PRODUCT item = _db.PRODUCTs.FirstOrDefault(t => t.id == id);
            if (item == null)
            {
                return null;
            }
            _db.PRODUCTs.DeleteOnSubmit(item);
            _db.SubmitChanges();
            return RedirectToAction("Index", "Product", null);

        }

        public static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }
            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
