$(document).ready(function () {
    $('#search').click(function () {
        var teacher = $("#teacher").val();
        var subject = $("#subject").val();        
        $.ajax({
            type: "GET",
            cache: false,
            url: "/Subject/Search",
            data: { teacher, subject },
            success: function (data) {
                OnSuccess(data);
            }
        });
    });
});
function OnSuccess(data) {
    var tablebody = $('#tablebody');
    tablebody.empty();
    for (var i = 0; i < data.length; i++)
    {
        tablebody.append('<tr class="' + (i % 2 === 0 ? "success" : "") + '"><td>' + data[i].SelectedTeacher + '</td>' + '<td>' + data[i].SelectedSubjectType + '</td>' + '<td>' + '<a href="/Subject/Edit/' + data[i].ID + '">' + 'Edit' + '</a>'+' ' + '<a href="/Subject/Details/' + data[i].ID + '">' + 'Details' + '</a>'+' '+ '<a href="/Subject/Delete/' + data[i].ID + '">' + 'Delete' + '</a>'+'</td>' +'</tr>')
    }
}
