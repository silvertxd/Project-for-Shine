using Project.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.IO;
using System.Data.Entity;
using System.Windows;
using System.Collections.ObjectModel;

namespace Project.MVVM.ViewModel
{
    public class ExportViewModel
    {
        public RelayCommand TotalSalesCountCommand { get; set; }
        public RelayCommand CountBySellerCommand { get; set; }


        public ExportViewModel()
        {
            TotalSalesCountCommand = new RelayCommand(o =>
            {
                DateTime startDate = new DateTime(2023, 4, 1);
                DateTime endDate = new DateTime(2024, 4, 1);

                using (var context = new ShineEntities())
                {
                    var sellers = context.Seller.ToList();
                    var products = context.Product.ToList();

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    var excelPackage = new ExcelPackage();

                    var worksheet = excelPackage.Workbook.Worksheets.Add("Количество проданного товара");

                    worksheet.Cells[1, 1].Value = "Наименование";

                    for (int i = 0; i < sellers.Count; i++)
                    {
                        worksheet.Cells[1, i + 2].Value = sellers[i].SellerName;
                    }

                    worksheet.Cells[1, sellers.Count + 2].Value = "Общее количество продаж";

                    int row = 2;

                    foreach (var product in products)
                    {
                        worksheet.Cells[row, 1].Value = product.ProductName;

                        int productSalesTotal = 0; 

                        for (int i = 0; i < sellers.Count; i++)
                        {
                            int sellerId = sellers[i].Id;
                            var salesTotal = context.Supply
                                .Where(s => s.SellerId == sellerId && s.ProductId == product.Id)
                                .Select(s => s.Quantity)
                                .DefaultIfEmpty(0)
                                .Sum();

                            worksheet.Cells[row, i + 2].Value = salesTotal;

                            productSalesTotal += salesTotal;
                        }

                        worksheet.Cells[row, sellers.Count + 2].Value = productSalesTotal;

                        row++;
                    }

                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();


                    // Сохранение файла Excel
                    string fileName = "SalesByProduct.xlsx";
                    var fileStream = new FileStream(fileName, FileMode.Create);
                    excelPackage.SaveAs(fileStream);
                    fileStream.Close();
                }
            });

            CountBySellerCommand = new RelayCommand(o =>
            {
                using (var context = new ShineEntities())
                {
                    var sellers = context.Seller.ToList();
                    var products = context.Product.ToList();

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    var excelPackage = new ExcelPackage();

                    foreach (var seller in sellers)
                    {
                        var worksheet = excelPackage.Workbook.Worksheets.Add(seller.SellerName);

                        worksheet.Cells[1, 1].Value = "Наименование";
                        worksheet.Cells[1, 2].Value = "Количество";
                        worksheet.Cells[1, 3].Value = "Цена за единицу";
                        worksheet.Cells[1, 4].Value = "Сумма";

                        int row = 2;

                        foreach (var product in products)
                        {
                            worksheet.Cells[row, 1].Value = product.ProductName;

                            var supply = context.Supply
                                .Where(s => s.SellerId == seller.Id && s.ProductId == product.Id)
                                .Select(s => new { s.Quantity, s.TotalPrice })
                                .FirstOrDefault();

                            if (supply != null)
                            {
                                var pricePerItem = product.Price - (product.Price * seller.Discount);

                                worksheet.Cells[row, 2].Value = supply.Quantity;
                                worksheet.Cells[row, 3].Value = pricePerItem;
                                worksheet.Cells[row, 4].Value = supply.Quantity * pricePerItem;
                            }

                            row++;
                        }

                        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                    }

                    string fileName = "SalesBySeller.xlsx";
                    var fileStream = new FileStream(fileName, FileMode.Create);
                    excelPackage.SaveAs(fileStream);
                    fileStream.Close();
                }



            });
        }
    }
}
