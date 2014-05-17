function invoice(invoiceDate, customerName, pdfLink, status) {
    var self = this;
    self.pdfLink = pdfLink;
    self.print = ko.observable(false);
    self.customerName = customerName;
    self.status = status;
    self.invoiceDate = invoiceDate;
    self.jsonPrice = ko.observable(0);


    self.pdfPagesCount = function () {
        return 1;
    };

    self.price = ko.computed(function () {
        return new price(self.jsonPrice().PriceExVat,
            self.jsonPrice().VatPercentage);
    });

    
    self.updatePrices = function () {
        $.ajax({
            url: '/SingleLetter/GetPriceFromUrl',
            type: 'POST',
            data: {
                pdfUrl: self.pdfLink,
            },
            dataType: 'json',
            success: function (data) {
                self.jsonPrice(data.price);
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

    console.log('Price ex vat: ' + exVat + ' , vatPercentage: ' + vatPercentage);

    self.total = ko.computed(function () {
        console.log('Vat percentage is: ' + self.vatPercentage);
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
    

    self.invoices = ko.observableArray([
        new invoice('05-05-2014', 'LetterAmazer IvS', "http://www.antennahouse.com/XSLsample/pdf/sample-link_1.pdf", "not printed"),
        new invoice('05-05-2014', 'LetterAmazer IvS', "http://www.antennahouse.com/XSLsample/pdf/sample-link_1.pdf", "not printed")
    ]);

    self.totalPrice = ko.computed(function () {
        var exVat = 0.0;
        var total = 0.0;

        $(self.invoices()).each(function (index, ele) {
            if (ele.print()) {
                exVat += ele.price().priceExVat;
                total += ele.price().total();
            }
        });

        return new price(exVat, total);
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
};


