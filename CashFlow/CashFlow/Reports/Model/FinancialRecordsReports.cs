using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashFlow.Data;
using static Org.BouncyCastle.Bcpg.Attr.ImageAttrib;

namespace CashFlow.Reports.Model
{
    public  class FinancialRecordsReports
    {
        public PdfPTable AddContentToPDF(PdfPTable tableLayout, List<FinancialRecords> financialRecords)
        {
            float[] headers = { 15, 35, 20, 20, 20 };

            tableLayout.SetWidths(headers);
            tableLayout.WidthPercentage = 80;

            tableLayout.AddCell(new PdfPCell(new Phrase("Relatório CashFlow", new Font(Font.NORMAL, 13, 1, new iTextSharp.text.BaseColor(153, 51, 0))))
            {
                Colspan = 5,
                Border = 0,
                PaddingBottom = 20,
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            //Add header  
            AddCellToHeader(tableLayout, "Date");
            AddCellToHeader(tableLayout, "Description");
            AddCellToHeader(tableLayout, "Credit");
            AddCellToHeader(tableLayout, "Debit");
            AddCellToHeader(tableLayout, "Total");

            decimal totalGeral = 0;
            foreach (var obj in financialRecords)
            {
                AddCellToBody(tableLayout, obj.DateRecords.ToString("dd/MM/yyyy"), Element.ALIGN_CENTER);
                AddCellToBody(tableLayout, obj.Description, Element.ALIGN_LEFT);
                if ((int)obj.RecordType == 1)
                {
                    AddCellToBody(tableLayout, "0,00", Element.ALIGN_RIGHT);
                    AddCellToBody(tableLayout, obj.FinancialValue.ToString("N2"), Element.ALIGN_RIGHT);
                    totalGeral -= obj.FinancialValue;
                }
                else
                {
                    AddCellToBody(tableLayout, obj.FinancialValue.ToString("N2"), Element.ALIGN_RIGHT);
                    AddCellToBody(tableLayout, "0,00", Element.ALIGN_RIGHT);
                    totalGeral += obj.FinancialValue;
                }
                AddCellToBody(tableLayout, totalGeral.ToString("N2"), Element.ALIGN_RIGHT);
            }

            return tableLayout;
        }

        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.NORMAL, 8, 1, BaseColor.WHITE)))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5,
                BackgroundColor = new BaseColor(0, 51, 102)
            });
        }

        private static void AddCellToBody(PdfPTable tableLayout, string cellText, int element)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.NORMAL, 8, 1, BaseColor.BLACK)))
            {
                HorizontalAlignment = element,
                VerticalAlignment = 1 //top, center, bottom
            });
        }

        private static void AddCellToFooter(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.NORMAL, 8, 1, BaseColor.WHITE)))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5,
                BackgroundColor = new BaseColor(0, 51, 102)
            });
        }
    }
}
