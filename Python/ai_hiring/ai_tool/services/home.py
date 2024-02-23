from rest_framework import viewsets
from django.shortcuts import render
from django.http import JsonResponse
from django.views import View
# from langchain.chat_models import AzureChatOpenAI
from langchain_openai import AzureChatOpenAI

class HomeService():
 
    def __init__(self):
        self.llm = AzureChatOpenAI(
        azure_endpoint="dummayurl",
        openai_api_version="2023-05-15",
        deployment_name="gpt3.5",
        openai_api_key="232343dvfds",
        openai_api_type="azure",
    )
 
    def filter_skills(self,prompt_and_data):
        try:
            # result = self.llm.invoke(prompt_and_data).content
            result = 'i am cool🐳'
            # print(data)
            return prompt_and_data
            # return result
        except Exception as e:
            print(f"Error invoking GPT: {e}")
            raise
    #     return data
    
    def analyse_answer(self,prompt_and_data):
        try:
            # result = self.llm.invoke(prompt_and_data).content
            # return result
            return prompt_and_data
        except Exception as e:
            print(f"Error invoking GPT: {e}")
            raise