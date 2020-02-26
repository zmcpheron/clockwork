//Run on page load
function Init() {
    LoadAvailableTimeZones();
    LoadAllEntries();
}

//Call to the API to get all timezones available on the server
function LoadAvailableTimeZones() {
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {    
            var responseObjArray = JSON.parse(this.response);  

            //For each time zone, create an option element in the select
            //The time zone where isLocal is true will be the default selection
            availableTimeZonesOptions = "";
            responseObjArray.forEach(function (obj) {
                if (obj.isLocal) {
                    availableTimeZonesOptions += '<option value="' + obj.id + '" selected="selected">' + obj.displayName + '</option> '
                }
                else {
                    availableTimeZonesOptions += '<option value="' + obj.id + '">' + obj.displayName + '</option> '
                }
            });

            document.getElementById("timeZoneSelect").innerHTML = availableTimeZonesOptions;
        }
    };
    xhttp.open("GET", "http://localhost:63362/api/currenttime/getAvailableTimeZones", true);
    xhttp.setRequestHeader("Content-type", "application/json");
    xhttp.send();

}

//Call to the API to get all requested times
function LoadAllEntries() {
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            var responseObjArray = JSON.parse(this.response);
            var allRequestedTimesListItems = "";

            //For each requested time, create a list item element
            responseObjArray.forEach(function (obj) {
                allRequestedTimesListItems += '<li class="list-group-item">' + GetTimeString(obj.time) + ' ' + obj.timeZoneId + '</li> ' 
            });

            //If there are no requests, let the user know
            if (allRequestedTimesListItems == "") {
                allRequestedTimesListItems = "No Requests";
            }

            document.getElementById("allTimeRequestsList").innerHTML = allRequestedTimesListItems;
        }
    };
    xhttp.open("GET", "http://localhost:63362/api/currenttime/getAllRequestEntries", true);
    xhttp.setRequestHeader("Content-type", "application/json");
    xhttp.send();

}

//Call to the server to get the current time in the selected time zone, store the request and display it
function UserAction() {
    var selectedTimeZoneId = document.getElementById("timeZoneSelect").value;
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            var currentTimeQuery = JSON.parse(this.response);
            document.getElementById("outputCurrentTimeRequest").innerHTML = '<strong>' + GetTimeString(currentTimeQuery.time) + '</strong><br>' + currentTimeQuery.timeZoneId;
            LoadAllEntries();
        }
    };
    xhttp.open("GET", "http://localhost:63362/api/currenttime/getCurrentTime?timeZoneId=" + selectedTimeZoneId, true);
    xhttp.setRequestHeader("Content-type", "application/json");
    xhttp.send();

}

// Parse date object iand return military time part as a string
function GetTimeString(date) {
    var dateObj = new Date(date);
    var timeString = dateObj.toTimeString();
    var lastColonIndex = timeString.lastIndexOf(':');
    var finalString = timeString.substring(0, lastColonIndex + 3);
    return finalString;
}

//Call the init function on load
window.onload = function () {
    Init();
}