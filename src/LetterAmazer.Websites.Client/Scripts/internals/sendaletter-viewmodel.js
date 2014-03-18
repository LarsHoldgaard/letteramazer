var SendALetterViewModel = function (formSelector, data) {
    var self = this;
    self.generatePDFUrl = data.generatePDFUrl;
    self.previewPDFUrl = data.previewPDFUrl;
    self.applyVoucherUrl = data.applyVoucherUrl;
    self.getPriceUrl = data.getPriceUrl;
    self.writeContentEditor = data.writeContentEditor;
    self.geocoder = data.geocoder;
    self.map = null;
    self.marker = null;
    self.geocodeInterval = null;
    self.voucherInterval = null;

    self.initMap = ko.observable(false);

    self.isAuthenticated = ko.observable(data.isAuthenticated);
    self.isValidCredits = ko.observable(false);
    self.paymentMethod = ko.observable("Paypal");

    self.currentStep = ko.observable(1);
    self.uploadPdf = ko.observable(0);
    self.useVoucher = ko.observable(0);
    self.showVatInclusive = ko.observable(true);

    self.orderid = ko.observable('');
    self.receiver = ko.observable('');
    self.address = ko.observable('');
    self.postal = ko.observable('');
    self.city = ko.observable('');
    self.state = ko.observable('');
    self.country = ko.observable('');
    self.countryCode = ko.observable('');
    self.countryId = ko.observable(0);

    self.lettersize = ko.observable(0);
    self.voucherCode = ko.observable('');
    self.voucherStatus = ko.observable('');
    self.voucherColor = ko.observable('color:green');
    self.cost = ko.observable(0);
    self.numberOfPages = ko.observable(0);
    self.countries = ko.observableArray(data.countryList);
    self.selectedCountry = ko.observable('');
    self.useUploadFile = ko.observable('');
    self.uploadFileKey = ko.observable('');
    self.priceStatus = ko.observable('');
    self.priceStatusMessage = ko.observable('');

    self.formSelector = formSelector;
    $(self.formSelector).validate({
        rules: {
            Email: {
                required: true,
                email: true
            },
            RecipientName: {
                required: true
            },
            countryCodes: {
                required: true
            },
            ZipCode: {
                required: true
            },
            DestinationCity: {
                required: true
            },
            DestinationAddress: {
                required: true
            },
            DestinationState: {
                required: false
            }
        }
    });

    self.showMissingUpload = ko.observable(false);
    self.showMissingWriteContent = ko.observable(false);

    self.goToStepOne = function () {
        self.currentStep(1);
    };
    self.goToStepTwo = function () {
        if ($(self.formSelector).valid()) self.currentStep(2);
    };
    self.goToStepThree = function () {
        if (self.uploadPdf() == 1 && self.uploadFileKey() == '') {
            self.showMissingUpload(true);
            return false;
        } else if (self.writeContentEditor.getData() == '') {
            self.showMissingWriteContent(true);
            return false;
        }

        var thiz = self;
        var writtenContent = encodeURIComponent(self.htmlEncode(self.writeContentEditor.getData()));
        console.log(self);
        $.ajax({
            url: self.getPriceUrl,
            type: 'POST',
            data: {
                usePdf: self.uploadPdf() == 1 ? 'true' : 'false',
                uploadFileKey: self.uploadFileKey(),
                content: writtenContent,
                address: self.address(),
                postal: self.postal(),
                city: self.city(),
                state: self.state(),
                country: self.countryId()
            },
            dataType: 'json',
            success: function (data) {
                thiz.priceStatus(data.status);
                thiz.priceStatusMessage(data.message);
                thiz.cost(data.price);
                thiz.numberOfPages(data.numberOfPages);
                if (data.isAuthenticated) {
                    self.isAuthenticated(true);
                    self.isValidCredits(data.isValidCredits);
                    self.paymentMethod('Credits (' + data.credits.toFixed(2) + '$ left)');
                }

                thiz.currentStep(3);

            }
        });
    };

    self.canSendLetter = ko.computed(function () {
        if (self.priceStatus() == "success") {
            return true;
        }
        return false;
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



    self.showStepOne = ko.computed(function () {
        return self.currentStep() == 1;
    });
    self.showStepTwo = ko.computed(function () {
        return self.currentStep() == 2;
    });
    self.showStepThree = ko.computed(function () {
        return self.currentStep() == 3;
    });

    self.showEnoughCredits = ko.computed(function () {
        return self.isAuthenticated() == true && self.isValidCredits() == true;
    });

    self.useUploadPdfMode = function () {
        self.uploadPdf(1);
    };
    self.useWriteContentMode = function () {
        self.uploadPdf(2);
    };
    self.isUploadFile = ko.computed(function () {
        return self.uploadPdf() == 1;
    });
    self.isWriteContent = ko.computed(function () {
        return self.uploadPdf() == 2;
    });
    self.showStepTwoAction = ko.computed(function () {
        return self.uploadPdf() == 1 || self.uploadPdf() == 2;
    });
    self.resetContentMode = function () {
        self.uploadPdf(0);
    };

    self.toggleVoucherPanel = function () {
        self.useVoucher(!self.useVoucher());
    };
    self.showVoucher = ko.computed(function () {
        return self.useVoucher();
    });

    self.showMap = ko.computed(function () {
        return self.currentStep() == 1 || self.currentStep() == 2;
    });

    self.loadMap = function () {
        var address = (self.address() ? self.address() + ' ' : '') + (self.postal() ? self.postal() + ' ' : '') + (self.city() ? self.city() + ' ' : '') + (self.country() ? self.country() : '');
        console.log('find address: ', address);

        var thiz = self;
        self.geocoder.geocode({ 'address': address }, function (results, status) {
            console.log('geocoder result: ', results, ', status: ', status);
            if (status == google.maps.GeocoderStatus.OK && results.length >= 1) {
                thiz.initializeMap(results[0].geometry.location);
            }
        });
    };
    self.selectCountry = function () {
        console.log('change country: ', self.selectedCountry());
        for (var i = 0; i < self.countries().length; i++) {
            if (self.countries()[i].value == self.selectedCountry()) {
                self.country(self.countries()[i].text);
                self.countryCode(self.countries()[i].value);
                self.loadMap();
                break;
            }
        }
    };

    self.previewWrittenContent = function () {
        var writtenContent = encodeURIComponent(self.htmlEncode(self.writeContentEditor.getData()));
        console.log('write content data: ', writtenContent);
        var url = self.generatePDFUrl + '?content=' + writtenContent;
        window.open(url);
    };

    self.previewPDF = function () {
        console.log("param: ", encodeURIComponent(self.uploadFileKey()));
        var url = self.previewPDFUrl + '?key=' + encodeURIComponent(self.uploadFileKey());
        window.open(url);
    };

    self.htmlEncode = function (value) {
        return $('<div/>').text(value).html();
    };

    self.initializeMap = function (position) {
        if (self.initMap() == true) {
            self.marker.setPosition(position);
            self.map.setCenter(position);
            return;
        }

        self.initMap(true);

        var mapOptions = {
            zoom: 15,
            center: position,
            mapTypeControlOptions: {
                mapTypeIds: [google.maps.MapTypeId.ROADMAP, google.maps.MapTypeId.HYBRID],
                position: google.maps.ControlPosition.LEFT_TOP
            },
            mapTypeId: google.maps.MapTypeId.ROADMAP,
            zoomControlOptions: {
                position: google.maps.ControlPosition.LEFT_TOP,
                style: google.maps.ZoomControlStyle.DEFAULT
            },
            streetViewControlOptions: {
                position: google.maps.ControlPosition.LEFT_TOP
            },
            panControlOptions: {
                position: google.maps.ControlPosition.LEFT_TOP
            }
        };

        self.map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);
        self.marker = new google.maps.Marker({
            position: position,
            map: self.map
        });
    };

    self.applyVoucher = function () {
        var thiz = self;
        $.ajax({
            url: self.applyVoucherUrl,
            type: 'POST',
            data: { code: self.voucherCode() },
            dataType: 'json',
            success: function (data) {
                if (data.couponValueLeft > 0) {
                    thiz.voucherColor('color:green');
                    var price = thiz.cost() - data.couponValueLeft;
                    if (price < 0) {
                        self.cost(0);
                    } else {
                        self.cost(price);
                    }
                    thiz.voucherStatus(data.couponValueLeft + ' $ left on voucher');
                } else {
                    thiz.voucherColor('color:red');
                    thiz.voucherStatus('No code');
                }
            },
            error: function () {
                thiz.voucherColor('color:red');
                thiz.voucherStatus('No code');
            }
        });
    };

    self.save = function (data, event) {
        $(event.target).prop("disabled", "disabled");

        if (self.uploadPdf() == 1) self.useUploadFile('true');
        else self.useUploadFile('false');

        var thiz = self;
        setTimeout(function () { $(thiz.formSelector).submit(); }, 1000);
    };


    $('input.address').bind('keyup', function () {
        var thiz = self;
        if (this.id == 'ZipCode') {
            self.postal(this.value);
        } else if (this.id == 'DestinationCity') {
            self.city(this.value);
        } else if (this.id == 'DestinationAddress') {
            self.address(this.value);
        }
        if (self.geocodeInterval) clearTimeout(self.geocodeInterval);
        self.geocodeInterval = setTimeout(function () { thiz.loadMap(); }, 300);
    });

    $('#voucherBox').bind('keyup', function () {
        var thiz = self;
        self.voucherCode(this.value);
        if (self.voucherInterval) clearTimeout(self.voucherInterval);
        self.voucherInterval = setTimeout(function () { thiz.applyVoucher(); }, 300);
    });
}