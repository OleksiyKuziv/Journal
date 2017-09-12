
//$(function () { //This section will run whenever we call Chat.cshtml page

//    $("#divChat").hide();
//    $("#divLogin").show();

//    var objHub = $.connection.liveMessage;

//    loadClientMethods(objHub);

//    $.connection.hub.start().done(function () {

//        loadEvents(objHub);

//    });

//});


//function loadEvents(objHub) {

//    $("#btnLogin").click(function () {

//        var name = $("#txtUserName").val();
//        var pass = $("#txtPassword").val();

//        if (name.length > 0 && pass.length > 0) {
//            // <<<<<-- ***** Return to Server [  Connect  ] *****
//            objHub.server.connect(name, pass);

//        }
//        else {
//            alert("Please Insert UserName and Password");
//        }

//    });

//    $('#btnSendMessage').click(function () {

//        var msg = $("#txtMessage").val();

//        if (msg.length > 0) {

//            var userName = $('#hUserName').val();
//            // <<<<<-- ***** Return to Server [  SendMessageToGroup  ] *****
//            objHub.server.sendMessageToGroup(userName, msg);

//        }
//    });

//    $("#txtPassword").keypress(function (e) {
//        if (e.which == 13) {
//            $("#btnLogin").click();
//        }
//    });

//    $("#txtMessage").keypress(function (e) {
//        if (e.which == 13) {
//            $('#btnSendMessage').click();
//        }
//    });
//}

//function loadClientMethods(objHub) {

//    objHub.client.NoExistAdmin = function () {
//        var divNoExist = $('<div><p>There is no Admin to response you try again later</P></div>');
//        $("#divChat").hide();
//        $("#divLogin").show();

//        $(divNoExist).hide();
//        $('#divalarm').prepend(divNoExist);
//        $(divNoExist).fadeIn(900).delay(9000).fadeOut(900);
//    }

//    objHub.client.getMessages = function (userName, message) {

//        $("#txtMessage").val('');
//        $('#divMessage').append('<div><p>' + userName + ': ' + message + '</p></div>');

//        var height = $('#divMessage')[0].scrollHeight;
//        $('#divMessage').scrollTop(height);
//    }

//    objHub.client.onConnected = function (id, userName, UserID, userGroup) {

//        var strWelcome = 'Welcome' + +userName;
//        $('#welcome').append('<div><p>Welcome:' + userName + '</p></div>');

//        $('#hId').val(id);
//        $('#hUserId').val(UserID);
//        $('#hUserName').val(userName);
//        $('#hGroup').val(userGroup);

//        $("#divChat").show();
//        $("#divLogin").hide();
//    }
//}
   
    var customerTemplate = '<span class="k-state-default" style="background-image: url()"></span>' +
        '<span class="k-state-default marker" value="#: data.Value #"><h3>#: data.Text #</h3><p>#: data.ValueRole #</p></span>';
    var dataSource = new kendo.data.DataSource({
        transport: {
            read: {
                type: "get",
                dataType: "json",
                url: "/Message/UserList"
            }
        }
    });
    $("#userList").kendoListBox({
        dataTextField: "Text",
        dataValueField: "Value",
        template: customerTemplate,
        dataSource: dataSource,
        height: 250,
        change: onChange,
        width:300,
        connectWith: "messageView",
    });    
    $("#tabstrip").kendoTabStrip({
        tabPosition: "top",
        select:onSelect,
        animation: {
            close: {
                duration: 500,
                effects: "fadeOut"
            },
            open: {
                duration: 300,
                effects: "fadeIn"
            }
        }
    });   
    var tabToActivate = $(".Messanger");
    $("#tabstrip").kendoTabStrip().data("kendoTabStrip").activateTab(tabToActivate);


function onChange(e) {
    var data = dataSource.view(),
        selectedUserArray = $.map(this.select(), function (item) {
            return data[$(item).index()].Value;
        });
    for(var i=0;i<selectedUserArray.length;i++)
    {
        var selectedUser = selectedUserArray[i]
    var currentMessangeType = $('.k-state-active').attr("id");
    chatOpen(selectedUser, currentMessangeType);
    }
}
function onSelect(e) {
    var currentMessangeType = $(e.item).attr("id");
    var data = $("#userList").data('kendoListBox');
    var index = data.select().index(),
        selectedUser = data.dataSource.view()[index].Value;
    chatOpen(selectedUser, currentMessangeType);

}


function chatOpen(selectedUser, currentMessangeType) {
    if (selectedUser !== undefined && currentMessangeType !== undefined) {
        $.ajax({
            type: "GET",
            cache: false,
            url: "/Message/ChatInfo",
            data: { selectedUser, currentMessangeType },
            success: function (messageList) {
                var emailMessageList = [];
                var smsMessageList = [];
                var messangerMessageList = [];
                for (var i = 0; i < messageList.length; i++) {
                    if (messageList[i].currentMessageType === "Email") {
                        emailMessageList.push(messageList[i]);
                    }
                    else if (messageList[i].currentMessageType === "Sms") {
                        smsMessageList.push(messageList[i]);
                    }
                    else {
                        messangerMessageList.push(messageList[i]);
                    }                   
                }
                if (emailMessageList.length !== 0) {
                    emailHistory(emailMessageList);
                }
                if (smsMessageList.length !== 0) {
                    smsHistory(smsMessageList);
                }
                if (messangerMessageList.length!==0) {
                    messageHistory(messangerMessageList);
                }
            }
        });
    }
    else {

        debugger;}
}

function messangerSend() {
    debugger;
}
function smsSend() {
    debugger;
}
function emailSend() {
    var subject = $('#emailSubject').val();
    var text = $('#emailText').val();
    var nameToUser = $(".k-state-selected .marker").attr("value");
    var currentMessageType = $('.k-state-active').attr("id");
    $.ajax({
        cache:false,
        type: "Get",
        url: "/Message/AddEmailMessage",
        data: { nameToUser, currentMessageType, text, subject },
        success: function (messageList) {
            emailHistory(messageList);}
    });
    debugger;
}
function emailHistory(messageList) {
    debugger;

}
function smsHistory(smsMessageList) {
    debugger;
}
function messageHistory(messangerMessageList) {
    debugger;
}




