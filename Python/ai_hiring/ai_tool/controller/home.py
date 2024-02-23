from rest_framework import viewsets
from django.shortcuts import render
from django.http import JsonResponse
from ai_tool.services.home import HomeService

class HomeController(viewsets.ModelViewSet):
 
    def __init__(self):
        self.service = HomeService()
    
    def filter_skills(self, request):
        try:
            # input_data = request.data
            data = {"data": "data"}
            job_des = request.body
            job_des_str = job_des.decode('utf-8')
            print(job_des)
            prompt = 'what is resume'
            result = self.service.filter_skills(f"{job_des_str}\n{prompt}")
            print(result)
            # print(request.body)
           
            # print(data)
            return JsonResponse({"result":result}, status=201)
            
        except Exception as e:
            return JsonResponse({"error": str(e)}, status=400) 
    
    #method to send answers to gpt model
    def analyse_answer(self, request):
        try:
            # input_data = request.data
            user_answers = request.body
            user_answers_str = user_answers.decode('utf-8')
            prompt = "analyse the answers"
            result = self.service.analyse_answer(f"{user_answers_str}\n{prompt}")
            print(result)
           
            # print(data)
            return JsonResponse({"result":result}, status=201)
        except Exception as e:
            return JsonResponse({"error": str(e)}, status=400)