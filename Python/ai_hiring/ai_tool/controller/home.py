from rest_framework import viewsets
from django.shortcuts import render
from django.http import JsonResponse
from ai_tool.services.home import HomeService
import json

class HomeController(viewsets.ModelViewSet):
 
    def __init__(self):
        self.service = HomeService()
    
    def filter_skills(self, request):
        try:

            filter_prompt = """You will be provided with a job description , Your task is to extract below mentioned things
TechnicalSkills : only the required one worded technical skills
Experiance : Overall years of experiance required for the job
Education : Education required to be eligible for the job
Location : Job location
Candidate role : Expected Candidate's current role
Candiate's np : Expected Candidate's current notice period

If any of the above mentioned things are not present in job description , then keep it empty.

Output must contain maximum of 10 skills only , experiance need to be a number,location must be a valid city name,Candiate's np must be number of months.

Don't assume anything which is not provided in job description.

Output must be in json format with below given keys
"TechnicalSkills":[
  Skill1,
  Skill2,
  Skill3,
  ....],
"Experiance":"..",
"Education":"..",
"Location":"..",
"Candidate role":"..",
"Candiate's np":"..",




Job description -
"""

            job_des_str = request.body.decode('utf-8')
            print("job_des_str - ",job_des_str)
            result = self.service.filter_skills(filter_prompt+job_des_str)
            print("result - ",result)
            # print(request.body)
            result = json.loads(result)
            # print(data)
            return JsonResponse(result, status=201)
            
        except Exception as e:
            return JsonResponse({"error": str(e)}, status=400) 
    
    #method to send answers to gpt model
    def get_qa(self, request):
        try:
            user_answers_str = request.body.decode('utf-8')
            role = user_answers_str.split(",")[0]
            exp = user_answers_str.split(",")[1]
            print("role - ",role," , exp - ",exp)
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
            # input_data = request.data
            
            result = self.service.get_qa(mcqprompt+output_format)
            print(result)
            result = json.loads(result)
            # print(data)
            return JsonResponse(result, status=201)
        except Exception as e:
            return JsonResponse({"error": str(e)}, status=400)