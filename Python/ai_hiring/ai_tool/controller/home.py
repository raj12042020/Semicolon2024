from rest_framework import viewsets
from django.shortcuts import render
from django.http import JsonResponse
from ai_tool.services.home import HomeService
from ai_tool.globals import my_global_variable

import json

class HomeController(viewsets.ModelViewSet):
 
    def __init__(self):
        self.service = HomeService()
    
    def filter_skills(self, request):
        try:
            filter_prompt = """You will be provided with a job description , Your task is to extract below mentioned things
TechnicalSkills : only the required one worded technical skills
MinExpYears : Overall years of experiance required for the job
Education : Education required to be eligible for the job
PreferedLocation : Given Job location
NoticePeriod : Expected Candidate's current notice period

If any of the above mentioned things are not present in job description , then keep it empty.

Output must contain maximum of 10 skills only , experiance need to be a number,location must be a valid city name,Candiate's np must be number of months.

Don't assume anything which is not provided in job description.

Output must be in json format with below given keys
"TechnicalSkills":[
  Skill1,
  Skill2,
  Skill3,
  ....],
"MinExpYears":"..",
"Education":"..",
"PreferedLocation":"..",
"NoticePeriod":"..",




Job description -
"""

            job_des_str = request.body.decode('utf-8')
            global my_global_variable
            my_global_variable = job_des_str
            result = self.service.filter_skills(filter_prompt+my_global_variable)
            result = json.loads(result)
            return JsonResponse(result, status=201)
            
        except Exception as e:
            return JsonResponse({"error": str(e)}, status=400) 
    
    def get_qa(self, request):
        try:
            user_answers_str = request.body.decode('utf-8')
            role = user_answers_str.split(",")[0]
            exp = user_answers_str.split(",")[1]
            mcqprompt = f"""
Task: Create a quiz with 10 multiple choice question and answer
Topic: for {role} with {exp} years experiance
Style: Technical
Tone: Professional
Audience: 30-year old
Format: Text
Output format:"""

            output_format = """
{
  "quiz":[
    {
      "question":"....",
      "options":[
        "a:...",
        "b:...",
        "c:...",
        "d:..."
        ],
        "correct option":"a"
    },
    {
      "question":"....",
      "options":[
        "a:...",
        "b:...",
        "c:...",
        "d:..."
        ],
        "correct option":"c"
    },
    {
      "question":"....",
      "options":[
        "a:...",
        "b:...",
        "c:...",
        "d:..."
        ],
        "correct option":"d"
    }
  ]
}
"""
            result = self.service.get_qa(mcqprompt+output_format)
            result = json.loads(result)
            return JsonResponse(result, status=201)
        except Exception as e:
            return JsonResponse({"error": str(e)}, status=400)





    def analyse_resume(self, request):
        try:
            resume_str = request.body.decode('utf-8')
            global my_global_variable
            jd = my_global_variable
            resume_prompt = f"""Task: Your task is to act as Resume Analyser.
Analyze the provided resume and job description to assess the candidate's suitability for the position.

Instructions:
Provide a score out of 10, indicating the degree to which the candidate's skills, qualifications, and experiences align with the requirements and preferences outlined in the job description.
Consider factors such as relevant experience, education, skills, certifications, and any additional criteria specified in the job description.


Additional Guidelines:
Evaluate the candidate objectively based on the information provided in the resume and job description.
Consider the relevance and depth of the candidate's experiences and qualifications.
Assess the extent to which the candidate meets or exceeds the criteria outlined in the job description.

Resume - {resume_str}
Job description - {jd}
"""

            Outputformat = """
Output Format:
Print output in below given json format:
{
  "score":".."
}"""

            result = self.service.analyse_resume(resume_prompt+Outputformat)
            result = json.loads(result)
            return JsonResponse(result, status=201)
        except Exception as e:
            return JsonResponse({"error": str(e)}, status=400)