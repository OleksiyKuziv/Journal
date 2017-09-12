function schoolChange() {
    var selectedSchool = $("#selectedSchool").val();
    $.ajax({
        type: "GET",
        cache: false,
        url: "/Account/SearchClass",
        data: { selectedSchool:selectedSchool },
        success: function (selectedClassList) {
            onSuccessClassList(selectedClassList);
        }
    });
}
function onSuccessClassList(selectedClassList) {
    var selectedClass = $('#selectedClass');    
    selectedClass.empty();
    selectedClass.append('<option></option>');
    for (var i = 0; i < selectedClassList.length; i++)
    {
        selectedClass.append('<option value="' + selectedClassList[i].ID + '">' + selectedClassList[i].Name+'</option>')

    }
}