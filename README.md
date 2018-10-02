# StackExchangeWebClient

Important:
To connect to stackexchange api it is required to supply parameters in appsettings.json file under values marked as "replacement".



-------------------------------------------------------------------------------------------
StackExchangeApi library manages requests and responses from the stackexchange api and is injected to MVC project through dependency injection.

Models:
RequestModel class
Manages the URI parameters for most common requests to stackexchange api. Can be used as a base for additional URI models for other requests.
TagRequestModel class 
Specifically constructed to manage URI parameters for requests to tags list response. It allows flexible creation of complex queries.
TagResponseModel class
Used to parse JSON data from api response.

ApiClientService class manages requests, responses and URI construction.
-------------------------------------------------------------------------------------------
