
$('#uploadFile').change(function () {
    var data = new FormData($('#uploadForm')[0]);
    $("#selectImg").prop("disabled", true);
    $.ajax({
        data: data,
        enctype: 'multipart/form-data',
        processData: false,
        contentType: false,
        cache: false,
        url: '/UploadFiles',
        type: "POST",
        success: function (resp) {
            console.log(resp);
            $('#PictureUrl').val(resp.relativePath);
            $("#selectImg").prop("disabled", false);
        },
        error: function (err) {
            $("#selectImg").prop("disabled", false);
            alert("Error");
        }
    });
});