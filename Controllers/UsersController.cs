using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using FloralHaven.Models;

namespace FloralHaven.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Users
        [HttpGet]
        [Route("User/Orders")]
        public ActionResult UserList()
        {
            var message = TempData["Message"] as string;
            if (!string.IsNullOrEmpty(message))
            {
                // Pass the success message to the view using ViewBag
                ViewBag.Message = message;
            }
            return View();
        }
        [HttpPost]
        [Route("User/GetUserList")]
        public JsonResult GetUserList()
        {
            string search = Request.Form.GetValues("search[value]")[0];
            int draw = Convert.ToInt32(Request.Form.GetValues("draw")[0]);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
            int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
            int page = (startRec / pageSize) + 1;
            var totalRecords = db.Users.Count();

            IQueryable<ApplicationUser> users = db.Users;

            if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
            {
                users = users.Where(u => u.UserName.Contains(search) || u.Email.Contains(search));
            }

            if (!string.IsNullOrEmpty(order) && !string.IsNullOrWhiteSpace(order) && !string.IsNullOrEmpty(orderDir) && !string.IsNullOrWhiteSpace(orderDir))
            {
                switch (order)
                {
                    case "1":
                        users = orderDir == "asc" ? users.OrderBy(u => u.UserName) : users.OrderByDescending(u => u.UserName);
                        break;
                    case "2":
                        users = orderDir == "asc" ? users.OrderBy(u => u.Email) : users.OrderByDescending(u => u.Email);
                        break;
                    default:
                        break;
                }
            }

            int recFilter = users.Count();

            var mappedUsers = users
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(user => new
                {
                    id = user.Id,
                    userName = user.UserName,
                    email = user.Email,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    avatar = user.Avatar
                })
                .ToList();

            return Json(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recFilter, data = mappedUsers }, JsonRequestBehavior.AllowGet);
        }
        // GET: Users/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "FirstName,LastName,Avatar,Email,UserName,PasswordHash")] ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,FirstName,LastName,Avatar,Email,UserName")] ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            ApplicationUser user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
