var chatR = {};

// Models
chatR.ChatMessage = function (sender, content, dateSent) {
    var self = this;
    self.Username = sender;
    self.Message = content;
    if (dateSent != null) {
        self.Timestamp = dateSent;
    }
}

chatR.User = function (username) {
    var self = this;
    self.username = username;
}

// ViewModels

chatR.ChatViewModel = function () {
    var self = this;
    self.messages = ko.observableArray();
}

chatR.ConnectedUsersViewModel = function () {
    var self = this;
    self.contacts = ko.observableArray();
    self.CustomRemove = function (userToRemove) {
        var usernameToRemove = userToRemove.username;
        self.contacts.remove(function (item) {
            return item.username === usernameToRemove;
        });
    }
}

