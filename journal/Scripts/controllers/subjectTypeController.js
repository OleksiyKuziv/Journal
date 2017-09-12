$(document).ready(function () {
    $('#search').click(function () {
        var school = $("#school").val();
        $.ajax({
            type: "GET",
            cache: false,
            url: "/SubjectType/Search",
            data: { school },
            success: function (data) {
                OnSuccess(data);
            }
        });
    });
});
function OnSuccess(data) {
    var tablebody = $('#tablebody');
    tablebody.empty();
    for (var i = 0; i < data.length; i++) {
        tablebody.append('<tr class="' + (i % 2 === 0 ? "success" : "") + '"><td>' + data[i].Name + '</td>' + '<td>' + data[i].Description + '</td>' + '<td>' + data[i].SelectedSchool + '</td>' + '<td>' + '<a href="/StudySubject/Edit/' + data[i].ID + '">' + 'Edit' + '</a>' + ' ' + '<a href="/StudySubject/Details/' + data[i].ID + '">' + 'Details' + '</a>' + ' ' + '<a href="/StudySubject/Delete/' + data[i].ID + '">' + 'Delete' + '</a>' + '</td>' + '</tr>')
    }
}