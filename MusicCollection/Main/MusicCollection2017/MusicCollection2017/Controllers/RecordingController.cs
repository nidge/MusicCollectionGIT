using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MusicCollection2017.DAL;
using MusicCollection2017.Models;
using PagedList;
using System.Data.Entity.Infrastructure;
using System.Security.Cryptography;
using MusicCollection2017.Logging;

namespace MusicCollection2017.Controllers
{
    public class RecordingController : Controller
    {
        private MusicContext db = new MusicContext();

        // GET: Recording
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.RecordingSortParm = String.IsNullOrEmpty(sortOrder) ? "recording_desc" : "";
            ViewBag.ArtistSortParm = sortOrder == "artist_asc" ? "artist_desc" : "artist_asc";
            ViewBag.InCloudSortParm = sortOrder == "inCloud_asc" ? "inCloud_desc" : "inCloud_asc";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var recordings = from s in db.Recordings
                             select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                recordings = recordings.Where(s => s.Title.Contains(searchString) ||
                                                   s.Artist.Title.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "recording_asc":
                    recordings = recordings.OrderBy(s => s.Title);
                    break;
                case "recording_desc":
                    recordings = recordings.OrderByDescending(s => s.Title);
                    break;
                case "inCloud_asc":
                    recordings = recordings.OrderBy(s => s.InCloud);
                    break;
                case "inCloud_desc":
                    recordings = recordings.OrderByDescending(s => s.InCloud);
                    break;
                case "artist_asc":
                    recordings = recordings.OrderBy(s => s.Artist.Title);
                    break;
                case "artist_desc":
                    recordings = recordings.OrderByDescending(s => s.Artist.Title);
                    break;
                default:
                    recordings = recordings.OrderBy(s => s.Title);
                    break;
            }

            int itemsPerPage = 30; // NB: This is the number of recordings to show per page
            int pageNumber = (page ?? 1);

            // Generate a random recording 
            Recording r = GenerateRandomRecording();

            Console.Out.WriteLine(r.Title + " " + r.Artist.Title);

            return View(recordings.ToPagedList(pageNumber, itemsPerPage));
        }

        // GET: Recording/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recording recording = db.Recordings.Find(id);
            if (recording == null)
            {
                return HttpNotFound();
            }
            return View(recording);
        }

        // GET: Recording/Create
        public ActionResult Create()
        {
            PopulateArtistsDropDownList();
            PopulateGenresDropDownList();
            return View();
        }

        // POST: Recording/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,ArtistId,GenreId,InCloud,Rating")] Recording recording)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Recordings.Add(recording);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("",
                    "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            PopulateArtistsDropDownList(recording.ArtistId);
            PopulateGenresDropDownList(recording.GenreId);
            return View(recording);
        }

        // GET: Recording/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recording recording = db.Recordings.Find(id);
            if (recording == null)
            {
                return HttpNotFound();
            }

            PopulateArtistsDropDownList(recording.ArtistId);
            PopulateGenresDropDownList(recording.GenreId);

            return View(recording);
        }

        // POST: Recording/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Title,ArtistId,GenreId,InCloud,Rating")] Recording recording)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(recording).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(recording);
        //}

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var recordingsToUpdate = db.Recordings.Find(id);
            if (TryUpdateModel(recordingsToUpdate, "",
                new string[] { "Title", "ArtistId", "GenreId", "InCloud", "Rating" }))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("",
                        "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(recordingsToUpdate);
        }

        // GET: Recording/Delete/5
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage =
                    "Delete failed. Try again, and if the problem persists see your system administrator.";
            }

            Recording recording = db.Recordings.Find(id);
            if (recording == null)
            {
                return HttpNotFound();
            }
            return View(recording);
        }

        // POST: Recording/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Recording recording = db.Recordings.Find(id);
        //    db.Recordings.Remove(recording);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                Recording recording = db.Recordings.Find(id);
                db.Recordings.Remove(recording);
                db.SaveChanges();
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
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

        private void PopulateArtistsDropDownList(object selectedArtist = null)
        {
            var artistQuery = from d in db.Artists
                              orderby d.Title
                              select d;
            ViewBag.ArtistId = new SelectList(artistQuery, "Id", "Title", selectedArtist);
        }

        public void PopulateGenresDropDownList(object seelctedGenre = null)
        {
            var genreQuery = from g in db.Genres
                             orderby g.Title
                             select g;
            ViewBag.GenreId = new SelectList(genreQuery, "Id", "Title", seelctedGenre);
        }

        public Recording GenerateRandomRecording()
        {
            var recordings = db.Recordings.Select(s => s);
            var recording = recordings.Where(c => c.InCloud).OrderBy(c => Guid.NewGuid()).FirstOrDefault();
            return recording;

            // Linq
            // var recordings = from s in db.Recordings 
            //                  select s;

            // pick a random recording from all recordings
            // var r = recordings.OrderBy(c => Guid.NewGuid()).FirstOrDefault();
        }

        public IOrderedQueryable<Recording> FindRecordingsByArtist(int? artistId)
        {
            var recordings = db.Recordings.Select(s => s);
            IOrderedQueryable<Recording> recording = recordings.Where(c => c.ArtistId == (artistId)).OrderBy(c => c.Title);

            return recording;
        }
    }
}