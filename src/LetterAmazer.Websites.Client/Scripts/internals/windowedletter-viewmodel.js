var SendWindowedLetterViewModel = function (formSelector, data) {
    var self = this;
    self.uploadstatus = ko.observable('');
    self.getPriceUrl = data.getPriceUrl;
    self.thumbnailUrl = data.thumbnailUrl;

    self.countryId = ko.observable(0);
    self.originCountryId = ko.observable(0);

    self.uploadFileKey = ko.observable('');
    self.cost = ko.observable(0);
    self.numberOfPages = ko.observable(0);
    self.shippingtime = ko.observable('');
    self.thumbnailImagePath = ko.observable('');

    self.uploadstatus('pending');

    self.downloadPrices = function () {
        console.log('Download prices is called');
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
                state: '',
                letterType: 0
            },
            dataType: 'json',
            success: function (data) {
                thiz.cost(data.price);
                thiz.numberOfPages(data.numberOfPages);
                if (data.price.Total > 0) {
                    console.log('Setting uploadstatus to success');
                    self.uploadstatus('success');
                } else {
                    console.log('Setting uploadstatus to failure. Price: ' + data.price.Total);
                    self.uploadstatus('failure');
                }

            },
            error: function (file, responseText) {
                console.log('Price error');
                
            }
        });

        console.log('setting thumlnail:');
        self.thumbnailImagePath(self.thumbnailUrl + '?uploadFileKey=' + self.uploadFileKey());
        console.log(' thumlnail set:' + self.thumbnailImagePath());

        console.log('Upload status: ' + self.uploadstatus());
    };

    self.getUploadStatus = ko.computed(function() {
        return self.uploadstatus();
    });

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

