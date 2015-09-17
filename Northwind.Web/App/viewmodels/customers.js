define(function(require) {

    var app = require('durandal/app');
    var system = require('durandal/system');
    var router = require('plugins/router');
    var dataContext = require('services/dataContext');

    var customerData = ko.observableArray();
    

    return {
        activate: activate,
        customerData: customerData,
        loadCustomerData: loadCustomerData,
        
    };
    
    function activate() {
        loadCustomerData();
    }
    
    function loadCustomerData() {
        dataContext.getData('Customers').then(function(result) {
            result.forEach(function(item) {
                customerData.push(item);
            });
        });
    }
});