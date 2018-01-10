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
    public class PaymentAuctionGmarketsController : Controller
    {
        private NoitionEntities2 db = new NoitionEntities2();

        // GET: PaymentAuctionGmarkets
        public ActionResult Index()
        {
            return View(db.PaymentAuctionGmarket.Where(p => p.정산상태.Contains("정산완료")).OrderBy(p => p.정산완료일).ToList());
        }

        // GET: PaymentAuctionGmarkets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentAuctionGmarket paymentAuctionGmarket = db.PaymentAuctionGmarket.Find(id);
            if (paymentAuctionGmarket == null)
            {
                return HttpNotFound();
            }
            return View(paymentAuctionGmarket);
        }

        // GET: PaymentAuctionGmarkets/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PaymentAuctionGmarkets/Create
        // 초과 게시 공격으로부터 보호하려면 바인딩하려는 특정 속성을 사용하도록 설정하십시오. 
        // 자세한 내용은 https://go.microsoft.com/fwlink/?LinkId=317598을(를) 참조하십시오.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,RegistrationDate,아이디,구매결정일자,주문번호,상품번호,정산상태,판매금액,판매단가,구매자명,구매자ID,상품명,수량,주문옵션,추가구성,사은품,수령인명,수령인_휴대폰,수령인_전화번호,배송번호,배송비_금액,발송일자,배송완료일자,택배사명_발송방법_,송장번호,구매자_휴대폰,구매자_전화번호,장바구니번호_결제번호_,주문일자_결제확인전_,판매자_관리코드,판매자_상세관리코드,서비스이용료,정산예정금액,주문확인일자,판매자쿠폰할인,스마일포인트적립,일시불할인,C_옥션_복수구매할인,C_옥션_우수회원할인,결제완료일,정산완료일,배송구분,주문종류,SKU번호_및_수량,글로벌샵구분,해외배송여부,제휴사명")] PaymentAuctionGmarket paymentAuctionGmarket)
        {
            if (ModelState.IsValid)
            {
                db.PaymentAuctionGmarket.Add(paymentAuctionGmarket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(paymentAuctionGmarket);
        }

        // GET: PaymentAuctionGmarkets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentAuctionGmarket paymentAuctionGmarket = db.PaymentAuctionGmarket.Find(id);
            if (paymentAuctionGmarket == null)
            {
                return HttpNotFound();
            }
            return View(paymentAuctionGmarket);
        }

        // POST: PaymentAuctionGmarkets/Edit/5
        // 초과 게시 공격으로부터 보호하려면 바인딩하려는 특정 속성을 사용하도록 설정하십시오. 
        // 자세한 내용은 https://go.microsoft.com/fwlink/?LinkId=317598을(를) 참조하십시오.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,RegistrationDate,아이디,구매결정일자,주문번호,상품번호,정산상태,판매금액,판매단가,구매자명,구매자ID,상품명,수량,주문옵션,추가구성,사은품,수령인명,수령인_휴대폰,수령인_전화번호,배송번호,배송비_금액,발송일자,배송완료일자,택배사명_발송방법_,송장번호,구매자_휴대폰,구매자_전화번호,장바구니번호_결제번호_,주문일자_결제확인전_,판매자_관리코드,판매자_상세관리코드,서비스이용료,정산예정금액,주문확인일자,판매자쿠폰할인,스마일포인트적립,일시불할인,C_옥션_복수구매할인,C_옥션_우수회원할인,결제완료일,정산완료일,배송구분,주문종류,SKU번호_및_수량,글로벌샵구분,해외배송여부,제휴사명")] PaymentAuctionGmarket paymentAuctionGmarket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paymentAuctionGmarket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(paymentAuctionGmarket);
        }

        // GET: PaymentAuctionGmarkets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentAuctionGmarket paymentAuctionGmarket = db.PaymentAuctionGmarket.Find(id);
            if (paymentAuctionGmarket == null)
            {
                return HttpNotFound();
            }
            return View(paymentAuctionGmarket);
        }

        // POST: PaymentAuctionGmarkets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PaymentAuctionGmarket paymentAuctionGmarket = db.PaymentAuctionGmarket.Find(id);
            db.PaymentAuctionGmarket.Remove(paymentAuctionGmarket);
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
