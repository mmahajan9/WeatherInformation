var constants = {
    //api.openweathermap.org/data/2.5/forecast?id=524901&APPID=
    //http://api.openweathermap.org/data/2.5/weather?id=524901&APPID=aa69195559bd4f88d79f9aadeb77a8f6
    //https://api.openweathermap.org/data/2.5/group?id=524901,519188&units=metric&appid=aa69195559bd4f88d79f9aadeb77a8f6
    SingleCity_WeatherAPIAddress: 'http://api.openweathermap.org/data/2.5/weather',
    MultitCity_weatherAPIAddress: 'https://api.openweathermap.org/data/2.5/group',
    weatherAPIAppId: 'aa69195559bd4f88d79f9aadeb77a8f6',
    fileUploadURL: '/Home/UploadFile'
}
var home = (function () {
    var uploadedCityIds = [];
    var JsonContent = {};
    JsonContent.json = [];
    JsonContent.isValid = false;
    var WeatherInformationData = {
        pageSize: 20,
        index: 0,
        totalIndex: 0,
        lstCityIds: ""
    }
    //var _uploadMessage = "";
    var uploadMessage = {
        get Message() { return _uploadMessage; },
        set Message(msg) {
            _uploadMessage = msg;
            $("._uploadProgress").text(msg);
        }
    };
    var getFileExtension = function (fileName) {
        var arrName = fileName.split('.');
        return arrName[arrName.length - 1];
    };
    var validateFile = function (elm) {
        var isValid = false;
        if (elm.files.length > 1) {
            // display the message in the list
            isValid = false;
        }
        var extension = getFileExtension(elm.files[0].name)
        if (extension == 'json') {
            isValid = true;
        }
        //if (isValid)
        //    readFileContent(elm, shouldUpload);
        //else
        //    _uploadMessage.Message = "Invalid file type.";
        return isValid;
    };
    
    var readJsonFile = function (file) {
        var arrIds = [];
    };
    var readExcelFile = function (file) {

    };
    var uploadInChunks = function (file) {
        var extension = getFileExtension(file.name)
        //max file chunk size set to 100 KB change as per requirement.
        var uploadSizeinKB = 250;

        var fileChunks = [];
        var chunkSizeInBytes = uploadSizeinKB * 1024;

        var currentStreamPosition = 0;
        var endPosition = chunkSizeInBytes;
        var size = file.size;

        while (currentStreamPosition < size) {
            fileChunks.push(file.slice(currentStreamPosition, endPosition));
            currentStreamPosition = endPosition;
            endPosition = currentStreamPosition + chunkSizeInBytes;
        }

        //Append random number to file name to make it unique
        var fileName = (new Date()).getTime() + "__" + file.name;
        uploadChunks(fileChunks, fileName, 1, fileChunks.length);

    };
    var getCompletionPercentage = function (_currentIteration, _totalIterations) {
        return Math.round(100 - (((_totalIterations - _currentIteration) / _totalIterations) * 100));
    };
    var uploadChunks = function (fileChunks, fileName, currentIteration, totalIterations) {
        var _data = new FormData();
        _data.append(fileName, fileChunks[currentIteration - 1]);
        var _url = "";
        if (currentIteration == totalIterations)
            _url = constants.fileUploadURL + "?isFinalChunk=true";
        else
            _url = constants.fileUploadURL + "?isFinalChunk=false";
        $.ajax({
            url: _url,
            type: 'POST',
            contentType: false,
            processData: false,
            data: _data,
            async: true,
            success: function (result) {
                uploadMessage.Message = "File Upload status - " + getCompletionPercentage(currentIteration, totalIterations) + "%";
                if (currentIteration < totalIterations) {
                    uploadChunks(fileChunks, fileName, ++currentIteration, totalIterations);
                }
                else {
                    uploadMessage.Message = "File Upload is completed succesfully";
                }
            },
            error: function (errResult) {
                uploadMessage.Message = "Error occurred in uploading";
            }
        });
    };
    var isValidJSON = function (str) {
        if (typeof (str) !== 'string') {
            return false;
        }
        try {
            JSON.parse(str);
            return true;
        }
        catch (e) {
            console.log(e.responseText)
            return false;
        }
    };
    var readFileContent = function (elm, callback) {
        var file = elm.files[0];
        if (file) {
            var reader = new FileReader();
            reader.readAsText(file, "UTF-8");
            reader.onload = function (evt) {
                if (!isValidJSON(evt.currentTarget.result)) {
                    uploadMessage.Message = "Invalid JSON content. Cannot upload a file.";
                    return false;
                }
                JsonContent.json = JSON.parse(evt.currentTarget.result);
                callback(elm);
            }
            reader.onerror = function (evt) {
                uploadMessage.Message = "Cannot read file content";
            }
        }
    };
    var getWeatherInformationForCities = function (_jsonContent, pageIndex, pageSize=20) {
        debugger;
        var lstCityIds = [];
        $.each(JsonContent.json, function (index, node) {
            if (lstCityIds.length >= 20)
                return false;
            lstCityIds.push(node.id);
        });
        var WeatherInformationData = {
            CityIds: lstCityIds.join(','),
            pageSize: 20,
            index: pageIndex
        }
        debugger;
        $.ajax({
            //url: '/Home/GetWeatherInformationForCities',
            url: constants.MultitCity_weatherAPIAddress + "?id=" + lstCityIds.join(',') + '&APPID=' + constants.weatherAPIAppId,
            type: 'GET',
            data: WeatherInformationData,
            dataType: 'JSON',
            async: true,
            success: function (result) {
                debugger;
                var html = '';
                var tableIndex = (pageIndex * pageSize);
                $.each(result.list, function (index, val) {
                    html += '<tr>';
                    html += '<td>' + ++index + '</td>';
                    html += '<td>' + val.id + '</td>';
                    html += '<td>' + val.name + '</td>';
                    html += '<td>' + val.main.temp + '</td>';
                    html += '<td>' + val.main.pressure + '</td>';
                    html += '<td>' + val.main.humidity + '</td>';
                    html += '<td>' + val.main.temp_min + '</td>';
                    html += '<td>' + val.main.temp_max + '</td>';
                    html += '</tr>';
                    //tableIndex+;
                });
                
                $("._weatherGrid tbody").html(html);
                $('._weatherGrid').removeClass('hide');
            },
            error: function (result) {
                debugger;
                alert('failure');
            }
        });
    };
    var uploadFile = function (elm) {
        uploadMessage.Message = "File upload started";
        $("._weatherGrid tbody").html('');
        //$("._weatherGrid").addClass('hide');
        // Verify the extesnion.
        var isValid = validateFile(elm);
        // read file content to verify the json and get the City Ids
        if (isValid)
            readFileContent(elm, processFileUploading);
        else
            uploadMessage.Message = "Invalid file type.";
        $("#fileUpload").html('');
    };
    var processFileUploading = function (elm) {
        
            // Uload a file in chunks
            uploadInChunks(elm.files[0]);
            // get the for first 20 cities.
        getWeatherInformationForCities(JsonContent.json);
        //if (validateFile(elm)) {
            //var arrIds = [];
            //var extension = getFileExtension(elm.files[0].name);
            //if (extension == 'json') {
            //    arrIds = readJsonFile(elm.files[0].file);
            //}
            //if (extension == 'xlsx' || extension == 'xls') {
            //    arrIds = readExcelFile(elm.files[0].file);
            //}
            //uploadedCityIds = arrIds;

            
            //// uncomment following code if we want to upload this file on blob
            //if (arrIds.length > 20)
            //    uploadInChunks(file, ext);
            //else {
            //    // upload in once
            //}
        //}
    };
    return {
        UploadFile: uploadFile,
        ValidateFileAndUpload: validateFile,
        UploadedCityIds: uploadedCityIds
    }
})();