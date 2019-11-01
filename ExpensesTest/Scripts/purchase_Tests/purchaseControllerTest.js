'use strict'
describe("Real Purchase Controller Test", function () {

    let mockScope = {};
    let backend;
    let model = {};

    beforeEach(angular.mock.module("purchApp"));

    //Задаем тестовую покупку, в качестве передаваемой
    //в контроллер "purchaseController"
    //проверяем, что все объекты, связанные с редактированем покупки
    //(список магазинов, список групп товаров, товары)
    //заполняются корректно
    beforeEach(function () {
        model = {
            Id: 1,
            Date: new Date(2018, 0, 1),
            Count: 7,
            Price: 20,
            Item: {
                Id: 7,
                GId: 3,
                Name: "Рога и копыта"
            },
            Shop: {
                Id: 2,
                Name: "Пятерочка",
                Address: "Baku Plaza"
            }
        };
    });

    beforeEach(angular.mock.inject(function ($httpBackend) {
            backend = $httpBackend;
        //Мок со списком всех доступных групп товаров
        backend.expect("GET", "/Group/GetGroupList").respond(
            { GroupList: groups }
        );

        //Мок со списком всех доступных магазинов
        backend.expect("GET", "/Shop/GetList").respond(
            { Shops: shops }
        );
        //Мок со списком товаров по выбранной группе
        backend.expect("GET", "/Item/GetItemListByGroupId?groupId_=" + model.Item.GId).respond(
            { ItemList: items.filter(it => it.GId == model.Item.GId) }
        );

    }));


    //Создаем контроллер
    beforeEach(angular.mock.inject(
        function ($controller, $rootScope, $http) {
            mockScope = $rootScope.$new();

            $controller("purchaseController", {
                $scope: mockScope,
                $http: $http
            });

            //mockScope.getGroups(2);
            mockScope.init(model);
            //mockScope.getGroups(2);
            backend.flush();
            
        }
    ));

    it("Ajax тест(ответы получены)", function () {
        backend.verifyNoOutstandingExpectation();
    });


    it("тестируем Groups init(model)", function () {
        console.log("preved!!!!!sdfsdf");
        console.log();
        console.log("тестируем Groups init(model)");
        expect(mockScope.Id).toEqual(1);
        expect(mockScope.Price).toEqual(20);
        expect(mockScope.Count).toEqual(7);
        expect(mockScope.Groups.Items.length).toEqual(5);
        console.log("mockScope.Id=" + mockScope.Id);
        console.log("mockScope.Price=" + mockScope.Price);
        console.log("mockScope.Count=" + mockScope.Count);
        console.log("mockScope.Groups.Items.length=" + mockScope.Groups.Items.length);
        console.log("mockScope.Groups=");
        console.log(mockScope.Groups.Items[0].Name);
        console.log(mockScope.Groups.Items[1].Name);
        console.log(mockScope.Groups.Items[2].Name);
        console.log(mockScope.Groups.Items[3].Name);
        console.log(mockScope.Groups.Items[4].Name);
    });

    it("тестируем Shops init(model)", function () {
        console.log();
        console.log("тестируем Shops init(model)");
        console.log("mockScope.Shops.Items.length=" + mockScope.Shops.Items.length);
        expect(mockScope.Shops.Items.length).toEqual(4);
        console.log(mockScope.Shops.Items[0].Name);
        console.log(mockScope.Shops.Items[1].Name);
        console.log(mockScope.Shops.Items[2].Name);
        console.log(mockScope.Shops.Items[3].Name);
    });

    it("тестируем Items init(model)", function () {
        console.log();
        console.log("тестируем Items init(model)");
        console.log("mockScope.Items.Items.length=" + mockScope.Items.Items.length);
        expect(mockScope.Items.Items.length).toEqual(4);
        console.log(mockScope.Items.Items[0].Name);
        console.log(mockScope.Items.Items[1].Name);
        console.log(mockScope.Items.Items[2].Name);
        console.log(mockScope.Items.Items[3].Name);
    });


});