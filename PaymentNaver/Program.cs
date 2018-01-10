using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;

/*
CREATE TABLE [dbo].[PaymentNaver](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RegistrationDate] [datetime] NOT NULL,
	[주문번호] [nvarchar](255) NULL,
	[상품주문번호] [nvarchar](255) NULL,
	[구분] [nvarchar](255) NULL,
	[상품명] [nvarchar](255) NULL,
	[구매자명] [nvarchar](255) NULL,
	[결제금액 정산예정일] [datetime] NULL,
	[결제금액 정산완료일] [datetime] NULL,
	[결제금액 정산기준일] [datetime] NULL,
	[정산구분] [nvarchar](255) NULL,
	[결제금액] [int] NULL,
	[결제수수료] [int] NULL,
	[주결제수단] [nvarchar](255) NULL,
	[주결제수단 금액] [int] NULL,
	[주결제수단 수수료] [int] NULL,
	[보조결제수단 금액] [int] NULL,
	[보조결제수단 수수료] [int] NULL,
	[매출 연동 수수료] [int] NULL,
	[채널수수료] [int] NULL,
	[무이자할부수수료] [int] NULL,
	[(구)판매수수료] [int] NULL,
	[혜택금액] [int] NULL,
	[정산예정금액] [int] NULL,
 CONSTRAINT [PK_PaymentNaver] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PaymentNaver] ADD  CONSTRAINT [DF_PaymentNaver_RegistrationDate]  DEFAULT (getdate()) FOR [RegistrationDate]
GO

CREATE INDEX [IX_PaymentNaver_OrderNumber] ON [PaymentNaver] ([주문번호])
GO
 */

namespace PaymentNaver
{
    class Program
    {
        static string fileName = Path.Combine(Directory.GetCurrentDirectory(), "스토어팜09.xlsx");
        static string connectionString = String.Format("Server=.;Database=Noition;Trusted_Connection=True;");
        static string firstColumn = "B";
        static string lastColumn = "W";
        static int titleRow = 1;
        static string tableName = "PaymentNaver";

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
                        parameters.Add(new SqlParameter("@F" + i, cell.Value2));    // 보여지는 값이 아닌 실제 저장된 값을 저장
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
