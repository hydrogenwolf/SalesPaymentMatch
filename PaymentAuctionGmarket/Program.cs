using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

/*
CREATE TABLE [dbo].[PaymentAuctionGmarket](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RegistrationDate] [datetime] NOT NULL,
	[아이디] [nvarchar](255) NULL,
	[구매결정일자] [datetime] NULL,
	[주문번호] [nvarchar](255) NULL,
	[상품번호] [nvarchar](255) NULL,
	[정산상태] [nvarchar](255) NULL,
	[판매금액] [int] NULL,
	[판매단가] [int] NULL,
	[구매자명] [nvarchar](255) NULL,
	[구매자ID] [nvarchar](255) NULL,
	[상품명] [nvarchar](255) NULL,
	[수량] [int] NULL,
	[주문옵션] [nvarchar](255) NULL,
	[추가구성] [nvarchar](255) NULL,
	[사은품] [nvarchar](255) NULL,
	[수령인명] [nvarchar](255) NULL,
	[수령인 휴대폰] [nvarchar](255) NULL,
	[수령인 전화번호] [nvarchar](255) NULL,
	[배송번호] [nvarchar](255) NULL,
	[배송비 금액] [int] NULL,
	[발송일자] [datetime] NULL,
	[배송완료일자] [datetime] NULL,
	[택배사명(발송방법)] [nvarchar](255) NULL,
	[송장번호] [nvarchar](255) NULL,
	[구매자 휴대폰] [nvarchar](255) NULL,
	[구매자 전화번호] [nvarchar](255) NULL,
	[장바구니번호(결제번호)] [nvarchar](255) NULL,
	[주문일자(결제확인전)] [datetime] NULL,
	[판매자 관리코드] [nvarchar](255) NULL,
	[판매자 상세관리코드] [nvarchar](255) NULL,
	[서비스이용료] [int] NULL,
	[정산예정금액] [int] NULL,
	[주문확인일자] [datetime] NULL,
	[판매자쿠폰할인] [int] NULL,
	[스마일포인트적립] [nvarchar](255) NULL,
	[일시불할인] [nvarchar](255) NULL,
	[(옥션)복수구매할인] [nvarchar](255) NULL,
	[(옥션)우수회원할인] [nvarchar](255) NULL,
	[결제완료일] [datetime] NULL,
	[정산완료일] [datetime] NULL,
	[배송구분] [nvarchar](255) NULL,
	[주문종류] [nvarchar](255) NULL,
	[SKU번호 및 수량] [nvarchar](255) NULL,
	[글로벌샵구분] [nvarchar](255) NULL,
	[해외배송여부] [nvarchar](255) NULL,
	[제휴사명] [nvarchar](255) NULL,
 CONSTRAINT [PK_PaymentAuctionGmarket] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PaymentAuctionGmarket] ADD  CONSTRAINT [DF_PaymentAuctionGmarket_RegistrationDate]  DEFAULT (getdate()) FOR [RegistrationDate]
GO

CREATE INDEX [IX_PaymentAuctionGmarket_InvoiceNumber] ON [PaymentAuctionGmarket] ([송장번호])
GO
*/

namespace PaymentAuctionGmarket
{
    class Program
    {
        static string fileName = Path.Combine(Directory.GetCurrentDirectory(), "Auction Gmarket_2017-09-29 13-26.xls");
        static string connectionString = String.Format("Server=.;Database=Noition;Trusted_Connection=True;");
        static string firstColumn = "A";
        static string lastColumn = "AS";
        static int titleRow = 1;
        static string tableName = "PaymentAuctionGmarket";

        static void Main(string[] args)
        {
            SetConnectionString();

            Application application = new Application();
            Workbook workbook = application.Workbooks.Open(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            foreach (Worksheet sheet in workbook.Sheets)
            {
                ReadSheet(sheet);
            }
        }

        [Conditional("DEBUG")]
        public static void SetConnectionString()
        {
            connectionString = String.Format("Initial Catalog={0};Data Source={1};User ID={2};Password={3}", "Noition", "192.168.1.120", "noition", "prune2017$");
        }

        static void ReadSheet(Worksheet sheet)
        {
            long fullRow = sheet.Rows.Count;
            long lastRow = sheet.Cells[fullRow, 1].End(XlDirection.xlUp).Row;

            Range titles = sheet.get_Range(String.Format("{0}{1}:{2}{1}", firstColumn, titleRow, lastColumn), Type.Missing);
            string[] fields = new string[titles.Count];
            int column = 0;
            foreach (Range title in titles)
            {
                if (title.Value2 == null)
                {
                    column += 1;
                    continue;
                }

                //fields[column] = title.Value2.ToString(); // 실제 저장된 값
                fields[column] = title.Text.ToString();     // 사용자에게 보여지는 값

                column += 1;
            }

            string c = "", v = "";
            for (int i = 0; i < fields.Length; i++)
            {
                if (!String.IsNullOrEmpty(fields[i]))
                {
                    if (c.Length > 0) c += ", ";
                    if (v.Length > 0) v += ", ";

                    c += "[" + fields[i] + "]";
                    v += "@F" + i;
                }
            }
            string query = String.Format("INSERT INTO {0} ({1}) VALUES ({2})", tableName, c, v);

            for (int row = titleRow + 1; row <= lastRow; row++)
            {
                Range cells = sheet.get_Range(String.Format("{0}{1}:{2}{1}", firstColumn, row, lastColumn), Type.Missing);
                bool goingOn = false;
                int i = 0;
                List<SqlParameter> parameters = new List<SqlParameter>();
                foreach (Range cell in cells)
                {
                    if (String.IsNullOrEmpty(fields[i]))
                    {
                        i += 1;
                        continue;
                    }

                    if (cell.Value2 == null)
                    {
                        parameters.Add(new SqlParameter("@F" + i, ""));
                    }
                    else
                    {
                        if (fields[i].Equals("판매금액") || fields[i].Equals("판매단가") || fields[i].Equals("배송비 금액")
                             || fields[i].Equals("서비스이용료") || fields[i].Equals("정산예정금액") || fields[i].Equals("판매자쿠폰할인"))
                        {
                            int number = 0;
                            try
                            {
                                number = int.Parse(cell.Value2, NumberStyles.AllowThousands);
                            }
                            catch
                            {
                            }

                            parameters.Add(new SqlParameter("@F" + i, number));
                        }
                        else if (fields[i].Equals("주문번호") || fields[i].Equals("장바구니번호(결제번호)"))
                        {
                            parameters.Add(new SqlParameter("@F" + i, cell.Text));
                        }
                        else if (fields[i].Equals("정산완료일") && String.IsNullOrEmpty(cell.Text))
                        {
                            parameters.Add(new SqlParameter("@F" + i, DBNull.Value));
                        }
                        else
                        {
                            parameters.Add(new SqlParameter("@F" + i, cell.Value2));
                        }

                        goingOn = true;
                    }

                    i += 1;
                }

                if (!goingOn) continue;

                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.Parameters.AddRange(parameters.ToArray<SqlParameter>());
                    connection.Open();
                    command.ExecuteNonQuery();

                    Console.WriteLine(row);
                }
            }
        }
    }
}
