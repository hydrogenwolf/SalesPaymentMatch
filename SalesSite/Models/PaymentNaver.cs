//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 템플릿에서 생성되었습니다.
//
//     이 파일을 수동으로 변경하면 응용 프로그램에서 예기치 않은 동작이 발생할 수 있습니다.
//     이 파일을 수동으로 변경하면 코드가 다시 생성될 때 변경 내용을 덮어씁니다.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SalesSite.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PaymentNaver
    {
        public int ID { get; set; }
        public System.DateTime RegistrationDate { get; set; }
        public string 주문번호 { get; set; }
        public string 상품주문번호 { get; set; }
        public string 구분 { get; set; }
        public string 상품명 { get; set; }
        public string 구매자명 { get; set; }
        public Nullable<System.DateTime> 결제금액_정산예정일 { get; set; }
        public Nullable<System.DateTime> 결제금액_정산완료일 { get; set; }
        public Nullable<System.DateTime> 결제금액_정산기준일 { get; set; }
        public string 정산구분 { get; set; }
        public Nullable<int> 결제금액 { get; set; }
        public Nullable<int> 결제수수료 { get; set; }
        public string 주결제수단 { get; set; }
        public Nullable<int> 주결제수단_금액 { get; set; }
        public Nullable<int> 주결제수단_수수료 { get; set; }
        public Nullable<int> 보조결제수단_금액 { get; set; }
        public Nullable<int> 보조결제수단_수수료 { get; set; }
        public Nullable<int> 매출_연동_수수료 { get; set; }
        public Nullable<int> 채널수수료 { get; set; }
        public Nullable<int> 무이자할부수수료 { get; set; }
        public Nullable<int> C_구_판매수수료 { get; set; }
        public Nullable<int> 혜택금액 { get; set; }
        public Nullable<int> 정산예정금액 { get; set; }
    }
}