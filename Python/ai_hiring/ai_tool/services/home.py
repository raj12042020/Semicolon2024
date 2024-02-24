from rest_framework import viewsets
from django.shortcuts import render
from django.http import JsonResponse
from django.views import View
# from langchain.chat_models import AzureChatOpenAI
from langchain_openai import AzureChatOpenAI
from langchain_openai import ChatOpenAI

class HomeService():
 
    def __init__(self):
        self.llm = ChatOpenAI(
        openai_api_base="https://4veynppxjm.us-east-1.awsapprunner.com",
        model_name="gpt-35-turbo-16k",
        openai_api_key="",
    )
 
    def filter_skills(self,prompt_and_data):
        try:
            print("prompt data - ",prompt_and_data)
            result = self.llm.invoke(prompt_and_data).content
            # result = 'i am cool🐳'
            print("llm output - ",result)
            return result
            # return result
        except Exception as e:
            print(f"Error invoking GPT: {e}")
            raise
    #     return data
    
    def get_qa(self,prompt_and_data):
        try:
            result = self.llm.invoke(prompt_and_data).content
            # return result
            print("llm mcq output - ",result)
            
            return result
        except Exception as e:
            print(f"Error invoking GPT: {e}")
            raise