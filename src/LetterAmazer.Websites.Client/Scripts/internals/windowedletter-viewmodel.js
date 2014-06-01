function letter(filePath, imagePath, countryId, originCountryId) {
    var self = this;
    self.filePath = filePath;
    self.imagePath = imagePath;
    self.countryId = countryId;
    self.originCountry = originCountryId;
    self.numberOfPages = ko.observable(0);
    self.shippingDays = ko.observable(0);
    self.uploadStatus = ko.observable('');
    self.priceExVat = ko.observable(0);
    self.priceTotal = ko.observable(0);
    self.vatPercentage = ko.observable(0);

    self.updatePrice = function () {
        $.ajax({
            url: '/SingleLetter/GetPrice',
            type: 'POST',
            data: {
                'uploadFileKey': self.filePath,
                'country': self.countryId,
                'originCountry': self.originCountry
            },
            dataType: 'json',
            success: function (data) {
                self.numberOfPages(data.numberOfPages);
                self.shippingDays(data.shippingDays);
                self.priceExVat(data.price.PriceExVat);
                self.priceTotal(data.price.Total);
                self.vatPercentage(data.price.VatPercentage);

                if (self.priceTotal() > 0) {
                    self.uploadStatus('success');
                }

                if (data.status === 'error') {
                    console.log('Cannot send letter');
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
    self.userCredits = data.userCredits;
    self.creditPaymentMethodId = data.creditPaymentMethodId;
    self.countryId = ko.observable(0);
    self.paymentMethodId = ko.observable(0);
    self.originCountryId = ko.observable(0);


    self.letters = ko.observableArray([]);
    self.selectedFiles = ko.computed(function () {
        var arr = self.letters();

        var ol = "";
        for (var i = 0; i < arr.length; i++) {
            ol += (ol.length > 0 ? ";" : "") +
                arr[i].filePath;
        }
        return ol;
    });

    self.updateAllPrices = function () {
        $(self.letters()).each(function (index, ele) {
            ele.updatePrice();
        });
    };

    self.updateLetterCountry = function(countryId) {
        $(self.letters()).each(function (index, ele) {
            ele.countryId = countryId;
        });
    };
    self.updateOriginCountry = function (countryId) {
        $(self.letters()).each(function (index, ele) {
            ele.originCountry = countryId;
        });
    };

    
    self.isCreditsEnough = ko.computed(function () {
        if (self.paymentMethodId() == self.creditPaymentMethodId) {
            
            if (parseFloat(self.getPriceTotal()) > parseFloat(self.userCredits)) {
                console.log('Comparing ' + self.getPriceTotal() + ' with ' + self.userCredits + ' returned false');
                return false;
            }
        }
        return true;
    });

    self.getPriceTotal = function () {
        var price = 0.0;
        $(self.letters()).each(function (index, ele) {
            price += ele.priceTotal();
        });
        return parseFloat(price).toFixed(2);
    };

    self.getPriceExVat = function () {
        var price = 0.0;
        $(self.letters()).each(function (index, ele) {
                price += ele.priceExVat();
        });
        return parseFloat(price).toFixed(2);
    };

    self.shippingDays = ko.computed(function() {
        if (self.letters().length == 0) {
            return 0;
        }

        var longest = 999;
        $(self.letters()).each(function(index, ele) {
            var days = ele.shippingDays();
            if (longest > days) {
                longest = days;
            }
        });
        return longest;
    });

    self.status = ko.computed(function () {
        if (self.letters().length == 0) {
            return false;
        }

        var re = true;
        $(self.letters()).each(function (index, ele) {
            if (ele.uploadStatus() === 'error' || ele.uploadStatus() === 'failure') {
                re = false;
            }
        });
        return re;
    });

    self.doneLoading = ko.computed(function () {
        var doneLoading = true;
        $(self.letters()).each(function (index, ele) {
            if (ele.uploadStatus() === '') {
                doneLoading = false;
            }
        });
        return doneLoading;
    });

};

