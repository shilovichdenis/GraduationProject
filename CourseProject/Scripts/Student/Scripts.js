$(document).ready(function () {
    $("#radio-personaldata").click(function () {
        $("#personaldata").show();
        $("#exams").hide();
        $("#tests").hide();
        $("#courseprojects").hide();
    });
    $("#radio-exams").click(function () {
        $("#personaldata").hide();
        $("#exams").show();
        $("#tests").hide();
        $("#courseprojects").hide();
    });
    $("#radio-tests").click(function () {
        $("#personaldata").hide();
        $("#exams").hide();
        $("#tests").show();
        $("#courseprojects").hide();
    });
    $("#radio-courseprojects").click(function () {
        $("#personaldata").hide();
        $("#exams").hide();
        $("#tests").hide();
        $("#courseprojects").show();
    });
});

var form = document.getElementById("sort-form");

document.getElementById("exam-name").addEventListener("click", function () {
    form.submit();
});

document.getElementById("exam-rating").addEventListener("click", function () {
    form.submit();
});