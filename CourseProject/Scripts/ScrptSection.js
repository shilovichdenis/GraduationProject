$('.single-item').slick({
    adaptiveHeight: false
});
$("#radio-student").change(function () {

    $("#label-student").removeClass('control-label-nonactive').addClass('control-label-active');
    $("#label-teacher").removeClass('control-label-active').addClass('control-label-nonactive');

    $("#input-formofeducation").removeClass('form-formofeducation-off').addClass('form-formofeducation-on');

    $("#form-cathedra").removeClass('input-cathedra-on').addClass('input-cathedra-off');
    $("#form-group").removeClass('input-group-off').addClass('input-group-on');
});
$("#radio-teacher").change(function () {

    $("#label-teacher").removeClass('control-label-nonactive').addClass('control-label-active');
    $("#label-student").removeClass('control-label-active').addClass('control-label-nonactive');

    $("#input-formofeducation").removeClass('form-formofeducation-on').addClass('form-formofeducation-off');

    $("#form-cathedra").removeClass('input-cathedra-off').addClass('input-cathedra-on');
    $("#form-group").removeClass('input-group-on').addClass('input-group-off');
});

$("#radio-male").change(function () {
    $("#label-male").removeClass('control-label-nonactive').addClass('control-label-active');
    $("#label-female").removeClass('control-label-active').addClass('control-label-nonactive');
});

$("#radio-female").change(function () {
    $("#label-female").removeClass('control-label-nonactive').addClass('control-label-active');
    $("#label-male").removeClass('control-label-active').addClass('control-label-nonactive');
});

$("#radio-paid").change(function () {
    $("#label-paid").removeClass('control-label-nonactive').addClass('control-label-active');
    $("#label-budgetary").removeClass('control-label-active').addClass('control-label-nonactive');
});

$("#radio-budgetary").change(function () {
    $("#label-budgetary").removeClass('control-label-nonactive').addClass('control-label-active');
    $("#label-paid").removeClass('control-label-active').addClass('control-label-nonactive');
});



$("#manage-btn").change(function () {
    $("#popup").removeClass('popup-off').addClass('popup-on');
});

$("#manage-btn").change(function () {
    $("#popup").removeClass('popup-on').addClass('popup-off');
});



$(function () {
    $.ajaxSetup({ cache: false });
    $(".create-item").click(function (e) {

        e.preventDefault();
        $.get(this.href, function (data) {
            $('#create-dialog').html(data);
            $('#create-form').modal('show');
        });
    });
})
$(function () {
    $.ajaxSetup({ cache: false });
    $(".rate-item").click(function (e) {

        e.preventDefault();
        $.get(this.href, function (data) {
            $('#rate-dialog').html(data);
            $('#rate-form').modal('show');
        });
    });
})
$(function () {
    $.ajaxSetup({ cache: false });
    $(".edit-item").click(function (e) {

        e.preventDefault();
        $.get(this.href, function (data) {
            $('#edit-dialog').html(data);
            $('#edit-form').modal('show');
        });
    });
})
$(function () {
    $.ajaxSetup({ cache: false });
    $(".info-item").click(function (e) {

        e.preventDefault();
        $.get(this.href, function (data) {
            $('#info-dialog').html(data);
            $('#info-form').modal('show');
        });
    });
})

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

$(document).ready(function () {
    $("#radio-personaldata").click(function () {
        $("#personaldata").show();
        $("#educationalmaterials").hide();
        $("#scientificwork").hide();
        $("#publications").hide();
    });
    $("#radio-educationalmaterials").click(function () {
        $("#personaldata").hide();
        $("#educationalmaterials").show();
        $("#scientificwork").hide();
        $("#publications").hide();
    });
    $("#radio-scientificwork").click(function () {
        $("#personaldata").hide();
        $("#educationalmaterials").hide();
        $("#scientificwork").show();
        $("#publications").hide();
    });
    $("#radio-publications").click(function () {
        $("#personaldata").hide();
        $("#educationalmaterials").hide();
        $("#scientificwork").hide();
        $("#publications").show();
    });
});
