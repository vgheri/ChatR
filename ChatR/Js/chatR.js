var chatR = {};

// Models
chatR.chatMessage = function (sender, content, dateSent) {
    var self = this;
    self.username = sender;
    self.content = content;
    if (dateSent != null) {
        self.timestamp = dateSent;
    }
}

chatR.user = function (username) {
    var self = this;
    self.username = username;
}

// ViewModels

chatR.chatViewModel = function () {
    var self = this;
    self.messages = ko.observableArray();
}

chatR.connectedUsersViewModel = function () {
    var self = this;
    self.contacts = ko.observableArray();
    self.customRemove = function (userToRemove) {
        var usernameToRemove = userToRemove.username;
        self.contacts.remove(function (item) {
            return item.username === usernameToRemove;
        });
    }
}

