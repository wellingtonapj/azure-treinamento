using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using comunidadecloud.Models;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace comunidadecloud.Controllers
{
    public class AlunosController : Controller
    {
        private readonly ILogger<AlunosController> _logger;

        public AlunosController(ILogger<AlunosController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = new List<Alunos>();
            string connectionString ="Server=tcp:comunidadecloud.database.windows.net,1433;Initial Catalog=comunidadecloud;Persist Security Info=False;User ID=augroberto;Password=P@ssw0rd;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("Select * from alunos");
                cmd.Connection = con;

                //cmd.CommandType = CommandType.Text;
                SqlDataReader rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    Alunos alunos = new Alunos();
                    alunos.id = Convert.ToInt32(rdr["id"]);
                    alunos.name = rdr["name"].ToString();

                    model.Add(alunos);
                }

                con.Close();
            }
            return View(model);
        }
    }
}