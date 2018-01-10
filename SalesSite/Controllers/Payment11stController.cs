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
    public class Payment11stController : Controller
    {
        private NoitionEntities2 db = new NoitionEntities2();

        // GET: Payment11st
        public ActionResult Index()
        {
            return View(db.Payment11st.OrderBy(p => p.송금완료일).ToList());
        }

        // GET: Payment11st/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment11st payment11st = db.Payment11st.Find(id);
            if (payment11st == null)
            {
                return HttpNotFound();
            }
            return View(payment11st);
        }

        // GET: Payment11st/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Payment11st/Create
        // 초과 게시 공격으로부터 보호하려면 바인딩하려는 특정 속성을 사용하도록 설정하십시오. 
        // 자세한 내용은 https://go.microsoft.com/fwlink/?LinkId=317598을(를) 참조하십시오.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,RegistrationDate,주문번호,주문순번,배송번호,주문상태,구매자명,구매자ID,결제완료일,발송처리일,배송완료일,수취확인일,송금완료일,상품번호,상품명,옵션명,수량,정산금액,판매금액합계,추가정산금액합계,공제금액합계,판매가,옵션가,선결제배송비,도서산간배송비,구매자부담_반품_교환배송비,반품_교환추가금,반품선결제배송비,반품도서산간배송비,해외취소배송비,티켓예매수수료,티켓취소예매수수료,티켓취소위약금,여행취소위약금,송장번호,서비스이용료정책,기본서비스이용율,서비스이용료,할인쿠폰이용료,판매자기본할인,판매자추가할인,C11번가할인,복수구매할인비용,포인트이용료,칩이용료,무이자할부이용료,후불광고비,OK캐쉬백_적립부담액,지정택배이용료,전세계배송_판매자책임반품,수출대행수수료,물류이용수수료")] Payment11st payment11st)
        {
            if (ModelState.IsValid)
            {
                db.Payment11st.Add(payment11st);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(payment11st);
        }

        // GET: Payment11st/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment11st payment11st = db.Payment11st.Find(id);
            if (payment11st == null)
            {
                return HttpNotFound();
            }
            return View(payment11st);
        }

        // POST: Payment11st/Edit/5
        // 초과 게시 공격으로부터 보호하려면 바인딩하려는 특정 속성을 사용하도록 설정하십시오. 
        // 자세한 내용은 https://go.microsoft.com/fwlink/?LinkId=317598을(를) 참조하십시오.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,RegistrationDate,주문번호,주문순번,배송번호,주문상태,구매자명,구매자ID,결제완료일,발송처리일,배송완료일,수취확인일,송금완료일,상품번호,상품명,옵션명,수량,정산금액,판매금액합계,추가정산금액합계,공제금액합계,판매가,옵션가,선결제배송비,도서산간배송비,구매자부담_반품_교환배송비,반품_교환추가금,반품선결제배송비,반품도서산간배송비,해외취소배송비,티켓예매수수료,티켓취소예매수수료,티켓취소위약금,여행취소위약금,송장번호,서비스이용료정책,기본서비스이용율,서비스이용료,할인쿠폰이용료,판매자기본할인,판매자추가할인,C11번가할인,복수구매할인비용,포인트이용료,칩이용료,무이자할부이용료,후불광고비,OK캐쉬백_적립부담액,지정택배이용료,전세계배송_판매자책임반품,수출대행수수료,물류이용수수료")] Payment11st payment11st)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payment11st).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(payment11st);
        }

        // GET: Payment11st/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment11st payment11st = db.Payment11st.Find(id);
            if (payment11st == null)
            {
                return HttpNotFound();
            }
            return View(payment11st);
        }

        // POST: Payment11st/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Payment11st payment11st = db.Payment11st.Find(id);
            db.Payment11st.Remove(payment11st);
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
