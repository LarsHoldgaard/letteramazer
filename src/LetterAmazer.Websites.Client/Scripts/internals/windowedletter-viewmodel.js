﻿var SendWindowedLetterViewModel = function (formSelector, data) {
    var self = this;
    self.getPriceUrl = data.getPriceUrl;
    self.thumbnailUrl = data.thumbnailUrl;

    self.countryId = ko.observable(0);
    self.uploadFileKey = ko.observable('');
    self.cost = ko.observable(0);
    self.numberOfPages = ko.observable(0);
    self.shippingtime = ko.observable('');
    self.thumbnailImagePath = ko.observable('');

    self.downloadPrices = function () {
        var thiz = self;
        $.ajax({
            url: self.getPriceUrl,
            type: 'POST',
            data: {
                usePdf: true,
                uploadFileKey: self.uploadFileKey(),
                country: self.countryId(),
                content: '',
                address: '',
                postal: '',
                city: '',
                state: ''
            },
            dataType: 'json',
            success: function (data) {
                thiz.cost(data.price);
                thiz.numberOfPages(data.numberOfPages);
            }
        });

        console.log('setting thumlnail:');
        self.thumbnailImagePath(self.thumbnailUrl + '?uploadFileKey=' + self.uploadFileKey());
        console.log(' thumlnail set:' + self.thumbnailImagePath());
    };

    //public JsonResult GetPrice(bool usePdf, string uploadFileKey, string content, string address, string postal, string city, string state,string country)

    self.getPriceTotal = ko.computed(function () {
        try {
            return self.cost().Total.toFixed(2);
        } catch (ex) {
            return 0;
        }
    });

    self.getPriceExVat = ko.computed(function () {
        try {
            return self.cost().PriceExVat.toFixed(2);
        } catch (ex) {
            return 0;
        }
    });


}

