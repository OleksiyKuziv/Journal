function deleteUserWithClass(value) {
    $.ajax({
        type: "Get",
        cache: false,
        url: "/Class/DeleteUserWithClass",
        data: { selectedUser: value },
        success: function (newList) {
            var newListOfPupil=[];
            var ListOfPupilWithoutClass=[];
            for (var i = 0; i < newList.length; i++)
            {
                if (newList[i].ValueClass) {
                    newListOfPupil.push(newList[i]);
                }
                else {
                    ListOfPupilWithoutClass.push(newList[i]);
                }
                }
            onSuccessUser(newListOfPupil);
            refreshAddUserDropdown(ListOfPupilWithoutClass);
        }
    });
}

function addPupilsToClass() {
    var selectedUser = $('#availableUsers').val();
    var classID = $('#classID').val();
    $.ajax({
        type: "Get",
        cache: false,
        url: "/Class/AddUserToClass",
        data: { selectedUser: selectedUser, classID: classID },
        success: function (newListOfUser) {
            onSuccessUser(newListOfUser);
        }
    });
    $("#availableUsers").find(":selected").remove();
}


function onSuccessUser(newListOfUser) {
    var tablebody = $('#tablebody');
    var counter = -1;
    tablebody.empty();
    for (var i = 0; i < newListOfUser.length; i++)
    {
        counter++;
        tablebody.append('<tr class="' + (counter % 2 === 0 ? "success" : "") + '"> <td>' + newListOfUser[i].Text + '</td>' + '<td>' + '<button class="btn btn-default" type="button" onclick="deleteUserWithClass(' + "'" + newListOfUser[i].Value + "'" + ')">' + 'Delete' + '</button>' + '</td>' + '</tr>')
    }
}

function refreshAddUserDropdown(newListOfUserWithoutClass)
{
    var availableUsers = $('#availableUsers');
    availableUsers.empty();
    availableUsers.append('<option></option>');
    for (var i = 0; i < newListOfUserWithoutClass.length; i++)
    {
        availableUsers.append('<option value="' + newListOfUserWithoutClass[i].Value + '">' + newListOfUserWithoutClass[i].Text + '</option>');
    }
}