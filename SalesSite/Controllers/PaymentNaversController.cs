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
    public class PaymentNaversController : Controller
    {
        private NoitionEntities2 db = new NoitionEntities2();

        // GET: PaymentNavers
        public ActionResult Index()
        {
            return View(db.PaymentNaver.OrderBy(p => p.결제금액_정산완료일).ToList());
        }

        // GET: PaymentNavers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentNaver paymentNaver = db.PaymentNaver.Find(id);
            if (paymentNaver == null)
            {
                return HttpNotFound();
            }
            return View(paymentNaver);
        }

        // GET: PaymentNavers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PaymentNavers/Create
        // 초과 게시 공격으로부터 보호하려면 바인딩하려는 특정 속성을 사용하도록 설정하십시오. 
        // 자세한 내용은 https://go.microsoft.com/fwlink/?LinkId=317598을(를) 참조하십시오.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,RegistrationDate,주문번호,상품주문번호,구분,상품명,구매자명,결제금액_정산예정일,결제금액_정산완료일,결제금액_정산기준일,정산구분,결제금액,결제수수료,주결제수단,주결제수단_금액,주결제수단_수수료,보조결제수단_금액,보조결제수단_수수료,매출_연동_수수료,채널수수료,무이자할부수수료,C_구_판매수수료,혜택금액,정산예정금액")] PaymentNaver paymentNaver)
        {
            if (ModelState.IsValid)
            {
                db.PaymentNaver.Add(paymentNaver);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(paymentNaver);
        }

        // GET: PaymentNavers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentNaver paymentNaver = db.PaymentNaver.Find(id);
            if (paymentNaver == null)
            {
                return HttpNotFound();
            }
            return View(paymentNaver);
        }

        // POST: PaymentNavers/Edit/5
        // 초과 게시 공격으로부터 보호하려면 바인딩하려는 특정 속성을 사용하도록 설정하십시오. 
        // 자세한 내용은 https://go.microsoft.com/fwlink/?LinkId=317598을(를) 참조하십시오.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,RegistrationDate,주문번호,상품주문번호,구분,상품명,구매자명,결제금액_정산예정일,결제금액_정산완료일,결제금액_정산기준일,정산구분,결제금액,결제수수료,주결제수단,주결제수단_금액,주결제수단_수수료,보조결제수단_금액,보조결제수단_수수료,매출_연동_수수료,채널수수료,무이자할부수수료,C_구_판매수수료,혜택금액,정산예정금액")] PaymentNaver paymentNaver)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paymentNaver).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(paymentNaver);
        }

        // GET: PaymentNavers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentNaver paymentNaver = db.PaymentNaver.Find(id);
            if (paymentNaver == null)
            {
                return HttpNotFound();
            }
            return View(paymentNaver);
        }

        // POST: PaymentNavers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PaymentNaver paymentNaver = db.PaymentNaver.Find(id);
            db.PaymentNaver.Remove(paymentNaver);
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
