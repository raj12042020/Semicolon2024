
from rest_framework.urlpatterns import format_suffix_patterns
from rest_framework.routers import SimpleRouter
from django.urls import path, include
from ai_tool.controller.home import HomeController
from django.conf import settings
from django.conf.urls.static import static
router = SimpleRouter()
 
api_routes = [
    path('filter_skills', HomeController.as_view({'post': 'filter_skills'}), name="filter_skills"),
    path('get_qa', HomeController.as_view({'post': 'get_qa'}), name="get_qa")
]
 
urlpatterns = api_routes
urlpatterns = format_suffix_patterns(urlpatterns)