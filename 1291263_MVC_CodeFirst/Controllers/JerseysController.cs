using _1291263_MVC_CodeFirst.Models;
using _1291263_MVC_CodeFirst.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace _1291263_MVC_CodeFirst.Controllers
{
    [Authorize]
    public class JerseysController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var jerseys = db.Jerseys.Include(j => j.Team).Include(j => j.JerseyStocks);
            return View(await jerseys.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "TeamName");

            var model = new JerseyInputModel();
            model.Stocks.Add(new JerseyStock()); 
            return PartialView("_Create", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create(JerseyInputModel model)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        string fileName = "";
                        if (model.Picture != null)
                        {
                            string extension = Path.GetExtension(model.Picture.FileName);
                            fileName = Guid.NewGuid().ToString() + extension;
                            string path = Path.Combine(Server.MapPath("~/Images"), fileName);
                            model.Picture.SaveAs(path);
                        }

                        string sqlJersey = "EXEC Jersey_Insert @Name, @ReleaseDate, @IsAvailable, @Picture, @TeamId";

                        var newJerseyId = await db.Database.SqlQuery<int>(sqlJersey,
                            new SqlParameter("@Name", model.Name),
                            new SqlParameter("@ReleaseDate", model.ReleaseDate),
                            new SqlParameter("@IsAvailable", model.IsAvailable),
                            new SqlParameter("@Picture", (object)fileName ?? DBNull.Value),
                            new SqlParameter("@TeamId", model.TeamId)
                        ).FirstOrDefaultAsync();

                        if (model.Stocks != null && model.Stocks.Any())
                        {
                            foreach (var stock in model.Stocks)
                            {
                                string sqlStock = "EXEC JerseyStock_Insert @Size, @Price, @Quantity, @JerseyId";
                                await db.Database.ExecuteSqlCommandAsync(sqlStock,
                                    new SqlParameter("@Size", (int)stock.Size), 
                                    new SqlParameter("@Price", stock.Price),
                                    new SqlParameter("@Quantity", stock.Quantity),
                                    new SqlParameter("@JerseyId", newJerseyId)
                                );
                            }
                        }

                        transaction.Commit();
                        return Json(new { success = true, message = "Jersey Saved Successfully!" });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return Json(new { success = false, message = "Error: " + ex.Message });
                    }
                }
            }
            return Json(new { success = false, message = "Please check your inputs." });
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var jersey = await db.Jerseys.Include(j => j.JerseyStocks).FirstOrDefaultAsync(j => j.JerseyId == id);
            if (jersey == null) return HttpNotFound();

            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "TeamName", jersey.TeamId);

            var model = new JerseyEditModel
            {
                JerseyId = jersey.JerseyId,
                Name = jersey.Name,
                ReleaseDate = jersey.ReleaseDate,
                IsAvailable = jersey.IsAvailable,
                TeamId = jersey.TeamId,
                ExistingPicture = jersey.Picture,
                Stocks = jersey.JerseyStocks.ToList()
            };

            return PartialView("_Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(JerseyEditModel model)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        string fileName = model.ExistingPicture;
                        if (model.Picture != null)
                        {
                            string extension = Path.GetExtension(model.Picture.FileName);
                            fileName = Guid.NewGuid().ToString() + extension;
                            string path = Path.Combine(Server.MapPath("~/Images"), fileName);
                            model.Picture.SaveAs(path);
                        }

                        string sqlUpdate = "EXEC Jersey_Update @JerseyId, @Name, @ReleaseDate, @IsAvailable, @Picture, @TeamId";
                        await db.Database.ExecuteSqlCommandAsync(sqlUpdate,
                            new SqlParameter("@JerseyId", model.JerseyId),
                            new SqlParameter("@Name", model.Name),
                            new SqlParameter("@ReleaseDate", model.ReleaseDate),
                            new SqlParameter("@IsAvailable", model.IsAvailable),
                            new SqlParameter("@Picture", (object)fileName ?? DBNull.Value),
                            new SqlParameter("@TeamId", model.TeamId)
                        );

                        
                        string sqlDeleteStocks = "DELETE FROM JerseyStocks WHERE JerseyId = @JerseyId";
                        await db.Database.ExecuteSqlCommandAsync(sqlDeleteStocks,
                            new SqlParameter("@JerseyId", model.JerseyId));

                        
                        if (model.Stocks != null)
                        {
                            foreach (var stock in model.Stocks)
                            {
                                string sqlStock = "EXEC JerseyStock_Insert @Size, @Price, @Quantity, @JerseyId";
                                await db.Database.ExecuteSqlCommandAsync(sqlStock,
                                    new SqlParameter("@Size", (int)stock.Size),
                                    new SqlParameter("@Price", stock.Price),
                                    new SqlParameter("@Quantity", stock.Quantity),
                                    new SqlParameter("@JerseyId", model.JerseyId)
                                );
                            }
                        }

                        transaction.Commit();
                        return Json(new { success = true, message = "Updated Successfully!" });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return Json(new { success = false, message = "Error: " + ex.Message });
                    }
                }
            }
            return Json(new { success = false, message = "Validation Failed" });
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var jersey = await db.Jerseys.Include(j => j.Team).FirstOrDefaultAsync(j => j.JerseyId == id);
            if (jersey == null) return HttpNotFound();
            return PartialView("_Delete", jersey);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                string sqlDelete = "EXEC Jersey_Delete @JerseyId";
                await db.Database.ExecuteSqlCommandAsync(sqlDelete, new SqlParameter("@JerseyId", id));

                return Json(new { success = true, message = "Deleted Successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
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