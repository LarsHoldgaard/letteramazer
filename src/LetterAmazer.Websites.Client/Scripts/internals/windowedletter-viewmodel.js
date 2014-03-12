var SendWindowedLetterViewModel = function (formSelector, data) {
    var self = this;
    self.countryid = ko.observable('');
    self.uploadFileKey = ko.observable('');
    self.price = ko.observable(0);
    self.shippingtime = ko.observable('');
}