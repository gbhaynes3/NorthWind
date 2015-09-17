define(function(require) {

    breeze.NamingConvention.camelCase.setAsDefault();

    var northwindConfig = require('northwindConfig');
    var system = require('durandal/system');
    var entityQuery = breeze.EntityQuery;
    var dataService = new breeze.DataService({ serviceName: northwindConfig.platformDataService });
    var manager = new breeze.EntityManager({ dataService: dataService, saveOptions: new breeze.SaveOptions({ allowConcurrentSaves: true }) });
    var store = manager.metadataStore;
    var hasChanges = ko.observable(false);
    
    manager.hasChangesChanged.subscribe(function (changeArgs) {
        hasChanges(changeArgs.hasChanges);
    });

    var subscribePropertyChanged = function (entity) {
        entity.entityAspect.propertyChanged.subscribe(function (changeArgs) {
            if (changeArgs.propertyName == null) {
                entity.isModified(false);
            } else {
                entity.isModified(true);
            }
        });
    };

   

    
    function getOrders() {
        var query = entityQuery.from("Orders");

        return manager.executeQuery(query);
    }

    function getDataCount(dataResource, predicate) {
        var query = entityQuery.from(dataResource);

        if (predicate) {
            query = query.where(predicate);
        }

        query = query.take(0);
        query = query.inlineCount(true);

        return manager.executeQuery(query).then(querySucceeded).fail(queryFailed);

        function querySucceeded(data) {
            system.log('record count succeeded');
            system.log(data);
            return (data.inlineCount);
        }
    }

    function getData(dataResource, orderBy, predicate, currentPage, pageSize) {

        return getPartialData(dataResource, null, orderBy, predicate, currentPage, pageSize);
    }

    function getPartialData(dataResource, fieldList, orderBy, predicate, currentPage, pageSize) {

        var query = entityQuery.from(dataResource);

        if (orderBy) {
            query = query.orderBy(orderBy);
        }

        if (fieldList) {
            query = query.select(fieldList);
        }

        if (predicate) {
            query = query.where(predicate);
        }

        if (currentPage > -1 && pageSize) {
            query = query.skip(currentPage).take(pageSize);
        }

        return manager.executeQuery(query).then(querySucceeded).fail(queryFailed);

        function querySucceeded(data) {
            var msg = 'Query for ' + dataResource;

            if (fieldList) {
                msg += ' (' + fieldList + ') ';
            }
            msg += ' succeeded';
            system.log(msg);
            system.log(data);

            return data.results;
        }
    }

    function createNewEntity(dataType) {
        var newEntity = manager.createEntity(dataType);

        if (newEntity.isAdded) {
            newEntity.isAdded(true);
        }
        return newEntity;
    }

    function saveChanges(selectedEntities) {
        return manager.saveChanges(selectedEntities).then(function (data) {
            system.log('save changes completed.');
            system.log(data);
            data.entities.forEach(function (item) {
                if (item.isAdded) {
                    item.isAdded(false);
                }
                if (item.isModified) {
                    item.isModified(false);
                }
                if (item.isDeleted) {
                    item.isDeleted(false);
                }
            });
            return data;
        }).fail(function (data) {
            system.log('save changes errored.');
            system.log(data);
            return data;
        });
    }

    function cancelChanges() {
        manager.rejectChanges();
    }

    function queryFailed(error) {
        var msg = 'Error retreiving data.  ' + error.message;
        system.log(msg);

        throw error;
    }

    var metadataContext = {
        getData: getData
    };

    return metadataContext;
});