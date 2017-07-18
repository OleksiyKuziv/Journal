function addPointToPupil()
{
    var subjectId = $('#subject').val();
    var pointLevel = $('#selectPointLevels').val();
    var users = $('.userRow').toArray();
    var userList = [];
    users.forEach(function (e) {
        userList.push({
            user: e.id,
            pointValue: $(e).find(".selectedPointValues").val()
        });
    });
    $.ajax({
        type: "POST",
        cache: false,
        url: "/TeacherPutPoint/AddPoint",
        contentType: 'application/json',
        data: JSON.stringify({
            userList: userList,
            subjectId: subjectId,
            pointLevel: pointLevel
        }),
        success: function (data) {
           
        }
    });
    debugger;
}

function Search()
{
    var subjectid = $('#subject').val();
        $.ajax({
        type: "GET",
        cache: false,
        url: "/TeacherPutPoint/Search",
        data: { subjectid },
        success: function (data) {
            tableList(data);
        }
    });
}

function tableList(data)
{
    var user = data.Users;
    var pointValue = data.PointValues;
    var string = null;
    var tablebody = $('#tablebody');
    tablebody.empty();
    for (var i = 0; i < user.length; i++)
    {
        string='<tr class="' + 'userRow" ' + 'id="' + user[i].Value + '"><td>' + user[i].Text + '</td>' + '<td><select class="' + 'selectedPointValues"' + '>' + '<option></option>';
      
        for (var item = 0; item < pointValue.length; item++)
        {
            string += '<option value="' + pointValue[item].Value + '">' + pointValue[item].Text + '</option>';
        }
        string += '</select></td></tr>';
        tablebody.append(string);
    }

}