function letter(filePath, imagePath, countryId) {
    var self = this;
    self.filePath = filePath;
    self.imagePath = imagePath;
    self.countryId = countryId;
    self.numberOfPages = ko.observable(0);
    self.uploadStatus = ko.observable('');
    self.priceExVat = 0;//ko.observable(0);
    self.priceTotal = 0;//ko.observable(0);
    self.vatPercentage = ko.observable(0);

    self.updatePrice = function () {
        $.ajax({
            url: '/SingleLetter/GetPrice',
            type: 'POST',
            data: {
                'pdfUrl': self.filePath,
                'country': self.countryId
            },
            dataType: 'json',
            success: function (data) {
                if (data.status === 'error') {
                    console.log('Cannot send letter');
                }

                //self.priceExVat=data.price.PriceExVat;
                //self.priceTotal=data.price.Total;
                //self.vatPercentage=data.price.VatPercentage;
                //self.numberOfPages(data.numberOfPages);

                self.priceExVat = 1.5;
                self.priceTotal = 1.85;
                self.vatPercentage = 0.25;

                if (self.priceTotal > 0) {
                    console.log('Setting uploadstatus to success');
                    self.uploadStatus('success');
                } else {
                    console.log('Setting uploadstatus to failure. Price: ' + data.price.Total);
                    self.uploadStatus('failure');
                }
            },
            error: function (file, responseText) {
                console.log('Price error');
            }
        });
    };

    self.thumbnail = function() {
        var path = '/SingleLetter/GetThumbnail' + '?uploadFileKey=' + self.filePath;
        return path;
    };
}


var SendWindowedLetterViewModel = function(formSelector, data) {
    var self = this;

    self.uploadstatus = ko.observable(''); // pending, success or failure depending on the current status of the fileupload
    self.uploadmode = data.uploadMode; // merge og multiple, depending on if we should allow multiple uploads and send separate or merge the files into a single pdf
    self.countryId = ko.observable(0);
    self.originCountryId = ko.observable(0);


    self.letters = ko.observableArray([]);
    self.selectedFiles = ko.computed(function () {
        var arr = self.letters();

        var ol = "";
        for (var i = 0; i < arr.length; i++) {
            ol += (ol.length > 0 ? ";" : "") +
                arr[i].filePath;
        }
        console.log('selectedFiles: ' + ol);
        return ol;
    });

    self.updateAllPrices = function() {
        $(self.letters()).each(function(index, ele) {
            ele.updatePrice();
        });
    };

    self.getPriceTotal = function () {
        var price = 0.0;
        $(self.letters()).each(function (index, ele) {
            price += 1.0;
            //price += ele.priceTotal;
        });
        return price;
    };

    self.getPriceExVat = function () {
        var price = 0.0;
        $(self.letters()).each(function (index, ele) {
            price += 1.0;
            //price += ele.priceExVat;
        });
        return price;
    };

    //self.getUploadStatus = function () {
    //    var uploadStatus = true;
    //    $(self.letters).each(function (index, ele) {
    //        uploadStatus += ele.uploadstatus;
    //    });
    //    return uploadStatus;
    //};
};

