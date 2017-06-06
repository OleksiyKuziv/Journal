function deleteUserWithClass(value) {
    $.ajax({
        type: "Get",
        cache: false,
        url: "/Class/DeleteUserWithClass",
        data: { selectedUser: value },
        success: function (newListOfUser) {
            onSuccessUser(newListOfUser);
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
    tablebody.empty();
    for (var i = 0; i < newListOfUser.length; i++)
    {
        tablebody.append('<tr class="' + (i % 2 === 0 ? "" : "success  ") + '"> <td>' + newListOfUser[i].Text + '</td>' + '<td>' + '<button class="btn btn-default" type="button" onclick="deleteUserWithClass('+ "'" + newListOfUser[i].Value + "'" + ')">' + 'Delete' + '</button>' + '</td>'+'</tr>')
    }
}