using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SemicolonConsoleApp
{
    internal class Experiment
    {
        public const string JobRole = "Junior Software Engieer,JSE,Software Engineer,SE,Senior Software Engieer,SSE,Developer,Junior Developer,Senior Developer,Database Developer,Techical Lead,TL,Project Lead ,PL,Senior Architect,SA,Architect,Solution Architect,Design Lead,System Engineer";
        public const string Education = "Mtech,MSc,Btech,BSc,MCA,BCA, Diploma,Master in Technology,Master in Computer Application,Bachelor in Technology,Bachelor in Engineering,BE,Bachelor in Telecommunication,Bachelor in Electonics";
        public const string noticeperiod = "30,45,60,90,15";
        public const string ExpYrs = "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18, 29,20";
        public const string RemainingDays = "7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45";
        public const string technicalSkill = @".NET, C#, WPF, WCF, SQL\\C#, ASP.NET,REST API, ReactJs, MVC, SQL\\C#.Net, ASP.NET,REST API, ReactJs, MVC, .Net Core, SQL\\C#, ASP.NET, .NET Core, JavaScript, SQL, Azure, MVC, EntityFramework, EF, PowerBI, PowerApps, Razor Pages, API development\\C#, ASP.NET, ASP.NET Core, REST API, JavaScript, SQL, MVC, EntityFramework, PowerApps, MS Test, Nunit\\C#.Net, ASP.NET,REST API, ReactJs, MVC, .Net Core\\.NET, C#, WPF,Winforms, WCF, SQL, AWS\\Dotnet, Csharp, Windowsform, Rest API, SQL, Azure\\Dotnet, Csharp, Windowsform, Rest API, SQL, Azure,MVC, EntityFramework, EF, PowerBI, PowerApps\\C#, ASP.NET,REST API, ReactJs, AngularJS, Moq test, Nunit, Crystal Report\\.Net ,ASP.NET MVC, ASP.NET Web API,JavaScript, CSS, JQuery, KendoUI, Bootstrap, MS SQL,  EF6\\Utilized ASP.NET MVC, ASP.NET Web API, JavaScript, CSS, JQuery, KendoUI, Bootstrap\\.NET Core,ASP MVC,Agile,Web API,IronPython,IronRuby,LINQ,OOD,SQL Server,EF Core,C#,F#,Visual Basic,Ada,Hangfire,EF 6,JavaScript,JQuery,JSON,PHP\\C#, ASP.NET, .NET Core, JS, SQL, Microsoft Azure, MVC, EntityFramework, EF, PowerBI, PowerApps, Razor Pages, API development\\C#, ASP.NET,REST API, ReactJs, AngularJS, Moq test, Nunit, Crystal Report, .Net core, Web API\\.NET, C#, WPF,Winforms, WCF, SQL,Python, Django, NumPy, Flask, HTML, CSS, JavaScript, RESTful APIs, Amazon Web Services\\Python, Django, NumPy, Flask, HTML, CSS, JavaScript, RESTful APIs, Pandas, ML, SQL, GIT,Dotnet, Csharp, Windowsform\\Python, Django, NumPy, Matplotlib, HTML, CSS, RESTful APIs, Pandas, ML, TenserFlow, Keras,C#, ASP.NET,REST API, ReactJs, MVC, SQL\\Python, Django, NumPy, Flask, HTML, CSS, JavaScript, RESTful APIs, Pandas, ML, SQL, GIT\\Python, Django, NumPy, Matplotlib, HTML, CSS, RESTful APIs, Pandas, ML, TenserFlow, Keras\\Python, Django, NumPy, Flask, HTML, CSS, JavaScript, GIT, Agile Methodologies\\Python, Django, Flask, NumPy, Matplotlib, HTML, CSS, JavaScript, RESTful APIs, Pandas, ML, TenserFlow, Keras, SQL\\Python, Django, NumPy, Flask, Pandas, ML, SQL, GIT\\Python, Django, NumPy, Flask, Pandas, ML, SQL, GIT, JavaScript, RESTful APIs, Amazon Web Services\\Python, Django, NumPy, Matplotlib, HTML, CSS, RESTful APIs, Pandas, ML, TenserFlow, Keras,ASP.NET Web API,JavaScript, CSS, JQuery, KendoUI, Bootstrap, MS SQL\\";
        const string location = "Ahmedabad,Bengaluru,Bhopal,Chennai,Goa,Gurugram,Hyderabad,Indore,Jaipur,Kochi,Kolkata,Lucknow,Mumbai,Nagpur,Noida,Pune";
        public List<string> JobRoleList = new List<string>();
        public List<string> EducationList = new List<string>();
        public List<string> technicalSkillList = new List<string>();
        public List<string> NoticePeriodList = new List<string>();
        public List<string> ExpYrsList = new List<string>();
        public List<string> RemainingDaysList = new List<string>();
        public List<string> LocationList = new List<string>();
        public List<string> ComapnyList = new List<string>();
        public void GenerateRandomJobRole()
        {
            GenerateJobRole();
            for (int i = 0; i < 35; i++)
            {
                string job = GenerateRandomValue(JobRoleList);
                Console.WriteLine(job);
            }
        }

        public string GenerateRandomValue<T>(List<T> inputlist)
        {
           T value = default(T);
            Random randNum = new Random();
            int aRandomPos = randNum.Next(inputlist.Count);

            value = inputlist[aRandomPos];
            return Convert.ToString(value);
        }
        public string GenerateRandomContactNumber()
        {
            Random randNum = new Random();
            long aRandomPos = randNum.Next(800000000,999999999);

           return aRandomPos.ToString();
        }
        public void GenerateJobRole()
        {
            JobRoleList = JobRole.Split(",").ToList<string>();

        }

        public void GenerateEducation()
        {
            EducationList = Education.Split(",").ToList<string>();
        }

        public void Generatenoticeperiod()
        {
            NoticePeriodList = noticeperiod.Split(",").ToList<string>();
        }

        public void GenerateExpYrs()
        {
            ExpYrsList = ExpYrs.Split(",").ToList<string>();
        }

        public void GenerateRemainingDays()
        {
            RemainingDaysList = RemainingDays.Split(",").ToList<string>();
        }
        public void GeneratetechnicalSkills()
        {
            technicalSkillList = technicalSkill.Split("\\").ToList<string>();
            technicalSkillList.RemoveAll(s => string.IsNullOrWhiteSpace(s));
        }

        public void GenerateLocation()
        {
            LocationList = location.Split(",").ToList<string>();
        }

        public void GenerateCompanyList(string path)
        {
            
                Application xlApp = new Application();
                Workbook xlWorkBook = xlApp.Workbooks.Open(path);
                Worksheet xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.get_Item(1);

                Microsoft.Office.Interop.Excel.Range xlRange = xlWorkSheet.UsedRange;
                int totalRows = xlRange.Rows.Count;
                int totalColumns = xlRange.Columns.Count;

            //iterate over the rows and columns and print to the console as it appears in the file
            //excel is not zero based!!
            for (int i = 2; i <= totalRows; i++)
            {
                var cellValue = (string)(xlWorkSheet.Cells[i, 1] as Microsoft.Office.Interop.Excel.Range).Value;
                if (cellValue != null)
                {
                    ComapnyList.Add(cellValue);
                }
            }


            xlWorkBook.Close();
                xlApp.Quit();

                Marshal.ReleaseComObject(xlWorkSheet);
                Marshal.ReleaseComObject(xlWorkBook);
                Marshal.ReleaseComObject(xlApp);

                Console.WriteLine("End of the file...");
        }

        public static void populateObject(object o)
        {
            Random r = new Random();
            FieldInfo[] propertyInfo = o.GetType().GetFields();
            for (int i = 0; i < propertyInfo.Length; i++)
            {
                FieldInfo info = propertyInfo[i];

                string strt = info.FieldType.Name;
                Type t = info.FieldType;
                try
                {
                    dynamic value = null;

                    if (t == typeof(string) || t == typeof(System.String))
                    {
                        value = "asdf";
                    }
                    else if (t == typeof(Int16) || t == typeof(Int32) || t == typeof(Int64))
                    {
                        value = (Int16)r.Next(999);
                        info.SetValue(o, value);
                    }
                    else if (t == typeof(Int16?))
                    {
                        Int16? v = (Int16)r.Next(999);
                        info.SetValue(o, v);
                    }
                    else if (t == typeof(Int32?))
                    {
                        Int32? v = (Int32)r.Next(999);
                        info.SetValue(o, v);
                    }
                    else if (t == typeof(Int64?))
                    {
                        Int64? v = (Int64)r.Next(999);
                        info.SetValue(o, v);
                    }
                    else if (t == typeof(DateTime) || t == typeof(DateTime?))
                    {
                        value = DateTime.Now;
                        info.SetValue(o, value);
                    }
                    else if (t == typeof(double) || t == typeof(float) || t == typeof(Double))
                    {
                        value = 17.2;
                        info.SetValue(o, value);
                    }
                    else if (t == typeof(char) || t == typeof(Char))
                    {
                        value = 'a';
                        info.SetValue(o, value);
                    }
                    else
                    {
                        //throw new NotImplementedException ("Tipo não implementado :" + t.ToString () );
                        object temp = info.GetValue(o);
                        if (temp == null)
                        {
                            temp = Activator.CreateInstance(t);
                            info.SetValue(o, temp);
                        }
                        populateObject(temp);
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
