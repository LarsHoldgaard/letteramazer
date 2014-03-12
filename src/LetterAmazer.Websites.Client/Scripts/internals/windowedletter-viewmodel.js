var SendWindowedLetterViewModel = function (formSelector, data) {
    var self = this;
    self.getPriceUrl = data.getPriceUrl;
    self.country = ko.observable('Denmark');
    self.uploadFileKey = ko.observable('');
    self.cost = ko.observable(0);
    self.numberOfPages = ko.observable(0);
    self.shippingtime = ko.observable('');


    self.downloadPrices = function () {
        var thiz = self;
        $.ajax({
            url: self.getPriceUrl,
            type: 'POST',
            data: {
                usePdf: true,
                uploadFileKey: self.uploadFileKey(),
                country: self.country(),
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

