using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;

namespace SemicolonConsoleApp
{
    public static class ReadJsonfile
    {
        public static void Read()
        {
            string filePath = @"C:\Semicolon\QuestionData.json";
            string text = File.ReadAllText(filePath);
            QuestionbankBase qb =JsonSerializer.Deserialize<QuestionbankBase>(text);
        }

       
    }

    public class QuestionbankBase
    {
        public List<QuestionBank> QuestionBank { get; set; }
    }

    public class QuestionBank
    {
        public string Technology { get; set; }
        public string Exp { get; set; }
        public List<Question> Questions { get; set; }
    }

    public class Question
    {
        public Question()
        {
        }
        public string Q { get; set; } 
        public List<string> Options { get; set; }
        public int A { get; set; }
    }

   
}
