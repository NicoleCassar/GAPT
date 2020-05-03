﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FYPAllocationTest.Models;
using FYPAllocationTest.ViewModels;
using System.Text;
using System.IO;
using Microsoft.Data.SqlClient;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace FYPAllocationTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStudentRepository _studentRepository;

        public HomeController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        

        public IActionResult Index()
        {
            ViewBag.Title = "Student Data";
            var model = new StudentViewModel();
            model.student = _studentRepository.GetAllData();
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult FormA()
        {
            return View();
        }

        public IActionResult StaffForm()
        {
            return View();
        }

        [HttpPost]
       public FileResult Export_Tutors()
        {
            List<String> columnData = new List<String>();
            string connectionstring = "Server=MSSQLSERVER2;Database=fypallocation;Trusted_Connection=True;MultipleActiveResultSets=true";
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                string query = "SELECT sup.name, sup.surname, area.title, sup.quota, area.available FROM supervisor sup INNER JOIN supervisor_area area ON sup.supervisor_id = area.supervisor_id ORDER BY sup.quota;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            columnData.Add(reader[0].ToString()
                                + " " + reader[1].ToString()
                                + "," + reader[2].ToString()
                                + "," + reader[3].ToString()
                                + "," + reader[4].ToString());
                        }
                    }
                }
            }
            var sw = new StreamWriter("Tutors.csv", false, Encoding.UTF8);
            foreach (var item in columnData)
                sw.WriteLine(item);
            sw.Close();
            byte[] fileBytes = System.IO.File.ReadAllBytes(@"Tutors.csv");
            string fileName = "Tutors.csv";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public FileResult Export_Students()
        {
            List<String>columnData = new List<String>();
            string connectionstring = "Server=MSSQLSERVER2;Database=fypallocation;Trusted_Connection=True;MultipleActiveResultSets=true";
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                string query =
                    "SELECT pref.preference_id, stud.name, stud.surname, stud.average_mark, stud.student_id, sup.name, sup.surname, area.title " +
                    "FROM student_preference pref " +
                    "INNER JOIN supervisor_area area " +
                    "ON pref.area_id = area.area_id " +
                    "INNER JOIN supervisor sup " +
                    "ON area.supervisor_id = sup.supervisor_id " +
                    "INNER JOIN student stud " +
                    "ON pref.student_id = stud.student_id " +
                    "ORDER BY pref.student_id, pref.preference_id, stud.average_mark;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            columnData.Add(reader[0].ToString()
                                + "," + reader[1].ToString()
                                + " " + reader[2].ToString()
                                + "," + reader[3].ToString()
                                + "," + reader[4].ToString()
                                + "," + reader[5].ToString()
                                + " " + reader[6].ToString()
                                + "," + reader[7].ToString());
                        }
                    }
                }
            }
            var sw = new StreamWriter("students.csv", false, Encoding.UTF8);
            foreach (var item in columnData)
                sw.WriteLine(item);
            sw.Close();
            byte[] fileBytes = System.IO.File.ReadAllBytes(@"students.csv");
            string fileName = "students.csv";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

       
        public ActionResult FYPAlloc()
        {
            Export_Tutors();
            Export_Students();
            ScriptEngine engine = Python.CreateEngine();
            engine.ExecuteFile(@"SMPAlgtoCSV.py");
            if (System.IO.File.Exists("SMPResult.csv"))
            {
                StreamReader sr = new StreamReader("SMPResult.csv");
                List<string> res = new List<string>();
                string line;
                // Read and display lines from the file until the end of 
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    res.Add(line);
                }
                var result = res.ToList();
                ViewBag.Message = result;
                return View(result);
            }
            else
            {
                List<string> res = new List<string>();
                res.Add("No results available");
                var result = res.ToList();
               // ViewBag.Message = result;
                return View(result);
            }
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}