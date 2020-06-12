using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EntityFrameworkPractice.Models;
using PagedList;

namespace EntityFrameworkPractice.Controllers
{
    public class FriendsController : Controller
    {
        private firstDBEntities db = new firstDBEntities();

        // GET: Friends
        public ActionResult Index(string sortOrder, string searchString1, string searchString2,
            string currentFilter1, string currentFilter2, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PlaceSortParm = sortOrder == "place" ? "place_desc" : "place";

            if (searchString1 != null)
            {
                page = 1;
            }
            else
            {
                searchString1 = currentFilter1;
            }

            ViewBag.CurrentFilter = searchString1;

            //if (searchString2 != null)
            //{
            //    page = 1;
            //}
            //else
            //{
            //    searchString2 = currentFilter2;
            //}

            //ViewBag.CurrentFilter = searchString2;

            var friends = from s in db.Friends select s;

            if (!String.IsNullOrEmpty(searchString1))
            {
                friends = friends.Where(s => s.FriendName.Contains(searchString1));
            }
            //if (!String.IsNullOrEmpty(searchString2))
            //{
            //    friends = friends.Where(s => s.Place.Contains(searchString2));
            //}

            switch (sortOrder)
            {
                case "name_desc":
                    friends = friends.OrderByDescending(s => s.FriendName);
                    break;
                case "place":
                    friends = friends.OrderBy(s => s.Place);
                    break;
                case "place_desc":
                    friends = friends.OrderByDescending(s => s.Place);
                    break;
                default:
                    friends = friends.OrderBy(s => s.FriendName);
                    break;
            }
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            return View(friends.ToPagedList(pageNumber, pageSize));
        }

        // GET: Friends/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Friend friend = db.Friends.Find(id);
            if (friend == null)
            {
                return HttpNotFound();
            }
            return View(friend);
        }

        // GET: Friends/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Friends/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FriendID,FriendName,Place")] Friend friend)
        {
            if (ModelState.IsValid)
            {
                db.Friends.Add(friend);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(friend);
        }

        // GET: Friends/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Friend friend = db.Friends.Find(id);
            if (friend == null)
            {
                return HttpNotFound();
            }
            return View(friend);
        }

        // POST: Friends/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FriendID,FriendName,Place")] Friend friend)
        {
            if (ModelState.IsValid)
            {
                db.Entry(friend).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(friend);
        }

        // GET: Friends/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Friend friend = db.Friends.Find(id);
            if (friend == null)
            {
                return HttpNotFound();
            }
            return View(friend);
        }

        // POST: Friends/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Friend friend = db.Friends.Find(id);
            db.Friends.Remove(friend);
            db.SaveChanges();
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
