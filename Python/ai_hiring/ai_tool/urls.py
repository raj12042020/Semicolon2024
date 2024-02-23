
from rest_framework.urlpatterns import format_suffix_patterns
from rest_framework.routers import SimpleRouter
from django.urls import path, include
from ai_tool.controller.home import HomeController
from django.conf import settings
from django.conf.urls.static import static
router = SimpleRouter()
 
api_routes = [
    path('filter_skills', HomeController.as_view({'post': 'filter_skills'}), name="filter_skills"),
    path('analyse_answer', HomeController.as_view({'post': 'analyse_answer'}), name="analyse_answer")
]
 
urlpatterns = api_routes
urlpatterns = format_suffix_patterns(urlpatterns)