$(document).ready(function () {
    
    $("#sup1").change(function () {
        var SelectedVal = $(this).val();
        $("#area1").html('');
        $("#area1").append($("<option></option>").attr("value", '')
            .text('Select Area'));
        if (SelectedVal != '') {

            $.get('/Forms/GetArea/',
                {
                    "supervisor_id": SelectedVal
                }).done(function (data) {
                    $.each(data, function (index, item) {
                        $("#area1").append($("<option></option>").attr("value", item.title)
                            .text(item.title));
                    });

                })
        }
    });
    $("#sup2").change(function () {
        var SelectedVal = $(this).val();
        $("#area2").html('');
        $("#area2").append($("<option></option>").attr("value", '')
            .text('Select Area'));
        if (SelectedVal != '') {

            $.get('/Forms/GetArea/',
                {
                    "supervisor_id": SelectedVal
                }).done(function (data) {
                    $.each(data, function (index, item) {
                        $("#area2").append($("<option></option>").attr("value", item.title)
                            .text(item.title));
                    });

                })
        }
    });
    $("#sup3").change(function () {
        var SelectedVal = $(this).val();
        $("#area3").html('');
        $("#area3").append($("<option></option>").attr("value", '')
            .text('Select Area'));
        if (SelectedVal != '') {

            $.get('/Forms/GetArea/',
                {
                    "supervisor_id": SelectedVal
                }).done(function (data) {
                    $.each(data, function (index, item) {
                        $("#area3").append($("<option></option>").attr("value", item.title)
                            .text(item.title));
                    });

                })
        }
    });
    $("#sup4").change(function () {
        var SelectedVal = $(this).val();
        $("#area4").html('');
        $("#area4").append($("<option></option>").attr("value", '')
            .text('Select Area'));
        if (SelectedVal != '') {

            $.get('/Forms/GetArea/',
                {
                    "supervisor_id": SelectedVal
                }).done(function (data) {
                    $.each(data, function (index, item) {
                        $("#area4").append($("<option></option>").attr("value", item.title)
                            .text(item.title));
                    });

                })
        }
    });
    $("#sup5").change(function () {
        var SelectedVal = $(this).val();
        $("#area5").html('');
        $("#area5").append($("<option></option>").attr("value", '')
            .text('Select Area'));
        if (SelectedVal != '') {

            $.get('/Forms/GetArea/',
                {
                    "supervisor_id": SelectedVal
                }).done(function (data) {
                    $.each(data, function (index, item) {
                        $("#area5").append($("<option></option>").attr("value", item.title)
                            .text(item.title));
                    });

                })
        }
    });
    $("#sup6").change(function () {
        var SelectedVal = $(this).val();
        $("#area6").html('');
        $("#area6").append($("<option></option>").attr("value", '')
            .text('Select Area'));
        if (SelectedVal != '') {

            $.get('/Forms/GetArea/',
                {
                    "supervisor_id": SelectedVal
                }).done(function (data) {
                    $.each(data, function (index, item) {
                        $("#area6").append($("<option></option>").attr("value", item.title)
                            .text(item.title));
                    });

                })
        }
    });
})