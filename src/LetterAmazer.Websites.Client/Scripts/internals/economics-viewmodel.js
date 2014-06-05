﻿function invoice(orderid, invoiceDate, customerName, customerCountry, amount,pdfLink, status, countryId) {
    var self = this;

    self.pdfLink = pdfLink;
    self.print = ko.observable(false);
    self.customerName = customerName;
    self.customerCountry = customerCountry;
    self.status = status;
    self.invoiceDate = invoiceDate;
    self.jsonPrice = ko.observable(0);
    self.amount = amount;
    self.orderid = orderid;
    self.countryId = countryId;

    self.uploadStatus = ko.observable('');
    
    self.printingPrice = ko.computed(function () {
        var jsonPrice = self.jsonPrice();
        return new price(jsonPrice.PriceExVat,
            jsonPrice.VatPercentage);
    });


    self.updatePrice = function () {
        var printStatus = self.print();
        $.ajax({
            url: '/SingleLetter/GetPriceFromUrl',
            type: 'POST',
            data: {
                'pdfUrl': self.pdfLink,
                'country': self.countryId
            },
            dataType: 'json',
            success: function (data) {
                if (printStatus) {
                    self.print(1);
                } else {
                    self.print(0);
                }

                self.jsonPrice(data.price);
                self.uploadStatus('success');

            },
            error: function (file, responseText) {
                console.log('Price error');

            }
        });

    };
}


function price(exVat, vatPercentage) {
    var self = this;
    self.currency = '€';

    self.priceExVat = exVat;
    self.vatPercentage = vatPercentage;

    self.total = ko.computed(function () {
        return self.priceExVat * (1 + self.vatPercentage);
    });
    self.vatAmount = ko.computed(function () {
        return self.priceExVat * self.vatPercentage;
    });
    self.priceExVatText = ko.computed(function () {
        return self.priceExVat + ' ' + self.currency;
    });
}


var EconomicsViewModel = function (formSelector, data) {
    var self = this;
    self.dateFrom = data.dateFrom;
    self.dateTo = data.dateTo;
    self.countryId = ko.observable(0);

    self.paymentMethodId = ko.observable(0);
    self.invoices = ko.observableArray([]);

    $(data.invoiceData).each(function (index, ele) {
        console.log('economics countryid: ' + self.countryId());
        // todo: should make logic awesome... yes
        var inv = new invoice(ele[0].orderid,
            ele[0].invoiceDate,
            ele[0].customerName,
            ele[0].customerCountry,
            ele[0].amount,
            ele[0].pdfLink,
            ele[0].status,
            59);//self.countryId()
        self.invoices.push(inv);
    });

    self.totalPrice = ko.computed(function () {
        var exVat = 0.0;
        var vatPercentage = 0.0;

        $(self.invoices()).each(function (index, ele) {
            if (ele.print()) {
                exVat += ele.printingPrice().priceExVat;
                if (exVat > 0) {
                    vatPercentage = ele.printingPrice().vatPercentage;
                }
                
                console.log('Total price. ExVat: ' + ele.printingPrice().priceExVat + ' , vat: ' + ele.printingPrice().vatPercentage);
            }
        });
        return new price(exVat,vatPercentage);
    });

    self.invoiceCount = ko.computed(function () {
        var count = 0;
        $(self.invoices()).each(function (index, ele) {
            if (ele.print()) {
                count++;
            }
        });
        return count;
    });


    self.doneLoading = ko.computed(function () {
        var doneLoading = true;
        $(self.invoices()).each(function (index, ele) {
            console.log('invoice status: ' + ele.uploadStatus());
            if (ele.uploadStatus() === '') {
                doneLoading = false;
            }
        });
        return doneLoading;
    });

    self.updateAllPrices = function () {
        $(self.invoices()).each(function (index, ele)
        {
            if (ele.print()) {
                ele.updatePrice();
            }
            
        });
    };

    self.updateLetterCountry = function (countryId) {
        $(self.invoices()).each(function (index, ele) {
            console.log('setting invoice country to ' + countryId);
            ele.countryId = countryId;
        });
    };

    self.selectedPrintList = ko.computed(function () {
        var arr = self.invoices();
        
        var ol = "";
        for (var i = 0; i < arr.length; i++) {
            if (arr[i].print()) ol += (ol.length > 0 ? ";" : "") + arr[i].orderid;
        }

        console.log('selectedPrintList: ' + ol);
        return ol;
    });

};


