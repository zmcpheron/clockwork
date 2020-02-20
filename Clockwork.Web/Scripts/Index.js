function Init() {
    LoadAllEntries();
}

function LoadAllEntries() {
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            var responseObjArray = JSON.parse(this.response);
            var allRequestedTimes = [];

            responseObjArray.forEach(function (obj) {
                allRequestedTimes.push(GetTimeString(obj.time));
            });

            document.getElementById("outputAllTimeRequests").innerHTML = allRequestedTimes.toString();
        }
    };
    xhttp.open("GET", "http://localhost:63362/api/currenttime/getAllRequestEntries", true);
    xhttp.setRequestHeader("Content-type", "application/json");
    xhttp.send();

}

//Call to the server to get the current time and display it
function UserAction() {
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            var currentTimeQuery = JSON.parse(this.response);
            document.getElementById("outputCurrentTimeRequest").innerHTML = GetTimeString(currentTimeQuery.time);
        }
    };
    xhttp.open("GET", "http://localhost:63362/api/currenttime/getCurrentTime", true);
    xhttp.setRequestHeader("Content-type", "application/json");
    xhttp.send();

}

// Parse json object and return time part as a string
function GetTimeString(date) {
    var dateObj = new Date(date);
    return dateObj.toTimeString();
}

window.onload = function () {
    Init();
}