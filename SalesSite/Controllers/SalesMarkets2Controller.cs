using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SalesSite.Models;

namespace SalesSite.Controllers
{
    public class SalesMarkets2Controller : Controller
    {
        private NoitionEntities2 db = new NoitionEntities2();

        // GET: SalesMarkets2
        public ActionResult Index()
        {
            return View(db.SalesMarket.OrderBy(s => s.일자).ToList());
        }

        // GET: SalesMarkets2/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesMarket salesMarket = db.SalesMarket.Find(id);
            if (salesMarket == null)
            {
                return HttpNotFound();
            }
            return View(salesMarket);
        }

        // GET: SalesMarkets2/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SalesMarkets2/Create
        // 초과 게시 공격으로부터 보호하려면 바인딩하려는 특정 속성을 사용하도록 설정하십시오. 
        // 자세한 내용은 https://go.microsoft.com/fwlink/?LinkId=317598을(를) 참조하십시오.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Market,RegistrationDate,일자,송장번호,수취인명,제품명,수량,공급가,택배비,주문구분,주문번호,상품코드")] SalesMarket salesMarket)
        {
            if (ModelState.IsValid)
            {
                db.SalesMarket.Add(salesMarket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(salesMarket);
        }

        // GET: SalesMarkets2/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesMarket salesMarket = db.SalesMarket.Find(id);
            if (salesMarket == null)
            {
                return HttpNotFound();
            }
            return View(salesMarket);
        }

        // POST: SalesMarkets2/Edit/5
        // 초과 게시 공격으로부터 보호하려면 바인딩하려는 특정 속성을 사용하도록 설정하십시오. 
        // 자세한 내용은 https://go.microsoft.com/fwlink/?LinkId=317598을(를) 참조하십시오.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Market,RegistrationDate,일자,송장번호,수취인명,제품명,수량,공급가,택배비,주문구분,주문번호,상품코드")] SalesMarket salesMarket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(salesMarket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(salesMarket);
        }

        // GET: SalesMarkets2/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesMarket salesMarket = db.SalesMarket.Find(id);
            if (salesMarket == null)
            {
                return HttpNotFound();
            }
            return View(salesMarket);
        }

        // POST: SalesMarkets2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SalesMarket salesMarket = db.SalesMarket.Find(id);
            db.SalesMarket.Remove(salesMarket);
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
