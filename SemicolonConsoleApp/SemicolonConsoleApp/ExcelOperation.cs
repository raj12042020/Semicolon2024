using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SemicolonConsoleApp
{
    internal class ExcelOperation
    {
        public ExcelOperation()
        {

        }
        private Random gen = new Random();
        IList<Candidate> candidatesList = new List<Candidate>();
        private List<string> genricList = new List<string>();
        public void ReadData(string path)
        {
            Application xlApp = new Application();
            Workbook xlWorkBook = xlApp.Workbooks.Open(path);
            Worksheet xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.get_Item(1);

            Microsoft.Office.Interop.Excel.Range xlRange = xlWorkSheet.UsedRange;
            int totalRows = xlRange.Rows.Count;
            int totalColumns = xlRange.Columns.Count;

            for (int i = 2; i <= totalRows; i++)
            {
                dynamic cellValue = (xlWorkSheet.Cells[i, 15] as Microsoft.Office.Interop.Excel.Range).Value;
                int np = Convert.ToInt32((xlWorkSheet.Cells[i, 14] as Microsoft.Office.Interop.Excel.Range).Value);
                if (cellValue == "Yes")
                {
                    xlWorkSheet.Cells[i, 16] = gen.Next(5, np);
                }
            }


            xlApp.DisplayAlerts = false;
            string saveasPath = @"C:\Semicolon\CandidatesDatabase_Version17.xlsx";
            xlWorkBook.SaveAs(saveasPath, XlFileFormat.xlOpenXMLWorkbook,
                Missing.Value, Missing.Value, Missing.Value, Missing.Value, XlSaveAsAccessMode.xlNoChange,
                XlSaveConflictResolution.xlLocalSessionChanges, Missing.Value, Missing.Value,
                Missing.Value, Missing.Value);
            xlWorkBook.Close();
            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);

            Console.WriteLine("End of the file...");
        }

        public string GenerateRandomValue<T>(List<T> inputlist)
        {
            T value = default(T);

            int aRandomPos = gen.Next(inputlist.Count);

            value = inputlist[aRandomPos];
            return Convert.ToString(value);
        }
        public string GenerateRandomContactNumber()
        {
            Random randNum = new Random();
            long aRandomPos = randNum.Next(800000000, 999999999);

            return aRandomPos.ToString();
        }

        
        DateTime RandomDay()
        {
            DateTime start = new DateTime(1975, 1, 1);
            DateTime End = new DateTime(2015, 1, 1);
            int range = (End - start).Days;
            DateTime dateTime = start.AddDays(gen.Next(range));
            return dateTime;
        }
        public void GenerateRandomEducation()
        {
            Experiment experiment = new Experiment();
            experiment.GenerateEducation();
            genricList = experiment.EducationList;

        }

        public void GenerateRandomLocation()
        {
            Experiment experiment = new Experiment();
            experiment.GenerateLocation();
            genricList = experiment.LocationList;

        }
        List<string> JuniorRole = new List<string>() { "Junior Software Engieer", "JSE", "Software Engineer", "SE", "System Engineer", "Developer", "Junior Developer" };
        List<string> SeniorRole = new List<string>() { "Senior Software Engieer", "SSE", "Senior Developer", "Database Developer" };
        List<string> LeadRole = new List<string>() { "Techical Lead", "TL", "Project Lead", "PL", "Design Lead" };
        List<string> ArchitectRole = new List<string>() { "SA", "Software Architect", "Architect", "Solution Architect", "Solution Designer" };
        List<string> SeniorArchitectRole = new List<string>() { "Senior Architect", "Solution Architect", "Senior Solution Architect" };
        public void GenerateRandomJobRole(int exp)
        {
            genricList = new List<string>();
            switch (exp) 
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    genricList = JuniorRole;
                    break;
                case 5:
                case 6:
                case 7:
                case 8:
                    genricList = SeniorRole;
                    break;
                case 9:
                case 10:
                case 11:
                case 12:
                    genricList = LeadRole;
                    break;
                case 13:
                case 14:
                case 15:
                case 16:
                    genricList = ArchitectRole;
                    break;
                case 17:
                case 18:
                case 19:
                case 20:
                    genricList = SeniorArchitectRole;
                    break;
                default:
                    break;
            }
        }



        public void GenerateCompanyList()
        {
            Experiment experiment = new Experiment();
            experiment.GenerateCompanyList(@"C:\Semicolon\CompanyDB.xlsx");
            genricList = experiment.ComapnyList;

        }

        public void GenerateTechnicalSkill()
        {
            Experiment experiment = new Experiment();
            experiment.GeneratetechnicalSkills();
            genricList = experiment.technicalSkillList;

        }

        public void GenerateYearofExp(Worksheet xlWorkSheet, int totalRows)
        {
            Experiment experiment = new Experiment();
            experiment.GenerateExpYrs();
            int basicEducation = 2006;
            for (int i = 2; i <= totalRows; i++)
            {
                var cellValue = (DateTime)(xlWorkSheet.Cells[i, 6] as Microsoft.Office.Interop.Excel.Range).Value;
                if (cellValue != null)
                {
                    xlWorkSheet.Cells[i, 10] = (basicEducation) - cellValue.Year;
                }
            }

        }


    }


    public class Candidate
    {
        public string CandidateID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string DOB { get; set; }
        public string CurrentLocation { get; set; }
        public string PreferedLocation { get; set; }
        public string CurrentRole { get; set; }
        public string TechnicalSkills { get; set; }
        public string ExpYears { get; set; }
        public string ExpMonths { get; set; }
        public string CurrentCompany { get; set; }
        public string NoticePeriod { get; set; }
        public string RemainingDays { get; set; }
    }
}
