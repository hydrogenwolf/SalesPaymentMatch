using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

/*
CREATE TABLE [dbo].[Payment11st](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RegistrationDate] [datetime] NOT NULL,
	[주문번호] [nvarchar](255) NULL,
	[주문순번] [int] NULL,
	[배송번호] [nvarchar](255) NULL,
	[주문상태] [nvarchar](255) NULL,
	[구매자명] [nvarchar](255) NULL,
	[구매자ID] [nvarchar](255) NULL,
	[결제완료일] [datetime] NULL,
	[발송처리일] [datetime] NULL,
	[배송완료일] [datetime] NULL,
	[수취확인일] [datetime] NULL,
	[송금완료일] [datetime] NULL,
	[상품번호] [nvarchar](255) NULL,
	[상품명] [nvarchar](255) NULL,
	[옵션명] [nvarchar](255) NULL,
	[수량] [int] NULL,
	[정산금액] [int] NULL,
	[판매금액합계] [int] NULL,
	[추가정산금액합계] [int] NULL,
	[공제금액합계] [int] NULL,
	[판매가] [int] NULL,
	[옵션가] [int] NULL,
	[선결제배송비] [int] NULL,
	[도서산간배송비] [int] NULL,
	[구매자부담 반품/교환배송비] [int] NULL,
	[반품/교환추가금] [int] NULL,
	[반품선결제배송비] [int] NULL,
	[반품도서산간배송비] [int] NULL,
	[해외취소배송비] [int] NULL,
	[티켓예매수수료] [int] NULL,
	[티켓취소예매수수료] [int] NULL,
	[티켓취소위약금] [int] NULL,
	[여행취소위약금] [int] NULL,
	[송장번호] [nvarchar](255) NULL,
	[서비스이용료정책] [nvarchar](255) NULL,
	[기본서비스이용율] [nvarchar](255) NULL,
	[서비스이용료] [int] NULL,
	[할인쿠폰이용료] [int] NULL,
	[판매자기본할인] [int] NULL,
	[판매자추가할인] [int] NULL,
	[11번가할인] [int] NULL,
	[복수구매할인비용] [int] NULL,
	[포인트이용료] [int] NULL,
	[칩이용료] [int] NULL,
	[무이자할부이용료] [int] NULL,
	[후불광고비] [int] NULL,
	[OK캐쉬백 적립부담액] [int] NULL,
	[지정택배이용료] [int] NULL,
	[전세계배송 판매자책임반품] [int] NULL,
	[수출대행수수료] [int] NULL,
	[물류이용수수료] [int] NULL,
 CONSTRAINT [PK_Payment11st] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Payment11st] ADD  CONSTRAINT [DF_Payment11st_RegistrationDate]  DEFAULT (getdate()) FOR [RegistrationDate]
GO

CREATE INDEX [IX_Payment11st_InvoiceNumber] ON [Payment11st] ([송장번호])
GO
*/

namespace Payment11st
{
    class Program
    {
        static string fileName = Path.Combine(Directory.GetCurrentDirectory(), "11번가 정산_확정건__20170901-20170930_ainmart.xls");
        static string connectionString = String.Format("Server=.;Database=Noition;Trusted_Connection=True;");
        static string firstColumn = "B";
        static string lastColumn = "AY";
        static int titleRow = 6;
        static string tableName = "Payment11st";

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
                        if (fields[i].Equals("서비스이용료"))
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
