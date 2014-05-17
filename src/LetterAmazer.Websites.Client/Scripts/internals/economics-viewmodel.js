function invoice(invoiceDate, customerName, pdfLink, status) {
    var self = this;
    self.pdfLink = pdfLink;
    self.print = ko.observable(false);
    self.customerName = customerName;
    self.status = status;
    self.invoiceDate = invoiceDate;

    self.pdfPagesCount = function () {
        return 1;
    };

    self.price = ko.computed(function () {
        return new price(1.8, 2.1);
    });
}


function price(exVat, total) {
    var self = this;
    self.currency = '€';
    self.total = total;
    self.priceExVat = exVat;
    self.vatPercentage = 0.25;
    self.vatAmount = self.exVat - self.total;

    self.priceExVatText = ko.computed(function () {
        return self.priceExVat + ' ' + self.currency;
    });
}


var EconomicsViewModel = function (formSelector, data) {
    var self = this;
    self.dateFrom = data.dateFrom;
    self.dateTo = data.dateTo;


    self.invoices = ko.observableArray([
        new invoice('05-05-2014', 'LetterAmazer IvS', "http://www.google.com", "not printed"),
        new invoice('05-05-2014', 'LetterAmazer IvS', "http://www.google.com", "not printed")
    ]);

    self.totalPrice = ko.computed(function () {
        var exVat = 0.0;
        $(self.invoices()).each(function (index, ele) {
            if (ele.print()) {
                exVat += ele.price().priceExVat;
            }
        });

        return new price(exVat, 0);
    });
};


