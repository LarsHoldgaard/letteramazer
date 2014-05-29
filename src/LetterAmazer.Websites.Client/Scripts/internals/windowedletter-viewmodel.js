function letter(filePath, imagePath, countryId) {
    var self = this;
    self.filePath = filePath;
    self.imagePath = imagePath;
    self.countryId = countryId;
    self.cost = ko.observable(0);
    self.numberOfPages = ko.observable(0);
    //self.uploadstatus('pending');
    self.updatePrices = function () {
        $.ajax({
            url: '/SingleLetter/GetPrice',
            type: 'POST',
            data: {
                'pdfUrl': self.filePath,
                'country': self.countryId
            },
            dataType: 'json',
            success: function (data) {
                self.cost(data.price);
                self.numberOfPages(data.numberOfPages);
                if (data.price.Total > 0) {
                    console.log('Setting uploadstatus to success');
                    //self.uploadstatus('success');
                } else {
                    console.log('Setting uploadstatus to failure. Price: ' + data.price.Total);
                    //self.uploadstatus('failure');
                }
            },
            error: function (file, responseText) {
                console.log('Price error');
            }
        });
    };

    self.thumbnail = function() {
        $.ajax({
            url: '/SingleLetter/GetThumbnail' + '?uploadFileKey=' + self.filePath,
            success: function(data) {
            
            },
            error: function (file, responseText) {
                console.log('Price error');
            }
        });
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
            ol += (ol.length > 0 ? ";" : "") + arr[i].filePath;
        }
        console.log('selectedFiles: ' + ol);
        return ol;
    });

    //self.getPriceTotal = function () {
    //    var price = 0.0;
    //    $(self.letters).each(function (index, ele) {
    //        price += ele.cost.Total.toFixed(2);
    //    });
    //    return price;
    //};

    //self.getPriceExVat = function () {
    //    var price = 0.0;
    //    $(self.letters).each(function (index, ele) {
    //        price += ele.cost.priceExVat.toFixed(2);
    //    });
    //    return price;
    //};

    //self.getUploadStatus = function () {
    //    var uploadStatus = true;
    //    $(self.letters).each(function (index, ele) {
    //        uploadStatus += ele.uploadstatus;
    //    });
    //    return uploadStatus;
    //};
};

