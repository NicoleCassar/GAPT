$(document).ready(function () {
    
    $("#pref1").change(function () {
        var SelecedVal = $(this).val();
        $("#area1").html('');
        $("#area1").append($("<option></option>").attr("value", '')
            .text('Select Area'));
        if (SelecedVal != '') {

            $.get('/Forms/GetArea/',
                {
                    "supervisor_id": SelecedVal
                }).done(function (data) {
                    $.each(data, function (index, item) {
                        $("#area1").append($("<option></option>").attr("value", item.supervisor_id)
                            .text(item.title));
                    });

                })
        }
    });
    $("#pref2").change(function () {
        var SelecedVal = $(this).val();
        $("#area2").html('');
        $("#area2").append($("<option></option>").attr("value", '')
            .text('Select Area'));
        if (SelecedVal != '') {

            $.get('/Forms/GetArea/',
                {
                    "supervisor_id": SelecedVal
                }).done(function (data) {
                    $.each(data, function (index, item) {
                        $("#area2").append($("<option></option>").attr("value", item.supervisor_id)
                            .text(item.title));
                    });

                })
        }
    });
    $("#pref3").change(function () {
        var SelecedVal = $(this).val();
        $("#area3").html('');
        $("#area3").append($("<option></option>").attr("value", '')
            .text('Select Area'));
        if (SelecedVal != '') {

            $.get('/Forms/GetArea/',
                {
                    "supervisor_id": SelecedVal
                }).done(function (data) {
                    $.each(data, function (index, item) {
                        $("#area3").append($("<option></option>").attr("value", item.supervisor_id)
                            .text(item.title));
                    });

                })
        }
    });
    $("#pref4").change(function () {
        var SelecedVal = $(this).val();
        $("#area4").html('');
        $("#area4").append($("<option></option>").attr("value", '')
            .text('Select Area'));
        if (SelecedVal != '') {

            $.get('/Forms/GetArea/',
                {
                    "supervisor_id": SelecedVal
                }).done(function (data) {
                    $.each(data, function (index, item) {
                        $("#area4").append($("<option></option>").attr("value", item.supervisor_id)
                            .text(item.title));
                    });

                })
        }
    });
    $("#pref5").change(function () {
        var SelecedVal = $(this).val();
        $("#area5").html('');
        $("#area5").append($("<option></option>").attr("value", '')
            .text('Select Area'));
        if (SelecedVal != '') {

            $.get('/Forms/GetArea/',
                {
                    "supervisor_id": SelecedVal
                }).done(function (data) {
                    $.each(data, function (index, item) {
                        $("#area5").append($("<option></option>").attr("value", item.supervisor_id)
                            .text(item.title));
                    });

                })
        }
    });
    $("#pref6").change(function () {
        var SelecedVal = $(this).val();
        $("#area6").html('');
        $("#area6").append($("<option></option>").attr("value", '')
            .text('Select Area'));
        if (SelecedVal != '') {

            $.get('/Forms/GetArea/',
                {
                    "supervisor_id": SelecedVal
                }).done(function (data) {
                    $.each(data, function (index, item) {
                        $("#area6").append($("<option></option>").attr("value", item.supervisor_id)
                            .text(item.title));
                    });

                })
        }
    });
})