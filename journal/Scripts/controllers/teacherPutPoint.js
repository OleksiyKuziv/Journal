var teacherRole = "2d7dd0aa-5d16-452c-8424-97f3fcf3823d";

function addPointToPupil()
{
    var subjectId = $('#subject').val();
    var pointLevel = $('#selectPointLevels').val();
    var users = $('.userRow').toArray();
    var userList = [];
    var chooseData = $('.chooseDatatime').val();
    users.forEach(function (e) {
        userList.push({
            user: e.id,
            pointValue: $(e).find(".k-button").val()
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
            pointLevel: pointLevel,
            chooseData: chooseData
        }),
        success: function (newdata) {
            debugger;
            if (newdata == 1)
            {
                alert('Current point is exist');
            }
            tableListWithoutPoints(newdata);
        },
       
    });    
}

function savePointToPupil (pointLevelid, dataEdit)
{
    var subjectId = $('#subject').val();
    var pointLevel = pointLevelid;
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
        url: "/TeacherPutPoint/SavePoint",
        contentType: 'application/json',
        data: JSON.stringify({
            userList: userList,
            subjectId: subjectId,
            pointLevel: pointLevel,
            dataEdit: dataEdit
        }),
        success: function (newdata) {
            tableListWithoutPoints(newdata);
        }
    });

}


function Search()
{
    var currentRole = $('.currentRole').text();
    var subjectid = $('#subject').val();
        $.ajax({
        type: "GET",
        cache: false,
        url: "/TeacherPutPoint/Search",
        data: { subjectid },
        success: function (newdata) {
            tableListWithoutPoints(newdata);
        }
    });
}

function tableListWithoutPoints(newdata)
{
    var currentRole = $('.currentRole').text();
    var dataEdit = newdata.DataEdit;
    var pointLevelEdit = newdata.PointLevelEdit;
    var user = newdata.Users;
    var pointValue = newdata.PointValues;
    var pointView = newdata.PointsView;
    var pointLevel = newdata.PointLevels;
    var line = [];
    var line1 = null;
    var lineCurrentPointLevel = "<tr><th> Ім'я Прізвище</th>" ;
    var lastCurrentPointLevel = null;
    var listPointLevel = [];
    var listOfUser = [];
    var tablebody = $('#tablebody');
    var currentPointLevel = $('.currentPointLevel');
    currentPointLevel.empty();
    tablebody.empty();
    if (user !== null) {
        if (currentRole == teacherRole) {
            lineCurrentPointLevel += '<th class="' + 'select">' + '<select class="form-control" id="' + 'selectPointLevels"' + 'required>' + '<option></option>';
            for (var zzz = 0; zzz < pointLevel.length; zzz++) {
                lineCurrentPointLevel += '<option value="' + pointLevel[zzz].Value + '">' + pointLevel[zzz].Text + '</option>';
            }
            lineCurrentPointLevel +='<br/><input type="date" class="chooseDatatime form-control" required /></th>';
        }
        currentPointLevel.append(lineCurrentPointLevel);        
        for (var i = 0; i < user.length; i++) {
            line1 = '<tr class="' + 'userRow" ' + 'id="' + user[i].Value + '"><td>' + user[i].Text + '</td>';
            if (currentRole == teacherRole) {
                line1 += '<td><select class="' + 'selectedPointValues form-control"' + '>' + '<option></option>';
                for (var item = 0; item < pointValue.length; item++) {
                    line1 += '<option value="' + pointValue[item].Value + '">' + pointValue[item].Text + '</option>';
                }
                line1 += '</select></td>';
            }
            line1 += '</tr>';
            tablebody.append(line1);
        }
        if (currentRole == teacherRole) {
            line1 = '<tr><td></td><td><div class="form-group"><div class="col-md-offset-2 col-sm-offset-3 col-md-10"><button class="btn btn-default" type="button" onclick="addPointToPupil()">Add</button></div></div></td></tr>';
            tablebody.append(line1);
        }
    }
    else
    {
        for (var ii = 0; ii < pointView.length; ii++) {
            var lastCurrentPointUser = listOfUser.indexOf(pointView[ii].UserID);
            if (lastCurrentPointUser == -1) {
                listOfUser.push(pointView[ii].UserID);
                line.push({ user: pointView[ii].UserID, text: '<tr class="' + 'userRow" ' + 'id="' + pointView[ii].UserID + '"><td>' + pointView[ii].SelectedUser + '</td>' });
            }
            lastCurrentPointLevel = searchInObject(pointView[ii].PointLevelID, pointView[ii].StrDate, listPointLevel);
            if (pointView[ii].StrDate == dataEdit && pointView[ii].PointLevelID == pointLevelEdit) {
                if (lastCurrentPointLevel == -1) {
                    listPointLevel.push({ pointLevel: pointView[ii].PointLevelID, data: pointView[ii].StrDate });
                    lineCurrentPointLevel += '<th><label>' + pointView[ii].SelectedPointLevel + '</label></th>';
                }
                addTextToUser(pointView[ii].UserID, '<td><select class="' + 'selectedPointValues form-control"><option></option>', line)
                for (jjj = 0; jjj < pointValue.length; jjj++) {
                    addTextToUser(pointView[ii].UserID, '<option value="' + pointValue[jjj].Value + '"' + (pointValue[jjj].Text === pointView[ii].SelectedPointValue ? 'selected' : "") + '>' + pointValue[jjj].Text + '</option>', line)
                }
                addTextToUser(pointView[ii].UserID, '</select></td>', line);
            }
            else if (lastCurrentPointLevel == -1) {
                listPointLevel.push({ pointLevel: pointView[ii].PointLevelID, data: pointView[ii].StrDate });
                lineCurrentPointLevel += '<th><label>' + pointView[ii].SelectedPointLevel + '</label></th>';
                if (pointView[ii].SelectedPointValue != null) {
                    addTextToUser(pointView[ii].UserID, '<td class="text-center">' + pointView[ii].SelectedPointValue + '</td>', line);
                }
                else {
                    addTextToUser(pointView[ii].UserID, '<td></td>', line);
                }
            }
            else {
                if (pointView[ii].SelectedPointValue != null) {
                    addTextToUser(pointView[ii].UserID, '<td class="text-center">' + pointView[ii].SelectedPointValue + '</td>', line);
                }
                else {
                    addTextToUser(pointView[ii].UserID, '<td></td>', line);
                }
            }
        }
        if (currentRole == teacherRole) {
            line.push({ user: 'edit', text: '<tr><td></td>' })
            for (var m = 0; m < listPointLevel.length; m++) {
                if (listPointLevel[m].data == dataEdit && listPointLevel[m].pointLevel == pointLevelEdit) {
                    addTextToUser('edit', '<td><div class="form-group"><div class="col-md-offset-2 col-sm-offset-3 col-md-10"><button class="btn btn-default" type="button" onclick="savePointToPupil(' + "'" + listPointLevel[m].pointLevel + "'" + "," + "'" + listPointLevel[m].data + "'" + ')">Save</button></div></div></td>', line)
                }
                else {
                    addTextToUser('edit', '<td><div class="form-group"><div class="col-md-offset-2 col-md-10"><button class="btn btn-default" type="button" onclick="editPoint(' + "'" + listPointLevel[m].pointLevel + "'" + "," + "'" + listPointLevel[m].data + "'" + ')">Edit</button></div></div></td>', line);
                }
            }
        }
        for (var j = 0; j < line.length; j++) {
            if (j != line.length - 1 && !dataEdit && currentRole == teacherRole) {
                addTextToUser(line[j].user, '<td><select class="' + 'selectedPointValues form-control"' + '>' + '<option></option>', line);
                for (jj = 0; jj < pointValue.length; jj++) {
                    addTextToUser(line[j].user, '<option value="' + pointValue[jj].Value + '">' + pointValue[jj].Text + '</option>', line)
                }
                addTextToUser(line[j].user, '</select></td></tr>', line);
            }
            else if (!dataEdit && currentRole == teacherRole) {
                addTextToUser('edit', '<td><div class="form-group"><div class="col-md-offset-2 col-sm-offset-3 col-md-10"><button class="btn btn-default" type="button" onclick="addPointToPupil()">Add</button></div></div></td>', line)
            }            
                tablebody.append(line[j].text);
            
        }
        if (!dataEdit && currentRole == teacherRole) {
            lineCurrentPointLevel += '<th class="' + 'select">' + '<select class="form-control" id="' + 'selectPointLevels"' + 'required>' + '<option></option>';
            for (var zzz = 0; zzz < pointLevel.length; zzz++) {
                lineCurrentPointLevel += '<option value="' + pointLevel[zzz].Value + '">' + pointLevel[zzz].Text + '</option>';
            }
            lineCurrentPointLevel += '<input type="date" class="chooseDatatime form-control" required /></th></tr>';
        }
            currentPointLevel.append(lineCurrentPointLevel);
        
    }

}


function searchInObject(pointLevel, data, myArray)
{
    for (var i = 0; i < myArray.length; i++)
    {
        if (myArray[i].pointLevel == pointLevel && myArray[i].data == data) {
            return myArray[i];
        }       
    }
    return -1;   
}
function addTextToUser(userId, text, myArray)
{
    for (var i = 0; i < myArray.length; i++)
    {
        if (myArray[i].user == userId)
        {
            myArray[i].text += text;
            return myArray[i];
        }
    }
    return -1;
}
function editPoint(pointLevelid, dataEdit)
{
    var subjectid = $('#subject').val();
    $.ajax({
        type: "GET",
        cache: false,
        url: "/TeacherPutPoint/Edit",
        data: { subjectid, pointLevelid, dataEdit },
        success: function (newdata) {
            tableListWithoutPoints(newdata);
        },
        error: function (xhr, status, error)
        {
            var err = eval("(" + xhr.responseText + ")");
            alert(err.Message);
        }
    });    
}