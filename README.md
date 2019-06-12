# Weather Information



These are the features of the application.
-	It takes a file. Validates the file format. Validates json if it is a json file. (If time permitted, excel file upload would also have been done.)
-	Uploads a file to the server.
o	If file size is bigger than 250KB, then file is uploaded in chunks.
-	Displays weather data in grid on UI for first 20 cities only – data is fetched from weather API.
-	On server, we fetch data for each city and stores in individual file.
o	Reads the uploaded file.
o	Get city ids
o	Pass comma separated 20 city ids to the weather API and gets data.
o	Reads data and stores in individual file.
	This IO operation is improved with the help of asynchronous transactions.
	Reading files. Fetching data from weather API and storing in file. – All these tasks are performed asynchronously.
o	File format:
	Folder – historicalData/Day.Month.Year/
	File Name – cityId_CityName_Hour.Minute.Seconds.json
NOTE: If the file upload is in chunks and file has hue number of records then percentage progress display stucks to 99%. – this is because I have not kept this operation asynchronous to the original file upload tasks. If time permits, I can do this. 


How to run a project:
-	Open project in Visual studio
-	Make sure Weatherinformation project is set as default project and not test project.
-	Press F5 key.
- YOu can upload files from "json templates" folder from repository or you can create your own files.

