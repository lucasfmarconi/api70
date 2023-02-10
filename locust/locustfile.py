import time
from locust import HttpUser, task, between

class QuickstartUser(HttpUser):
    wait_time = between(1, 5)

    def on_start(self):
        self.client.verify = False;

    @task
    def POST_WeatherForecast(self):
        self.client.post("/WeatherForecast", json={"date":"2023-01-27T15:31:27.135Z","temperatureC":24,"summary":"string"})