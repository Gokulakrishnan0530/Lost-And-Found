using AspNetCore.Reporting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Lost_And_Found.Controllers
{
    public class ReportController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public ReportController(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            this._webHostEnvironment = webHostEnvironment;
            this._configuration = configuration;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Print(string format)
        {
            string mimeType = "";
            string fileExtension = "";
            var path = $"{this._webHostEnvironment.WebRootPath}\\Reports\\Report2.rdlc";

            // Fetch data from LostItem table
            DataTable lostItemData = GetLostItemData();

            // Create a LocalReport object
            LocalReport localReport = new LocalReport(path);

            // Add the data source to the report
            localReport.AddDataSource("DataSet1", lostItemData);

            RenderType renderType;

            switch (format?.ToLower())
            {
                case "word":
                    renderType = RenderType.Word;
                    mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    fileExtension = "docx";
                    break;
                case "excel":
                    renderType = RenderType.Excel;
                    mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    fileExtension = "xlsx";
                    break;
                case "html":
                    renderType = RenderType.Html;
                    mimeType = "text/html";
                    fileExtension = "html";
                    break;
                //case "ppt":
                //    renderType = RenderType.Ppt;
                //    mimeType = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                //    fileExtension = "pptx";
                //    break;
                default:
                    renderType = RenderType.Pdf;
                    mimeType = "application/pdf";
                    fileExtension = "pdf";
                    break;
            }

            // Render the report
            var result = localReport.Execute(renderType, 1, null, mimeType);

            // Return the file
            return File(result.MainStream, mimeType, $"Lost Item.{fileExtension}");
        }
        public IActionResult Print2(string format)
        {
            string mimeType = "";
            string fileExtension = "";
            var path = $"{_webHostEnvironment.WebRootPath}\\Reports\\Report4.rdlc"; // Path to Report3.rdlc

            // Fetch data from Finded table
            DataTable findedData = GetFindedData();

            // Create a LocalReport object
            LocalReport localReport = new LocalReport(path);

            // Add the data source to the report (DataSet2 for Finded)
            localReport.AddDataSource("DataSet1", findedData);

            RenderType renderType;

            switch (format?.ToLower())
            {
                case "word":
                    renderType = RenderType.Word;
                    mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    fileExtension = "docx";
                    break;
                case "excel":
                    renderType = RenderType.Excel;
                    mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    fileExtension = "xlsx";
                    break;
                case "html":
                    renderType = RenderType.Html;
                    mimeType = "text/html";
                    fileExtension = "html";
                    break;
                default:
                    renderType = RenderType.Pdf;
                    mimeType = "application/pdf";
                    fileExtension = "pdf";
                    break;
            }

            // Render the report
            var result = localReport.Execute(renderType, 1, null, mimeType);

            // Return the file
            return File(result.MainStream, mimeType, $"Finded Items.{fileExtension}");
        }

        private DataTable GetLostItemData()
        {
            string connectionString = _configuration.GetConnectionString("CoreProjectContextConnection");
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM LostItem";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataTable);
                }
            }

            return dataTable;
        }
        private DataTable GetFindedData()
        {
            string connectionString = _configuration.GetConnectionString("CoreProjectContextConnection");
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Finded";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataTable);
                }
            }

            return dataTable;
        }
    }
}
