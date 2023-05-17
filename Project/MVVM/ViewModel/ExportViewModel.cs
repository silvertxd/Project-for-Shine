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
using Microsoft.Win32;
using System.Security.Cryptography.X509Certificates;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Globalization;
using System.Windows.Data;
using Xceed.Wpf.Toolkit.Primitives;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Controls;
using System.Windows.Forms;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;
using Telegram.Bot;
using Telegram.Bot.Args;
using System.Net.Http;

namespace Project.MVVM.ViewModel
{

    public class ExportViewModel : ObservableObject
    {
        public RelayCommand ExportCommand { get; set; }
        public RelayCommand ExportCommandTelegram { get; set; }
        public RelayCommand ProductSalesCommand { get; set; }
        public RelayCommand SalesBySellerCommand { get; set; }
        public RelayCommand ChangedCommand { get; set; }

        public class TypeCommands
        {
            public static string TotalSales { get; } = "TotalSales";
            public static string TotalSeller { get; } = "TotalSeller";
        }

        private bool _enableCalendar;
        public bool EnableCalendar
        {
            get { return _enableCalendar; }
            set
            {
                if (_enableCalendar != value)
                {
                    _enableCalendar = value;
                    OnPropertyChanged();
                }
            }
        }



        private DateTime _datestart;

        public DateTime DateStart
        {
            get { return _datestart; }
            set 
            { 
                _datestart = value;
                OnPropertyChanged();
            }
        }

        private DateTime _dateend;

        public DateTime DateEnd
        {
            get { return _dateend; }
            set 
            {
                _dateend = value; 
                OnPropertyChanged();
            }
        }



        private string _typeCommand;

        public string TypeCommand
        {
            get { return _typeCommand; }
            set
            {
                _typeCommand = value;
                OnPropertyChanged();
            }
        }

        private string _selectedDate;
        public string SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                if (SelectedDate == "Certain range")
                    EnableCalendar = true;
                else
                    EnableCalendar = false;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> Dates { get; set; }


        public ExportViewModel()
        {
            DateStart = DateTime.Now.AddMonths(-1);
            DateEnd = DateTime.Now;
            EnableCalendar = false; 
            Dates = new ObservableCollection<string>
            {
                "All time",
                "Last month",
                "Last 3 month",
                "Last 6 month",
                "Last year",
                "Certain range"
            };
            SelectedDate = Dates.FirstOrDefault();
            TypeCommand = TypeCommands.TotalSales;
            ExportCommand = new RelayCommand(o =>
            {
                if(TypeCommand == TypeCommands.TotalSales)
                {
                    switch (SelectedDate)
                    {
                        case "All time":
                            TotalSalesCountExport(new DateTime(0001, 1, 1), DateTime.Now);
                            break;
                        case "Last month":
                            TotalSalesCountExport(DateTime.Now.AddMonths(-1), DateTime.Now);
                            break;
                        case "Last 3 month":
                            TotalSalesCountExport(DateTime.Now.AddMonths(-3), DateTime.Now);
                            break;
                        case "Last 6 month":
                            TotalSalesCountExport(DateTime.Now.AddMonths(-6), DateTime.Now);
                            break;
                        case "Last year":
                            TotalSalesCountExport(DateTime.Now.AddYears(-1), DateTime.Now);
                            break;
                        case "Certain range":
                            if (DateStart < DateEnd)
                                TotalSalesCountExport(DateStart, DateEnd);
                            break;
                    }
                }
                else
                {
                    switch (SelectedDate)
                    {
                        case "All time":
                            CountBySellerExport(new DateTime(0001, 1, 1), DateTime.Now);
                            break;
                        case "Last month":
                            CountBySellerExport(DateTime.Now.AddMonths(-1), DateTime.Now);
                            break;
                        case "Last 3 month":
                            CountBySellerExport(DateTime.Now.AddMonths(-3), DateTime.Now);
                            break;
                        case "Last 6 month":
                            CountBySellerExport(DateTime.Now.AddMonths(-6), DateTime.Now);
                            break;
                        case "Last year":
                            CountBySellerExport(DateTime.Now.AddYears(-1), DateTime.Now); 
                            break;
                        case "Certain range":
                            if (DateStart < DateEnd)
                                CountBySellerExport(DateStart, DateEnd);
                            break;
                    }
                }
            });
            ExportCommandTelegram = new RelayCommand(async o =>
            {
                if (TypeCommand == TypeCommands.TotalSales)
                {
                    switch (SelectedDate)
                    {
                        case "All time":
                            await TotalSalesCountExportTelegram(new DateTime(0001, 1, 1), DateTime.Now);
                            break;
                        case "Last month":
                            await TotalSalesCountExportTelegram(DateTime.Now.AddMonths(-1), DateTime.Now);
                            break;
                        case "Last 3 month":
                            await TotalSalesCountExportTelegram(DateTime.Now.AddMonths(-3), DateTime.Now);
                            break;
                        case "Last 6 month":
                            await TotalSalesCountExportTelegram(DateTime.Now.AddMonths(-6), DateTime.Now);
                            break;
                        case "Last year":
                            await TotalSalesCountExportTelegram(DateTime.Now.AddYears(-1), DateTime.Now);
                            break;
                        case "Certain range":
                            if (DateStart < DateEnd)
                                await TotalSalesCountExportTelegram(DateStart, DateEnd);
                            break;
                    }
                }
                else
                {
                    switch (SelectedDate)
                    {
                        case "All time":
                            await CountBySellerExportTelegram(new DateTime(0001, 1, 1), DateTime.Now);
                            break;
                        case "Last month":
                            await CountBySellerExportTelegram(DateTime.Now.AddMonths(-1), DateTime.Now);
                            break;
                        case "Last 3 month":
                            await CountBySellerExportTelegram(DateTime.Now.AddMonths(-3), DateTime.Now);
                            break;
                        case "Last 6 month":
                            await CountBySellerExportTelegram(DateTime.Now.AddMonths(-6), DateTime.Now);
                            break;
                        case "Last year":
                            await CountBySellerExportTelegram(DateTime.Now.AddYears(-1), DateTime.Now);
                            break;
                        case "Certain range":
                            if (DateStart < DateEnd)
                                await CountBySellerExportTelegram(DateStart, DateEnd);
                            break;
                    }
                }
            });


            ProductSalesCommand = new RelayCommand(o =>
            {
                TypeCommand = TypeCommands.TotalSales;
            });
            SalesBySellerCommand = new RelayCommand(o =>
            {
                TypeCommand = TypeCommands.TotalSeller;
            });
            ChangedCommand = new RelayCommand(o =>
            {

            });

        }

        public async Task TotalSalesCountExportTelegram(DateTime startDate, DateTime endDate)
        {
            using (var context = new ShineEntities())
            {
                var sellers = context.Seller.ToList();
                var products = context.Product.ToList();

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var excelPackage = new ExcelPackage();

                var worksheet = excelPackage.Workbook.Worksheets.Add("Количество проданного товара");

                // Add the date range row at the top
                worksheet.Cells["A1"].Value = $"Промежуток: {startDate.ToShortDateString()} - {endDate.ToShortDateString()}";
                worksheet.Cells["A1"].Style.Font.Bold = true;

                worksheet.Cells[2, 1].Value = "Наименование";

                for (int i = 0; i < sellers.Count; i++)
                {
                    worksheet.Cells[2, i + 2].Value = sellers[i].SellerName;
                }

                worksheet.Cells[2, sellers.Count + 2].Value = "Общее количество продаж";

                int row = 3;

                foreach (var product in products)
                {
                    worksheet.Cells[row, 1].Value = product.ProductName;

                    int productSalesTotal = 0;

                    for (int i = 0; i < sellers.Count; i++)
                    {
                        int sellerId = sellers[i].Id;
                        var salesTotal = context.Supply
                            .Where(s => s.SellerId == sellerId && s.ProductId == product.Id && s.SupplyDate >= startDate && s.SupplyDate <= endDate)
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

                using (var stream = new MemoryStream())
                {
                    excelPackage.SaveAs(stream);
                    stream.Position = 0;

                    string telegramBotToken = "6188195711:AAF-SG6kEvOtGE1G_5a1HGatG3J6Z5K-OXs";
                    long chatId = 502493346;

                    var botClient = new TelegramBotClient(telegramBotToken);

                    string tempFilePath = Path.GetTempFileName();

                    using (var fileStream = new FileStream(tempFilePath, FileMode.Create))
                    {
                        await stream.CopyToAsync(fileStream);
                    }

                    using (var fileStream = new FileStream(tempFilePath, FileMode.Open))
                    {
                        var httpClient = new HttpClient();
                        var streamContent = new StreamContent(fileStream);

                        var formData = new MultipartFormDataContent();
                        formData.Add(streamContent, "document", "TotalSales.xlsx");

                        var response = await httpClient.PostAsync($"https://api.telegram.org/bot{telegramBotToken}/sendDocument?chat_id={chatId}", formData);
                    }

                    File.Delete(tempFilePath);
                }
            }
        }

        public async Task CountBySellerExportTelegram(DateTime startDate, DateTime endDate)
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

                    // Add the date range row at the top
                    worksheet.Cells["A1:D1"].Merge = true;
                    worksheet.Cells[1, 1].Value = $"Промежуток: {startDate.ToShortDateString()} - {endDate.ToShortDateString()}";
                    worksheet.Cells[1, 1].Style.Font.Bold = true;

                    worksheet.Cells[2, 1].Value = "Наименование";
                    worksheet.Cells[2, 2].Value = "Количество";
                    worksheet.Cells[2, 3].Value = "Цена за единицу";
                    worksheet.Cells[2, 4].Value = "Сумма";
                    int row = 3;

                    foreach (var product in products)
                    {
                        worksheet.Cells[row, 1].Value = product.ProductName;
                        var supply = context.Supply
                            .Where(s => s.SellerId == seller.Id && s.ProductId == product.Id && s.SupplyDate >= startDate && s.SupplyDate < endDate)
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

                using (var stream = new MemoryStream())
                {
                    excelPackage.SaveAs(stream);
                    stream.Position = 0;

                    string telegramBotToken = "6188195711:AAF-SG6kEvOtGE1G_5a1HGatG3J6Z5K-OXs";
                    long chatId = 502493346;

                    var botClient = new TelegramBotClient(telegramBotToken);

                    string tempFilePath = Path.GetTempFileName();

                    using (var fileStream = new FileStream(tempFilePath, FileMode.Create))
                    {
                        await stream.CopyToAsync(fileStream);
                    }

                    using (var fileStream = new FileStream(tempFilePath, FileMode.Open))
                    {
                        var httpClient = new HttpClient();
                        var streamContent = new StreamContent(fileStream);

                        var formData = new MultipartFormDataContent();
                        formData.Add(streamContent, "document", "TotalSellers.xlsx");

                        var response = await httpClient.PostAsync($"https://api.telegram.org/bot{telegramBotToken}/sendDocument?chat_id={chatId}", formData);
                    }

                    File.Delete(tempFilePath);
                }
            }
        }





        public void TotalSalesCountExport(DateTime startDate, DateTime endDate)
        {
            using (var context = new ShineEntities())
            {
                var sellers = context.Seller.ToList();
                var products = context.Product.ToList();

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var excelPackage = new ExcelPackage();

                var worksheet = excelPackage.Workbook.Worksheets.Add("Количество проданного товара");

                // Add the date range row at the top
                worksheet.Cells["A1"].Value = $"Промежуток: {startDate.ToShortDateString()} - {endDate.ToShortDateString()}";
                worksheet.Cells["A1"].Style.Font.Bold = true;

                worksheet.Cells[2, 1].Value = "Наименование";

                for (int i = 0; i < sellers.Count; i++)
                {
                    worksheet.Cells[2, i + 2].Value = sellers[i].SellerName;
                }

                worksheet.Cells[2, sellers.Count + 2].Value = "Общее количество продаж";

                int row = 3;

                foreach (var product in products)
                {
                    worksheet.Cells[row, 1].Value = product.ProductName;

                    int productSalesTotal = 0;

                    for (int i = 0; i < sellers.Count; i++)
                    {
                        int sellerId = sellers[i].Id;
                        var salesTotal = context.Supply
                            .Where(s => s.SellerId == sellerId && s.ProductId == product.Id && s.SupplyDate >= startDate && s.SupplyDate <= endDate)
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

                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
                saveFileDialog.FileName = "TotalSales.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string fileName = saveFileDialog.FileName;

                    using (var fileStream = new FileStream(fileName, FileMode.Create))
                    {
                        excelPackage.SaveAs(fileStream);
                    }
                }
            }
        }

        public void CountBySellerExport(DateTime startDate, DateTime endDate)
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

                    // Add the date range row at the top
                    worksheet.Cells["A1:D1"].Merge = true;
                    worksheet.Cells[1, 1].Value = $"Промежуток: {startDate.ToShortDateString()} - {endDate.ToShortDateString()}";
                    worksheet.Cells[1, 1].Style.Font.Bold = true;

                    worksheet.Cells[2, 1].Value = "Наименование";
                    worksheet.Cells[2, 2].Value = "Количество";
                    worksheet.Cells[2, 3].Value = "Цена за единицу";
                    worksheet.Cells[2, 4].Value = "Сумма";
                    int row = 3;

                    foreach (var product in products)
                    {
                        worksheet.Cells[row, 1].Value = product.ProductName;
                        var supply = context.Supply
                            .Where(s => s.SellerId == seller.Id && s.ProductId == product.Id && s.SupplyDate >= startDate && s.SupplyDate < endDate)
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

                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
                saveFileDialog.FileName = "SalesBySeller.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string fileName = saveFileDialog.FileName;

                    using (var fileStream = new FileStream(fileName, FileMode.Create))
                    {
                        excelPackage.SaveAs(fileStream);
                    }
                }
            }
        }


      

    }
}
